/*
 * Copyright © 2019 Acoustic, L.P. All rights reserved.
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
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using FormsVideoLibrary;

namespace Sample
{
	public partial class PostInboxTemplateView : ContentView
	{
		InboxMessage InboxMessage;

		public PostInboxTemplateView (InboxMessage message, ViewCell viewCell)
		{
			InboxMessage = message;
			InitializeComponent ();

			var headerImage = message.Content ["headerImage"];
			var header = message.Content ["header"];
			var subHeader = message.Content ["subHeader"];
			var contentText = message.Content ["contentText"];
			var actions = message.Content ["actions"];
			var contentVideo = message.Content ["contentVideo"];
			var contentImage = message.Content ["contentImage"];
            
			if (headerImage != null) {
				var headerImageUri = new Uri (headerImage.ToString ());
				HeaderImage.Source = ImageSource.FromUri (headerImageUri);
			}
					
			if (header != null) {
				Header.Text = header.ToString ();
			}

			if (subHeader != null) {
				SubHeader.Text = subHeader.ToString ();
			}

			if (contentVideo != null) {
				ContentImage.HeightRequest = 1;
				ContentImage.IsVisible = false;
                ContentVideo.Source = new UriVideoSource() { Uri = contentVideo.ToString() };
                if (viewCell == null) {
                    ContentVideo.AreTransportControlsEnabled = true;
                } else {
                    ContentVideo.UpdateStatus += ContentVideo_UpdateStatus;
                    ContentVideo.AreTransportControlsEnabled = false;
                }
			} else if (contentImage != null) {
				ContentVideo.HeightRequest = 1;
				ContentVideo.IsVisible = false;
				var contentImageUri = new Uri (contentImage.ToString ());
				var contentImageSource = ImageSource.FromUri (contentImageUri);
				ContentImage.Source = contentImageSource;
					
			} else {
				ContentImage.HeightRequest = 1;
				ContentVideo.HeightRequest = 1;
				ContentImage.IsVisible = false;
				ContentVideo.IsVisible = false;
			}

			if (contentText == null) 
			{
				ContentText.HeightRequest = 1;
				ContentText.IsVisible = false;
			}
			else
			{
				if (viewCell == null)
					ContentText.HeightRequest = -1;
				else
					ContentText.HeightRequest = 44;
				ContentText.Text = contentText.ToString ();
			}

			if (actions == null) {
				Actions.HeightRequest = 1;
				Actions.IsVisible = false;
			}
			else
			{
				var attributes = new Dictionary<string, string>() {
							{"richContentId", message.RichContentId},
							{"inboxMessageId", message.InboxMessageId}
						};

				Actions.HeightRequest = 44;
				if (actions [0] != null) {
					CenterAction.Text = actions [0]["name"].ToString();
					CenterAction.Clicked += (object sender, EventArgs e) => {
						SDK.Instance.ExecuteInboxAction(actions[0], message.Attribution, message.MailingId, attributes);
					};
				}
				if (actions [1] != null) {
					LeftAction.Text = actions [1]["name"].ToString();
					LeftAction.Clicked += (object sender, EventArgs e) => {
						SDK.Instance.ExecuteInboxAction(actions[1], message.Attribution, message.MailingId, attributes);
					};
				}
				if (actions [2] != null) {
					RightAction.Text = actions [2]["name"].ToString();
					RightAction.Clicked += (object sender, EventArgs e) => {
						SDK.Instance.ExecuteInboxAction(actions[2], message.Attribution, message.MailingId, attributes);
					};
				}
			}

		}

        void ContentVideo_UpdateStatus(object sender, EventArgs e)
        {
            if(ContentVideo.Status == VideoStatus.Playing) {
                ContentVideo.UpdateStatus -= ContentVideo_UpdateStatus;
                ContentVideo.Pause();
            }
        }

        public void Disappearing()
		{
			if (ContentVideo != null)
			{
                ContentVideo.Pause();
			}
		}
	}
}

