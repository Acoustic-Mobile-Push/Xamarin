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
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Sample
{
    public class CustomAction : AcousticMobilePush.Forms.PushAction
    {
        Action<JObject> callback;
        public CustomAction( Action<JObject> callback )
        {
            this.callback = callback;
        }

        public override void HandleAction(JObject action, JObject payload, string attribution, string mailingId, int id)
        {
            callback(action);
        }
    }


    public partial class CustomActionPage : ContentPage
    {
        public CustomActionPage()
        {
            InitializeComponent();

            AcousticMobilePush.Forms.SDK.Instance.ActionNotRegistered += (actionType) =>
            {
                Status.Text = "Unregistered Custom Action Received: " + actionType;
                Status.TextColor = App.LookupColor("Failure");
            };

            AcousticMobilePush.Forms.SDK.Instance.ActionNotYetRegistered += (actionType) =>
            {
                Status.Text = "Previously Registered Custom Action Received: " + actionType;
                Status.TextColor = App.LookupColor("Warning");
            };

            RegisterButton.Clicked += (sender, e) => {
                Status.Text = "Registered Action Type " + TypeEntry.Text;
                Status.TextColor = App.LookupColor("Success");

                AcousticMobilePush.Forms.SDK.Instance.RegisterAction(TypeEntry.Text, new CustomAction( (action) => {
                    Status.Text = "Recevied push for action type " + action["type"];
                    Status.TextColor = App.LookupColor("Success");
                }));
            };

            UnregisterButton.Clicked += (sender, e) =>
            {
                AcousticMobilePush.Forms.SDK.Instance.UnregisterAction(TypeEntry.Text);
                Status.Text = "Unregistered Action Type " + TypeEntry.Text;
                Status.TextColor = App.LookupColor("Success");
            };

        }
    }
}
