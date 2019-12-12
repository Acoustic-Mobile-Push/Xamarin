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
using System.Collections.ObjectModel;
using System.Linq;

using AcousticMobilePush.Forms;

using Xamarin.Forms;

namespace Sample
{
	public partial class InboxMessagePage : ContentPage
	{

		public InboxMessagePage (InboxMessage message, InboxPage inbox)
		{
			InitializeComponent ();

			SDK.Instance.ReadInboxMessage(message);

			if (inbox == null) {
                DismissLayout.Padding = new Thickness (10, Device.OnPlatform (Math.Max(SDK.Instance.SafeAreaInsets().Top, 20), 0, 0), 10, 0);
				Dismiss.Clicked += (object sender, EventArgs e) => {
					Navigation.PopModalAsync();
				};
			} else {
				Layout.Children.Clear ();
			}

			var handler = SDK.Instance.RegisteredInboxTemplate(message.TemplateName);
			if(handler != null && handler.ShouldDisplayInboxMessage(message))
			{
				var view = handler.MessageView(message);
				Layout.Children.Add (view, 0, 1);
			}

            if (inbox == null || inbox.MessageIndex(message) == 0) {
				ToolbarItems.Remove (PrevButton);
			}
            if (inbox == null || inbox.MessageIndex(message) == inbox.MessageCount() - 1) {
				ToolbarItems.Remove (NextButton);
			}

            PrevButton.Clicked += (object sender, EventArgs e) => {
				int index = inbox.MessageIndex(message) - 1;
                Navigation.PushAsync(new InboxMessagePage(inbox.MessageAtIndex(index), inbox));
				Navigation.RemovePage(this);
                inbox.UpdateCell(index);
			};
			NextButton.Clicked += (object sender, EventArgs e) => {
                int index = inbox.MessageIndex(message) + 1;
                Navigation.PushAsync(new InboxMessagePage(inbox.MessageAtIndex(index), inbox));
                Navigation.RemovePage(this);
                inbox.UpdateCell(index);
			};
			TrashButton.Clicked += (object sender, EventArgs e) => {
				SDK.Instance.DeleteInboxMessage(message);
				SDK.Instance.SyncInboxMessages();
				Navigation.PopAsync();
			};

            var attributes = new Dictionary<string, object>() {
							{"richContentId", message.RichContentId},
							{"inboxMessageId", message.InboxMessageId}
						};
			SDK.Instance.QueueAddEvent("messageOpened", "inbox", DateTimeOffset.Now, message.Attribution, message.MailingId, attributes, true);
		}
	}
}

