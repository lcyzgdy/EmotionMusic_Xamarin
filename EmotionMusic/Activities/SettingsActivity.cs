using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EmotionMusic
{
	[Activity(Label = "SettingsActivity")]
	public class SettingsActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			//SetContentView(Resource.Layout.Settings);
			// Create your application here

			if (HasLogin())
			{
				;
			}
			else
			{
				;
			}
		}

		private bool HasLogin()
		{
			try
			{
				StreamReader file = new StreamReader("AllSettings.txt");
				var count = Convert.ToInt32(file.ReadLine());
				var hasLogin = Convert.ToInt32(file.ReadLine());
				if (hasLogin == 1)
				{
					return true;
				}
				file.Close();
			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
			}
			return false;
		}
	}

	public static class AllSettings
	{
		static AllSettings()
		{

		}

#pragma warning disable CS0649 // 从未对字段“AllSettings.hasLogin”赋值，字段将一直保持其默认值 false
		private static bool hasLogin;
#pragma warning restore CS0649 // 从未对字段“AllSettings.hasLogin”赋值，字段将一直保持其默认值 false
#pragma warning disable CS0649 // 从未对字段“AllSettings.count”赋值，字段将一直保持其默认值 0
		private static int count;
#pragma warning restore CS0649 // 从未对字段“AllSettings.count”赋值，字段将一直保持其默认值 0
#pragma warning disable CS0649 // 从未对字段“AllSettings.skinColor”赋值，字段将一直保持其默认值 0
		private static int skinColor;
#pragma warning restore CS0649 // 从未对字段“AllSettings.skinColor”赋值，字段将一直保持其默认值 0

		public static bool HasLogin { get => hasLogin; }
		public static int Count { get => count; }
		public static int SkinColor { get => skinColor; }
	}
}