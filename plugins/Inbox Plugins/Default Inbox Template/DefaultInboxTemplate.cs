﻿/*
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

namespace Sample
{
	public class DefaultInboxTemplate : IInboxTemplate
	{
		public DefaultInboxTemplate ()
		{
		}

		public bool ShouldDisplayInboxMessage (InboxMessage message)
		{
			return true;
		}

		public ViewCell MessageCell (InboxMessage message)
		{
			return new DefaultInboxTemplateCell (message);
		}

		public View MessageView (InboxMessage message)
		{
			return new DefaultInboxTemplateView (message);
		}
	}
}

