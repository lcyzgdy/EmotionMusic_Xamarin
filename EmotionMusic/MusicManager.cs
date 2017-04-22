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
		private Dictionary<string, string> currentMusicList;

		public MusicManager()
		{
			musicList = new Dictionary<string, string>();
			currentMusicList = new Dictionary<string, string>();
		}

		public void Add(Dictionary<string, string> ss)
		{
			musicList = ss;
		}

		public void AddCurrent(Dictionary<string, string> ss)
		{
			currentMusicList = ss;
		}

		public void Add(string _name, string _url)
		{
			currentMusicList.Add(_name, _url);
			if (!musicList.ContainsValue(_url))
			{
				musicList.Add(_name, _url);
			}
		}

		public string GetUrl(string name)
		{
			return musicList[name];
		}

		public string GetUrl(int id)
		{
			return musicList.Values.ToArray()[id];
		}

		public string GetName(int id)
		{
			return musicList.Keys.ToArray()[id];
		}

		public void ClearCurrent()
		{
			currentMusicList.Clear();
		}
	}
}