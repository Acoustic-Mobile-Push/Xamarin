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
using AcousticMobilePush.Forms;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace Sample
{
	public partial class InAppPage : ContentPage
	{
		public InAppPage ()
		{
			InitializeComponent ();

		}

		public void ExecuteInApp(object sender, EventArgs e)
		{
			var styleId = ((Cell)sender).StyleId;
			SDK.Instance.ExecuteInAppRule (new string[] { styleId });
			}
			
		public void CannedInApp(object sender, EventArgs e)
		{
            var styleId = ((Cell)sender).StyleId;

            var message = new InAppMessage();
            message.Id = Guid.NewGuid().ToString();
            message.TriggerDate = DateTimeOffset.Now.AddDays(-1);
            message.ExpirationDate = DateTimeOffset.Now.AddDays(1);
            message.Rules = new JArray();
            message.Rules.Add(styleId);
            message.Rules.Add("all");
            var action = new JObject() { { "type", "url" }, { "value", "http://ibm.co" } };
            var content = new JObject();
            content.Add("action", action);


            message.MaxViews = 10;
            message.NumViews = 0;

			if (styleId.Equals("bottomBanner") || styleId.Equals ("topBanner")) {
                message.Template = "default";
                content.Add("text", "Canned Banner Template Text");
                content.Add("icon", "note");
                content.Add("color", "0077FF");

				if (styleId.Equals ("topBanner")) {
                    content.Add("orientation", "top");
				}
			}

			if (styleId.Equals ("video") || styleId.Equals("image"))
			{
                message.Template = styleId;
                content.Add("title", "Canned Video Template Title");
                content.Add("text", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque rhoncus, eros sed imperdiet finibus, purus nibh placerat leo, non fringilla massa tortor in tellus. Donec aliquet pharetra dui ac tincidunt. Ut eu mi at ligula varius suscipit. Vivamus quis quam nec urna sollicitudin egestas eu at elit. Nulla interdum non ligula in lobortis. Praesent lobortis justo at cursus molestie. Aliquam lectus velit, elementum non laoreet vitae, blandit tempus metus. Nam ultricies arcu vel lorem cursus aliquam. Nunc eget tincidunt ligula, quis suscipit libero. Integer velit nisi, lobortis at malesuada at, dictum vel nisi. Ut vulputate nunc mauris, nec porta nisi dignissim ac. Sed ut ante sapien. Quisque tempus felis id maximus congue. Aliquam quam eros, congue at augue et, varius scelerisque leo. Vivamus sed hendrerit erat. Mauris quis lacus sapien. Nullam elit quam, porttitor non nisl et, posuere volutpat enim. Praesent euismod at lorem et vulputate. Maecenas fermentum odio non arcu iaculis egestas. Praesent et augue quis neque elementum tincidunt.");

				if (styleId.Equals ("video")) {
                    content.Add("video", "http://techslides.com/demos/sample-videos/small.mp4");
				} else {
                    content.Add("image", "https://picsum.photos/200/300");
				}
			}

            message.Content = content;
            SDK.Instance.InsertInAppAsync(message);
		}

	}
	public class InAppLayout : AbsoluteLayout
	{
		public InAppLayout() : base()
		{
		}
	}
}

