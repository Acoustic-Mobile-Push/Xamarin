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

using Xamarin.Forms;
using AcousticMobilePush.Forms;

namespace Sample
{
    public partial class BeaconPage : ContentPage
	{
		Dictionary<int, string> RegionDetails = new Dictionary<int, string>();

		void UpdateStatus()
		{
			if (SDK.Instance.GeofenceEnabled())
			{
				if (SDK.Instance.LocationInitialized())
				{
					Status.Text = "ENABLED";
					Status.TextColor = App.LookupColor("Success");

				}
				else
				{
					Status.Text = "DELAYED (Touch to enable)";
					Status.TextColor = App.LookupColor("Disabled");
                }
			}
			else
			{
				Status.Text = "DISABLED";
				Status.TextColor = App.LookupColor("Failure");
            }
		}

		public BeaconPage()
		{
			InitializeComponent();
            UpdateStatus();
			SDK.Instance.LocationAuthorizationChanged += () => {
				UpdateStatus();
			};

			Status.Clicked += (sender, e) => {
				SDK.Instance.ManualLocationInitialization();
			};

			var uuid = SDK.Instance.BeaconUUID();
			if (uuid == null)
			{
				UUID.Detail = "No UUID Found";
			}
			else
			{
				UUID.Detail = SDK.Instance.BeaconUUID().Value.ToString();
			}
			UpdateRegions();

			SDK.Instance.BeaconEntered += (beaconRegion) => {
				RegionDetails[beaconRegion.Major.Value] = "Entered Minor " + beaconRegion.Minor.Value.ToString();
				UpdateRegions();
			};

			SDK.Instance.BeaconExited += (beaconRegion) =>
			{
				RegionDetails[beaconRegion.Major.Value] = "Exited Minor " + beaconRegion.Minor.Value.ToString();
				UpdateRegions();
			};
		}		

		void UpdateRegions()
		{
			Regions.Clear();
            var regions = SDK.Instance.BeaconRegions();
            if(regions != null)
            {
				foreach (BeaconRegion region in regions)
				{
					var cell = new TextCell() { Text = region.Major.ToString() };
					if (RegionDetails.ContainsKey(region.Major.Value))
						cell.Detail = RegionDetails[region.Major.Value];
					Regions.Add(cell);
				}
			}
		}
	}
}
