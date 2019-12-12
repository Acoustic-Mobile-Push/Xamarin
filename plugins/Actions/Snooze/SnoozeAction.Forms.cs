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
using AcousticMobilePush.Forms;
using Newtonsoft.Json.Linq;

namespace Sample
{
	public interface ISnoozeAction
	{
		void Snooze(JObject payload, int minutes, int id, string attribution);
	}

	public class SnoozeAction : PushAction
	{
		ISnoozeAction snooze;
		public SnoozeAction ()
		{
			snooze = DependencyService.Get<ISnoozeAction> ();
		}

		public override void HandleAction (JObject action, JObject payload, string attribution, string mailingId, int id)
		{
			var minutes = int.Parse (action ["value"].ToString ());
			snooze.Snooze (payload, minutes, id, attribution);
		}
	}
}

