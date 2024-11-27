using Godot;
using System;

public class Intro3 : Node
{
	private AnimatedSprite animatedSprite;
	private TextureButton domek1Button;
	private TextureButton domek2Button;
	private TextureButton domek3Button;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		domek1Button = GetNode<TextureButton>("AnimatedSprite/Domek1Button");
		domek2Button = GetNode<TextureButton>("AnimatedSprite/Domek2Button");
		domek3Button = GetNode<TextureButton>("AnimatedSprite/Domek3Button");

		// Ukryj przyciski na początku
		domek1Button.Hide();
		domek2Button.Hide();
		domek3Button.Hide();

		// Odtwórz animację
		animatedSprite.Play("Intro3");
		animatedSprite.Connect("animation_finished", this, nameof(OnAnimationFinished));

		// Połączenie przycisków
		domek1Button.Connect("pressed", this, nameof(OnDomek1ButtonPressed));
		domek2Button.Connect("pressed", this, nameof(OnDomek2ButtonPressed));
	}

	private void OnAnimationFinished()
	{
		// Pokaż przyciski po zakończeniu animacji
		domek1Button.Show();
		domek2Button.Show();
		domek3Button.Show();
	}

	private void OnDomek1ButtonPressed()
	{
		Global global = GetNode<Global>("/root/Global");
		global.ResetProgress();
	}
	private void OnDomek2ButtonPressed()
	{
		// Zmiana sceny, załóżmy, że nazywa się NextScene.tscn
		GetTree().ChangeScene("res://scenes/Intro4.tscn");
	}


}
