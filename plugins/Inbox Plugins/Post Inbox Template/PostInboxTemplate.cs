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
using System.Collections.Generic;

namespace Sample
{
	public class PostInboxTemplate : IInboxTemplate
	{
		public PostInboxTemplate ()
		{
		}

		public bool ShouldDisplayInboxMessage (InboxMessage message)
		{
			return true;
		}

		public ViewCell MessageCell (InboxMessage message)
		{
			return new PostInboxViewCell (message);
		}

		public View MessageView (InboxMessage message)
		{
			return new ScrollView () { Content = new PostInboxTemplateView (message, null) };
		}
	}

	public class PostInboxViewCell : InboxTemplateCell
	{
		PostInboxTemplateView PostView;

		public PostInboxViewCell(InboxMessage message)
		{
			PostView = new PostInboxTemplateView (message, this);
			View = PostView;
		}

		public override void CellDisappearing()
		{
			PostView.Disappearing ();
		}
	}
}

