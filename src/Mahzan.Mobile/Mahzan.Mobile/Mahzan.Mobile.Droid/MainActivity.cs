using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidX.Core.Content;
using Plugin.Media;
using Xamarin.Essentials;
using ZXing.Mobile;

namespace Mahzan.Mobile.Droid
{
    [Activity(Label = "Mahzan.Mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            
            Platform.Init(this, savedInstanceState);
            
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            Android.Glide.Forms.Init(this);

            LoadApplication(new App());
            
            await Permissions.RequestAsync<BlePermission>();
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {

            Platform.OnRequestPermissionsResult(requestCode,permissions,grantResults);

            base.OnRequestPermissionsResult(requestCode,permissions, grantResults);
        }

    }
}