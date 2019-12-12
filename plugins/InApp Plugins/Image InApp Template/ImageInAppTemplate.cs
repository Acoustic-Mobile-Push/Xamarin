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
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;

namespace Sample
{
	public class ImageInAppTemplate : MediaInAppTemplate
	{
		int Duration = 5000;

		public ImageInAppTemplate ()
		{
		}

		public async override Task<bool> Wait()
		{
			if (Duration > 0) {
				await Task.Delay (Duration);
				return true;
			}
			return false;
		}

		public override void Configure () 
		{
			base.Configure();
			if(Message.Content["image"] != null)
				ContentImage.Source = ImageSource.FromUri (new Uri (Message.Content ["image"].ToString ()));

			if (Message.Content ["duration"] != null)
				Duration = (int)(1000 * float.Parse (Message.Content ["duration"].ToString (), CultureInfo.InvariantCulture));
			else
				Duration = 5000;
		}

		protected override View Content { get { return ContentImage; } }

		Image _ContentImage;
		Image ContentImage {
			get {
				if (_ContentImage == null) {
					_ContentImage = new Image ();
					var tapGesture = new TapGestureRecognizer ();
					tapGesture.Tapped += (object sender, EventArgs e) => {
						SDK.Instance.ExecuteInAppAction(Message.Content["action"], Message.MailingId, Message.Attribution);
						SDK.Instance.DeleteInAppMessage(Message);
						Dismiss(null, EventArgs.Empty);
					};
					_ContentImage.GestureRecognizers.Add (tapGesture);
				}
				return _ContentImage;
			}
		}
	}
}