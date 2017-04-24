using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using Android.Gestures;

namespace EmotionMusic
{
	public class MainFragment : Fragment
	{
		MusicManager musicManager;
		//GestureDetector detector;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var v = inflater.Inflate(Resource.Layout.MainFragment, container, false);
			return v;
		}

		public override void OnStart()
		{
			base.OnStart();
			//musicManager = (Activity as MainActivity).CloudMusicManager;
			if ((Activity as MainActivity).isExit)
			{
				GetMyListAsync();
				(Activity as MainActivity).isExit = false;
			}
			//detector = new GestureDetector(this);
		}

		public async void GetMyListAsync()
		{
			musicManager = new MusicManager();
			ListView listView = View.FindViewById<ListView>(Resource.Id.MainFragment_RecommendMusicListView);

			string[] name = null;
			try
			{
				ClientClass client = new ClientClass();
				var ss = await client.GetMyMusicListAsync();
				name = ss.Keys.ToArray();
				musicManager.Add(ss);
			}
			catch (Exception ex)
			{
				name = new string[2];
				name[0] = "Internet error";
				name[1] = ex.Message;
			}
			try
			{
				listView.Adapter = null;
				//listView.AddHeaderView(Activity.FindViewById(Resource.Id.CameraButtonListView_Button));
				listView.Adapter = new ArrayAdapter<string>(Activity, Resource.Layout.RecommendMusic, Resource.Id.RecommendMusic_TextView, name);
			}
			catch (Exception e)
			{
				Toast.MakeText(Activity, e.Message, ToastLength.Long);
			}
			listView.ItemClick += ListViewClick;
			//(Activity as MainActivity).CloudMusicManager = musicManager;
			MusicBoss.CurrentMusicManager = musicManager;
		}

		private void ListViewClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			try
			{
				var url = MusicBoss.CurrentMusicManager.GetUrl((int)e.Id);
				var name = MusicBoss.CurrentMusicManager.GetName((int)e.Id);
				if (name.Contains("Internet error")) return;

				var act = Activity as MainActivity;
				act.PlayMusic(name, url);
			}
			catch (Exception ex)
			{
				Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
			}
		}
	}
}