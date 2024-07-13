using Godot;
using System;

public class Zadania : Node
{
	public override void _Ready()
	{
		// Podłącz przyciski do metod obsługi
		GetNode<TextureButton>("QuizyButton").Connect("pressed", this, nameof(OnQuizyButtonPressed));
		GetNode<TextureButton>("MemoryButton").Connect("pressed", this, nameof(OnMemoryButtonPressed));
		GetNode<TextureButton>("KrzyzowkiButton").Connect("pressed", this, nameof(OnKrzyzowkiButtonPressed));
		GetNode<TextureButton>("StrzelnicaButton").Connect("pressed", this, nameof(OnStrzelnicaButtonPressed));
		GetNode<TextureButton>("LawinaButton").Connect("pressed", this, nameof(OnLawinaButtonPressed));
		GetNode<TextureButton>("RybakButton").Connect("pressed", this, nameof(OnRybakButtonPressed));
		GetNode<TextureButton>("Description/BackButton").Connect("pressed", this, nameof(OnBackButtonPressed));
	}

	private void OnQuizyButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Quizy.tscn");
	}

	private void OnMemoryButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Memory.tscn");
	}

	private void OnKrzyzowkiButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Krzyzowki.tscn");
	}

	private void OnStrzelnicaButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Strzelnica.tscn");
	}

	private void OnLawinaButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Lawina.tscn");
	}

	private void OnRybakButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Rybak.tscn");
	}

	private void OnBackButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/MainScene.tscn");
	}
}
