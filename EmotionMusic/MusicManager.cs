using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Database;
using Android.Database.Sqlite;

namespace EmotionMusic
{
	class MusicManager
	{
		/// <summary>
		/// Key: name, Value: url
		/// </summary>
		private Dictionary<string, string> musicList;
		//private Dictionary<string, string> currentMusicList;
		private List<string> name;
		private List<string> url;
		private int cur;

		public MusicManager()
		{
			musicList = new Dictionary<string, string>();
			name = new List<string>();
			url = new List<string>();
			cur = 0;
			//currentMusicList = new Dictionary<string, string>();
		}

		public void Add(Dictionary<string, string> ss)
		{
			musicList = ss;
			name = new List<string>();
			url = new List<string>();
			foreach (var i in ss)
			{
				name.Add(i.Key);
				url.Add(i.Value);
			}
			cur = 0;
		}

		/*public void AddCurrent(Dictionary<string, string> ss)
		{
			currentMusicList = ss;
		}
		*/
		public void Add(string _name, string _url)
		{
			//currentMusicList.Add(_name, _url);
			if (!musicList.ContainsValue(_url))
			{
				musicList.Add(_name, _url);
				name.Add(_name);
				url.Add(_url);
			}
			cur = 0;
		}

		public string GetUrl(string _name)
		{
			cur = name.FindIndex(delegate (string l) { return l == _name; });
			return musicList[_name];
		}

		public string GetUrl()
		{
			return url[cur];
		}

		public string GetUrl(int id)
		{
			//return musicList.Values.ToArray()[id];
			cur = id;
			return url[id];
		}

		public string GetName()
		{
			return name[cur];
		}

		public string GetName(int id)
		{
			//return musicList.Keys.ToArray()[id];
			return name[id];
		}

		public KeyValuePair<string, string> GetPre()
		{
			cur--;
			if (cur< 0) cur = url.Count - 1;
			return new KeyValuePair<string, string>(name[cur], url[cur]);
		}

		public KeyValuePair<string, string> GetNext()
		{
			cur++;
			if (cur >= url.Count) cur = 0;
			return new KeyValuePair<string, string>(name[cur], url[cur]);
		}
	}

	static class MusicBoss
	{
		static MusicManager currentMusicManager;// = new MusicManager();

		internal static MusicManager CurrentMusicManager { get => currentMusicManager; set => currentMusicManager = value; }
	}
}