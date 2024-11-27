using Godot;
using System;
using System.Collections.Generic;

using Dictionary = Godot.Collections.Dictionary;

public class Global : Node
{
	[Signal]
	public delegate void MoneyChanged(int newMoney);

	private int money = 100;

	/// 
	///
	/// TODO:
	/// Po ponownym otwarciu gry wraca na 100 noków.
	/// Animacje produktow zablokowanych
	/// Scroll w bazie wedzyss
	///


	///
	//
	//	Skończone na:
	//	Animacje outro dodane, sklep po wyprzedaniu przekierowuje do outro
	//	
	//
	///



	public List<Dictionary> ProductList;

	public Dictionary<string, string> productImages;
	public Dictionary<string, int> HighScores;



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
	{	InitializeProductImagesList();
		Load(); // Load game state when the game starts
	}

	public void Save()
	{
		GD.Print("Saving game...");
		//GD.Print(ProductList);
		if (ProductList == null )
		{
			InitializeDefaultProductList();
		}
		
		var godotProductArray = new Godot.Collections.Array();
		foreach (var product in ProductList)
		{
			godotProductArray.Add(product);
		}

		var highScoresDict = new Godot.Collections.Dictionary();
		foreach (var keyValuePair in HighScores)
		{
			highScoresDict[keyValuePair.Key] = keyValuePair.Value;
		}

		var saveData = new Godot.Collections.Dictionary
		{
			{"money", Money},
			{"productList", godotProductArray},
			{"highscores", highScoresDict}
		};

		//
		// TODO zapisywanie highscorea
		//
		//

		GD.Print("Save data: ", JSON.Print(saveData["highscores"]));

		var file = new File();
		var error = file.Open("user://savegame.save", File.ModeFlags.Write);
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to open save file: ", error);
			return;
		}
		file.StoreLine(JSON.Print(saveData));
		file.Close();
		GD.Print("Game saved successfully.");
	}

	public void Load()
	{
		GD.Print("Loading game...");

		var file = new File();
		if (file.FileExists("user://savegame.save"))
		{
			var error = file.Open("user://savegame.save", File.ModeFlags.Read);
			if (error != Error.Ok)
			{
				GD.PrintErr("Failed to open save file: ", error);
				return;
			}

			var jsonResult = JSON.Parse(file.GetLine());
			if (jsonResult.Error != Error.Ok)
			{
				GD.PrintErr("Failed to parse save file: ", jsonResult.ErrorString);
				file.Close();
				return;
			}

			var saveData = (Godot.Collections.Dictionary)jsonResult.Result;
			//GD.Print("Loaded save data: ", saveData);
			file.Close();

			if (saveData.Contains("money"))
			{
				GD.Print(saveData["money"].GetType());
				if (saveData["money"] is int moneyInt)
				{
					Money = moneyInt;
				}
				else if (saveData["money"] is long moneyLong)
				{
					Money = (int)moneyLong;
				}
				else if (saveData["money"] is float moneyFloat)
				{
					Money = (int)moneyFloat;
				}
				else
				{
					GD.PrintErr("Invalid money value in save file");
					Money = 100;
				}
			}

			if (saveData.Contains("productList") && saveData["productList"] != null)
			{
				var productArray = (Godot.Collections.Array)saveData["productList"];
				ProductList = new List<Dictionary>();

				foreach (var product in productArray)
				{
					ProductList.Add((Dictionary)product);
				}
			}
			else
			{
				InitializeDefaultProductList();
			}

			if (saveData.Contains("highscores") && saveData["highscores"] != null)
			{
				var highScoresDict = (Godot.Collections.Dictionary)saveData["highscores"];
				HighScores = new Dictionary<string, int>();

				foreach (string key in highScoresDict.Keys)
				{
					HighScores[key] = (int)(float)highScoresDict[key];
				}
			}
			else
			{
				InitializeHighScores();
			}

			// Load other game states here in the future
			GD.Print("Game loaded successfully.");
		}
		else
		{
			GD.Print("Save file does not exist. Setting default values.");
			Money = 100;
			InitializeDefaultProductList();
		}
	}

	public void SetHighScore(string gameName, int score)
	{
		if (HighScores.ContainsKey(gameName))
		{
			if (score > HighScores[gameName])
			{
				HighScores[gameName] = score;
				Save();
			}
		}
		else
		{
			HighScores[gameName] = score;
			Save();
		}
	}

	public int GetHighScore(string gameName)
	{
		if (HighScores.ContainsKey(gameName))
		{
			return HighScores[gameName];
		}
		return 0;
	}

	private void InitializeDefaultProductList()
	{
		// Initialize the default product list
		ProductList = new List<Dictionary>
		{
			new Dictionary { { "name", "Bielizna termiczna" }, { "price", 50 }, { "image", "res://assets/products/bielizna_termiczna.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Buty" }, { "price", 100 }, { "image", "res://assets/products/buty.png" }, { "isLocked", false }, { "isOutOfOrder", false } },  //should be locked
			new Dictionary { { "name", "Lioflizaty" }, { "price", 200 }, { "image", "res://assets/products/liofilizaty.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Skarpetki" }, { "price", 20 }, { "image", "res://assets/products/skarpetki.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Bidon" }, { "price", 30 }, { "image", "res://assets/products/bidon.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Broń" }, { "price", 400 }, { "image", "res://assets/products/bron.png" }, { "isLocked", false }, { "isOutOfOrder", false } },  //should be locked
			new Dictionary { { "name", "Kurtka wodoodporna" }, { "price", 120 }, { "image", "res://assets/products/kurtka_wodoodporna.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Czapka" }, { "price", 15 }, { "image", "res://assets/products/czapka.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Namiot" }, { "price", 200 }, { "image", "res://assets/products/namiot.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Jetboil" }, { "price", 100 }, { "image", "res://assets/products/jetboil.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Termos" }, { "price", 30 }, { "image", "res://assets/products/termos.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Kubek" }, { "price", 10 }, { "image", "res://assets/products/kubek.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Kurtka puchowa" }, { "price", 100 }, { "image", "res://assets/products/kurtka_puchowa.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Materac" }, { "price", 50 }, { "image", "res://assets/products/materac.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Rękawiczki" }, { "price", 30 }, { "image", "res://assets/products/rekawiczki.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Sztućce" }, { "price", 10 }, { "image", "res://assets/products/sztucce.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Spodnie" }, { "price", 50 }, { "image", "res://assets/products/spodnie.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Amunicja" }, { "price", 150 }, { "image", "res://assets/products/amunicja.png" }, { "isLocked", false }, { "isOutOfOrder", false } }, // should be locked
			new Dictionary { { "name", "Gaz" }, { "price", 50 }, { "image", "res://assets/products/gas.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Śpiwór" }, { "price", 150 }, { "image", "res://assets/products/spiwor.png" }, { "isLocked", false }, { "isOutOfOrder", false } },
			new Dictionary { { "name", "Łódź" }, { "price", 300 }, { "image", "res://assets/products/lodz.png" }, { "isLocked", false }, { "isOutOfOrder", true } } // is invisible on the screen, in OutOfOrder by default, solves problem of the invisible last row 
			};
	}
	private void InitializeProductImagesList()
	{
		productImages = new Dictionary<string, string>
		{
			{ "Bielizna termiczna", "res://assets/products/bielizna_termiczna.png" },
			{ "Buty", "res://assets/products/buty.png" },
			{ "Lioflizaty", "res://assets/products/liofilizaty.png" },
			{ "Skarpetki", "res://assets/products/skarpetki.png" },
			{ "Bidon", "res://assets/products/bidon.png" },
			{ "Broń", "res://assets/products/bron.png" },
			{ "Kurtka wodoodporna", "res://assets/products/kurtka_wodoodporna.png" },
			{ "Czapka", "res://assets/products/czapka.png" },
			{ "Namiot", "res://assets/products/namiot.png" },
			{ "Jetboil", "res://assets/products/jetboil.png" },
			{ "Termos", "res://assets/products/termos.png" },
			{ "Kubek", "res://assets/products/kubek.png" },
			{ "Kurtka puchowa", "res://assets/products/kurtka_puchowa.png" },
			{ "Materac", "res://assets/products/materac.png" },
			{ "Rękawiczki", "res://assets/products/rekawiczki.png" },
			{ "Sztućce", "res://assets/products/sztucce.png" },
			{ "Gaz", "res://assets/products/gas.png" },
			{ "Śpiwór", "res://assets/products/spiwor.png" },
			{ "Spodnie", "res://assets/products/spodnie.png" },
			{ "Amunicja", "res://assets/products/amunicja.png" },
			{ "Łódź", "res://assets/products/lodz.png" }
		};
	}

	private void InitializeHighScores()
	{
		HighScores = new Dictionary<string, int>
		{
			
		};
	}


	public void AddMoney(int amount)
	{
		Money += amount;
		GD.Print(amount);
		Save(); // Save the updated game state
	}

	public void ResetProgress()
	{
		Money = 100;
		InitializeDefaultProductList();
		InitializeHighScores();
		Save();
	}

	public bool IsFirstRun()
	{
		return HighScores == null || HighScores.Count == 0;
	}

}
