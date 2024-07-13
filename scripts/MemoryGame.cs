using Godot;
using System;
using Godot.Collections;

using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;

public class MemoryGame : Control
{
	[Export]
	public string cardDataFilePath = "res://memory/test/test.json";

	private GridContainer gridContainer;
	private Array<TextureButton> cardButtons;
	private Array<TextureButton> disabledButtons = new Array<TextureButton>();
	private Array<Dictionary> cards = new Array<Dictionary>();
	private Texture cardBackTexture;
	private Texture cardBackTexturePressed;
	private Texture cardBackTextureHover;
	
	private int card1Id = -1;
	private TextureButton card1Button;
	
	private int[] cardIndices;

	public override void _Ready()
	{
		gridContainer = GetNode<GridContainer>("MemoryPanel/GridContainer");
		cardBackTexture = (Texture)GD.Load("res://memory/CoverDefault.png");
		cardBackTexturePressed = (Texture)GD.Load("res://memory/CoverPress.png");
		cardBackTextureHover = (Texture)GD.Load("res://memory/CoverHover.png");
			
		cardButtons = new Array<TextureButton>
		{
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton2"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton3"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton4"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton5"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton6"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton7"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton8"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton9"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton10"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton11"),
			GetNode<TextureButton>("MemoryPanel/GridContainer/TextureButton12")
		};

		LoadCards();
		InitializeCards();
	}

	private void LoadCards()
	{
		GD.Print("Loading cards");

		var file = new File();
		if (file.FileExists(cardDataFilePath))
		{
			file.Open(cardDataFilePath, File.ModeFlags.Read);
			var json = file.GetAsText();
			file.Close();

			var result = JSON.Parse(json);
			if (result.Error != Error.Ok)
			{
				GD.PrintErr("Failed to parse JSON: ", result.ErrorString);
				return;
			}

			 var rawCards = result.Result as Array;
			if (rawCards == null)
			{
				GD.PrintErr("Failed to cast parsed JSON to Array");
				return;
			}

			foreach (Dictionary cardData in rawCards)
			{
				var cardImage = new Dictionary
				{
					{ "type", "image" },
					{ "path", cardData["image"].ToString() },
					{ "name", cardData["name"].ToString() }
				};
				var cardText = new Dictionary
				{
					{ "type", "text" },
					{ "path", cardData["text"].ToString() },
					{ "name", cardData["name"].ToString() }
				};

				cards.Add(cardImage);
				cards.Add(cardText); // Add a pair of each card
				GD.Print(cardData);
			}


			GD.Print(cards);
			GD.Print("Cards loaded successfully");
		}
		else
		{
			GD.PrintErr($"File not found: {cardDataFilePath}");
		}
	}

	private void InitializeCards()
	{
		// Create an array of indices for the card positions
		cardIndices = new int[cardButtons.Count];
		for (int i = 0; i < cardIndices.Length; i++)
		{
			cardIndices[i] = i;
		}

		// Shuffle the card indices
		ShuffleIndicies(cardIndices);

		// Assign the cards to the buttons
		for (int i = 0; i < cardButtons.Count; i++)
		{
			int cardIndex = cardIndices[i];
			var card = cards[cardIndex];

			var cardButton = cardButtons[i];
			cardButton.Connect("pressed", this, nameof(OnCardButtonPressed), new Array { card, cardButton });

			GD.Print($"Assigned card {card["name"]} to button {i}");
		}
	}

	private void ShuffleIndicies(int[] array)
	{
		Random random = new Random();
		for (int i = array.Length - 1; i > 0; i--)
		{
			int j = random.Next(i + 1);
			int temp = array[i];
			array[i] = array[j];
			array[j] = temp;
		}
	}


	private async void OnCardButtonPressed(Dictionary card, TextureButton cardButton)
	{
		GD.Print($"Card {card["name"]} of type {card["type"]} pressed");

		if (card1Id < 0)
		{
			// First card is selected
			card1Id = cards.IndexOf(card);
			card1Button = cardButton;
			cardButton.TextureNormal = (Texture)GD.Load(card["path"].ToString());
			cardButton.TextureHover = null;
			cardButton.TexturePressed = null;
			cardButton.Disabled = true;
		}
		else
		{
			DisableAllButtons();
			// Second card is selected
			cardButton.TextureNormal = (Texture)GD.Load(card["path"].ToString());
			cardButton.TextureHover = null;
			cardButton.TexturePressed = null;

			if (card1Id != cards.IndexOf(card) && cards[card1Id]["name"].ToString() == card["name"].ToString())
			{
				// Cards match
				GD.Print("Match found!");
				cardButton.Disabled = true;
				disabledButtons.Add(cardButton);
				card1Button.Disabled = true;
				disabledButtons.Add(card1Button);
				card1Id = -1;
				card1Button = null;
				EnableAllButtons();
			}
			else
			{
				// Cards do not match
				GD.Print("No match. Hiding cards...");
				await ToSignal(GetTree().CreateTimer(1), "timeout");
				
				cardButton.TextureNormal = cardBackTexture;
				card1Button.TextureNormal = cardBackTexture;
				cardButton.TexturePressed = cardBackTexturePressed;
				card1Button.TexturePressed = cardBackTexturePressed;
				cardButton.TextureHover = cardBackTextureHover;
				card1Button.TextureHover = cardBackTextureHover;
				
				card1Id = -1;
				card1Button = null;
				EnableAllButtons();
			}
		}
	}
	
	private void DisableAllButtons()
	{
		foreach (var button in cardButtons)
		{
			button.Disabled = true;
		}
	}

	private void EnableAllButtons()
	{
		foreach (var button in cardButtons)
		{
			if (!disabledButtons.Contains(button))
			{
				button.Disabled = false;
			}
		}
	}
}
