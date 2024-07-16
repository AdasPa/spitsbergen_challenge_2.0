using Godot;
using System;

public class Strzelnica : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Find the QuizGame instance in the scene
		var backButton = GetNode<TextureButton>("Description/BackButton");
		backButton.Connect("pressed", this, nameof(OnBackButtonPressed));
	}
	private void OnBackButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Zadania.tscn");
	}
}
