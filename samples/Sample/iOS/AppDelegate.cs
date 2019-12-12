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
using System.Linq;
using Foundation;
using UIKit;
using AcousticMobilePush.iOS;
using AcousticMobilePush.Forms;
using AcousticMobilePush.Forms.iOS;
using UserNotifications;
using Xamarin.Forms;
using Xamarin;

namespace Sample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// Avoid Some Mono Crashes
			Environment.SetEnvironmentVariable("MONO_XMLSERIALIZER_THS", "no");
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            new FreshEssentials.iOS.AdvancedFrameRendereriOS();

            Forms.Init ();
			FormsMaps.Init();

			App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
			App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;

			LoadApplication (new App ());

            // AcousticMobilePush Integration
            MCESdk.SharedInstance().HandleApplicationLaunch();

            // Setup Push Message Permission
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				Logging.Verbose("iOS > 10");
                UIApplication.SharedApplication.RegisterForRemoteNotifications();

                UNUserNotificationCenter.Current.Delegate = MCENotificationDelegate.SharedInstance();
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.CarPlay | UNAuthorizationOptions.Sound, (approved, err) =>
				{
                    if(approved)
                    {
                        Logging.Verbose("Push Notifications approved");
                    }
                    else
                    {
                        Logging.Verbose("Push Notifications denied");
                    }
                });
			}
			else if (UIDevice.CurrentDevice.CheckSystemVersion(8,0))
			{
				Logging.Verbose ("iOS > 8");
				var settings = UIUserNotificationSettings.GetSettingsForTypes (UIUserNotificationType.Sound |
					UIUserNotificationType.Alert | UIUserNotificationType.Badge, null);

				UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications ();
			}
			else
			{
				Logging.Verbose ("iOS < 8");
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Badge |
					UIRemoteNotificationType.Sound | UIRemoteNotificationType.Alert);
			}

			return base.FinishedLaunching (app, options);
		}

		// AcousticMobilePush Integration
		public override void DidRegisterUserNotificationSettings (UIApplication application, UIUserNotificationSettings notificationSettings)
		{
            MCEEventService.SharedInstance().SendPushEnabledEvent();
        }

		// AcousticMobilePush Integration
		public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
		{
            MCEEventService.SharedInstance().SendPushEnabledEvent();
        }

		// AcousticMobilePush Integration
		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
            MCESdk.SharedInstance().RegisterDeviceToken(deviceToken);
            Logging.Verbose("deviceToken: " + MCEApiUtil.DeviceToken(deviceToken));
            MCEEventService.SharedInstance().SendPushEnabledEvent();
        }

		// AcousticMobilePush Integration
		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
            MCEInAppManager.SharedInstance().ProcessPayload(userInfo);
            MCESdk.SharedInstance().PresentOrPerformNotification(userInfo);
        }

		// AcousticMobilePush Integration
		public override void ReceivedLocalNotification (UIApplication application, UILocalNotification notification)
		{
            MCEInAppManager.SharedInstance().ProcessPayload(notification.UserInfo);
            MCESdk.SharedInstance().PresentOrPerformNotification(notification.UserInfo);
        }

		// AcousticMobilePush Integration
		public override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            MCEInAppManager.SharedInstance().ProcessPayload(userInfo);
            MCESdk.SharedInstance().PresentDynamicCategoryNotification(userInfo);
        }

        // AcousticMobilePush Integration
        public override void HandleAction (UIApplication application, String actionIdentifier, NSDictionary remoteNotificationInfo, Action completionHandler)
		{
            MCESdk.SharedInstance().ProcessCategoryNotification(remoteNotificationInfo, actionIdentifier);
        }

		// AcousticMobilePush Integration
		public override void HandleAction (UIApplication application, String actionIdentifier, UILocalNotification localNotification, Action completionHandler)
		{
            MCESdk.SharedInstance().ProcessDynamicCategoryNotification(localNotification.UserInfo, actionIdentifier, null);
        }

		public override void HandleAction(UIApplication application, string actionIdentifier, NSDictionary remoteNotificationInfo, NSDictionary responseInfo, Action completionHandler)
		{
			
		}

		public override void HandleAction(UIApplication application, string actionIdentifier, UILocalNotification localNotification, NSDictionary responseInfo, Action completionHandler)
		{
			NSString response = null;
			if(responseInfo.ContainsKey(UIUserNotificationAction.ResponseTypedTextKey))
				response = (NSString)responseInfo.ObjectForKey(UIUserNotificationAction.ResponseTypedTextKey);
            MCESdk.SharedInstance().ProcessDynamicCategoryNotification(localNotification.UserInfo, actionIdentifier, response);
        }
	}
}

