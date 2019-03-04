using Plugin.Beacons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iBeaconScanner
{
    public partial class MainPage : ContentPage
    {
        IObservable<bool> permission;
        public MainPage()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Console.WriteLine("samBeacon Permission: Start");
            permission = CrossBeacons.Current.RequestPermission();
            Console.WriteLine("samBeacon Permission: End");
            permission.Subscribe(result =>
            {
                Console.WriteLine("samBeacon Permission:" + result);
                if (result)
                {
                    var scanner = CrossBeacons
                        .Current
                        .WhenBeaconRanged(new BeaconRegion(
                            "Whatever",
                            new Guid("12345678-1234-1234-1234-123456780001"), 0, 0
                        ))
                        .Subscribe(scanResult =>
                        {
                            // do something with it - FYI: this will not be on the main thread, so if you are displaying to the UI, make sure to invoke on it
                            Console.WriteLine("samBeacon Beacon Found:" + scanResult.Proximity);
                            OnBeaconFound(scanResult);
                        });
                }
            });
        }
        private void OnBeaconFound(Beacon scanResult)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage.DisplayAlert("Beacon", "Beacon Found:" + scanResult.Proximity, "OK");
            });
        }
    }
}
