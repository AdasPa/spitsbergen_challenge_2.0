using Godot;
using System;

public class BazaWiedzy : Node
{
	private TextureButton historiaButton;
	private TextureButton geografiaButton;
	private TextureButton zwierzetaButton;
	private TextureButton naukaButton;
	private TextureButton roslinyButton;
	private TextureButton genetykaButton;
	private TextureButton backButton;
	private Label moneyCounter;

	public override void _Ready()
	{
		// Pobierz referencje do przycisków
		historiaButton = GetNode<TextureButton>("Sprite/HistoriaButton");
		geografiaButton = GetNode<TextureButton>("Sprite/GeografiaButton");
		zwierzetaButton = GetNode<TextureButton>("Sprite/ZwierzetaButton");
		naukaButton = GetNode<TextureButton>("Sprite/NaukaButton");
		roslinyButton = GetNode<TextureButton>("Sprite/RoslinyButton");
		genetykaButton = GetNode<TextureButton>("Sprite/TextureButton");
		backButton = GetNode<TextureButton>("Sprite/BackButton");
		moneyCounter = GetNode<Label>("Control/Label");

		// Ustaw początkową wartość licznika monet
		UpdateMoneyCounter();
		
		
		// Podłącz zdarzenia przycisków
		historiaButton.Connect("pressed", this, nameof(OnHistoriaButtonPressed));
		geografiaButton.Connect("pressed", this, nameof(OnGeografiaButtonPressed));
		zwierzetaButton.Connect("pressed", this, nameof(OnZwierzetaButtonPressed));
		naukaButton.Connect("pressed", this, nameof(OnNaukaButtonPressed));
		roslinyButton.Connect("pressed", this, nameof(OnRoslinyButtonPressed));
		genetykaButton.Connect("pressed", this, nameof(OnGenetykaButtonPressed));
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

	private void OnHistoriaButtonPressed()
	{
		// Przejdź do sceny BazaHistoria
		GetTree().ChangeScene("res://scenes/BazaHistoria.tscn");
	}

	private void OnGeografiaButtonPressed()
	{
		// Przejdź do sceny BazaGeografia
		GetTree().ChangeScene("res://scenes/BazaGeografia.tscn");
	}

	private void OnZwierzetaButtonPressed()
	{
		// Przejdź do sceny BazaZwierzeta
		GetTree().ChangeScene("res://scenes/BazaZwierzeta.tscn");
	}

	private void OnNaukaButtonPressed()
	{
		// Przejdź do sceny BazaNauka
		GetTree().ChangeScene("res://scenes/BazaNauka.tscn");
	}

	private void OnRoslinyButtonPressed()
	{
		// Przejdź do sceny BazaRosliny
		GetTree().ChangeScene("res://scenes/BazaRosliny.tscn");
	}

	private void OnGenetykaButtonPressed()
	{
		// Przejdź do sceny BazaGenetyka
		GetTree().ChangeScene("res://scenes/BazaGenetyka.tscn");
	}

	private void OnBackButtonPressed()
	{
		// Przejdź do poprzedniej sceny lub wykonaj inną akcję
		GetTree().ChangeScene("res://scenes/MainScene.tscn");
	}
}
