using Godot;
using System;

public class MoneyCounter : Control
{
	private Label _label;

	public override void _Ready()
	{
		// Pobierz referencję do Label
		_label = GetNode<Label>("Label");

		// Ustaw początkową wartość licznika monet
		UpdateMoneyCounter();

		// Podłącz sygnał zmiany wartości monet
		Global global = (Global)GetNode("/root/Global");
		global.Connect("MoneyChanged", this, nameof(OnMoneyChanged));
	}

	private void UpdateMoneyCounter()
	{
		Global global = (Global)GetNode("/root/Global");
		_label.Text = $"{global.Money} nok";
	}

	private void OnMoneyChanged(int newMoney)
	{
		UpdateMoneyCounter();
	}
}
