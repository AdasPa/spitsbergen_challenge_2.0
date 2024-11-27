using Godot;

public class Product : Panel
{
	[Export] public string ProductName { get; set; }
	[Export] public int Price { get; set; }
	[Export] public Texture ProductImage { get; set; }
	[Export] public bool IsLocked { get; set; }
	[Export] public bool IsOutOfOrder { get; set; }

	private Label nameLabel;
	private Label priceLabel;
	private TextureRect productImage;
	private TextureRect coin;
	private TextureButton buyButton;
	private Control lockOverlay;
	private Control outOfOrderOverlay;
	private Sklep sklep;

	public override void _Ready()
	{
		nameLabel = GetNode<Label>("NameLabel");
		priceLabel = GetNode<Label>("PriceLabel");
		productImage = GetNode<TextureRect>("ProductImage");
		coin = GetNode<TextureRect>("Coin");
		buyButton = GetNode<TextureButton>("BuyButton");
		lockOverlay = GetNode<Control>("LockOverlay");
		outOfOrderOverlay = GetNode<Control>("OutOfOrderOverlay");
		
		buyButton.Connect("pressed", this, nameof(Buy));

		//SetProductInfo("essa", 69, GD.Load<Texture>("res://assets/products/bielizna_termiczna.png"), false, false);

		//
		// TODO: setLocked, setUnlocked, setOOO
		//  przygotować ekrany wyprzedanych produktów
		//  ogarnąć kupowanie
		//
		UpdateProductInfo();
	}

	public void SetSklep(Sklep sklepInstance)
	{
		sklep = sklepInstance;
	}
	public void UpdateProductInfo()
	{
		nameLabel.Text = ProductName;
		priceLabel.Text = $"{Price} nok";
		productImage.Texture = ProductImage;
		lockOverlay.Visible = IsLocked;
		outOfOrderOverlay.Visible = IsOutOfOrder;
		buyButton.Disabled = IsLocked || IsOutOfOrder;

		Global global = (Global)GetNode("/root/Global");
		if(global.Money < Price)
		{
			// Ustaw kolor tekstu na czerwony
			priceLabel.AddColorOverride("font_color", new Color(1, 0, 0)); // RGB dla czerwonego
		}
		else
		{
			// Przywróć domyślny kolor tekstu
			priceLabel.AddColorOverride("font_color", new Color(1, 1, 1)); // RGB dla białego
		}
	}

	public void SetProductInfo(string name, int price, Texture image, bool isLocked, bool isOutOfOrder)
	{
		ProductName = name;
		Price = price;
		ProductImage = image;
		IsLocked = isLocked;
		IsOutOfOrder = isOutOfOrder;
		UpdateProductInfo();
	}
	public void OutOfOrder()
	{
		IsOutOfOrder = true;

		buyButton.Visible = false;
		coin.Visible = false;
		priceLabel.Visible = false;
		outOfOrderOverlay.Visible = true;


		UpdateProductInfo();
	}

	public void Unlock()
	{
		IsLocked = false;

		buyButton.Visible = true;
		coin.Visible = true;
		priceLabel.Visible = true;

		UpdateProductInfo();
	}

	public void Lock()
	{
		IsLocked = true;

		buyButton.Visible = false;
		coin.Visible = false;
		priceLabel.Visible = false;
		lockOverlay.Visible = true;

		UpdateProductInfo();
	}

	public void Buy()
	{
		if (!IsLocked && !IsOutOfOrder)
		{
			Global global = (Global)GetNode("/root/Global");
			if (global.Money >= Price)
			{
				global.AddMoney(-Price);
				OutOfOrder();
				UpdateProductInfo();
				var productInfo = new Godot.Collections.Dictionary
				{
					{ "name", ProductName },
					{ "price", Price },
					{ "image", ProductImage },
					{ "isLocked", IsLocked },
					{ "isOutOfOrder", IsOutOfOrder }
				};
				GD.Print(productInfo);
				UpdateGlobalProductList(productInfo);
				sklep.UpdateAllProductsInfo();
			}
		}
	}

	private void UpdateGlobalProductList(Godot.Collections.Dictionary updatedProduct)
	{
		Global global = (Global)GetNode("/root/Global");
		for (int i = 0; i < global.ProductList.Count; i++)
		{
			GD.Print("e");
			if (global.ProductList[i]["name"].ToString() == updatedProduct["name"].ToString())
			{
				global.ProductList[i] = updatedProduct;
				GD.Print(updatedProduct["name"].ToString());
				global.Save();
				break;
			}
		}
	}

}
