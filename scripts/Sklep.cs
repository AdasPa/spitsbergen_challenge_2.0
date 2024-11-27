using Godot;
using System.Collections.Generic;

using Dictionary = Godot.Collections.Dictionary;

public class Sklep : Control
{
	public PackedScene ProductScene;

	private GridContainer productGridContainer;
	//private List<Dictionary> productList;

	public override void _Ready()
	{

		//
		//	BUG: scroll nie kończy się równo z ostatnim produktem 
		//	ostatni produkt tak naprawdę schowany
		//	można to próbować spacerami
		//

		var global = (Global)GetNode("/root/Global");


		productGridContainer = GetNode<GridContainer>("ScrollContainer/ProductGridContainer");

		ProductScene = (PackedScene)ResourceLoader.Load("res://scenes/Product.tscn");
		
		var backButton = GetNode<TextureButton>("Description/BackButton");
		backButton.Connect("pressed", this, nameof(OnBackButtonPressed));

		// Przykładowe dane produktów
		// w sumie ~2150
		// productList = new List<Dictionary>
		// {
		// 	new Dictionary { { "name", "Bielizna termiczna" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/bielizna_termiczna.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Buty" }, { "price", 100 }, { "image", GD.Load<Texture>("res://assets/products/buty.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Lioflizaty" }, { "price", 200 }, { "image", GD.Load<Texture>("res://assets/products/liofilizaty.png") }, { "isLocked", true }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Skarpetki" }, { "price", 20 }, { "image", GD.Load<Texture>("res://assets/products/skarpetki.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Bidon" }, { "price", 30 }, { "image", GD.Load<Texture>("res://assets/products/bidon.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Broń" }, { "price", 400 }, { "image", GD.Load<Texture>("res://assets/products/bron.png") }, { "isLocked", true }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Kurtka wodoodporna" }, { "price", 120 }, { "image", GD.Load<Texture>("res://assets/products/kurtka_wodoodporna.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Czapka" }, { "price", 15 }, { "image", GD.Load<Texture>("res://assets/products/czapka.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Namiot" }, { "price", 200 }, { "image", GD.Load<Texture>("res://assets/products/namiot.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Jetboil" }, { "price", 100 }, { "image", GD.Load<Texture>("res://assets/products/jetboil.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Termos" }, { "price", 30 }, { "image", GD.Load<Texture>("res://assets/products/termos.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Kubek" }, { "price", 10 }, { "image", GD.Load<Texture>("res://assets/products/kubek.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Kurtka puchowa" }, { "price", 100 }, { "image", GD.Load<Texture>("res://assets/products/kurtka_puchowa.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Łódź" }, { "price", 300 }, { "image", GD.Load<Texture>("res://assets/products/lodz.png") }, { "isLocked", true }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Materac" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/materac.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Rękawiczki" }, { "price", 30 }, { "image", GD.Load<Texture>("res://assets/products/rekawiczki.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Sztućce" }, { "price", 10 }, { "image", GD.Load<Texture>("res://assets/products/sztucce.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Gaz" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/gas.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Śpiwór" }, { "price", 150 }, { "image", GD.Load<Texture>("res://assets/products/spiwor.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Spodnie" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/spodnie.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
		// 	new Dictionary { { "name", "Amunicja" }, { "price", 150 }, { "image", GD.Load<Texture>("res://assets/products/amunicja.png") }, { "isLocked", true }, { "isOutOfOrder", false } }
		// };

		
		LoadProducts();
	}

	private void LoadProducts()
	{
		var global = (Global)GetNode("/root/Global");

		foreach (var productInfo in global.ProductList)
		{
			//248 664
			Product productInstance = (Product)ProductScene.Instance();
			productGridContainer.AddChild(productInstance);
			productInstance.SetSklep(this);

			int columns = productGridContainer.Columns; // Liczba kolumn w GridContainer
			int totalItems = global.ProductList.Count;  // Całkowita liczba produktów
			int currentIndex = productGridContainer.GetChildCount(); // Liczba dzieci w GridContainer


			var columnSpacer = new ColorRect();  // Używamy ColorRect zamiast Control

			if (currentIndex % columns == 1)  // Jeśli to pierwsza kolumna
			{
				columnSpacer.RectMinSize = new Vector2(655, 0); // Tylko szerokość, brak wpływu na wysokość
				columnSpacer.Color = new Color(0.3f, 0.9f, 0.1f, 0.0f); 
				//columnSpacer.SizeFlagsHorizontal = (int)Control.SizeFlags.Fill;  // Rozciąganie na szerokość
				//columnSpacer.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkCenter;  // Dostosowanie do wysokości
				productGridContainer.AddChild(columnSpacer);
			}
			if(currentIndex % columns == 3)
			{
				if(currentIndex <= 2*totalItems - 4)
					columnSpacer.RectMinSize = new Vector2(0, 248); // Tylko wysokość, brak wpływu na szerokość
				else
					columnSpacer.RectMinSize = new Vector2(0, 200);
				//columnSpacer.SizeFlagsHorizontal = (int)Control.SizeFlags.ShrinkCenter; // Dostosowanie do szerokości
				//columnSpacer.SizeFlagsVertical = (int)Control.SizeFlags.Fill; // Rozciąganie na wysokość
				columnSpacer.Color = new Color(1.0f, 0.5f, 0.3f, 0.0f);  // Pomarańczowy
				productGridContainer.AddChild(columnSpacer);
			}
			

			// Ustawienie koloru tła na pomarańczowy
			





			string name = productInfo["name"].ToString();
			int price;
			if (productInfo["price"] is int priceInt)
			{
				price = priceInt;
			}
			else if (productInfo["price"] is long priceLong)
			{
				price = (int)priceLong;
			}
			else if (productInfo["price"] is float priceFloat)
			{
				price = (int)priceFloat;
			}
			else
			{
				GD.PrintErr("Invalid price value in save file");
				price = 69;
			}
			
			var pName = productInfo["name"].ToString();
			GD.Print(global.productImages[pName]);
			Texture image = GD.Load<Texture>(global.productImages[pName]);
			bool isLocked = (bool)productInfo["isLocked"];
			bool isOutOfOrder = (bool)productInfo["isOutOfOrder"];

			productInstance.SetProductInfo(name, price, image, isLocked, isOutOfOrder);

			if ((bool)productInfo["isOutOfOrder"])
			{
				productInstance.OutOfOrder();
			}
			else if ((bool)productInfo["isLocked"])
			{
				productInstance.Lock();
			}
		}

	}
	public void UpdateAllProductsInfo()
	{
		bool isEverythingOutOfOrder = true;

		foreach (Product product in productGridContainer.GetChildren())
		{
			product.UpdateProductInfo();
			if(!product.IsOutOfOrder)
				isEverythingOutOfOrder = false;
		}
		if(isEverythingOutOfOrder)
			GetTree().ChangeScene("res://scenes/GameOverScene1.tscn");
	}

	
	private void OnBackButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/MainScene.tscn");
	}
}
