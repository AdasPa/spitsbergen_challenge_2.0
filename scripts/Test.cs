using Godot;
using System;

public class Test : Control
{
	public override void _Ready()
	{
		// Pobierz instancję LinkContainer
		var linkContainer = GetNode<LinkContainer>("LinkContainer");

		// Ustaw wartości na twardo do testowania
		linkContainer.LinkTitles = new string[] 
		{ 
			"Wikipedia", 
			"WP", 
			"PG", 
			"Google", 
			"YouTube", 
			"GitHub" 
		};
		linkContainer.LinkDescriptions = new string[]
		{
			"Wikipedia, wolna encyklopedia",
			"Wirtualna Polska",
			"Politechnika Gdańska",
			"Najpopularniejsza wyszukiwarka",
			"Największa platforma wideo",
			"Największy serwis hostingowy dla projektów programistycznych"
		};
		linkContainer.LinkURLs = new string[]
		{
			"https://pl.wikipedia.org",
			"https://wp.pl",
			"https://pg.edu.pl",
			"https://google.com",
			"https://youtube.com",
			"https://github.com"
		};

		// Ręczne wywołanie _Ready, aby zastosować zmiany (opcjonalne, jeśli scena jest uruchamiana później)
		linkContainer._Ready();
	}
}
