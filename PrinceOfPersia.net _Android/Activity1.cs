using Android.App;
using Android.Content.PM;
using Android.OS;

namespace PrinceOfPersia.net__Android_
{
    [Activity(Label = "PrinceOfPersia.net"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape  //sensorLandcape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            PrinceOfPersia.Game.Activity = this;
            var g = new Game();
            SetContentView(g.Window);
            g.Run();
        }
    }
}

