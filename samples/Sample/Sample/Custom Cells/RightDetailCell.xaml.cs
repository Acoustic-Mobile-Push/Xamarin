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
	public partial class RightDetailCell : ViewCell
	{
		string text;
		public string Text { get { return text; } set { text = value; TextLabel.Text = text; } }

		string detail;
		public string Detail { 
			get { 
				return detail; 
			} 
			set { 
				detail = value; 
				Device.BeginInvokeOnMainThread (() => {
					DetailLabel.Text = detail; 
				});
			} 
		}

		public RightDetailCell ()
		{
			InitializeComponent ();
		}
	}
}

