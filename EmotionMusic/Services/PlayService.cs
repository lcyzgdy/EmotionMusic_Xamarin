using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System;

namespace EmotionMusic
{
	[Service]
	class PlayService : Service//, MediaPlayer.IOnPreparedListener, MediaPlayer.IOnCompletionListener
	{
		MediaPlayer mediaPlayer;

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override void OnCreate()
		{
			base.OnCreate();
			if (mediaPlayer == null)
				mediaPlayer = new MediaPlayer();
			mediaPlayer.Error += delegate
			  {
				  mediaPlayer.Reset();
			  };
			mediaPlayer.SetAudioStreamType(Stream.Music);
		}

		[return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			return base.OnStartCommand(intent, flags, startId);
		}
		
		[Obsolete]
		public override void OnStart(Intent intent, int startId)
		{
			base.OnStart(intent, startId);
			string act = null;
			try
			{
				var _act = intent.GetStringExtra("act");
				act = _act;
			}
			catch (Exception e)
			{

			}
			if (act == null) return;
			if (act.Equals(string.Empty)) return;
			if (act.Equals("play"))
			{
				try
				{
					var url = intent.GetStringExtra("url");
					if (url == null) return;
					if (url.Equals(string.Empty)) return;
					
					//if (mediaPlayer.IsPlaying)
					{
						mediaPlayer.Stop();
						mediaPlayer.Reset();
					}
					//GC.Collect();
					//mediaPlayer = new MediaPlayer();
					mediaPlayer.SetAudioStreamType(Stream.Music);
					mediaPlayer.SetDataSource(url);
					mediaPlayer.PrepareAsync();
					mediaPlayer.Prepared -= StartPlay;
					mediaPlayer.Prepared += StartPlay;
					//mediaPlayer.Prepare();
					//mediaPlayer.Start();
				}
				catch (Exception e)
				{ }
			}
			else if (act.Equals("pause"))
			{
				mediaPlayer.Pause();
			}
			else if (act.Equals("stop"))
			{
				mediaPlayer.Stop();
				mediaPlayer.Reset();
			}
			else if (act.Equals("replay"))
			{
				mediaPlayer.Start();
			}
			GC.Collect();
		}

		private void StartPlay(object sender, EventArgs e)
		{
			(sender as MediaPlayer).Start();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			mediaPlayer.Release();
			mediaPlayer = null;
		}

		public void OnPrepared(MediaPlayer mp)
		{
			mp.Start();
		}

		public void OnCompletion(MediaPlayer mp)
		{
			mp.Start();
		}
	}
}