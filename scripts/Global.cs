using Godot;
using System;

public class Global : Node
{
	[Signal]
	public delegate void MoneyChanged(int newMoney);

	private int money = 1000;

	public int Money
	{
		get => money;
		set
		{
			money = value;
			EmitSignal(nameof(MoneyChanged), money);
		}
	}

	public override void _Ready()
	{
		if (ProjectSettings.HasSetting("application/config/money"))
		{
			Money = int.Parse(ProjectSettings.GetSetting("application/config/money").ToString());
		}
		else
		{
			Money = 1000;
		}
	}

	public void SaveMoney()
	{
		ProjectSettings.SetSetting("application/config/money", Money.ToString());
		ProjectSettings.Save();
	}
	
	public void AddMoney(int amount)
	{
		Money += amount;
		SaveMoney();
	}

	// public void ResetProgress()
	// {

	// }
}
