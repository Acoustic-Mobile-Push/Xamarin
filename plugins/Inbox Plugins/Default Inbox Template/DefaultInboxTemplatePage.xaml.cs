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
using Xamarin.Forms;
using AcousticMobilePush.Forms;

namespace Sample
{
	public partial class DefaultInboxTemplateView : ContentView
	{
		public DefaultInboxTemplateView(InboxMessage message)
		{
			InitializeComponent ();

			var messagePreview = message.Content["messagePreview"];
			var messageDetails = message.Content["messageDetails"];
			var subject = messagePreview ["subject"];
			var richContent = messageDetails ["richContent"];
			var actions = message.Content ["actions"];
            var html = richContent.ToString();
            if(!html.Contains("prefers-color-scheme"))
            {
                html = "<style>body {background-color: #FFFFFF;color: #000000;}@media (prefers-color-scheme: dark) {body {background-color: #000000;color: #ffffff;}}</style>" + html;
            }

            Content.Source = new HtmlWebViewSource () { Html = html };
			Content.Navigating += (object sender, WebNavigatingEventArgs e) => {
				var url = new Uri(e.Url);
				if(url.Scheme == "actionid")
				{
					var action = actions[url.PathAndQuery];
					if(action != null)
					{
						var attributes = new Dictionary<string, string>() { 
							{"richContentId", message.RichContentId},
							{"inboxMessageId", message.InboxMessageId}
						};
						SDK.Instance.ExecuteInboxAction(action, message.Attribution, message.MailingId, attributes);
					}
					e.Cancel = true;
				}
			};
			Subject.Text = subject.ToString();
			Date.Text = message.SendDate.ToString("f");
		}
	}
}

