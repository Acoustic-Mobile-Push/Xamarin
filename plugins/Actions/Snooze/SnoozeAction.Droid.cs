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
using AcousticMobilePush.Droid;
using AcousticMobilePush.Forms;
using Android.Content;
using Android.App;
using Newtonsoft.Json.Linq;

[assembly: Dependency(typeof(Sample.Droid.SnoozeActionImpl))]
namespace Sample.Droid
{
	public class SnoozeActionImpl : ISnoozeAction
	{
		public SnoozeActionImpl ()
		{
		}


		public void Snooze(JObject payload, int minutes, int id, string attribution)
		{
			Logging.Verbose ("snooze payload: " + payload);
			var context = Xamarin.Forms.Forms.Context;

			AlarmManager mgr = (AlarmManager) context.GetSystemService(Android.Content.Context.AlarmService);
			Intent intent =  new Intent(context, typeof(SnoozeIntentService));
			intent.PutExtra("payload", payload.ToString());
			intent.PutExtra("attribution", attribution);
			intent.PutExtra("id", id);
			PendingIntent pi = PendingIntent.GetService(context, 0, intent, 0);
			(new SnoozeIntentService()).ScheduleSnooze(mgr, pi, minutes);
			NotificationManager notificationManager = (NotificationManager) context.GetSystemService("notification");
			notificationManager.Cancel(id);

		}
	}
}

