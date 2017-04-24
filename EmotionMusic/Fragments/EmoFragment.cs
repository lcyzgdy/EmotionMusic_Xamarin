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
	public class EmoFragment : Fragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.EmoFragment, container, false);
		}

		public override void OnStart()
		{
			base.OnStart();
			var button = View.FindViewById<Button>(Resource.Id.EmoFragment_Button);
			button.Click -= EmoFragment_Click;
			button.Click += EmoFragment_Click;
		}

		private void EmoFragment_Click(object sender, EventArgs e)
		{
			(Activity as MainActivity).OpenCamera();
		}
	}
}