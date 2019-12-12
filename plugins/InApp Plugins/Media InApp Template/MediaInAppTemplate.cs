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
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;

namespace Sample
{
	public abstract class MediaInAppTemplate : InAppTemplate
	{
		const int TEXT_HEIGHT = 80;

		public async override Task<bool> Show()
		{
			InAppView.TranslationY = InAppView.Height;
			await InAppView.TranslateTo(0, 0, 250, Easing.CubicInOut);
			return true;
		}

		public async override Task<bool> Hide()
		{
			await InAppView.TranslateTo(0, InAppView.Height, 250, Easing.CubicInOut);
			RemoveInAppLayout();
			return true;
		}

		public override void Configure()
		{
			MediaLayout = new RelativeLayout()
			{
				BackgroundColor = Color.Black.MultiplyAlpha(0.8),
				HeightRequest = Layout.Height,
				WidthRequest = Layout.Width
			};

			var DismissImage = new Image()
			{
				Source = ImageSource.FromFile("dismiss.png")
			};

			var tapGesture = new TapGestureRecognizer();
			tapGesture.Tapped += Dismiss;
			DismissImage.GestureRecognizers.Add(tapGesture);

			MediaLayout.Children.Add(DismissImage,
				xConstraint: Constraint.Constant(Layout.Width - 30 - 5),
				yConstraint: Constraint.Constant(5),
				widthConstraint: Constraint.Constant(30),
				heightConstraint: Constraint.Constant(30)
			);


			var HandleImage = new Image()
			{
				Source = ImageSource.FromFile("handle.png")
			};

			MediaLayout.Children.Add(HandleImage,
				xConstraint: Constraint.Constant(Layout.Width / 2 - 60 / 2),
				yConstraint: Constraint.Constant(13),
				widthConstraint: Constraint.Constant(60),
				heightConstraint: Constraint.Constant(14)
			);

			var TitleLabel = new Label()
			{
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.White,
				LineBreakMode = LineBreakMode.TailTruncation,
				Text = Message.Content["title"] == null ? "" : Message.Content["title"].ToString()
			};

			var TextLabel = new Label()
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				TextColor = Color.White,
				Text = Message.Content["text"] == null ? "" : Message.Content["text"].ToString()
			};

			var TextStack = new StackLayout()
			{
				WidthRequest = Layout.Width - 10,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = {
					TitleLabel,
					new BoxView () {
						HeightRequest=1,
						WidthRequest=Layout.Width-10,
						Color=Color.White
					},
					TextLabel
					}
			};

			var expanded = false;
			var textGesture = new TapGestureRecognizer();
			textGesture.Tapped += (object sender, EventArgs e) =>
			{
				canceled = true;
				expanded = !expanded;
				MediaLayout.Children.Remove(TextStack);

				if (expanded)
				{
					TextStack.BackgroundColor = Color.Black.MultiplyAlpha(0.7);
					MediaLayout.Children.Add(TextStack,
						xConstraint: Constraint.Constant(5),
						yConstraint: Constraint.Constant(30),
						widthConstraint: Constraint.Constant(Layout.Width - 10),
						heightConstraint: Constraint.Constant(Layout.Height - 30)
					);
				}
				else
				{
					TextStack.BackgroundColor = Color.Transparent;
					MediaLayout.Children.Add(TextStack,
						xConstraint: Constraint.Constant(5),
						yConstraint: Constraint.Constant(Layout.Height - TEXT_HEIGHT),
						widthConstraint: Constraint.Constant(Layout.Width - 10),
						heightConstraint: Constraint.Constant(Layout.Height - 30)
					);
				}
			};
			TextStack.GestureRecognizers.Add(textGesture);

			MediaLayout.Children.Add(Content,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(40),
				widthConstraint: Constraint.Constant(Layout.Width),
				heightConstraint: Constraint.Constant(Layout.Height - 40 - TEXT_HEIGHT)
			);

			MediaLayout.Children.Add(TextStack,
				xConstraint: Constraint.Constant(5),
				yConstraint: Constraint.Constant(Layout.Height - TEXT_HEIGHT),
				widthConstraint: Constraint.Constant(Layout.Width - 10),
				heightConstraint: Constraint.Constant(Layout.Height - 30)
			);

			_InAppView = MediaLayout;


		}

		RelativeLayout MediaLayout;



		protected abstract View Content { get; }

		View _InAppView;
		public override View InAppView { get { return _InAppView; } }
		public override Constraint XConstraint { get { return Constraint.Constant (0); } }
		public override Constraint YConstraint { get { return Constraint.Constant (0); } }
		public override Constraint WidthConstraint { get { return Constraint.Constant (Layout.Width ); } }
		public override Constraint HeightConstraint { get { return Constraint.Constant (Layout.Height ); } }
	}
}