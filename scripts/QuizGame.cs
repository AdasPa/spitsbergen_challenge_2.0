using Godot;
using System;
using Godot.Collections;

using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;

public class QuizGame : Control
{
	[Export]
	public string questionFilePath = "res://questions/test_questions.json";

	private Label questionLabel;
	private TextureButton[] answerButtons;
	private Label[] answerLabels;
	private Label questionCounterLabel;
	
	private TextureButton listaQuizowButton;
	private TextureButton sklepButton1;
	private ColorRect quizPanel;
	private ColorRect winPanel;
	private Label wynikLabel;
	private Label moneyLabel;



	private Array<Dictionary> questions;
	private int currentQuestionIndex = 0;
	private int correctAnswers = 0;
	private int earnedMoney = 0;
	private int maxQuestions = 10;
	
	

	public override void _Ready()
	{
		// Initialize nodes
		questionLabel = GetNode<Label>("QuizPanel/QuestionPanel/Question");
		answerButtons = new TextureButton[]
		{
			GetNode<TextureButton>("QuizPanel/QuestionPanel/Answer0Button"),
			GetNode<TextureButton>("QuizPanel/QuestionPanel/Answer1Button"),
			GetNode<TextureButton>("QuizPanel/QuestionPanel/Answer2Button"),
			GetNode<TextureButton>("QuizPanel/QuestionPanel/Answer3Button")
		};
		answerLabels = new Label[]
		{
			GetNode<Label>("QuizPanel/QuestionPanel/Answer0Button/Answer0"),
			GetNode<Label>("QuizPanel/QuestionPanel/Answer1Button/Answer1"),
			GetNode<Label>("QuizPanel/QuestionPanel/Answer2Button/Answer2"),
			GetNode<Label>("QuizPanel/QuestionPanel/Answer3Button/Answer3")
		};
		questionCounterLabel = GetNode<Label>("QuizPanel/QuestionPanel/Counter");

		quizPanel = GetNode<ColorRect>("QuizPanel");
		winPanel = GetNode<ColorRect>("WinPanel");
		winPanel.Visible = false;
		
		wynikLabel = GetNode<Label>("WinPanel/WynikLabel");
		moneyLabel = GetNode<Label>("WinPanel/MonetyPanel/MoneyLabel");

		listaQuizowButton = GetNode<TextureButton>("WinPanel/ColorRect/ListaQuizowButton");
		sklepButton1 = GetNode<TextureButton>("WinPanel/ColorRect/SklepButton1");

		listaQuizowButton.Connect("pressed", this, nameof(OnListaQuizowButtonPressed));
		sklepButton1.Connect("pressed", this, nameof(OnSklepButton1Pressed));

		for (int i = 0; i < answerButtons.Length; i++)
		{
			int index = i;
			answerButtons[i].Connect("pressed", this, nameof(OnAnswerButtonPressed), new Array { index });
		}
		
		listaQuizowButton.Connect("pressed", this, nameof(OnListaQuizowButtonPressed));
		sklepButton1.Connect("pressed", this, nameof(OnSklepButton1Pressed));
	}
	
	public void SetQuestionFilePath(string path)
	{
		questionFilePath = path;
		InitializeQuiz();
	}

	private void InitializeQuiz()
	{
		LoadQuestions();
		ShuffleQuestions();
		DisplayQuestion();
	}

	private void LoadQuestions()
	{
		GD.Print("Loading questions");

		var file = new File();
		if (file.FileExists(questionFilePath))
		{
			file.Open(questionFilePath, File.ModeFlags.Read);
			var json = file.GetAsText();
			file.Close();

			var result = JSON.Parse(json);
			if (result.Error != Error.Ok)
			{
				GD.PrintErr("Failed to parse JSON: ", result.ErrorString);
				return;
			}

			//GD.Print("Parsed JSON result: ", result.Result);
			var rawQuestions = result.Result as Array;
			if (rawQuestions == null)
			{
				GD.PrintErr("Failed to cast parsed JSON to Array");
				return;
			}

			questions = new Array<Dictionary>();
			foreach (var item in rawQuestions)
			{
				var questionDict = item as Dictionary;
				if (questionDict != null)
				{
					questions.Add(questionDict);
				}
				else
				{
					GD.PrintErr("Failed to cast question item to Dictionary");
				}
			}

			GD.Print("Questions loaded successfully");
		}
		else
		{
			GD.PrintErr($"File not found: {questionFilePath}");
		}
	}

	private void ShuffleQuestions()
	{
		GD.Print("Shuffling questions");

		// Shuffle the questions
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		for (int i = 0; i < questions.Count; i++)
		{
			int j = rng.RandiRange(0, questions.Count - 1);
			var temp = questions[i];
			questions[i] = questions[j];
			questions[j] = temp;
		}

		GD.Print("Questions shuffled");
	}

	private void DisplayQuestion()
	{
		if (currentQuestionIndex >= maxQuestions)
		{
			GD.Print("Reached max questions, showing result");
			ShowResult();
			return;
		}

		var questionData = questions[currentQuestionIndex];
		questionLabel.Text = questionData["question"].ToString();
		var answers = questionData["answers"] as Array;

		for (int i = 0; i < answers.Count; i++)
		{
			answerLabels[i].Text = answers[i].ToString();
		}

		questionCounterLabel.Text = $"{currentQuestionIndex + 1}/{maxQuestions}";

		GD.Print($"Displayed question {currentQuestionIndex + 1}/{maxQuestions}");
	}

	private void OnAnswerButtonPressed(int index)
	{
		GD.Print($"Answer button {index} pressed");

		var questionData = questions[currentQuestionIndex];
		GD.Print(questionData);
		int correctIndex = Convert.ToInt32(questionData["correct"]);


		if (index == correctIndex)
		{
			correctAnswers++;
			answerButtons[index].Modulate = new Color(0, 1, 0, 0.5f); // Green for correct answer
			GD.Print("Correct answer selected");
		}
		else
		{
			answerButtons[index].Modulate = new Color(1, 0, 0, 0.5f); // Red for incorrect answer
			answerButtons[correctIndex].Modulate = new Color(0, 1, 0, 0.5f); // Highlight the correct answer
			GD.Print("Incorrect answer selected");
		}

		// Delay before showing the next question
		GetTree().CreateTimer(1).Connect("timeout", this, nameof(OnNextQuestion));
	}

	private void OnNextQuestion()
	{
		// Reset button colors
		foreach (var button in answerButtons)
		{
			button.Modulate = new Color(1, 1, 1, 1);
		}

		currentQuestionIndex++;
		DisplayQuestion();
	}

	private void ShowResult()
	{
		// Hide the quiz panel and show the win panel
		quizPanel.Visible = false;
		winPanel.Visible = true;
		
		earnedMoney = correctAnswers*10;
		
		wynikLabel.Text = $"Twój wynik: {correctAnswers} / {maxQuestions}";
		moneyLabel.Text = $"+{earnedMoney} nok";

		// Add earned money to the global state
		Global global = GetNode<Global>("/root/Global");
		global.AddMoney(earnedMoney);

		// Display the result screen
		GD.Print("Showing result screen");
	}
	
	private void OnListaQuizowButtonPressed()
	{
		GD.Print("Lista Quizow Button Pressed");
		// Tutaj dodaj akcję do wykonania po naciśnięciu przycisku, np. zmiana sceny
		GetTree().ChangeScene("res://scenes/Quizy.tscn");
	}

	private void OnSklepButton1Pressed()
	{
		GD.Print("Sklep Button 1 Pressed");
		// Tutaj dodaj akcję do wykonania po naciśnięciu przycisku, np. zmiana sceny
		GetTree().ChangeScene("res://scenes/Sklep.tscn");
	}
}