using Godot;
using System;

public class MainScene : Node
{
	private Label moneyCounter;
	private TextureButton bazaWiedzyButton;
	private TextureButton zadaniaButton;
	private TextureButton sklepButton;

	public override void _Ready()
	{
		// Pobierz referencje do elementów UI
		moneyCounter = GetNode<Label>("Control2/Label");
		bazaWiedzyButton = GetNode<TextureButton>("Control/Panel/BazaWiedzyButton");
		zadaniaButton = GetNode<TextureButton>("Control/Panel/ZadaniaButton");
		sklepButton = GetNode<TextureButton>("Control/Panel/SklepButton");

		// Zaktualizuj licznik monet na starcie
		UpdateMoneyCounter();
		
		// Podłącz zdarzenia przycisków
		bazaWiedzyButton.Connect("pressed", this, nameof(OnBazaWiedzyButtonPressed));
		zadaniaButton.Connect("pressed", this, nameof(OnZadaniaButtonPressed));
		sklepButton.Connect("pressed", this, nameof(OnSklepButtonPressed));

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

	private void OnBazaWiedzyButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/BazaWiedzy.tscn");
	}

	private void OnZadaniaButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Zadania.tscn");
	}

	private void OnSklepButtonPressed()
	{
		// Pobierz instancję globalnego singletona
		Global global = (Global)GetNode("/root/Global");

		// Dodaj 100 monet
		global.AddMoney(100);
		//GetTree().ChangeScene("res://ShopScene.tscn");
	}
}
