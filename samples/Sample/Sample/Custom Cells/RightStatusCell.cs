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
using Xamarin.Forms;

namespace Sample
{
	public enum RightStatus {Normal, Sending, Received, Queued, Failed};
	public class RightStatusCell : RightDetailCell
	{
		RightStatus status;
		public RightStatus Status { 
			get { 
				return status; 
			} 
			set {
				status = value;
				if (status == RightStatus.Received) {
					Device.StartTimer (new TimeSpan (0, 0, 5), () => {
						Device.BeginInvokeOnMainThread(() => {
							status = RightStatus.Normal;
							Detail = "";
						});
						return false;
					});
				}
				if (status == RightStatus.Normal) {
					Detail = "";
				}
				else
				{
					Detail = status.ToString ();
				}
			}
		}

		public RightStatusCell ()
		{
			Status = RightStatus.Normal;
		}
	}
}


