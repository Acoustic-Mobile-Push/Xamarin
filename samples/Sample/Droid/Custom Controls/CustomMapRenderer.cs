/*
 * Copyright © 2016, 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */

using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Sample;
using Sample.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer (typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Sample.Droid
{
	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		GoogleMap map;
		CustomMap mapElement;
		CurrentLocation currentLocation;
		List<Circle> Circles;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        void Refresh()
		{
			foreach (var circle in Circles)
				circle.Remove();
			
			foreach (var circle in mapElement.Circles)
			{
				var circleOptions = new CircleOptions();
				circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
				circleOptions.InvokeRadius(circle.Radius);
				circleOptions.InvokeFillColor(0X66FF0000);
				circleOptions.InvokeStrokeColor(0X66FF0000);
				circleOptions.InvokeStrokeWidth(0);
				Circles.Add(map.AddCircle(circleOptions));
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				mapElement.Refresh -= Refresh;
			}

			if (e.NewElement != null) {
				if(Circles == null)
					Circles = new List<Circle>();
				mapElement = (CustomMap)e.NewElement;
				mapElement.Refresh += Refresh;

				((MapView)Control).GetMapAsync (this);
			}
		}

		public void OnMapReady (GoogleMap googleMap)
		{
			map = googleMap;
			map.MyLocationEnabled = true;
			Refresh();
		}
	}
}
