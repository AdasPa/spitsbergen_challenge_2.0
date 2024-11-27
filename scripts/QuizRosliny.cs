using Godot;
using System;

public class QuizRosliny : Node
{
	private QuizGame quizGame;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		// Find the QuizGame instance in the scene
		quizGame = GetNode<QuizGame>("QuizGame");
		var backButton = GetNode<TextureButton>("Description/BackButton");
		backButton.Connect("pressed", this, nameof(OnBackButtonPressed));

		// Set the question file path for history quiz
		if (quizGame != null)
		{
			quizGame.SetQuestionFilePathAndGameName("res://questions/rosliny_questions.json", "quiz_rosliny");
		}
		else
		{
			GD.PrintErr("QuizGame node not found in the scene.");
		}
	}
	private void OnBackButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Quizy.tscn");
	}
}
