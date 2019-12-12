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
using AcousticMobilePush.Forms;
using Newtonsoft.Json.Linq;
using System;

namespace Sample
{
	public class WebViewAction : PushAction
	{
		public WebViewAction () 
		{
		}

		public override void HandleAction (JObject action, JObject payload, string attribution, string mailingId, int id)
		{
			var page = new ContentPage ();
			var dismiss = new Button () {
				Text = "Dismiss",
				HorizontalOptions = LayoutOptions.Start
			};
			dismiss.Clicked += (object sender, EventArgs e) => {
				page.Navigation.PopModalAsync();
			};

			page.Content = new StackLayout () {
				Padding = new Thickness (0, Device.OnPlatform (20, 0, 0), 0, 0),
				Children = {
					new StackLayout () {
						Padding = new Thickness(10,0),
						Children = {
							dismiss
						}
					},
					new BoxView() {
						HeightRequest = 1,
						Color = Color.FromHex("#ccc")
					},
					new WebView () {
						Source = new UrlWebViewSource
						{
							Url = action["value"].ToString(),
						},
						VerticalOptions = LayoutOptions.FillAndExpand
					}
				}
			};
			Application.Current.MainPage.Navigation.PushModalAsync (page);
		}	
	}
}

