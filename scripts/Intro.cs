using Godot;
using System;

public class Intro : Node
{
	private AnimatedSprite animatedSprite;
	private TextureButton zagrajButton;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		zagrajButton = GetNode<TextureButton>("AnimatedSprite/ZagrajButton");
		zagrajButton.Hide(); // Ukryj przycisk na początku
		
		animatedSprite.Play("Intro");
		animatedSprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
		
		zagrajButton.Connect("pressed", this, nameof(OnZagrajButtonPressed));

		//ResetProgress();
	}

	private void OnAnimationFinished()
	{
		// Pokaż przycisk "Zagraj" po zakończeniu animacji
		zagrajButton.Show();
	}
	
		private void OnZagrajButtonPressed()
	{
		// Zmiana sceny, załóżmy, że następna scena nazywa się NextScene.tscn
		GetTree().ChangeScene("res://scenes/Intro2.tscn");
	}
}
