/*
 * Copyright © 2016, 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */

using Xamarin.Forms;
using AcousticMobilePush.Forms;

namespace Sample
{
	public partial class App : Application
	{
		public static double ScreenHeight;
		public static double ScreenWidth;
        NavigationPage navigationPage = new NavigationPage(new MainPage());

        protected override void OnResume()
        {
            navigationPage.BarTextColor = LookupColor("TintColor");
        }

        protected override void OnStart()
        {
            navigationPage.BarTextColor = LookupColor("TintColor");
        }

        public App ()
		{
            InitializeComponent();

            MainPage = navigationPage;

            Device.BeginInvokeOnMainThread(() =>
            {
                // Custom Actions
                SDK.Instance.RegisterAction("sendEmail", new EmailAction());
                SDK.Instance.RegisterAction("calendar", new CalendarAction());
                SDK.Instance.RegisterAction("snooze", new SnoozeAction());

                // iOS and Android use different names for this plugin
                var displayWebAction = new WebViewAction();
                SDK.Instance.RegisterAction("displayWebView", displayWebAction);
                SDK.Instance.RegisterAction("displayweb", displayWebAction);

                // Inbox Plugin
                SDK.Instance.RegisterAction("openInboxMessage", new InboxAction());
                SDK.Instance.RegisterInboxTemplate("default", new DefaultInboxTemplate());
                SDK.Instance.RegisterInboxTemplate("post", new PostInboxTemplate());

                // InApp Plugin
                SDK.Instance.RegisterInAppTemplate("video", new VideoInAppTemplate());
                SDK.Instance.RegisterInAppTemplate("image", new ImageInAppTemplate());
                SDK.Instance.RegisterInAppTemplate("default", new BannerInAppTemplate());
            });
		}

        public static Color LookupColor(string key)
        {
            try
            {
                Application.Current.Resources.TryGetValue(key, out var newColor);
                return (Color)newColor;
            }
            catch
            {
                return Color.White;
            }
        }

        public static string AppTheme
        {
            get; set;
        }
    }
}

