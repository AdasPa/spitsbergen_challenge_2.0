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

		for (int i = 0; i < LinkTitles.Length; i++)
		{
			var linkComponent = (LinkComponent)GD.Load<PackedScene>("res://scenes/LinkComponent.tscn").Instance();
			linkComponent.LinkTitle = LinkTitles[i];
			linkComponent.LinkDescription = LinkDescriptions[i];
			linkComponent.LinkURL = LinkURLs[i];

			linkContainer.AddChild(linkComponent);
		}
	}
}
