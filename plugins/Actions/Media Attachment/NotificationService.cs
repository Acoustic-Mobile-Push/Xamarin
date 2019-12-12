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
using Foundation;
using UIKit;
using UserNotifications;

namespace Sample.iOS.Notification
{
	[Register("NotificationService")]
	public class NotificationService : UNNotificationServiceExtension
	{
		class MediaDownloadDelegate : NSUrlSessionDownloadDelegate
		{
			public Action<UNNotificationContent> ContentHandler { get; set; }
			public UNMutableNotificationContent BestAttemptContent { get; set; }

			public override void DidFinishDownloading(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
			{
				NSError error = null;
				var identifier = new NSUuid().ToString();
				var attachmentUrl = new NSUrl(downloadTask.OriginalRequest.Url.LastPathComponent, NSFileManager.DefaultManager.GetTemporaryDirectory());
				NSFileManager.DefaultManager.Copy(location, attachmentUrl, out error);
				if (error != null)
				{
					Deliver();
					return;
				}
				var attachment = UNNotificationAttachment.FromIdentifier(identifier, attachmentUrl, new NSDictionary(), out error);
				if (error != null)
				{
					Deliver();
					return;
				}
				BestAttemptContent.Attachments = new UNNotificationAttachment[] { attachment };
				Deliver();
			}

			public void Deliver()
			{ 
				ContentHandler(BestAttemptContent);
			}
		}

		MediaDownloadDelegate DownloadDelegate;

		protected NotificationService(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void DidReceiveNotificationRequest(UNNotificationRequest request, Action<UNNotificationContent> contentHandler)
		{
			DownloadDelegate = new MediaDownloadDelegate();
			DownloadDelegate.ContentHandler = contentHandler;
			DownloadDelegate.BestAttemptContent = (UNMutableNotificationContent)request.Content.MutableCopy();

			var url = new NSUrl(request.Content.UserInfo["media-attachment"].ToString());
			if (url != null)
			{
				InvokeOnMainThread(() =>
				{
					var session = NSUrlSession.FromConfiguration(NSUrlSessionConfiguration.DefaultSessionConfiguration, DownloadDelegate, NSOperationQueue.MainQueue);
					var task = session.CreateDownloadTask(NSUrlRequest.FromUrl(url));
					task.Resume();
				});
				return;
			}

			DownloadDelegate.Deliver();
		}

		public override void TimeWillExpire()
		{
			// Called just before the extension will be terminated by the system.
			// Use this as an opportunity to deliver your "best attempt" at modified content, otherwise the original push payload will be used.

			DownloadDelegate.Deliver();
		}
	}
}
