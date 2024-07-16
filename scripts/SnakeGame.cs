using Godot;
using System;
using System.Collections.Generic;

public class SnakeGame : Control
{
	[Export] public int GridSize = 50; // Wymiary pojedynczego kwadracika
	[Export] public int MoveInterval = 120; // Interwał ruchu węża w milisekundach
	
	private Timer _moveTimer;
	private Timer _spawnFishTimer;
	private Label _scoreLabel;
	private Label _moneyLabel;
	private Label _wynikLabel;
	private Panel _snakePanel;
	private ColorRect _winPanel;
	private TextureRect _snakeHead;
	private TextureButton _sklepButton;
	private TextureButton _listaZadanButton;
	private Node2D _fishNode;
	private List<TextureRect> _snakeBody = new List<TextureRect>();
	private Vector2 _direction = Vector2.Right;
	private Random _random = new Random();
	private int _score = 0;
	private string fish1Texture = "res://assets/snake/ryba01.png";
	private string fish2Texture = "res://assets/snake/ryba02.png";
	private string fish3Texture = "res://assets/snake/ryba03.png";
		
	private Timer _countdownTimer;
	private Label _countdownLabel;
	private int countdown = 2;

	private const int BoardWidth = 1000;
	private const int BoardHeight = 700;
	
	private int _earnedMoney = 0;
	
	public override void _Ready()
	{
		_moveTimer = GetNode<Timer>("SnakePanel/MoveTimer");
		_spawnFishTimer = GetNode<Timer>("SnakePanel/SpawnFishTimer");
		_scoreLabel = GetNode<Label>("SnakePanel/ScoreLabel");
		_snakeHead = GetNode<TextureRect>("SnakePanel/SnakeHead");
		_snakePanel = GetNode<Panel>("SnakePanel");
		_winPanel = GetNode<ColorRect>("WinPanel");
		_fishNode = GetNode<Node2D>("SnakePanel/FishNode");
		_countdownLabel = GetNode<Label>("SnakePanel/CountdownLabel");
		_countdownTimer = GetNode<Timer>("SnakePanel/CountdownTimer");
		
		_snakePanel.Visible = true;
		_winPanel.Visible = false;

		_countdownTimer = new Timer();
		AddChild(_countdownTimer);
		_countdownTimer.WaitTime = 1.0f; // Update every second
		_countdownTimer.Connect("timeout", this, nameof(OnCountdownTimerTimeout));
		_countdownTimer.Start();
		
		_moveTimer.WaitTime = MoveInterval / 1000f;
		_moveTimer.Connect("timeout", this, nameof(OnMoveTimerTimeout));


		_spawnFishTimer.WaitTime = 2;
		_spawnFishTimer.Connect("timeout", this, nameof(OnSpawnFishTimerTimeout));

		
			// Connect buttons
		_sklepButton = GetNode<TextureButton>("WinPanel/ColorRect/SklepButton1");
		_listaZadanButton = GetNode<TextureButton>("WinPanel/ColorRect/ListaZadanButton");
		
		_wynikLabel = GetNode<Label>("WinPanel/WynikLabel");
		_moneyLabel = GetNode<Label>("WinPanel/MonetyPanel/MoneyLabel");

		_sklepButton.Connect("pressed", this, nameof(OnSklepButtonPressed));
		_listaZadanButton.Connect("pressed", this, nameof(OnListaZadanButtonPressed));
		
		SpawnFish();
		UpdateScore();
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("ui_right") && _direction != Vector2.Left)
		{ 
			_direction = Vector2.Right;
			_snakeHead.Texture = (Texture)GD.Load("res://assets/snake/kuter_right.png");
		}
		if (Input.IsActionPressed("ui_left") && _direction != Vector2.Right)
		{
			 _direction = Vector2.Left;
			_snakeHead.Texture = (Texture)GD.Load("res://assets/snake/kuter_left.png");
		}
		if (Input.IsActionPressed("ui_up") && _direction != Vector2.Down)
		{
			 _direction = Vector2.Up;
			_snakeHead.Texture = (Texture)GD.Load("res://assets/snake/kuter.png");
		}
		if (Input.IsActionPressed("ui_down") && _direction != Vector2.Up)
		{
			 _direction = Vector2.Down;
			_snakeHead.Texture = (Texture)GD.Load("res://assets/snake/kuter_down.png");
		}
	}

	private void StartGame()
	{
		_countdownLabel.Visible = false;
		_moveTimer.Start();
		_spawnFishTimer.Start();
	}

	private void OnCountdownTimerTimeout()
	{
		if (countdown > 0)
		{
			_countdownLabel.Text = countdown.ToString();
			countdown--;
		}
		else
		{
			_countdownLabel.Text = "START!";
			_countdownTimer.Stop();
			// Delay the actual start by 1 second to show "START!"
			GetTree().CreateTimer(1.0f).Connect("timeout", this, nameof(StartGame));
		}
	}
	private void OnMoveTimerTimeout()
	{
		MoveSnake();
	}

	private void OnSpawnFishTimerTimeout()
	{
		SpawnFish();
	}

	private void MoveSnake()
	{
		var headPosition = _snakeHead.RectPosition;
		var newHeadPosition = headPosition + _direction * GridSize;

		if (newHeadPosition.x < 0 || newHeadPosition.y < 0 || newHeadPosition.x >= BoardWidth || newHeadPosition.y >= BoardHeight)
		{
			GameOver();
			return;
		}

		foreach (var segment in _snakeBody)
		{
			if (segment.RectPosition == newHeadPosition)
			{
				GameOver();
				return;
			}
		}

		for (int i = _snakeBody.Count - 1; i > 0; i--)
		{
			_snakeBody[i].RectPosition = _snakeBody[i - 1].RectPosition;
		}

		if (_snakeBody.Count > 0)
		{
			_snakeBody[0].RectPosition = _snakeHead.RectPosition;
		}

		_snakeHead.RectPosition = newHeadPosition;

		CheckForFish(newHeadPosition);
	}

	private void CheckForFish(Vector2 position)
	{
		foreach (TextureRect fish in GetTree().GetNodesInGroup("Fish"))
		{
			if (fish.RectPosition == position)
			{
				_score += 1;
				UpdateScore();
				ExtendSnake();
				fish.QueueFree();
				return;
			}
		}
	}

	private void ExtendSnake()
	{
		var newSegment = new TextureRect();
		var r = _random.Next(0, 3);
		if(r == 0)	newSegment.Texture = (Texture)GD.Load(fish1Texture);
		if(r == 1)	newSegment.Texture = (Texture)GD.Load(fish2Texture);
		if(r == 2)	newSegment.Texture = (Texture)GD.Load(fish3Texture);
		newSegment.RectSize = new Vector2(GridSize, GridSize);
		newSegment.RectPosition = _snakeHead.RectPosition;
		_fishNode.AddChild(newSegment);
		_snakeBody.Add(newSegment);
	}

	private void SpawnFish()
	{
		var fish = new TextureRect();
		var r = _random.Next(0, 3);
		if(r == 0)	fish.Texture = (Texture)GD.Load(fish1Texture);
		if(r == 1)	fish.Texture = (Texture)GD.Load(fish2Texture);
		if(r == 2)	fish.Texture = (Texture)GD.Load(fish3Texture);
		fish.RectSize = new Vector2(GridSize, GridSize);
		fish.RectPosition = new Vector2(_random.Next(0, BoardWidth / GridSize) * GridSize,
										 _random.Next(0, BoardHeight / GridSize) * GridSize);
										
		fish.AddToGroup("Fish");
		_fishNode.AddChild(fish);
	}

	private void UpdateScore()
	{
		_scoreLabel.Text = $"Wynik: {_score}";
	}

	private void GameOver()
	{
		GD.Print("Game Over");
		_moveTimer.Stop();
		_spawnFishTimer.Stop();
		
		_snakePanel.Visible = false;
		_winPanel.Visible = true;
		
		_earnedMoney = _score;
		_moneyLabel.Text = $"+{_earnedMoney} nok";
		_wynikLabel.Text = $"Wynik: {_score}.";
		

		// Add earned money to the global state
		Global global = GetNode<Global>("/root/Global");
		global.AddMoney(_earnedMoney);

		// Display the result screen
		GD.Print("Showing result screen");
	}
	
	private void OnSklepButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Sklep.tscn");
	}

	private void OnListaZadanButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Zadania.tscn");
	}

}
