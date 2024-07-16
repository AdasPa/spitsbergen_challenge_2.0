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

	public override void _Ready()
	{
		nameLabel = GetNode<Label>("NameLabel");
		priceLabel = GetNode<Label>("PriceLabel");
		productImage = GetNode<TextureRect>("ProductImage");
		coin = GetNode<TextureRect>("Coin");
		buyButton = GetNode<TextureButton>("BuyButton");
		lockOverlay = GetNode<Control>("LockOverlay");
		outOfOrderOverlay = GetNode<Control>("OutOfOrderOverlay");
		
		//SetProductInfo("essa", 69, GD.Load<Texture>("res://assets/products/bielizna_termiczna.png"), false, false);

		//
		// TODO: setLocked, setUnlocked, setOOO
		//  przygotować ekrany wyprzedanych produktów
		//  ogarnąć kupowanie
		//
		UpdateProductInfo();
	}

	private void UpdateProductInfo()
	{
		nameLabel.Text = ProductName;
		priceLabel.Text = $"{Price} nok";
		productImage.Texture = ProductImage;
		lockOverlay.Visible = IsLocked;
		outOfOrderOverlay.Visible = IsOutOfOrder;
		buyButton.Disabled = IsLocked || IsOutOfOrder;
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

		UpdateProductInfo();
	}
}
