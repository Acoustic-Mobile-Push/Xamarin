/*
 * Copyright © 2019, 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */
 
using System;
using Foundation;
using UserNotifications;
using AcousticMobilePush.iOS.Notification;

namespace Sample.iOS.Notification
{
    [Register("NotificationService")]
    public class NotificationService : MCENotificationService
    {
        public override void DidReceiveNotificationRequest(UNNotificationRequest request, Action<UNNotificationContent> contentHandler)
        {
            base.DidReceiveNotificationRequest(request, contentHandler);
        }
    }
}
