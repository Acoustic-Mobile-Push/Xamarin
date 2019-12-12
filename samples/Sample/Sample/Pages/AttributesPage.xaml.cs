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

using Newtonsoft.Json.Linq;

using AcousticMobilePush.Forms;
using FreshEssentials;

namespace Sample
{
	public partial class AttributesPage : ContentPage
	{
		AttributesStorage storage = new AttributesStorage();
        public AttributesPage()
        {
            InitializeComponent();
            DateValue.Date = storage.DateTime.Date;
            TimeValue.Time = storage.DateTime.TimeOfDay;
            BooleanValue.SelectedIndex = storage.BoolValue;

            BooleanValue.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if (e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName)
                {
                    return;
                }
                storage.BoolValue = BooleanValue.SelectedIndex;
            };

            DateValue.DateSelected += (object sender, DateChangedEventArgs e) =>
            {
                storage.DateTime = new DateTime(DateValue.Date.Year, DateValue.Date.Month, DateValue.Date.Day, TimeValue.Time.Hours, TimeValue.Time.Minutes, TimeValue.Time.Seconds, TimeValue.Time.Milliseconds);
            };

            TimeValue.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                {
                    storage.DateTime = new DateTime(DateValue.Date.Year, DateValue.Date.Month, DateValue.Date.Day, TimeValue.Time.Hours, TimeValue.Time.Minutes, TimeValue.Time.Seconds, TimeValue.Time.Milliseconds);
                }
            };
            Name.Text = storage.Name;
            Value.Text = storage.Value;
            Type.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if (e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName)
                {
                    return;
                }

                UpdateType();
            };
            UpdateType();
            Type.SelectedIndex = storage.Type;

            Name.TextChanged += (object sender, TextChangedEventArgs e) => {
				storage.Name = Name.Text;
			};
			Value.Text = storage.Value;
            Value.TextChanged += (object sender, TextChangedEventArgs e) => {
                var current = Type.SegmentedButtons[Type.SelectedIndex];
                switch (current.Title)
                {
                    case "String":
                        storage.Value = Value.Text;
                        break;
                    case "Number":
                        try
                        {
                            storage.NumberValue = Convert.ToDouble(Value.Text);
                        } catch {

                        }
                        break;
                }
			};
			Action.SelectedIndex = storage.Action;
            Action.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if (e.PropertyName != SegmentedButtonGroup.SelectedIndexProperty.PropertyName)
                {
                    return;
                }

                storage.Action = Action.SelectedIndex;
			};

            SendQueue.Pressed += (object sender, EventArgs e) => {
                Status.TextColor = App.LookupColor("Disabled");
                var name = storage.Name;
                var currentAction = Action.SegmentedButtons[Action.SelectedIndex];
                switch (currentAction.Title)
				{
				case "Update":
                        var currentType = Type.SegmentedButtons[Type.SelectedIndex];
                        switch (currentType.Title)
                        {
                            case "Date":
                                var dateValue = storage.DateTime;
                                Status.Text = "Queued User Attribute Update\n" + name + " = " + dateValue;
                                SDK.Instance.QueueUpdateUserAttribute(name, dateValue);
                                break;
                            case "String":
                                var stringValue = storage.Value;
                                Status.Text = "Queued User Attribute Update\n" + name + " = " + stringValue;
                                SDK.Instance.QueueUpdateUserAttribute(name, stringValue);
                                break;
                            case "Bool":
                                var boolValue = storage.BoolValue == 1;
                                Status.Text = "Queued User Attribute Update\n" + name + " = " + boolValue;
                                SDK.Instance.QueueUpdateUserAttribute(name, boolValue);
                                break;
                            case "Number":
                                var numberValue = Convert.ToDouble(Value.Text);
                                Status.Text = "Queued User Attribute Update\n" + name + " = " + numberValue;
                                SDK.Instance.QueueUpdateUserAttribute(name, numberValue);
                                break;
                        }

					break;
				case "Delete":
                        Status.Text = "Queued User Attribute Removal\nName " + name;
                        SDK.Instance.QueueDeleteUserAttribute(name);
					break;
				}
			};

        }

        void UpdateType()
        {
            var current = Type.SegmentedButtons[Type.SelectedIndex];
            storage.Type = Type.SelectedIndex;
            switch (current.Title)
            {
                case "Date":
                    Value.IsVisible = false;
                    DateTime.IsVisible = true;
                    BooleanValue.IsVisible = false;
                    break;
                case "String":
                    Value.IsVisible = true;
                    DateTime.IsVisible = false;
                    BooleanValue.IsVisible = false;
                    Value.Keyboard = Keyboard.Default;
                    break;
                case "Bool":
                    Value.IsVisible = false;
                    BooleanValue.IsVisible = true;
                    DateTime.IsVisible = false;
                    break;
                case "Number":
                    Value.IsVisible = true;
                    DateTime.IsVisible = false;
                    BooleanValue.IsVisible = false;
                    Value.Keyboard = Keyboard.Numeric;
                    Value.Text = Convert.ToString(storage.NumberValue);
                    break;
            }
        }
        protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			storage.Name = Name.Text;
            storage.DateTime = new DateTime(DateValue.Date.Year, DateValue.Date.Month, DateValue.Date.Day, TimeValue.Time.Hours, TimeValue.Time.Minutes, TimeValue.Time.Seconds, TimeValue.Time.Milliseconds);
            storage.BoolValue = BooleanValue.SelectedIndex;
            storage.Value = Value.Text;
			storage.Action = Action.SelectedIndex;
            storage.Type = Type.SelectedIndex;
			SDK.Instance.AttributeQueueResults -= QueueCallback;
		}
		protected override void OnAppearing()
		{
			SDK.Instance.AttributeQueueResults += QueueCallback;
		}

		void QueueCallback(bool success, string key, object value, AttributeOperation operation) {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (success)
                {
                    Status.TextColor = App.LookupColor("Success");
                    if (operation == AttributeOperation.DeleteUserAttributes) {
                        Status.Text = "Deleted User attribute " + key;
                    }
                    else if (operation == AttributeOperation.UpdateUserAttributes)
                    {
                        Status.Text = "Updated User attribute " + key + " = " + value;
                    }
                }
                else
                {
                    Status.TextColor = App.LookupColor("Failure");
                    if (operation == AttributeOperation.DeleteUserAttributes) {
                        Status.Text = "Couldn't delete user attribute " + key;
                    }
                    else if (operation == AttributeOperation.UpdateUserAttributes) {
                        Status.Text = "Couldn't update user attribute " + key + " = " + value;
                    }
                }
            });
		}

	}
}

