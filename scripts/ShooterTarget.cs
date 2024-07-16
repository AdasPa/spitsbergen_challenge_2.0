using Godot;
using System;

public class ShooterTarget : Sprite
{
	private Random random = new Random();
	private string[] textures = {
		"res://assets/shooter/puszka1.png",
		"res://assets/shooter/puszka2.png",
		"res://assets/shooter/puszka3.png"
	};

	public override void _Ready()
	{
		// Losowanie tekstury
		int randomIndex = random.Next(0, textures.Length);
		Texture = (Texture)GD.Load(textures[randomIndex]);
	}
}
