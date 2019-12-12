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

namespace Sample
{
	public partial class RegistrationPage : ContentPage
	{
		public RegistrationPage ()
		{
            InitializeComponent();
            SDK.Instance.RegistrationUpdated += UpdateRegistration;
			UpdateRegistration ();
            Registration.Tapped += (object sender, EventArgs e) => {
                if (SDK.Instance.UserId() == null && SDK.Instance.ChannelId() == null)
                {
                    Registration.Detail = "Registering";
                    SDK.Instance.ManualSdkInitialization();
                }
            };
		}

		protected override void OnDisappearing ()
		{
			SDK.Instance.RegistrationUpdated -= UpdateRegistration;
		}


		public void UpdateRegistration()
		{
			Device.BeginInvokeOnMainThread(() => {
                var userId = SDK.Instance.UserId();
                var channelId = SDK.Instance.ChannelId();
                if(userId == null && channelId == null) {
                    Registration.Detail = "Tap to Register";
                } else {
                    Registration.Detail = "Registered";
                }
                if(SDK.Instance.UserInvalidated()) {
                    Registration.Detail += " (Invalidated State)";
                }
                UserId.Detail = userId;
                ChannelId.Detail = channelId;
				AppKey.Detail = SDK.Instance.AppKey ();
			});
		}
	}
}

