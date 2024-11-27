using Godot;
using System;

public class GameOverScene2 : Node
{
	private AnimatedSprite animatedSprite;
	private TextureButton zagrajOdPoczatku;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("GameOverAnimation2");
		zagrajOdPoczatku = GetNode<TextureButton>("ZagrajOdPoczatkuButton");
		zagrajOdPoczatku.Hide();

		animatedSprite.Play("GameOverAnimation2");
		animatedSprite.Connect("animation_finished", this, nameof(OnAnimationFinished));

		zagrajOdPoczatku.Connect("pressed", this, nameof(OnZagrajOdPoczatkuPressed));
	}

	private void OnAnimationFinished()
	{
		zagrajOdPoczatku.Show();
	}

	private void OnZagrajOdPoczatkuPressed()
	{
		var global = (Global)GetNode("/root/Global");
		global.ResetProgress();
		GetTree().ChangeScene("res://scenes/Intro.tscn");
	}
}
