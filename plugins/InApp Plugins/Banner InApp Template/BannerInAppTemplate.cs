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
using AcousticMobilePush.Forms;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Globalization;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Sample
{
	public class BannerInAppTemplate : InAppTemplate
	{ 
		uint AnimationLength = 250;
		int Duration = 5000;

		public BannerInAppTemplate ()
		{
		}

		bool IsTop()
		{
			return Message != null && Message.Content != null && Message.Content ["orientation"] != null && Message.Content ["orientation"].ToString ().Equals ("top");
		}

		public float Height { 
			get { 
                if(IsTop()) {
                    return 44;
                }
                return 44 + (float)SDK.Instance.SafeAreaInsets().Bottom;
			} 
		}

		Image _IconImage;
		public Image IconImage
		{
			get { 
				if (_IconImage == null) {
					_IconImage = new Image () {
						HorizontalOptions = LayoutOptions.StartAndExpand,
						VerticalOptions = LayoutOptions.Center,
						Source = ImageSource.FromFile ("note.png")
					};
				}
				return _IconImage;
			}		
		}

		Image _CancelImage;
		public Image CancelImage
		{
			get { 
				if (_CancelImage == null) {
					_CancelImage = new Image () {
						HorizontalOptions = LayoutOptions.EndAndExpand,
						VerticalOptions = LayoutOptions.Center,
						Source = ImageSource.FromFile ("cancel.png")
					};

					var tapGesture = new TapGestureRecognizer();
					tapGesture.Tapped += Dismiss;
					_CancelImage.GestureRecognizers.Add(tapGesture);
				}
				return _CancelImage;
			}
		}

		Label _LabelText;
		public Label LabelText
		{
			get { 
				if (_LabelText == null) {
					_LabelText = new Label () { 
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						VerticalOptions = LayoutOptions.Center,
						Text = "Test"
					};
				}
				return _LabelText;
			}
		}

		Image _ContentImage;
		Image ContentImage
		{
			get
			{
				if (_ContentImage == null)
				{
                    _ContentImage = new Image() {
                        Aspect = Aspect.AspectFill
                    };
				}
				return _ContentImage;
			}
		}

        StackLayout _Content;
		StackLayout Content
		{
			get
			{
				if (_Content == null)
				{
                    _Content = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Children = {
                                    IconImage,
                                    LabelText,
                                    CancelImage
                                }
                    };
				}
				return _Content;
			}
		}

        AbsoluteLayout _InAppView;
		public override View InAppView
		{ 
			get 
			{
                if (_InAppView == null)
                {
                    _InAppView = new AbsoluteLayout()
                    {
                        Children = {
                            Content,
							ContentImage
						}
                    };

					var tapGesture = new TapGestureRecognizer ();
					tapGesture.Tapped += (object sender, EventArgs e) => {
						SDK.Instance.ExecuteInAppAction(Message.Content["action"], Message.Attribution, Message.MailingId);
						SDK.Instance.DeleteInAppMessage(Message);
						Dismiss(null, EventArgs.Empty);
					};
					_InAppView.GestureRecognizers.Add (tapGesture);
				}
                _InAppView.HeightRequest = Height;
                _ContentImage.WidthRequest = Layout.Width;
				_Content.HeightRequest = Height;
				_Content.WidthRequest = Layout.Width - 20;
                _InAppView.WidthRequest = Layout.Width;

                if (IsTop()) {
                    _Content.Padding = new Thickness(10, 0, 10, 0);
                    _Content.HeightRequest = Height;
                    _ContentImage.HeightRequest = Height;
                } else {
                    _Content.Padding = new Thickness(10, 0, 10, SDK.Instance.SafeAreaInsets().Bottom);
                    _Content.HeightRequest = Height - SDK.Instance.SafeAreaInsets().Bottom;
                    _ContentImage.HeightRequest = Height - SDK.Instance.SafeAreaInsets().Bottom;
                }

                _InAppView.RaiseChild(Content);
				return _InAppView;
			}
		}

		public override async Task<bool> Show()
		{
			if (IsTop ()) {
				InAppView.TranslationY = -1 * InAppView.Height;
				await InAppView.TranslateTo (0, 0, AnimationLength, Easing.CubicInOut);
			} else {
				InAppView.TranslationY = InAppView.Height;
				await InAppView.TranslateTo (0, 0, AnimationLength, Easing.CubicInOut);
			}
			return true;
		}

		public override Constraint XConstraint { 
			get { 
				return Constraint.Constant (0);
			} 
		}

		public override Constraint YConstraint { 
			get { 
				if(IsTop())
					return Constraint.Constant (0);
				else
                    return Constraint.Constant (Layout.Height - Height );
			} 
		}

		public override Constraint WidthConstraint { 
			get {
				return Constraint.Constant (Layout.Width );
			}
		}

		public override Constraint HeightConstraint {
			get {
                return Constraint.Constant (Height);
			}
		}

		public override async Task<bool> Wait ()
		{
			await Task.Delay(Duration);
			return true;
		}

		public override async Task<bool> Hide ()
		{
			if (IsTop ()) {
				await InAppView.TranslateTo (0, -1 * InAppView.Height, AnimationLength, Easing.CubicInOut);
			} else {
				await InAppView.TranslateTo (0, InAppView.Height, AnimationLength, Easing.CubicInOut);
			}

			RemoveInAppLayout ();
			return true;
		}

		public override void Configure ()
		{
            ContentImage.Source = null;
            InAppView.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            if (Message.Content["mainImage"] != null)
                ContentImage.Source = ImageSource.FromUri(new Uri(Message.Content["mainImage"].ToString()));
			else if (Message.Content ["color"] != null)
				InAppView.BackgroundColor = Color.FromHex(Message.Content ["color"].ToString ());
			else 
				InAppView.BackgroundColor = Color.FromRgb (18, 84, 189);
			

			if (Message.Content ["text"] != null)
				LabelText.Text = Message.Content ["text"].ToString ();
			

			if (Message.Content ["icon"] != null) {
				IconImage.Source = ImageSource.FromFile (Message.Content ["icon"].ToString () + ".png");
				IconImage.WidthRequest = 24;
			} else {
				IconImage.WidthRequest = 0;
			}

			if (Message.Content ["foreground"] != null)
				LabelText.TextColor = Color.FromHex (Message.Content ["foreground"].ToString ());
			else
				LabelText.TextColor = Color.White;
			
			if (Message.Content ["duration"] != null)
				Duration = (int)(1000 * float.Parse (Message.Content ["duration"].ToString (), CultureInfo.InvariantCulture));
			else
				Duration = 5000;

		
			if (Message.Content ["animationLength"] != null)
				AnimationLength = (uint)(1000 * float.Parse (Message.Content ["animationLength"].ToString (), CultureInfo.InvariantCulture)); 
			else
				AnimationLength = 250;
		}

	}
}

