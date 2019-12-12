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
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using UIKit;
using Foundation;
using AcousticMobilePush.iOS;
using AcousticMobilePush.Forms;

[assembly: Dependency(typeof(Sample.iOS.SnoozeActionImpl))]
namespace Sample.iOS
{
	public class SnoozeActionImpl : ISnoozeAction
	{
		public SnoozeActionImpl ()
		{
		}

		public void Snooze(JObject payload, int minutes, int id, string attribution)
		{
			var notification = new UILocalNotification ();

			var jsonString = payload.ToString ();
			var jsonData = NSData.FromString (jsonString);
			NSError error = null;
			var userInfo = (NSDictionary) NSJsonSerialization.Deserialize (jsonData, 0, out error);
			if (error != null) {
				Logging.Error ("Could not deserialize json data for snooze");
				return;
			}
			notification.UserInfo = userInfo;

			var aps = payload ["aps"];
			if (aps == null) {
				Logging.Error ("Could not find aps in snooze payload");
				return;
			}

			if (aps ["category"] != null) {
				notification.Category = aps ["category"].ToString ();
			}

			if (aps ["sound"] != null) {
				notification.SoundName = aps ["sound"].ToString ();
			}

			if (aps ["badge"] != null) {
				notification.ApplicationIconBadgeNumber = int.Parse(aps["badge"].ToString());
			}

			if (aps ["alert"] != null && aps["alert"].Type == JTokenType.Object && aps ["alert"] ["action-loc-key"] != null) {
				notification.AlertAction = aps["alert"] ["action-loc-key"].ToString();
				notification.HasAction = true;
			}

            notification.AlertBody = MCESdk.SharedInstance().ExtractAlert((NSDictionary)userInfo[new NSString("aps")]);
            notification.FireDate = NSDate.FromTimeIntervalSinceNow (minutes * 60);
			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}
	}
}
