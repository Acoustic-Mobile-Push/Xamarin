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
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Collections.Generic;
using Android.OS;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(ContentPage), typeof(Sample.Droid.Renderers.PageRenderer))]
namespace Sample.Droid.Renderers
{
    public class PageRenderer : Xamarin.Forms.Platform.Android.PageRenderer
    {
        public PageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetAppTheme();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\t\t\tERROR: {ex.Message}");
            }
        }

        void SetAppTheme()
        {            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var uiModeFlags = MainActivity.Current.ApplicationContext.Resources.Configuration.UiMode & UiMode.NightMask;

                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        App.Current.Resources = new DarkTheme();
                        App.AppTheme = "dark";
                        break;
                    case UiMode.NightNo:
                        App.AppTheme = "light";
                        break;
                    default:
                        throw new NotSupportedException($"UiMode {uiModeFlags} not supported");
                }
            }
            else
            {
                App.AppTheme = "light";
            }
        }
    }
}
