using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SantaLetters;

// Portal Mini is a landscape-first tabletop device, so lock to (either way up) landscape.
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
	ScreenOrientation = ScreenOrientation.SensorLandscape,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
