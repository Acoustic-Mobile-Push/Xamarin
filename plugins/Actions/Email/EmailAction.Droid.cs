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
using Android.Content;
using AcousticMobilePush.Forms;

[assembly: Dependency(typeof(Sample.Droid.EmailActionImpl))]
namespace Sample.Droid
{
	public class EmailActionImpl : IEmailAction
	{
		Context ApplicationContext;
		public EmailActionImpl ()
		{
			ApplicationContext = Xamarin.Forms.Forms.Context;
		}

		public void SendEmail(string subject, string body, string recipient)
		{
			var address = recipient + "?subject=" + Uri.EscapeDataString (subject) + "&body=" + Uri.EscapeDataString (body);
			var intent = new Intent (Intent.ActionSendto, Android.Net.Uri.FromParts("mailto", address, null));
			intent.SetType ("text/plain");
			intent.PutExtra (Intent.ExtraEmail, recipient);
			intent.SetData (Android.Net.Uri.Parse ("mailto:" + address));
			intent.PutExtra (Intent.ExtraSubject, subject);
			intent.PutExtra (Intent.ExtraText, body);
			intent.AddFlags (ActivityFlags.NewTask);
			//intent.AddFlags(Intent.FLAG_FROM_BACKGROUND);
			try {
				ApplicationContext.StartActivity (intent);
			} catch (Android.Content.ActivityNotFoundException e) {
				Logging.Error ("No Email activity found:" + e.Message, e);
			}
		}
	}
}