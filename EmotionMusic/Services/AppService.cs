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

namespace EmotionMusic
{
	[Service]
	public class AppService : Service
	{
		public override IBinder OnBind(Intent intent)
		{
			//throw new NotImplementedException();
			return null;
		}

#pragma warning disable CS0672 // 成员“AppService.OnStart(Intent, int)”将重写过时的成员“Service.OnStart(Intent, int)”。请向“AppService.OnStart(Intent, int)”中添加 Obsolete 特性。
		public override void OnStart(Intent intent, int startId)
#pragma warning restore CS0672 // 成员“AppService.OnStart(Intent, int)”将重写过时的成员“Service.OnStart(Intent, int)”。请向“AppService.OnStart(Intent, int)”中添加 Obsolete 特性。
		{
#pragma warning disable CS0618 // “Service.OnStart(Intent, int)”已过时:“deprecated”
			base.OnStart(intent, startId);
#pragma warning restore CS0618 // “Service.OnStart(Intent, int)”已过时:“deprecated”
			//Toast.MakeText(this, "appServiceStart", ToastLength.Long).Show();
		}

		public override void OnTaskRemoved(Intent rootIntent)
		{
			base.OnTaskRemoved(rootIntent);
			ISharedPreferences preferences = GetSharedPreferences("EmotionMusic", FileCreationMode.Private);
			ISharedPreferencesEditor editor = preferences.Edit();
			editor.PutBoolean("FirstStart", true);
			editor.Commit();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			ISharedPreferences preferences = GetSharedPreferences("EmotionMusic", FileCreationMode.Private);
			ISharedPreferencesEditor editor = preferences.Edit();
			editor.PutBoolean("FirstStart", true);
			editor.Commit();
			Toast.MakeText(this, "ToTRUE", ToastLength.Long).Show();
		}
	}
}