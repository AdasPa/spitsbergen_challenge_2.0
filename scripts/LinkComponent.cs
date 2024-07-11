using Godot;
using System;

public class LinkComponent : Control
{
	private ColorRect linkPanel;
	private Label linkTitle;
	private Label linkDescription;
	private TextureButton expandButton;

	private string _linkTitle;
	private string _linkDescription;
	private string _linkURL;

	public string LinkTitle
	{
		get => _linkTitle;
		set
		{
			_linkTitle = value;
			if (linkTitle != null)
			{
				GD.Print("Setting LinkTitle to: ", value);
				linkTitle.Text = value;
			}
		}
	}

	public string LinkDescription
	{
		get => _linkDescription;
		set
		{
			_linkDescription = value;
			if (linkDescription != null)
			{
				GD.Print("Setting LinkDescription to: ", value);
				linkDescription.Text = value;
			}
		}
	}

	public string LinkURL
	{
		get => _linkURL;
		set
		{
			_linkURL = value;
			if (!string.IsNullOrEmpty(_linkURL))
			{
				GD.Print("Setting LinkURL to: ", value);
			}
		}
	}


	public override void _Ready()
	{
		linkPanel = GetNode<ColorRect>("LinkPanel");
		linkTitle = GetNode<Label>("LinkPanel/Title");
		linkDescription = GetNode<Label>("LinkPanel/Description");
		expandButton = GetNode<TextureButton>("LinkPanel/OpenButton");

		// Podłącz zdarzenia
		linkPanel.Connect("mouse_entered", this, nameof(OnMouseEntered));
		linkPanel.Connect("mouse_exited", this, nameof(OnMouseExited));
		linkPanel.Connect("gui_input", this, nameof(OnPanelClicked));
		expandButton.Connect("pressed", this, nameof(OnExpandButtonPressed));
		
		GD.Print("LinkComponent _Ready completed.");

		// Ustawienie wartości po zainicjalizowaniu komponentu
		UpdateLink();
	}
	
	private void UpdateLink()
	{
		LinkTitle = _linkTitle;
		LinkDescription = _linkDescription;
		LinkURL = _linkURL;
	}

	private void OnMouseEntered()
	{
		linkPanel.Color = new Color(0.47f, 0.92f, 0.93f, 0.5f); // Zmiana koloru tła na jaśniejszy odcień
	}

	private void OnMouseExited()
	{
		linkPanel.Color = new Color(1, 1, 1, 0.2f); // Powrót do oryginalnego koloru tła
	}

	private void OnPanelClicked(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == (int)ButtonList.Left)
			{
				OpenURL();
			}
		}
	}

	private void OnExpandButtonPressed()
	{
		OpenURL();
	}

	private void OpenURL()
	{
		if (!string.IsNullOrEmpty(LinkURL))
		{
			OS.ShellOpen(LinkURL);
		}
		else
		{
			GD.Print("Link URL is not set.");
		}
	}
}
