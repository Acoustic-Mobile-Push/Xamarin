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
using AcousticMobilePush.Forms;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Sample
{
	// Email can't be sent directly from a Xamarin.Forms app, it must go through the native implementations
	public interface IEmailAction
	{
		void SendEmail(string subject, string body, string recipient);
	}

	public class EmailAction : PushAction
	{
		IEmailAction emailSender;
		public EmailAction ()
		{
			emailSender = DependencyService.Get<IEmailAction> ();
		}

		public override void HandleAction (JObject action, JObject payload, string attribution, string mailingId, int id)
		{
			JToken values = null;
			string subject = null;
			string body = null;
			string recipient = null;
			if (action.TryGetValue("value", out values))
			{
				subject = values["subject"].ToString();
				body = values["body"].ToString();
				recipient = values["recipient"].ToString();
			}
			else
			{
				subject = action["subject"].ToString();
				body = action["body"].ToString();
				recipient = action["recipient"].ToString();
			}

			if (subject != null && body != null && recipient != null)
				emailSender.SendEmail(subject, body, recipient);
			else
				Logging.Error("Email Action can not complete, can't find subject, body or recipient in payload.");
		}
	}
}

