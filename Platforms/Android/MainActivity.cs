using Android.App;
using Android.Content.PM;
using Android.OS;

using Plugin.LocalNotification;
using Android.Content;

namespace DamianIonutLab7
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Apelul către bază trebuie să fie primul
            base.OnCreate(savedInstanceState);

            // Adaugă această linie pentru a inițializa pachetul de notificări
            LocalNotificationCenter.CreateNotificationChannel();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            // Adaugă această linie pentru a gestiona notificările la deschiderea aplicației
            LocalNotificationCenter.NotifyNotificationTapped(intent);
        }
    }
}
