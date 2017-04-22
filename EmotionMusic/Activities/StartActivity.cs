using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Threading;

namespace EmotionMusic
{
	[Activity(Label = "EmotionMusic", MainLauncher = true, Icon = "@drawable/mainicon", Theme ="@style/AppTheme")]
	public class StartActivity : Activity
	{
		private ISharedPreferences preference;
		private ISharedPreferencesEditor editor;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Create your application here
			preference = GetSharedPreferences("EmotionMusic", FileCreationMode.Private);
			if (preference.GetBoolean("FirstStart", true))
			{
				RequestWindowFeature(WindowFeatures.NoTitle);
				SetContentView(Resource.Layout.Start);
				editor = preference.Edit();
				editor.PutBoolean("FirstStart", false);
				editor.Commit();
				//Toast.MakeText(this, "TRUE", ToastLength.Long).Show();
				new Handler().PostDelayed(new System.Action(() =>
				{
					LoadMainActivity();
					Finish();
				}), 2000);
			}
			else
			{
				//Toast.MakeText(this, "FALSE", ToastLength.Long).Show();
				LoadMainActivity();
				Finish();
			}
		}

		private void LoadMainActivity()
		{
			var intent = new Intent(this, typeof(MainActivity));
			StartActivity(intent);
			//Toast.MakeText(this, "Start", ToastLength.Long).Show();
		}
	}
}