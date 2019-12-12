/*
 * Copyright © 2016, 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using AcousticMobilePush.Forms;
using System.Collections.Generic;

namespace Sample
{
	public partial class GeofencePage : ContentPage
	{
		ICurrentLocation CurrentLocation;
		bool follow;

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

		public GeofencePage()
		{
			InitializeComponent();

            SDK.Instance.LocationAuthorizationChanged += () => {
                UpdateStatus();
            };

            Status.Clicked += (sender, e) => {
                SDK.Instance.ManualLocationInitialization();
            };
            UpdateStatus();

			follow = true;
			var syncItem = new ToolbarItem();
			syncItem.Text = "Sync";
			syncItem.Command = new Command(() =>
			{
				SDK.Instance.SyncGeofences();
			});
			ToolbarItems.Add(syncItem);

			var followItem = new ToolbarItem();
			followItem.Text = "Follow";
			followItem.Command = new Command(() =>
			{
				follow = !follow;
                if(follow)
                {
                    followItem.Text = "Follow";
                } else
                {
                    followItem.Text = "Don't Follow";
                }
			});
			ToolbarItems.Add(followItem);

			CustomMap.Circles = new HashSet<CustomCircle>();

			CurrentLocation = DependencyService.Get<ICurrentLocation>();
			CurrentLocation.LocationUpdated += (latitude, longitude) =>
			{
				CustomMap.Circles.Clear();

				var geofences = SDK.Instance.GeofencesNear(latitude, longitude);
				foreach (Geofence geofence in geofences)
				{
					var position = new Position(geofence.Latitude, geofence.Longitude);
					CustomMap.Circles.Add(new CustomCircle
					{
						Position = position,
						Radius = geofence.Radius
					});
				}
				if(CustomMap.Refresh != null)
					CustomMap.Refresh();
				if (follow)
				{
					CustomMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromMiles(1.0)));
				}
			};
				
		}
	}
}
