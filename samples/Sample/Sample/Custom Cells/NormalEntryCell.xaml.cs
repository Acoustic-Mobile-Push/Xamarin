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

namespace Sample
{
	public partial class NormalEntryCell : ViewCell
	{
		public NormalEntryCell ()
		{
			InitializeComponent ();
			TextEntry.TextChanged += (sender, e) => {
				Text = ((Entry) sender).Text;
				if(TextChanged != null)
				{
					TextChanged(sender, e);
				}
			};
			View.SizeChanged += (object sender, EventArgs e) => {

				SizeRequest sizeRequest = TextLabel.GetSizeRequest(Layout.Width, Double.PositiveInfinity);
				TextLabel.WidthRequest = sizeRequest.Request.Width;
				TextEntry.WidthRequest = View.Width - sizeRequest.Request.Width - Layout.Padding.Left - Layout.Padding.Right - 10;
			};
		}
			
		public delegate void ChangedDelegate(object sender, EventArgs e);
		public ChangedDelegate TextChanged { get; set; }

		string text;
		public string Text { get { return text; } set { text = value; TextEntry.Text = text; } }

		string label;
		public string Label { get { return label; } set { label = value; TextLabel.Text = label; } }
	}
}

