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
using Xamarin.Forms;
using CoreLocation;

[assembly: Dependency(typeof(Sample.iOS.CurrentLocation))]
namespace Sample.iOS
{
	public class CurrentLocation : ICurrentLocation
	{
		CLLocationManager LocationManager;
		public CurrentLocation()
		{
			LocationManager = new CLLocationManager();
			LocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
				LocationUpdated(e.Locations[0].Coordinate.Latitude, e.Locations[0].Coordinate.Longitude);
			};
			LocationManager.StartUpdatingLocation();
		}

		public UpdatedDelegate LocationUpdated { get; set; }
		
	}
}
