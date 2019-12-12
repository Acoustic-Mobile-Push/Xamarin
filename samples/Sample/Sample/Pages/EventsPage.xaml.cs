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
using FreshEssentials;
using AcousticMobilePush.Forms;
using Xamarin.Forms;

namespace Sample
{
    enum AttributeTypes { Date, String, Bool, Number };
	public partial class EventsPage : ContentPage
	{
        void UpdateEventNames(List<string> names) {
            EventNameSwitch.IsVisible = true;
            EventName.IsVisible = false;
            EventNameSwitch.SegmentedButtons.Clear();
            foreach (string name in names) {
                EventNameSwitch.SegmentedButtons.Add(new SegmentedButton() { Title = name });
            }
            if (EventNameSwitch.SelectedIndex >= names.Count)
            {
                EventNameSwitch.SelectedIndex = names.Count - 1;
            }
        }

        void UpdateEventTypes(List<string> types)
        {
            EventType.SegmentedButtons.Clear();
            foreach (string type in types)
            {
                EventType.SegmentedButtons.Add(new SegmentedButton() { Title = type });
            }
            if(EventType.SelectedIndex >= types.Count) {
                EventType.SelectedIndex = types.Count - 1;
            }
        }

        void AllowNoAttributes() {
            AttributeBooleanValue.IsVisible = false;
            AttributeName.IsVisible = true;
            AttributeValue.IsVisible = true;
            AttributeName.IsEnabled = false;
            AttributeValue.IsEnabled = false;
            AttributeType.IsEnabled = false;
            AttributeName.Text = "";
            AttributeValue.Text = "";
        }

        void AllowNumberAttributes() {
            AttributeName.IsVisible = true;
            AttributeValue.IsVisible = true;
            AttributeValue.IsEnabled = true;
            AttributeName.IsEnabled = false;
            AttributeType.IsEnabled = false;
            AttributeType.SelectedIndex = (int) AttributeTypes.Number;
        }

        void AllowStringAttributes()
        {
            AttributeName.IsVisible = true;
            AttributeValue.IsVisible = true;
            AttributeValue.IsEnabled = true;
            AttributeName.IsEnabled = false;
            AttributeType.IsEnabled = false;
            AttributeType.SelectedIndex = (int)AttributeTypes.String;
        }

        void AllowAllAttributes()
        {
            AttributeName.IsVisible = true;
            AttributeValue.IsVisible = true;
            AttributeValue.IsEnabled = true;
            AttributeType.IsEnabled = true;
            AttributeName.IsEnabled = true;
        }

        void AllowBoolAttributes()
        {
            AttributeType.SelectedIndex = (int)AttributeTypes.Bool;
            AttributeType.IsVisible = true;
            AttributeName.IsVisible = true;
            AttributeValue.IsVisible = false;
            AttributeType.IsEnabled = true;
            AttributeName.IsEnabled = true;
            AttributeValue.IsEnabled = false;
        }

        void ReloadSimulatedEvents(bool enabled) {
            SimulatedEvent.IsEnabled = enabled;
            if (enabled) {
                SimulatedEvent.OnColor = App.LookupColor("SecondaryTextColor");
                SimulatedEvent.OffColor = App.LookupColor("SecondaryBackgroundColor");
            }
            else {
                SimulatedEvent.OnColor = App.LookupColor("Disabled");
                SimulatedEvent.OffColor = App.LookupColor("Disabled");
            }
            var button = new SegmentedButton();
            SimulatedEvent.SegmentedButtons.Add(button);
            SimulatedEvent.SegmentedButtons.Remove(button);
        }

        void SendEvent()
        {
            var eventType = EventType.SegmentedButtons[EventType.SelectedIndex].Title;
            var eventName = CustomEvent.SelectedIndex == 1 ? EventName.Text : EventNameSwitch.SegmentedButtons[EventNameSwitch.SelectedIndex].Title;
            var attribution = Attribution.Text;
            var mailingId = MailingId.Text;
            var attributes = new Dictionary<string, object>();
            var timestamp = new DateTimeOffset();

            var attributeName = AttributeName.Text;
            if (attributeName.Length > 0) {
                switch ((AttributeTypes)AttributeType.SelectedIndex)
                {
                    case AttributeTypes.Bool:
                        attributes.Add(attributeName, AttributeBooleanValue.SelectedIndex == 1);
                        break;
                    case AttributeTypes.String:
                        var stringValue = AttributeValue.Text;
                        if (stringValue.Length > 0)
                        {
                            attributes.Add(attributeName, stringValue);
                        }
                        break;
                    case AttributeTypes.Date:
                        var date = new DateTime(DateValue.Date.Year, DateValue.Date.Month, DateValue.Date.Day, TimeValue.Time.Hours, TimeValue.Time.Minutes, TimeValue.Time.Seconds, TimeValue.Time.Milliseconds);
                        attributes.Add(attributeName, date);
                        break;
                    case AttributeTypes.Number:
                        try {
                            var numberValue = Convert.ToDouble(AttributeValue.Text);
                            attributes.Add(attributeName, numberValue);
                        }
                        catch {
                        }
                        break;
                }
            }

            SDK.Instance.QueueAddEvent(eventName, eventType, timestamp, attribution, mailingId, attributes, true);
            UpdateStatus(Color.Gray, "Queued event name=" + eventName + ", type=" + eventType );
        }

        void UpdateEventTypes() {
            if(CustomEvent.SelectedIndex == 1) {
                UpdateEventTypes(new List<string>() { "custom" });
                EventName.IsVisible = true;
                EventNameSwitch.IsVisible = false;
                AllowAllAttributes();
                UpdateAttributeTypes();
                ReloadSimulatedEvents(false);
                return;
            }

            ReloadSimulatedEvents(true);

            var current = SimulatedEvent.SegmentedButtons[SimulatedEvent.SelectedIndex];
            EventType.SegmentedButtons.Clear();

            switch (current.Title)
            {
                case "App":
                    UpdateEventTypes(new List<string>() {"application"} );
                    UpdateEventNames(new List<string>() { "sessionStarted", "sessionEnded", "uiPushEnabled", "uiPushDisabled" });
                    if (EventNameSwitch.SelectedIndex == 1)
                    {
                        AllowNumberAttributes();
                        AttributeName.Text = "sessionLength";
                    }
                    else {
                        AllowNoAttributes();
                    }
                    break;
                case "Action":
                    UpdateEventTypes(new List<string>() { "simpleNotification", "inboxMessage", "inAppMessage" });
                    UpdateEventNames(new List<string>() { "urlClicked", "appOpened", "phoneNumberClicked", "inboxMessageOpened" });
                    switch( EventNameSwitch.SegmentedButtons[EventNameSwitch.SelectedIndex].Title ) {
                        case "urlClicked":
                            AttributeName.Text = "url";
                            AllowStringAttributes();
                            break;
                        case "appOpened":
                            AttributeName.Text = "";
                            AllowNoAttributes();
                            break;
                        case "phoneNumberClicked":
                            AttributeName.Text = "phoneNumber";
                            AllowStringAttributes();
                            break;
                        case "inboxMessageOpened":
                            AttributeName.Text = "richContentId";
                            AllowStringAttributes();
                            break;
                    }
                    break;
                case "Inbox":
                    UpdateEventTypes(new List<string>() { "inbox" });
                    UpdateEventNames(new List<string>() { "messageOpened" });
                    AttributeName.Text = "inboxMessageId";
                    AllowStringAttributes();
                    break;
                case "Geofence":
                    UpdateEventTypes(new List<string>() { "geofence" });
                    UpdateEventNames(new List<string>() { "disabled", "enabled", "enter", "exit" });
                    break;
                case "iBeacon":
                    UpdateEventTypes(new List<string>() { "ibeacon" });
                    UpdateEventNames(new List<string>() { "disabled", "enabled", "enter", "exit" });
                    break;
            }
            if(current.Title == "Geofence" || current.Title == "iBeacon") {
                switch (EventNameSwitch.SegmentedButtons[EventNameSwitch.SelectedIndex].Title)
                {
                    case "disabled":
                        AttributeName.Text = "reason";
                        AttributeValue.Text = "not_enabled";
                        AllowStringAttributes();
                        break;
                    case "enabled":
                        AllowNoAttributes();
                        break;
                    case "enter":
                    case "exit":
                        AllowStringAttributes();
                        AttributeName.Text = "locationId";
                        break;
                }
            }

        }

        void UpdateAttributeTypes()
        {
            switch ((AttributeTypes)AttributeType.SelectedIndex)
            {
                case AttributeTypes.Bool:
                    AttributeBooleanValue.IsVisible = true;
                    AttributeValue.IsVisible = false;
                    DateTime.IsVisible = false;
                    break;
                case AttributeTypes.String:
                    AttributeBooleanValue.IsVisible = false;
                    AttributeValue.IsVisible = true;
                    DateTime.IsVisible = false;
                    AttributeValue.Keyboard = Keyboard.Default;
                    break;
                case AttributeTypes.Date:
                    AttributeBooleanValue.IsVisible = false;
                    AttributeValue.IsVisible = false;
                    DateTime.IsVisible = true;
                    break;
                case AttributeTypes.Number:
                    AttributeBooleanValue.IsVisible = false;
                    AttributeValue.IsVisible = true;
                    DateTime.IsVisible = false;
                    AttributeValue.Keyboard = Keyboard.Numeric;
                    try {
                        AttributeValue.Text = Convert.ToString(Convert.ToDouble(AttributeValue.Text));
                    } catch {
                        AttributeValue.Text = "0";
                    }
                    break;
            }
        }

        public EventsPage ()
		{
			InitializeComponent ();
            UpdateEventTypes();

            CustomEvent.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if (e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName)
                {
                    return;
                }
                UpdateEventTypes();
            };
            EventType.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if (e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName)
                {
                    return;
                }
                UpdateEventTypes();
            };
            EventNameSwitch.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if (e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName)
                {
                    return;
                }
                UpdateEventTypes();
            };
            SimulatedEvent.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if(e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName) {
                    return;
                }
                UpdateEventTypes();
            };

            UpdateAttributeTypes();
            AttributeType.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName)
                {
                    return;
                }
                UpdateAttributeTypes();
            };

            SendEventButton.Clicked += (object sender, EventArgs e) => {
                SendEvent();
            };
        }

		public void QueueCallback(bool success, string name, string type, DateTimeOffset timestamp, string attribution, string mailingId, Dictionary<string,object> attributes)
		{
            if(success) {
                UpdateStatus(Color.Green, "Sent event name=" + name + ", type=" + type);
            } else {
                UpdateStatus(Color.Red, "Failed to send event name=" + name + ", type=" + type);
            }
		}

        void UpdateStatus(Color color, String text) {

            Device.BeginInvokeOnMainThread(() =>
            {
                Status.Text = text;
                Status.TextColor = color;
            });
        }

		protected override void OnDisappearing ()
		{
			SDK.Instance.EventQueueResults -= QueueCallback;
		}

		protected override void OnAppearing()
		{
			SDK.Instance.EventQueueResults += QueueCallback;
		}

    }
}

