/*
 * Copyright © 2016, 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */

using AcousticMobilePush.Forms;
using System;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace Sample
{
	// Calendar can't be accessed directly from a Xamarin.Forms app, it must go through the native implementations
	public interface ICalendarAction
	{
		void AddEvent(string title, string description, DateTimeOffset startDate, DateTimeOffset endDate, bool interactive);
	}

	public class CalendarAction : PushAction
	{
		ICalendarAction calendar;
		public CalendarAction ()
		{
			calendar = DependencyService.Get<ICalendarAction> ();
		}

		public override void HandleAction (JObject action, JObject payload, string attribution, string mailingId, int id)
		{
			var title = action.GetValue ("title").ToString();
			var startDate = DateTimeOffset.Parse( action.GetValue ("startDate").ToString() );
			var endDate = DateTimeOffset.Parse( action.GetValue ("endDate").ToString () );
			var description = action.GetValue ("description").ToString ();

			var interactive = false;
			if (action.GetValue ("interactive") != null) {
                interactive = action.GetValue("interactive").ToObject   <bool>();
			}

			calendar.AddEvent (title, description, startDate, endDate, interactive);
		}
	}
}

