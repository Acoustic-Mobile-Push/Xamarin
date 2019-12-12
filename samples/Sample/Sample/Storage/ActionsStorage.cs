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

namespace Sample
{
	public class ActionsStorage : Storage
	{
		public ActionsStorage ()
		{
		}

		private string StandardTypeKey = "StandardType";
		public string StandardType { 
			get {
				return GetValue<string> (StandardTypeKey, "url");
			}
			set {
				SetValue<string> (StandardTypeKey, value);
			}
		}

		private string StandardNameKey = "StandardName";
		public string StandardName { 
			get {
				return GetValue<string> (StandardNameKey, StandardType, StandardNameDefault);
			}
			set {
				SetValue<string> (StandardNameKey, value);
			}
		}

		string StandardNameDefault { 
			get {
				switch (StandardType) {
				case "url":
					return "URL";
				case "dial":
					return "Dial";
				case "openApp":
					return "OpenApp";
				}
				return null;
			}
		}

		private string StandardValueKey = "StandardValue";
		public string StandardValue { 
			get {
				return GetValue<string> (StandardValueKey, StandardType, StandardValueDefault);
			}
			set {
				SetValue<string> (StandardValueKey, value);
			}
		}

		string StandardValueDefault { 
			get {
				switch (StandardType) {
				case "url":
					return "http://ibm.co";
				case "dial":
					return "18884266840";
				case "openApp":
					return null;
				}
				return null;
			}
		}

		private string CustomTypeKey = "CustomType";
		public string CustomType { 
			get {
				return GetValue<string> (CustomTypeKey, "emailAction");
			}
			set {
				SetValue<string> (CustomTypeKey, value);
			}
		}

	
		private string CustomNameKey = "CustomName";
		public string CustomName { 
			get {
				return GetValue<string> (CustomNameKey, "Send Email");
			}
			set {
				SetValue<string> (CustomNameKey, value);
			}
		}

		private string CustomValueKey = "CustomValue";
		public string CustomValue { 
			get {
				return GetValue<string> (CustomValueKey, "{\"subject\":\"Hello from Sample App\", \"body\": \"This is an example email body\", \"recipient\":\"fake-email@fake-site.com\"}");
			}
			set {
				SetValue<string> (CustomValueKey, value);
			}
		}

	}
}

