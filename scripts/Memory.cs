using Godot;
using System;

public class Memory : Node
{
	private TextureButton ssakiButton;
	private TextureButton ptakiButton;
	private TextureButton roslinyButton;
	private TextureButton backButton;
	private Label moneyCounter;

	public override void _Ready()
	{
		// Pobierz referencje do przycisków
		ssakiButton = GetNode<TextureButton>("SsakiButton");
		ptakiButton = GetNode<TextureButton>("PtakiButton");
		roslinyButton = GetNode<TextureButton>("RoslinyButton");
		backButton = GetNode<TextureButton>("Description/BackButton");
		moneyCounter = GetNode<Label>("Control/Label");

		// Ustaw początkową wartość licznika monet
		UpdateMoneyCounter();

		// Podłącz zdarzenia przycisków
		ssakiButton.Connect("pressed", this, nameof(OnSsakiButtonPressed));
		ptakiButton.Connect("pressed", this, nameof(OnPtakiButtonPressed));
		roslinyButton.Connect("pressed", this, nameof(OnRoslinyButtonPressed));
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

	private void OnSsakiButtonPressed()
	{
		// Przejdź do sceny Memoryssakissaki
		GetTree().ChangeScene("res://scenes/MemorySsaki.tscn");
	}

	private void OnPtakiButtonPressed()
	{
		// Przejdź do sceny MemoryPtaki
		GetTree().ChangeScene("res://scenes/MemoryPtaki.tscn");
	}

	private void OnRoslinyButtonPressed()
	{
		// Przejdź do sceny MemoryRosliny
		GetTree().ChangeScene("res://scenes/MemoryRosliny.tscn");
	}

	private void OnBackButtonPressed()
	{
		// Przejdź do poprzedniej sceny lub wykonaj inną akcję
		GetTree().ChangeScene("res://scenes/Zadania.tscn");
	}
}
