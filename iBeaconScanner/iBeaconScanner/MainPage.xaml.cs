﻿using Plugin.Beacons;
using Plugin.LocalNotifications;
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
            CrossBeacons.Current.StopAllMonitoring();
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

        private void BackgroundSearchButton_Clicked(object sender, EventArgs e)
        {
            ResultLabel.Text = "Background searching for \"" + UUIDEntry.Text + "\"";
            scanner = CrossBeacons.Current.WhenRegionStatusChanged().Subscribe(regionArgs =>
                {
                    
                    CrossLocalNotifications.Current.Show("Office Beacon", regionArgs.IsEntering?"Welcome Home :o)":"Sad to see you go :o(", 101);
                });

            CrossBeacons.Current.StartMonitoring(new BeaconRegion("Whatever", new Guid(UUIDEntry.Text), 0, 0));
        }
        private void AdvertiseButton_Clicked(object sender, EventArgs e)
        {
            ResultLabel.Text = "Advertising "+UUIDEntry.Text;
            DependencyService.Get<IAdvertise>().Start(UUIDEntry.Text);
        }
    }
}
