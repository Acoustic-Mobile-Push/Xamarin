/*
 * Copyright © 2016, 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */

using System;
using System.Collections.Generic;
using AcousticMobilePush.Forms;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
            this.Appearing += (sender, e) => {
                SDKVersionLabel.Text = "Native SDK Version: " + SDK.Instance.Version();
                PluginVersionLabel.Text = "Xamarin Plugin Version: " + SDK.Instance.XamarinPluginVersion();
                if (Device.RuntimePlatform == Device.Android)
				{
					Logo.HeightRequest = 100;
					Logo.WidthRequest = 245;

                    if (App.AppTheme == "dark")
                    {
                        Logo.Source = ImageSource.FromFile("logo_dark.png");
                        Campaign.Source = ImageSource.FromFile("campaign_dark.png");
                    }
                    else
                    {
                        Logo.Source = ImageSource.FromFile("logo.png");
                        Campaign.Source = ImageSource.FromFile("campaign.png");
                    }
                }
                else
                {
                    Logo.Source = ImageSource.FromFile("logo.png");
                    Campaign.Source = ImageSource.FromFile("campaign.png");
                }
            };
		}
        
		public async void OpenPage(object sender, EventArgs e) 
		{
			var styleId = ((Cell)sender).StyleId;

            switch (styleId)
            {
                case "CustomAction":
                    await Navigation.PushAsync(new CustomActionPage());
                    break;
                case "Registration":
                    await Navigation.PushAsync(new RegistrationPage());
                    break;
                case "Inbox":
                    await Navigation.PushAsync(new InboxPage());
                    break;
                case "InApp":
                    await Navigation.PushAsync(new InAppPage());
                    break;
                case "Events":
                    await Navigation.PushAsync(new EventsPage());
                    break;
                case "Attributes":
                    await Navigation.PushAsync(new AttributesPage());
                    break;
                case "Geofences":
                    await Navigation.PushAsync(new GeofencePage());
                    break;
                case "iBeacons":
                    await Navigation.PushAsync(new BeaconPage());
                    break;
            }
		}
	}

}
