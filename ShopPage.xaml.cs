using DamianIonutLab7.Models;

using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using Plugin.LocalNotification;

namespace DamianIonutLab7;

public partial class ShopPage : ContentPage
{
	public ShopPage()
	{
		InitializeComponent();
	}

    async void OnSaveButtonClicked(object sender, EventArgs e) {
		var shop = (Shop)BindingContext; 
		await App.Database.SaveShopAsync(shop);
		await Navigation.PopAsync(); 
	}

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address); 
        var options = new MapLaunchOptions { Name = "Magazinul meu preferat" };
        var shoplocation = locations?.FirstOrDefault();
       var myLocation = await Geolocation.GetLocationAsync();
        /*  var myLocation = new Location(46.7731796289, 23.6213886738); //pentru Windows Machine */
        var distance = myLocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);
        if (distance < 5) { var request = new NotificationRequest 
        { 
            Title = "Ai de facut cumparaturi in apropiere!",
            Description = address, 
            Schedule = new NotificationRequestSchedule 
            { 
                NotifyTime = DateTime.Now.AddSeconds(1)
            } 
        }; 
        LocalNotificationCenter.Current.Show(request); }
        await Map.OpenAsync(shoplocation, options);
    }

    // Noul handler de eveniment pentru butonul Delete Shop
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        // Verificăm dacă magazinul este unul existent (are un ID valid)
        if (shop.ID != 0)
        {
            // Apelăm metoda de ștergere din baza de date
            await App.Database.DeleteShopAsync(shop);
        }

        // Revenim la pagina anterioară (ShopEntryPage)
        await Navigation.PopAsync();
    }
}