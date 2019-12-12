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
using Xamarin.Forms;
using AcousticMobilePush.Forms;

namespace Sample
{
	public partial class DefaultInboxTemplateCell : InboxTemplateCell
	{
		public DefaultInboxTemplateCell (InboxMessage message)
		{
			InitializeComponent ();
			var messagePreview = message.Content["messagePreview"];
			var subject = messagePreview ["subject"];
			var previewContent = messagePreview ["previewContent"];

			Content.Text = previewContent.ToString();
			Subject.Text = subject.ToString();

			if (message.ExpirationDate.Subtract (DateTimeOffset.Now).TotalSeconds < 0) {
				Date.Text = "Expired " + message.ExpirationDate.ToString ("d");
				Date.TextColor = App.AppTheme == "dark" ? Color.IndianRed : Color.DarkRed;
			} else {
				Date.Text = message.SendDate.ToString("d");
				Date.TextColor = App.AppTheme == "dark" ? Color.LightBlue : Color.DarkBlue;
			}

			if (message.IsRead) {
				Subject.FontAttributes = FontAttributes.None;
			}
			else {
				Subject.FontAttributes = FontAttributes.Bold;
			}
		}

		public override void CellDisappearing()
		{
		}

	}
}

