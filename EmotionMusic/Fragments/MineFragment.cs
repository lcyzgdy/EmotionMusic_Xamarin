using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace EmotionMusic
{
	public class MineFragment : Fragment
	{
		MusicManager musicManager;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var v = inflater.Inflate(Resource.Layout.MineFragment, container, false);
			return v;
		}

		public override void OnStart()
		{
			base.OnStart();
			//musicManager = (Activity as MainActivity).LocalMusicManager;
		}
	}
}