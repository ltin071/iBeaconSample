using iBeaconScanner.iOS;
using Xamarin.Forms;
using CoreBluetooth;
using CoreFoundation;
using CoreLocation;
using Foundation;
using System;
using UIKit;

[assembly: Dependency(typeof(Advertise_iOS))]
namespace iBeaconScanner.iOS
{
    class Advertise_iOS : IAdvertise
    {
        public static CBPeripheralManager peripheralMgr;
        BTPeripheralDelegate peripheralDelegate;
        public void Start(string UUID)
        {
            var proximityUUID = new NSUuid("12345678-1234-1234-1234-123456780001");
            var beaconRegion = new CLBeaconRegion(proximityUUID, 100, 1, "Whatever");

            //power - the received signal strength indicator (RSSI) value (measured in decibels) of the beacon from one meter away
            var power = new NSNumber(-59);
            NSMutableDictionary peripheralData = beaconRegion.GetPeripheralData(power);
            peripheralDelegate = new BTPeripheralDelegate(peripheralData);
            peripheralMgr = new CBPeripheralManager(peripheralDelegate, DispatchQueue.DefaultGlobalQueue);


            Console.WriteLine("Beacon Debug: "+ "Started Advertising:" + UUID);
            ShowAlert("Started Advertising:" + UUID, 5);
        }

        class BTPeripheralDelegate : CBPeripheralManagerDelegate
        {
            NSMutableDictionary peripheralData;
            public BTPeripheralDelegate(NSMutableDictionary peripheralData)
            {
                this.peripheralData = peripheralData;
            }
            public override void StateUpdated(CBPeripheralManager peripheral)
            {
                if (peripheral.State == CBPeripheralManagerState.PoweredOn)
                {
                    Console.WriteLine("Beacon Debug: powered on");
                    peripheralMgr.StartAdvertising(peripheralData);
                }
            }
            public override void AdvertisingStarted(CBPeripheralManager peripheral, NSError error)
            {
                //base.AdvertisingStarted(peripheral, error);
                Console.WriteLine("Beacon Debug: AdvertisingStarted");
            }



        }
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }
        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}