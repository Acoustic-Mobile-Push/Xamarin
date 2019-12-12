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
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Util;

using Newtonsoft.Json.Linq;
using AcousticMobilePush.Droid.Notification;
using AcousticMobilePush.Forms;

namespace Sample.Droid
{
	[Service (Exported = true, Enabled = true)]
	public class SnoozeIntentService : IntentService
	{
		public SnoozeIntentService() : base("Snooze") {
		}

		protected override void OnHandleIntent(Intent intent) {
			Bundle extras = new Bundle();

			extras.PutString("alert", intent.GetStringExtra("payload"));
			var mce = new JObject () { { "attribution", intent.GetStringExtra ("attribution") } };
			extras.PutString("mce", mce.ToString());
			try {
				NotificationsUtility.ShowNotification(Xamarin.Forms.Forms.Context, extras, 0, null);
			} catch (Exception jsone) {
				Logging.Error ("couldn't parse notification.");
			}
		}


		/**
	     * This method schedules a notification reappearance
	     * @param mgr The alarm manager
	     * @param pi The pending intent for the action
	     * @param delayInMinutes The number of minutes after which the notification will reappear
	     */
		public void ScheduleSnooze(AlarmManager mgr, PendingIntent pi, int delayInMinutes) {
			var alertTime = Java.Util.Calendar.Instance;
			alertTime.Time = new Date ();
			alertTime.Add(CalendarField.Minute, delayInMinutes);
			mgr.Set(AlarmType.Rtc, alertTime.TimeInMillis, pi);
		}	
	}
}

