/*
 * Copyright © 2016, 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */

using System.Linq;
using MapKit;
using Sample;
using Sample.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer (typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Sample.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		MKCircleRenderer circleRenderer;

		void Refresh()
		{
			var nativeMap = Control as MKMapView;
			if(nativeMap.Overlays != null)
				nativeMap.RemoveOverlays(nativeMap.Overlays);
			foreach (var circle in mapElement.Circles)
			{
				nativeMap.OverlayRenderer = GetOverlayRenderer;
				var circleOverlay = MKCircle.Circle(new CoreLocation.CLLocationCoordinate2D(circle.Position.Latitude, circle.Position.Longitude), circle.Radius);
				nativeMap.AddOverlay(circleOverlay);
			}
			nativeMap.SetNeedsDisplay();
		}
		CustomMap mapElement;
		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				mapElement.Refresh -= Refresh;
				var nativeMap = Control as MKMapView;
				nativeMap.ShowsUserLocation = true;
				nativeMap.OverlayRenderer = null;
			}

			if (e.NewElement != null) {
				mapElement = e.NewElement as CustomMap;
				mapElement.Refresh += Refresh;
				var nativeMap = Control as MKMapView;
				nativeMap.ShowsUserLocation = true;
			}
		}

		MKOverlayRenderer GetOverlayRenderer (MKMapView mapView, IMKOverlay overlay)
		{
			if (circleRenderer == null) {
				circleRenderer = new MKCircleRenderer (overlay as MKCircle);
				circleRenderer.FillColor = UIColor.Red;
				circleRenderer.Alpha = 0.4f;
			}	
			return circleRenderer;
		}
	}
}
