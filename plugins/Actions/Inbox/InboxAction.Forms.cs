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
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace Sample
{
	public interface IOpenAppAction
	{
		void OpenApp();
	}

	public class InboxAction : PushAction
	{
        String inboxMessageId = null;
		IOpenAppAction openApp;
		public InboxAction()
		{
			openApp = DependencyService.Get<IOpenAppAction>();
            SDK.Instance.InboxMessagesUpdate += InboxMessagesUpdate;;
        }

		public override void HandleAction (JObject action, JObject payload, string attribution, string mailingId, int id)
		{
            var inboxMessageId = action["inboxMessageId"];
            if(inboxMessageId != null)
            {
				SDK.Instance.FetchInboxMessage(inboxMessageId.ToString(), (message) => {
                    if (message == null)
                    {
                        this.inboxMessageId = inboxMessageId.ToString();
                        SDK.Instance.SyncInboxMessages();
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var page = new InboxMessagePage(message, null);
                            Application.Current.MainPage.Navigation.PushModalAsync(page);
                        });
                    }
				});
			}
		}

        void InboxMessagesUpdate()
        {
            if (this.inboxMessageId != null)
            {
                SDK.Instance.FetchInboxMessage(this.inboxMessageId, (message) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var page = new InboxMessagePage(message, null);
                        Application.Current.MainPage.Navigation.PushModalAsync(page);
                    });

                    this.inboxMessageId = null;
                });
            }
		}
    }
}

