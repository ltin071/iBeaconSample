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
        IDisposable scanner;
        public MainPage()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            permission = CrossBeacons.Current.RequestPermission();
            permission.Subscribe(result =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ResultLabel.Text = "Permission Granted: "+result;
                });
            });
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            ResultLabel.Text = "Searching for \""+ UUIDEntry.Text+"\"";
            //if(scanner != null) scanner.Dispose();
            scanner = CrossBeacons.Current.WhenBeaconRanged(new BeaconRegion("Whatever", new Guid(UUIDEntry.Text), 0, 0))
                        .Subscribe(scanResult =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ResultLabel.Text = "Beacon Found:" + scanResult.Proximity+" "+scanResult.Accuracy;
                            });
                        });
        }
    }
}
