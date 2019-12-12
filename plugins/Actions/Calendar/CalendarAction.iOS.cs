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
using EventKit;
using EventKitUI;
using Foundation;
using AcousticMobilePush.Forms;
using AcousticMobilePush.Forms.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(Sample.iOS.CalendarActionImpl))]
namespace Sample.iOS
{
    public class CalendarActionImpl : ICalendarAction
    {
        public CalendarActionImpl()
        {
        }

        public void AddEvent(string title, string description, DateTimeOffset startDate, DateTimeOffset endDate, bool interactive)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
           {
               var store = new EKEventStore();
               var granted = await store.RequestAccessAsync(EKEntityType.Event);

               if (granted.Item2 != null)
               {
                   Logging.Error("Could not add to calendar " + granted.Item2.LocalizedDescription);
                   return;
               }

               if (granted.Item1 == false)
               {
                   Logging.Error("Could not get access to EventKit, can't add to calendar");
                   return;
               }

               var newEvent = EKEvent.FromStore(store);
               newEvent.Calendar = store.DefaultCalendarForNewEvents;
               newEvent.Title = title;
               newEvent.Notes = description;

               DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));

               newEvent.StartDate = AcousticMobilePushImpl.ConvertDate(startDate, NSDate.Now);
               newEvent.EndDate = AcousticMobilePushImpl.ConvertDate(endDate, NSDate.Now);

               if (interactive)
               {
                   var controller = new EKEventEditViewController();
                   controller.Event = newEvent;
                   controller.EventStore = store;
                   var topViewController = Utility.FindCurrentViewController();
                   controller.Completed += (object sender, EKEventEditEventArgs e) =>
                   {
                       topViewController.DismissViewController(true, null);
                   };
                   topViewController.PresentViewController(controller, true, null);
               }
               else
               {
                   NSError error = null;
                   store.SaveEvent(newEvent, EKSpan.ThisEvent, out error);
                   if (error != null)
                   {
                       Logging.Error("Could not save event: " + error.LocalizedDescription);
                   }
               }
           });
        }
    }
}

