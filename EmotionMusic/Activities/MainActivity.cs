using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Threading;
using Android.Gestures;
using System.Threading.Tasks;

namespace EmotionMusic
{
	[Activity(Label = "EmotionMusic_MainActivity")]
	public class MainActivity : Activity
	{
		ClientClass client = new ClientClass();
		MusicManager cloudMusicManager = new MusicManager();
		MusicManager localMusicManager = new MusicManager();
		FragmentChange fragmentChange;
		//MainFragment mainFragment = new MainFragment();
		//MineFragment mineFragment = new MineFragment();
		bool isPlaying = false;
		public bool isExit = true;

		internal MusicManager LocalMusicManager { get => localMusicManager; set => localMusicManager = value; }
		internal MusicManager CloudMusicManager { get => cloudMusicManager; set => cloudMusicManager = value; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);			
			SetContentView(Resource.Layout.MainLayout_Relative);

			//Window.AddFlags(WindowManagerFlags.TranslucentStatus);
			//Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
			Window.AddFlags(WindowManagerFlags.Fullscreen);

			fragmentChange = new FragmentChange();
			fragmentChange.SetManager(FragmentManager.BeginTransaction());
			fragmentChange.Show();

			CameraHelper.CreateDirectoryForPictures();

			new Thread(new ThreadStart(() =>
			{
				Intent intent = new Intent(this, typeof(PlayService));
				StartService(intent);

				Intent appIntent = new Intent(this, typeof(AppService));
				StartService(appIntent);
			})).Start();

			FindViewById<RelativeLayout>(Resource.Id.MainLayout_Foot).Touch += FootChangeColor;
			FindViewById<RadioGroup>(Resource.Id.MainLayout_RadioGroup).CheckedChange += RadioGroup_CheckedChange;
		}

		private void RadioGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
		{
			fragmentChange.SetManager(FragmentManager.BeginTransaction());
			//Toast.MakeText(this, e.CheckedId.ToString(), ToastLength.Long).Show();
			switch (e.CheckedId)
			{
				case Resource.Id.MainLayout_RadioButton0:
					{
						fragmentChange.ShowSettingFragment();
						break;
					}
				case Resource.Id.MainLayout_RadioButton1:
					{
						fragmentChange.ShowMainFragment();
						break;
					}
				case Resource.Id.MainLayout_RadioButton2:
					{
						fragmentChange.ShowMineFragment();
						break;
					}
				case Resource.Id.MainLayout_RadioButton3:
					{
						//OpenCamera();
						//(sender as RadioGroup).Check(Resource.Id.MainLayout_RadioButton1);
						fragmentChange.ShowEmoFragment();
						break;
					}
				default: break;
			}
		}

		private void FootChangeColor(object sender, View.TouchEventArgs e)
		{
			var l = sender as RelativeLayout;
			switch (e.Event.Action)
			{
				case MotionEventActions.Down:
					{
						l.Background = new ColorDrawable(new Color(222, 222, 222, 222));
						break;
					}
				case MotionEventActions.Up:
					{
						l.Background = new ColorDrawable(new Color(247, 247, 247, 247));
						Intent intent;
						intent = new Intent(this, typeof(PlayActivity));
						var nameText = FindViewById<TextView>(Resource.Id.MainLayout_Foot_Text1);
						var authorText = FindViewById<TextView>(Resource.Id.MainLayout_Foot_Text2);
						if (nameText.Text.Equals(string.Empty)) break;
						intent.PutExtra("name", nameText.Text);
						intent.PutExtra("author", authorText.Text);
						intent.PutExtra("isPlaying", isPlaying);
						StartActivityForResult(intent, (int)ActivityManager.Activities.PlayActivity);

						break;
					}
			}
		}
		
		private void GetMyList()
		{
			var v = FragmentManager.FindFragmentById<MainFragment>(Resource.Id.MainLayout_Body);
			v.GetMyListAsync();
		}

		private void OpenCamera(object sender, EventArgs e)
		{
			try
			{
				var intent = new Intent(MediaStore.ActionImageCapture);
				CameraHelper._file = new Java.IO.File(CameraHelper._dir, string.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
				intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(CameraHelper._file));
				StartActivityForResult(intent, (int)ActivityManager.Activities.Camera);
			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
			}
		}

		private void OpenCamera()
		{
			//try
			//{
			var intent = new Intent(MediaStore.ActionImageCapture);
			CameraHelper._file = new Java.IO.File(CameraHelper._dir, string.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(CameraHelper._file));
			StartActivityForResult(intent, (int)ActivityManager.Activities.Camera);
			//}
			//catch (Exception ex)
			//{
			//	Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
			//}
		}

		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			switch (requestCode)
			{
				case (int)ActivityManager.Activities.EmoActivity:
					{
						try
						{
							var state = data.GetStringExtra("state");
							var name = data.GetStringExtra("name");
							if (state.Equals("play"))
							{
								var button = FindViewById<ImageButton>(Resource.Id.MainLayout_PlayButton);
								button.SetImageResource(Android.Resource.Drawable.IcMediaPause);
							}
							if (state.Equals("pause"))
							{
								var button = FindViewById<ImageButton>(Resource.Id.MainLayout_PlayButton);
								button.SetImageResource(Android.Resource.Drawable.IcMediaPlay);
							}
							FindViewById<TextView>(Resource.Id.MainLayout_Foot_Text1).Text = name;
						}
						catch (Exception ex)
						{ }
						break;
					}
				case (int)ActivityManager.Activities.Camera:
					{
						try
						{
							var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
							var contentUri = Android.Net.Uri.FromFile(CameraHelper._file);
							mediaScanIntent.SetData(contentUri);
							SendBroadcast(mediaScanIntent);
							Upload(CameraHelper._file.Path);
						}
						catch (Exception e)
						{
							Toast.MakeText(this, e.Message, ToastLength.Long).Show();
						}
						break;
					}
				case (int)ActivityManager.Activities.PlayActivity:
					{
						var state = data.GetStringExtra("state");
						if (state == null)
						{
							break;
						}
						//var author = data.GetStringExtra("author");
						if (state.Equals("play"))
						{
							var button = FindViewById<ImageButton>(Resource.Id.MainLayout_PlayButton);
							button.SetImageResource(Android.Resource.Drawable.IcMediaPause);
						}
						if (state.Equals("pause"))
						{
							var button = FindViewById<ImageButton>(Resource.Id.MainLayout_PlayButton);
							button.SetImageResource(Android.Resource.Drawable.IcMediaPlay);
						}
						break;
					}
				default:
					{
						break;
					}
			}
			GC.Collect();
		}

		private void Upload(string fileUrl)
		{
			Intent intent = new Intent(this, typeof(EmoActivity));
			intent.PutExtra("fileurl", fileUrl);

			StartActivityForResult(intent, (int)ActivityManager.Activities.EmoActivity);
		}

		public async void PlayMusic(string name, string url)
		{
			TextView nameText = FindViewById<TextView>(Resource.Id.MainLayout_Foot_Text1);

			nameText.Text = name;

			ImageButton button = FindViewById<ImageButton>(Resource.Id.MainLayout_PlayButton);
			button.SetImageResource(Android.Resource.Drawable.IcMediaPause);
			button.Click += PausePlay;

			await Task.Run(new Action(()=>
			{
				Intent intent = new Intent(this, typeof(PlayService));
				intent.PutExtra("act", "play");
				intent.PutExtra("name", name);
				intent.PutExtra("url", url);
				StartService(intent);
			}));
			isPlaying = true;
		}

		private void PausePlay(object sender, EventArgs e)
		{
			Intent intent = new Intent(this, typeof(PlayService));
			intent.PutExtra("act", "pause");
			StartService(intent);

			var button = sender as ImageButton;
			button.SetImageResource(Android.Resource.Drawable.IcMediaPlay);
			try
			{
				button.Click -= PausePlay;
			}
			catch (Exception ex)
			{ }
			button.Click += Replay;
			isPlaying = false;
		}

		private void Replay(object sender, EventArgs e)
		{
			Intent intent = new Intent(this, typeof(PlayService));
			intent.PutExtra("act", "replay");
			StartService(intent);
			var button = sender as ImageButton;
			button.SetImageResource(Android.Resource.Drawable.IcMediaPause);
			try
			{
				button.Click -= Replay;
			}
			catch (Exception ex)
			{ }
			button.Click += PausePlay;
			isPlaying = true;
		}

		protected override void OnDestroy()
		{
			//Toast.MakeText(this, "Exit", ToastLength.Long).Show();
			base.OnDestroy();
			isExit = true;
		}
	}
}

