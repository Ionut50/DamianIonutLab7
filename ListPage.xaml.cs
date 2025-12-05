using DamianIonutLab7.Models;

//using DamianIonutLab7.Data;

namespace DamianIonutLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e) {
		var slist = (ShopList)BindingContext; 
        slist.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ShopID = selectedShop.ID;
        await App.Database.SaveShopListAsync(slist);
		await Navigation.PopAsync();
	}
    async void OnDeleteButtonClicked(object sender, EventArgs e) { 
		var slist = (ShopList)BindingContext; 
		await App.Database.DeleteShopListAsync(slist);
		await Navigation.PopAsync(); 
	}
    async void OnChooseButtonClicked(object sender, EventArgs e) { 
		await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext) {
			BindingContext = new Product() }); 
	}
    protected override async void OnAppearing() {
		base.OnAppearing();
        var items = await App.Database.GetShopsAsync(); 
        ShopPicker.ItemsSource = (System.Collections.IList)items; 
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");
        var shopl = (ShopList)BindingContext; 
		listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID); 
	}

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        // 1. Verificãm dacã este selectat un produs în listã
        if (listView.SelectedItem == null)
        {
            await DisplayAlert("Eroare", "Te rog selecteazã un produs din listã pentru a-l ?terge.", "OK");
            return;
        }

        // 2. Ob?inem produsul selectat ?i lista curentã
        var selectedProduct = listView.SelectedItem as Product;
        var currentList = (ShopList)BindingContext;

        // 3. Apelãm metoda din baza de date pentru a ?terge legãtura
        await App.Database.DeleteProductFromListAsync(currentList.ID, selectedProduct.ID);

        // 4. Reîncãrcãm lista pentru a vedea modificãrile
        listView.ItemsSource = await App.Database.GetListProductsAsync(currentList.ID);

        // 5. Resetãm selec?ia (op?ional)
        listView.SelectedItem = null;
    }
}