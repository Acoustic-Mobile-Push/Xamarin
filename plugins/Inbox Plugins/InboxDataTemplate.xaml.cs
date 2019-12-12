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

namespace Sample
{
	public partial class InboxDataTemplate : ViewCell
	{
		InboxTemplateCell MessageCell;

		public InboxDataTemplate ()
		{
			InitializeComponent ();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged ();

			var inboxMessage = BindingContext as InboxMessage;
			if (inboxMessage == null)
				return;
			var template = SDK.Instance.RegisteredInboxTemplate (inboxMessage.TemplateName);
			if (template == null) {
				return;
			}
			MessageCell = template.MessageCell (inboxMessage) as InboxTemplateCell;
			View = MessageCell.View;
			Height = MessageCell.Height;
		}

		override protected void OnDisappearing()
		{
			base.OnDisappearing();
			if(MessageCell != null)
				MessageCell.CellDisappearing ();
		}
	}
}

