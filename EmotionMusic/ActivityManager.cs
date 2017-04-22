using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Widget;
using Java.IO;
using System.IO;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;

namespace EmotionMusic
{
    class ActivityManager
    {
        public enum Activities
        {
            StartActivity,
            EmoActivity,
            Camera,
            MainActivity,
			PlayActivity,
			LoginActivity,
			SettingsActivity,
			EmotionDataVisualizationActivity
        };

        public const string myKey = "28113027e2ec4d3f8305cb60cac4c981";
    }

	static class MyColors
	{
		static Color[] aColor;

		static MyColors()
		{
			aColor = new Color[9];
			aColor[0] = new Color(0xe7, 0xe7, 0xe7, 0x77);
			aColor[1] = Color.Gray;
			aColor[2] = Color.DarkGray;
		}

		public static Color[] AColor { get => aColor; }
	}
}