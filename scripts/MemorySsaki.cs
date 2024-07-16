using Godot;
using System;

public class MemorySsaki : Node
{
	private MemoryGame memoryGame;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the QuizGame instance in the scene
		memoryGame = GetNode<MemoryGame>("MemoryGame");
		var backButton = GetNode<TextureButton>("Description/BackButton");
		backButton.Connect("pressed", this, nameof(OnBackButtonPressed));

		// Set the question file path for history quiz
		if (memoryGame != null)
		{
			memoryGame.SetCardDataFilePath("res://memory/test/test.json");
		}
		else
		{
			GD.PrintErr("MemoryGame node not found in the scene.");
		}
	}
	private void OnBackButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Memory.tscn");
	}
}
