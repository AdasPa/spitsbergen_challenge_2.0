using Godot;
using System;

public class Intro4 : Node
{
	private AnimatedSprite animatedSprite;
	private TextureButton dalejButton;

	public override void _Ready()
	{
		// Znajdź węzeł AnimatedSprite i Button w drzewie sceny
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		dalejButton = GetNode<TextureButton>("AnimatedSprite/DalejButton");
		dalejButton.Hide();

		animatedSprite.Play("Intro4");
		animatedSprite.Connect("animation_finished", this, nameof(OnAnimationFinished));

		dalejButton.Connect("pressed", this, nameof(OnDalejButtonPressed));
	}

	private void OnAnimationFinished()
	{
		// Pokaż przycisk Dalej po zakończeniu animacji
		dalejButton.Show();
	}

	private void OnDalejButtonPressed()
	{
		// Zmiana sceny, załóżmy, że następna scena nazywa się MainScene.tscn
		GetTree().ChangeScene("res://scenes/MainScene.tscn");
	}
}
