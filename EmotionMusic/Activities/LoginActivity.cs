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
using System.Threading.Tasks;

namespace EmotionMusic
{
	[Activity(Label = "LoginActivity")]
	public class LoginActivity : Activity
	{
		ClientClass client;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.LoginLayout);
			// Create your application here
			client = new ClientClass();
			Button button = FindViewById<Button>(Resource.Id.LoginLayout_LoginButton);
			button.Click += Login;
		}

		private async void Login(object sender, EventArgs e)
		{
			var username = FindViewById<TextView>(Resource.Id.LoginLayout_Username).Text;
			var password = FindViewById<TextView>(Resource.Id.LoginLayout_Password).Text;
			var userID = await client.LoginAsync(username, password);
			if (userID.Equals("FAILED"))
			{
				Toast.MakeText(this, Resource.String.LoginFailed, ToastLength.Long).Show();
			}
			else
			{
				await Task.Run(new Action(() =>
				{
					ISharedPreferences preference = GetSharedPreferences("EmotionMusic", FileCreationMode.Private);
					ISharedPreferencesEditor editor = preference.Edit();
					editor.PutString("UserID", userID);
					editor.PutString("Username", username);
					editor.Commit();
				}));
				Intent intent = new Intent(this, typeof(MainActivity));
				intent.PutExtra("Username", username);
				intent.PutExtra("Password", password);
				StartActivity(intent);
			}
		}
	}
}