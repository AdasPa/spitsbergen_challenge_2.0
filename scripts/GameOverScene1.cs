using Godot;
using System;

public class GameOverScene1 : Node
{
	private AnimatedSprite animatedSprite;
	private TextureButton dalejButton;

	public override void _Ready()
	{
		// Znajdź węzeł AnimatedSprite i Button w drzewie sceny
		animatedSprite = GetNode<AnimatedSprite>("Background/GameOverAnimation1");
		dalejButton = GetNode<TextureButton>("Background/DalejButton");
		dalejButton.Hide();

		animatedSprite.Play("GameOverAnimation1");
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
		GetTree().ChangeScene("res://scenes/GameOverScene2.tscn");
	}
}
