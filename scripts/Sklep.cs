using Godot;
using System.Collections.Generic;

using Dictionary = Godot.Collections.Dictionary;

public class Sklep : Control
{
	public PackedScene ProductScene;

	private GridContainer productGridContainer;
	private List<Dictionary> productList;

	public override void _Ready()
	{
		productGridContainer = GetNode<GridContainer>("ScrollContainer/ProductGridContainer");

		ProductScene = (PackedScene)ResourceLoader.Load("res://scenes/Product.tscn");
		
		var backButton = GetNode<TextureButton>("Description/BackButton");
		backButton.Connect("pressed", this, nameof(OnBackButtonPressed));

		// Przykładowe dane produktów
productList = new List<Dictionary>
{
	new Dictionary { { "name", "Bielizna termiczna" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/bielizna_termiczna.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Buty" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/buty.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Lioflizaty" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/liofilizaty.png") }, { "isLocked", true }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Skarpetki" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/skarpetki.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Bidon" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/bidon.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Broń" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/bron.png") }, { "isLocked", true }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Kurtka wodoodporna" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/kurtka_wodoodporna.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Czapka" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/czapka.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Namiot" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/namiot.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Jetboil" }, { "price", 500 }, { "image", GD.Load<Texture>("res://assets/products/jetboil.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Termos" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/termos.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Kubek" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/kubek.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Kurtka puchowa" }, { "price", 300 }, { "image", GD.Load<Texture>("res://assets/products/kurtka_puchowa.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Łódź" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/lodz.png") }, { "isLocked", true }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Materac" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/materac.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Rękawiczki" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/rekawiczki.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Sztućce" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/sztucce.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Gaz" }, { "price", 50 }, { "image", GD.Load<Texture>("res://assets/products/gas.png") }, { "isLocked", false }, { "isOutOfOrder", false } },
	new Dictionary { { "name", "Śpiwór" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/spiwor.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Spodnie" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/spodnie.png") }, { "isLocked", false }, { "isOutOfOrder", true } },
	new Dictionary { { "name", "Amunicja" }, { "price", 0 }, { "image", GD.Load<Texture>("res://assets/products/amunicja.png") }, { "isLocked", true }, { "isOutOfOrder", false } }
};

		
		LoadProducts();
	}

	private void LoadProducts()
	{
		foreach (var productInfo in productList)
		{
			Product productInstance = (Product)ProductScene.Instance();
			productGridContainer.AddChild(productInstance);
			productInstance.SetProductInfo(
				productInfo["name"].ToString(),
				(int)productInfo["price"],
				(Texture)productInfo["image"],
				(bool)productInfo["isLocked"],
				(bool)productInfo["isOutOfOrder"]
			);

			
		}
	}
	
	private void OnBackButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/MainScene.tscn");
	}
}
