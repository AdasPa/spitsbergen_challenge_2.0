using Godot;
using System;
using System.Collections.Generic;



public class ShooterGame : Control
{
	[Export] public float MoveSpeed = 300f; // Prędkość poruszania celownikiem
	[Export] public int GameDuration = 60; // Czas gry w sekundach

	private Sprite celownik;
	private Label scoreLabel;
	private Node2D targetNode;
	private int score = 0;
	private int earnedMoney = 0;
	private Random random = new Random();
	private Timer spawnTimer;
	private PackedScene targetScene; // Scene containing target Sprite
	private Panel shooterPanel;
	private ColorRect winPanel;

	private Timer gameTimer;
	private float remainingTime;
	private Label timeLabel;
	private TextureButton sklepButton;
	private TextureButton listaZadanButton;
	private Label moneyLabel;
	private Label wynikLabel;

	private Timer countdownTimer;
	private Label countdownLabel;
	private int countdown = 2;

	private Texture originalCelownikTexture;
	private Texture shotCelownikTexture;



	private const int celownikSizeX = 88;
	private const int celownikSizeY = 88;
	private const int shooterPanelSizeX = 1280;
	private const int shooterPanelSizeY = 700;


	public override void _Ready()
	{
		celownik = GetNode<Sprite>("ShooterPanel/Celownik");
		scoreLabel = GetNode<Label>("ShooterPanel/ScoreLabel");
		targetNode = GetNode<Node2D>("ShooterPanel/TargetNode");
		shooterPanel = GetNode<Panel>("ShooterPanel");
		winPanel = GetNode<ColorRect>("WinPanel");
		timeLabel = GetNode<Label>("ShooterPanel/TimeLabel");
		countdownLabel = GetNode<Label>("ShooterPanel/CountdownLabel");
		countdownTimer = GetNode<Timer>("ShooterPanel/CountdownTimer");

		originalCelownikTexture = celownik.Texture;
		shotCelownikTexture = (Texture)GD.Load("res://assets/shooter/shotCelownik.png");

		
		shooterPanel.Visible = true;
		winPanel.Visible = false;
		
		celownik.Position = new Vector2(shooterPanelSizeX/2, shooterPanelSizeY/2);

		spawnTimer = new Timer();
		AddChild(spawnTimer);
		spawnTimer.WaitTime = (float)random.NextDouble() * 2 + 1; // Random time between 1 and 3 seconds
		spawnTimer.Connect("timeout", this, nameof(OnSpawnTimerTimeout));

		countdownTimer = new Timer();
		AddChild(countdownTimer);
		countdownTimer.WaitTime = 1.0f; // Update every second
		countdownTimer.Connect("timeout", this, nameof(OnCountdownTimerTimeout));
		countdownTimer.Start();


		gameTimer = new Timer();
		AddChild(gameTimer);
		gameTimer.WaitTime = 1.0f; // Update every second
		gameTimer.Connect("timeout", this, nameof(OnGameTimerTimeout));

		sklepButton = GetNode<TextureButton>("WinPanel/ColorRect/SklepButton1");
		listaZadanButton = GetNode<TextureButton>("WinPanel/ColorRect/ListaZadanButton");
		
		wynikLabel = GetNode<Label>("WinPanel/WynikLabel");
		moneyLabel = GetNode<Label>("WinPanel/MonetyPanel/MoneyLabel");

		sklepButton.Connect("pressed", this, nameof(OnSklepButtonPressed));
		listaZadanButton.Connect("pressed", this, nameof(OnListaZadanButtonPressed));

		remainingTime = GameDuration;
		UpdateTimeLabel();


		targetScene = (PackedScene)ResourceLoader.Load("res://scenes/ShooterTarget.tscn"); // Load your target scene

		
	}

	public override void _Process(float delta)
	{
		Vector2 movement = Vector2.Zero;

		if (Input.IsActionPressed("ui_right") )
			movement.x += 1;
		if (Input.IsActionPressed("ui_left") )
			movement.x -= 1;
		if (Input.IsActionPressed("ui_up"))
			movement.y -= 1;
		if (Input.IsActionPressed("ui_down") )
			movement.y += 1;

		// Aktualizacja pozycji celownika
		celownik.Position += movement * MoveSpeed * delta;

		// Zabezpieczenie przed wyjściem poza ekran
		celownik.Position = new Vector2(
			Mathf.Clamp(celownik.Position.x, celownikSizeX/2, shooterPanelSizeX - celownikSizeX/2),
			Mathf.Clamp(celownik.Position.y, celownikSizeY/2, shooterPanelSizeY - celownikSizeY/2)
		);

		// Logika strzału
		if (Input.IsActionJustPressed("ui_accept")) // ui_accept jest domyślnie przypisane do klawisza spacji
		{
			CheckForHit();
		}
	}

private void StartGame()
{
	countdownLabel.Visible = false;
	celownik.Visible = true;
	spawnTimer.Start();
	gameTimer.Start();
}


	private void CheckForHit()
	{
		Vector2 celownikCenter = celownik.GlobalPosition;
		Rect2 celownikRect = new Rect2(
			celownikCenter.x - 10, 
			celownikCenter.y - 10, 
			20, 
			20
		);
		
		GD.Print("Shot!");

		celownik.Texture = shotCelownikTexture;
		GetTree().CreateTimer(0.1f).Connect("timeout", this, nameof(RestoreCelownikTexture));


		foreach (Node target in GetTree().GetNodesInGroup("Targets"))
		{
			Sprite targetSprite = (Sprite)target;
			Rect2 targetRect = new Rect2(targetSprite.GlobalPosition - targetSprite.Texture.GetSize() / 2, targetSprite.Texture.GetSize());

			if (celownikRect.Intersects(targetRect))
			{
				OnTargetHit(targetSprite);
				targetSprite.QueueFree(); // Usunięcie trafionego celu
				break; // Break to avoid hitting multiple targets in one shot
			}
		}
	}

	private void OnTargetHit(Sprite target)
	{
		score++;
		scoreLabel.Text = $"Wynik: {score}";
	}

	private void OnSpawnTimerTimeout()
	{
		if (GetTree().GetNodesInGroup("Targets").Count < 3)
		{
			SpawnTarget();
		}

		// Restart the timer with a new random time
		spawnTimer.WaitTime = (float)random.NextDouble() * 2 + 1;
		spawnTimer.Start();
	}

	private void SpawnTarget()
	{
		Node2D newTarget = (Node2D)targetScene.Instance();
		newTarget.Position = new Vector2(
			random.Next(celownikSizeX / 2, shooterPanelSizeX - celownikSizeX / 2),
			random.Next(celownikSizeY / 2, shooterPanelSizeY - celownikSizeY / 2)
		);
		newTarget.AddToGroup("Targets");
		targetNode.AddChild(newTarget);
	}

	private void OnCountdownTimerTimeout()
{
	if (countdown > 0)
	{
		countdownLabel.Text = countdown.ToString();
		countdown--;
	}
	else
	{
		countdownLabel.Text = "START!";
		countdownTimer.Stop();
		// Delay the actual start by 1 second to show "START!"
		GetTree().CreateTimer(1.0f).Connect("timeout", this, nameof(StartGame));
	}
}


	private void OnGameTimerTimeout()
	{
		remainingTime -= 1;
		UpdateTimeLabel();

		if (remainingTime <= 0)
		{
			GameOver();
		}
	}
	private void UpdateTimeLabel()
	{
		timeLabel.Text = $"{remainingTime}";
	}
	private void GameOver()
	{
		GD.Print("Game Over");
		spawnTimer.Stop();
		gameTimer.Stop();

		
		shooterPanel.Visible = false;
		winPanel.Visible = true;

		earnedMoney = score;
		moneyLabel.Text = $"+{earnedMoney} nok";
		wynikLabel.Text = $"Wynik: {score}.";
		

		// Add earned money to the global state
		Global global = GetNode<Global>("/root/Global");
		global.AddMoney(earnedMoney);

		// Display the result screen
		GD.Print("Showing result screen");
		
	}

	private void RestoreCelownikTexture()
	{
		celownik.Texture = originalCelownikTexture;
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
