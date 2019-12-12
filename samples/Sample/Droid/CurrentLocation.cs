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
using Android.Locations;
using Sample;
using Android.Content;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(Sample.Droid.CurrentLocation))]
namespace Sample.Droid
{
	public class CurrentLocation : Java.Lang.Object, ICurrentLocation, ILocationListener
	{
		string locationProvider;
		LocationManager LocationManager;
		string TAG = "CurrentLocation";
		Location lastLocation;

		public CurrentLocation()
		{
			LocationManager = (LocationManager)Xamarin.Forms.Forms.Context.GetSystemService(Context.LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = LocationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				locationProvider = string.Empty;
			}

			LocationManager.RequestLocationUpdates(locationProvider, 0, 0, this);

			Log.Debug(TAG, "Using " + locationProvider + ".");
		}

		UpdatedDelegate myDelegate;
		public UpdatedDelegate LocationUpdated { 
			get 
			{
				return myDelegate;
			} 
			set 
			{
				myDelegate = value;
				if (lastLocation != null)
					myDelegate(lastLocation.Latitude, lastLocation.Longitude);
			} 
		}

		public void OnLocationChanged(Location location)
		{
			lastLocation = location;
			if (LocationUpdated != null)
				LocationUpdated(location.Latitude, location.Longitude);
		}

		public void OnProviderDisabled(string provider)
		{
			Log.Debug(TAG, "OnProviderDisabled");
		}

		public void OnProviderEnabled(string provider)
		{
			Log.Debug(TAG, "OnProviderEnabled");
		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			Log.Debug(TAG, "OnStatusChanged " + status.ToString());
		}

	}
}
