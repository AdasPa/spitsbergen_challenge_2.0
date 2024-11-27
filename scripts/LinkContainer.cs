using Godot;
using System;

public class LinkContainer : Control
{
	[Export]
	public string[] LinkTitles;

	[Export]
	public string[] LinkDescriptions;

	[Export]
	public string[] LinkURLs;
	
	private VBoxContainer linkContainer;

	public override void _Ready()
	{
		linkContainer = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");

		//linkContainer.AddConstantOverride("separation", 0);

		for (int i = 0; i < LinkTitles.Length; i++)
		{
			var linkComponent = (LinkComponent)GD.Load<PackedScene>("res://scenes/LinkComponent.tscn").Instance();
			linkComponent.LinkTitle = LinkTitles[i];
			linkComponent.LinkDescription = LinkDescriptions[i];
			linkComponent.LinkURL = LinkURLs[i];

			linkContainer.AddChild(linkComponent);

			var spacer = new Control();
			spacer.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkCenter; // Kontroluje rozciąganie

			if (i < LinkTitles.Length - 1)
				spacer.RectMinSize = new Vector2(0, 240); // Odstęp 240
			else
				spacer.RectMinSize = new Vector2(0, 217); // Przed ostatnim elementem mniejszy odstęp, by uniknąć mnadmiarowej pustej przestrzeni
				
			linkContainer.AddChild(spacer);
			
		}
	}
}
