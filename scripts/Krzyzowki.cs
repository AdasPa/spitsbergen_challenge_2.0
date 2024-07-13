using Godot;
using System;

public class Krzyzowki : Node
{
	private TextureButton krzyzowka1Button;
	private TextureButton krzyzowka2Button;
	private TextureButton krzyzowka3Button;
	private TextureButton backButton;
	private Label moneyCounter;

	public override void _Ready()
	{
		// Pobierz referencje do przycisków
		krzyzowka1Button = GetNode<TextureButton>("Krzyzowka01Button");
		krzyzowka2Button = GetNode<TextureButton>("Krzyzowka02Button");
		krzyzowka3Button = GetNode<TextureButton>("Krzyzowka03Button");
		backButton = GetNode<TextureButton>("Description/BackButton");
		moneyCounter = GetNode<Label>("MoneyCounter/Label");

		// Ustaw początkową wartość licznika monet
		UpdateMoneyCounter();

		// Podłącz zdarzenia przycisków
		krzyzowka1Button.Connect("pressed", this, nameof(OnKrzyzowka1ButtonPressed));
		krzyzowka2Button.Connect("pressed", this, nameof(OnKrzyzowka2ButtonPressed));
		krzyzowka3Button.Connect("pressed", this, nameof(OnKrzyzowka3ButtonPressed));
		backButton.Connect("pressed", this, nameof(OnBackButtonPressed));

		// Podłącz sygnał zmiany wartości monet
		Global global = (Global)GetNode("/root/Global");
		global.Connect("MoneyChanged", this, nameof(OnMoneyChanged));
	}

	private void UpdateMoneyCounter()
	{
		Global global = (Global)GetNode("/root/Global");
		moneyCounter.Text = $"{global.Money} nok";
	}

	private void OnMoneyChanged(int newMoney)
	{
		UpdateMoneyCounter();
	}

	private void OnKrzyzowka1ButtonPressed()
	{
		// Przejdź do sceny Krzyzowka_01
		GetTree().ChangeScene("res://scenes/Krzyzowka_01.tscn");
	}

	private void OnKrzyzowka2ButtonPressed()
	{
		// Przejdź do sceny Krzyzowka_02
		GetTree().ChangeScene("res://scenes/Krzyzowka_02.tscn");
	}

	private void OnKrzyzowka3ButtonPressed()
	{
		// Przejdź do sceny Krzyzowka_03
		GetTree().ChangeScene("res://scenes/Krzyzowka_03.tscn");
	}

	private void OnBackButtonPressed()
	{
		// Przejdź do poprzedniej sceny lub wykonaj inną akcję
		GetTree().ChangeScene("res://scenes/Zadania.tscn");
	}
}
