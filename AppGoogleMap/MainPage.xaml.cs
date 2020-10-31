using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace AppGoogleMap
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                try
                {
                    IGeolocator locator = CrossGeolocator.Current;
                    // 1. 50mの精度に指定
                    // locator.DesiredAccuracy = 50
                    Position position = await locator.GetPositionAsync();

                    string result = position.Latitude.ToString() + ',' + position.Longitude.ToString();
                    string search_text = ((Button)sender).Text;

                    await Launcher.OpenAsync("geo:" + result + "?q=" + search_text);
                }
                catch
                {
                    await DisplayAlert("確認", "位置情報をオンにしてください。", "OK");
                }

            }
        }
    }
}
