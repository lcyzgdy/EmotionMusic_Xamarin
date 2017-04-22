using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;

namespace EmotionMusic
{
	[Activity(Label = "PlayActivity")]
	public class PlayActivity : Activity
	{
		string stateNow;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.PlayLayout);

			TextView textView = FindViewById<TextView>(Resource.Id.PlayLayout_Top_Text);
			var str = Intent.GetStringExtra("name") ?? "没有音乐";
			textView.Text = str;

			stateNow = Intent.GetBooleanExtra("isPlaying", false) ? "play" : "pause";
			
			var backButton = FindViewById<ImageButton>(Resource.Id.PlayLayout_Top_BackButton);
			backButton.Click += delegate
			  {
				  Finish();
			  };
			var playButton = FindViewById<ImageButton>(Resource.Id.PlayLayout_Foot_PlayButton);
			if (!str.Equals("没有音乐"))
			{
				playButton.SetImageResource(Android.Resource.Drawable.IcMediaPause);
				playButton.Click += PausePlay;
			}
		}


		private void PausePlay(object sender, EventArgs e)
		{
			Intent intent = new Intent(this, typeof(PlayService));
			intent.PutExtra("act", "pause");
			StartService(intent);
			stateNow = "pause";

			var button = sender as ImageButton;
			button.SetImageResource(Android.Resource.Drawable.IcMediaPlay);
			try
			{
				button.Click -= PausePlay;
			}
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
			catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
			{ }
			button.Click += Replay;
		}

		private void Replay(object sender, EventArgs e)
		{
			Intent intent = new Intent(this, typeof(PlayService));
			intent.PutExtra("act", "replay");
			StartService(intent);
			stateNow = "play";
			var button = sender as ImageButton;
			button.SetImageResource(Android.Resource.Drawable.IcMediaPause);
			try
			{
				button.Click -= Replay;
			}
			catch (Exception ex)
			{ }
			button.Click += PausePlay;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		public override void Finish()
		{
			Intent result = new Intent();
			result.PutExtra("state", stateNow);
			SetResult((Result)ActivityManager.Activities.PlayActivity, result);
			base.Finish();
			GC.Collect();
		}
	}
}