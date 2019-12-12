/*
 * Copyright © 2019 Acoustic, L.P. All rights reserved.
 *
 * NOTICE: This file contains material that is confidential and proprietary to
 * Acoustic, L.P. and/or other developers. No license is granted under any intellectual or
 * industrial property rights of Acoustic, L.P. except as may be provided in an agreement with
 * Acoustic, L.P. Any unauthorized copying or distribution of content from this file is
 * prohibited.
 */
 
using Xamarin.Forms;
using Android.Content;


[assembly: Dependency(typeof(Sample.Droid.CalendarActionImpl))]
namespace Sample.Droid
{
	public class OpenAppActionImpl : IOpenAppAction
	{
		public OpenAppActionImpl()
		{
		}

		public async void OpenApp()
		{
			Context context = Android.App.Application.Context;
			Intent intent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
			Forms.Context.StartActivity(intent);
		}
	}
}

