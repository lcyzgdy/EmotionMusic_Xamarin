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
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Android.Graphics;
using System.IO;
using System.Windows;
using System.Drawing;

namespace EmotionMusic
{
	[Activity(Label = "EmoActivity")]
	public class EmoActivity : Activity
	{
		EmotionServiceClient emotionServiceClient;
		ClientClass client;
		string stateNow;
		string nameT;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			try
			{
				SetContentView(Resource.Layout.PlayLayout);
				emotionServiceClient = new EmotionServiceClient(ActivityManager.myKey);
				client = new ClientClass();
				UploadAsync(Intent.GetStringExtra("fileurl"));
				var backButton = FindViewById<ImageButton>(Resource.Id.PlayLayout_Top_BackButton);
				backButton.Click += delegate
				{
					Finish();
				};
				var playButton = FindViewById<ImageButton>(Resource.Id.PlayLayout_Foot_PlayButton);
				playButton.SetImageResource(Android.Resource.Drawable.IcMediaPause);
				playButton.Click += PausePlay;

			}
			catch (Exception e)
			{
				Toast.MakeText(this, e.Message, ToastLength.Long).Show();
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
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
			catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
			{ }
			button.Click += PausePlay;
		}

		private async void UploadAsync(string fileUrl)
		{
			Emotion[] allEmos = null;

			string emotion = string.Empty;
			int ty = -1;
			TextView text = FindViewById<TextView>(Resource.Id.PlayLayout_LrcText);
			try
			{
				using (System.IO.Stream imageStream = System.IO.File.OpenRead(CameraHelper._file.Path))
				{
					allEmos = await emotionServiceClient.RecognizeAsync(imageStream);
				}
				var aEmo = allEmos.FirstOrDefault();
				float[] emos = new float[8];
				emos[0] = aEmo.Scores.Anger;
				emos[1] = aEmo.Scores.Contempt;
				emos[2] = aEmo.Scores.Disgust;
				emos[3] = aEmo.Scores.Fear;
				emos[4] = aEmo.Scores.Happiness;
				emos[5] = aEmo.Scores.Neutral;
				emos[6] = aEmo.Scores.Sadness;
				emos[7] = aEmo.Scores.Surprise;

				text.Text = string.Empty;
				text.Text += "Anger: " + emos[0].ToString() + System.Environment.NewLine;
				text.Text += "Contempt: " + emos[1].ToString() + System.Environment.NewLine;
				text.Text += "Disgust: " + emos[2].ToString() + System.Environment.NewLine;
				text.Text += "Fear: " + emos[3].ToString() + System.Environment.NewLine;
				text.Text += "Happiness: " + emos[4].ToString() + System.Environment.NewLine;
				text.Text += "Neutral: " + emos[5].ToString() + System.Environment.NewLine;
				text.Text += "Sadness: " + emos[6].ToString() + System.Environment.NewLine;
				text.Text += "Surprise: " + emos[7].ToString();

				for (int i = 0; i < 8; i++)
				{
					if (emos.Max().Equals(emos[i]))
					{
						ty = i;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Toast.MakeText(null, "未检测到人脸，请重试", ToastLength.Long).Show();
				Finish();
			}
			Dictionary<string, string> musicInfo = new Dictionary<string, string>();
			try
			{
				musicInfo = await client.GetMusicAsync(ty);
				Toast.MakeText(this, musicInfo.Keys.FirstOrDefault(), ToastLength.Long).Show();
				MusicBoss.CurrentMusicManager.Add(musicInfo);
				switch (ty)
				{
				case 0:
					{
						emotion = "Anger";
						break;
					}
				case 1:
					{
						emotion = "Contempt";
						break;
					}
				case 2:
					{
						emotion = "Disgust";
						break;
					}
				case 3:
					{
						emotion = "Fear";
						break;
					}
				case 4:
					{
						emotion = "Happiness";
						break;
					}
				case 5:
					{
						emotion = "Neutral";
						break;
					}
				case 6:
					{
						emotion = "Sadness";
						break;
					}
				case 7:
					{
						emotion = "Surprise";
						break;
					}
				}
				Toast.MakeText(this, emotion, ToastLength.Long).Show();

				//FindViewById<TextView>(Resource.Id.PlayLayout_Top_Text).Text = musicInfo["name"];

				FindViewById<TextView>(Resource.Id.PlayLayout_Top_Text).Text = MusicBoss.CurrentMusicManager.GetName();
				nameT = MusicBoss.CurrentMusicManager.GetName();//.musicInfo["name"];
				Intent intent = new Intent(this, typeof(PlayService));
				intent.PutExtra("act", "play");
				//intent.PutExtra("url", musicInfo["url"]);
				intent.PutExtra("url", MusicBoss.CurrentMusicManager.GetUrl());
				StartService(intent);
				stateNow = "play";

			}
			catch (Exception e)
			{
				Toast.MakeText(this, "获取音乐失败，请检查网络连接后重试", ToastLength.Long).Show();
				//Toast.MakeText(this, e.Message, ToastLength.Long).Show();
				//text.Text = e.Message;
				//Finish();
			}
		}

		public override void Finish()
		{
			Intent intent = new Intent();
			intent.PutExtra("state", stateNow);
			intent.PutExtra("name", nameT);
			SetResult((Result)ActivityManager.Activities.EmoActivity, intent);

			GC.Collect();
			base.Finish();
			OnDestroy();
		}
	}
}