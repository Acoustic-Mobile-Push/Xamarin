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
using System;

namespace Sample
{
	public class Storage
	{
		protected T GetValue<T>(string key, T defaultValue)
		{
			if(Application.Current.Properties.ContainsKey(key))
			{
				return (T) Application.Current.Properties [key];
			}
			return defaultValue;
		}

		protected void SetValue<T>(string key, T value)
		{
			Application.Current.Properties [key] = value;
		}

		protected T GetValue<T>(string key, string prependKey, T defaultValue)
		{
			if(Application.Current.Properties.ContainsKey(key))
			{
				return (T) Application.Current.Properties [key];
			}
			return defaultValue;
		}

		protected void SetValue<T>(string key, string prependKey, T value)
		{
			Application.Current.Properties [key] = value;
		}
	}
}

