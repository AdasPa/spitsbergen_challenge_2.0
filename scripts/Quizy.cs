using Godot;
using System;

public class Quizy : Node
{
	private TextureButton historiaButton;
	private TextureButton geografiaButton;
	private TextureButton zwierzetaButton;
	private TextureButton naukaButton;
	private TextureButton roslinyButton;
	private TextureButton ciekawostkiButton;
	private TextureButton backButton;
	private Label moneyCounter;

	public override void _Ready()
	{
		// Pobierz referencje do przycisków
		historiaButton = GetNode<TextureButton>("HistoriaButton");
		geografiaButton = GetNode<TextureButton>("GeografiaButton");
		zwierzetaButton = GetNode<TextureButton>("ZwierzetaButton");
		naukaButton = GetNode<TextureButton>("NaukaButton");
		roslinyButton = GetNode<TextureButton>("RoslinyButton");
		ciekawostkiButton = GetNode<TextureButton>("CiekawostkiButton");
		backButton = GetNode<TextureButton>("Description/BackButton");
		moneyCounter = GetNode<Label>("Control/Label");

		// Ustaw początkową wartość licznika monet
		UpdateMoneyCounter();
		
		
		// Podłącz zdarzenia przycisków
		historiaButton.Connect("pressed", this, nameof(OnHistoriaButtonPressed));
		geografiaButton.Connect("pressed", this, nameof(OnGeografiaButtonPressed));
		zwierzetaButton.Connect("pressed", this, nameof(OnZwierzetaButtonPressed));
		naukaButton.Connect("pressed", this, nameof(OnNaukaButtonPressed));
		roslinyButton.Connect("pressed", this, nameof(OnRoslinyButtonPressed));
		ciekawostkiButton.Connect("pressed", this, nameof(OnCiekawostkiButtonPressed));
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
		GetTree().ChangeScene("res://scenes/QuizHistoria.tscn");
	}

	private void OnGeografiaButtonPressed()
	{
		// Przejdź do sceny BazaGeografia
		GetTree().ChangeScene("res://scenes/QuizGeografia.tscn");
	}

	private void OnZwierzetaButtonPressed()
	{
		// Przejdź do sceny BazaZwierzeta
		GetTree().ChangeScene("res://scenes/QuizZwierzeta.tscn");
	}

	private void OnNaukaButtonPressed()
	{
		// Przejdź do sceny BazaNauka
		GetTree().ChangeScene("res://scenes/QuizNauka.tscn");
	}

	private void OnRoslinyButtonPressed()
	{
		// Przejdź do sceny BazaRosliny
		GetTree().ChangeScene("res://scenes/QuizRosliny.tscn");
	}

	private void OnCiekawostkiButtonPressed()
	{
		// Przejdź do sceny BazaCiekawostki
		GetTree().ChangeScene("res://scenes/QuizCiekawostki.tscn");
	}

	private void OnBackButtonPressed()
	{
		// Przejdź do poprzedniej sceny lub wykonaj inną akcję
		GetTree().ChangeScene("res://scenes/Zadania.tscn");
	}
}
