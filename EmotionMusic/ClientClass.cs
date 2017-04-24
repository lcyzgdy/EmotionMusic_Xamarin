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
using System.Net.Http;
using System.Threading.Tasks;

namespace EmotionMusic
{
	class ClientClass
	{
		private string baseUrl;
		private string url;
		private HttpClient client;
		private int valueCount;

		public ClientClass()
		{
			baseUrl = "http://123.206.23.102/emotionmusic/test";
			client = new HttpClient();
			valueCount = 0;
		}

		public void Add(string name, string value)
		{
			if (valueCount == 0)
			{
				url = baseUrl + "/?" + name + "=" + value;
				valueCount++;
			}
			else
			{
				url += "&" + name + "=" + value;
				valueCount++;
			}
		}

		public void Add(string name, float value)
		{
			if (valueCount == 0)
			{
				url = baseUrl + "/?" + name + "=" + Convert.ToString(value);
				valueCount++;
			}
			else
			{
				url += "&" + name + "=" + Convert.ToString(value);
				valueCount++;
			}
		}

		private void Clear()
		{
			url = string.Empty;
			valueCount = 0;
		}

		public async Task<Dictionary<string, string>> GetMyMusicListAsync()
		{
			Clear();
			var url = baseUrl + "/?act=1";
			var x = await client.GetAsync(url);
			var result = await x.Content.ReadAsStringAsync();			
			var pairs = result.Split(',');
			Dictionary<string, string> musics = new Dictionary<string, string>();
			foreach (var i in pairs)
			{
				var j = i.Split('|');
				musics.Add(j[0], j[1]);
			}
			Clear();
			return musics;
		}

		public async Task<Dictionary<string, string>> GetNeutralMusicAsync()
		{
			Clear();
			var url = baseUrl + "/?act=2&emotion=neutral";
			var response = await client.GetAsync(url);
			var result = await response.Content.ReadAsStringAsync();
			var pairs = result.Split('|');
			Dictionary<string, string> music = new Dictionary<string, string>
			{
				{ pairs[0], pairs[1] }
			};
			return music;
		}

		public async Task<Dictionary<string, string>> GetMusicAsync(int emotionI)
		{
			string emotion = string.Empty;
			switch (emotionI)
			{
				case 0:
					{
						emotion = "anger";
						break;
					}
				case 1:
					{
						emotion = "contempt";
						break;
					}
				case 2:
					{
						emotion = "disgust";
						break;
					}
				case 3:
					{
						emotion = "fear";
						break;
					}
				case 4:
					{
						emotion = "happiness";
						break;
					}
				case 5:
					{
						emotion = "neutral";
						break;
					}
				case 6:
					{
						emotion = "sadness";
						break;
					}
				case 7:
					{
						emotion = "surprise";
						break;
					}
			}
			Clear();
			var url = baseUrl + "/?act=2&emotion=" + emotion;
			var response = await client.GetAsync(url);
			Toast.MakeText(null, response.StatusCode.ToString(), ToastLength.Long).Show();
			var result = await response.Content.ReadAsStringAsync();
			var pairs = result.Split(',');
			Dictionary<string, string> music = new Dictionary<string, string>();
			foreach (var i in pairs)
			{
				var nameUrl = i.Split('|');
				music.Add(nameUrl[0], nameUrl[1]);
			}
			return music;
		}

		public async Task<string> LoginAsync(string username, string password)
		{
			Clear();
			var url = baseUrl + "/?act=3&username=" + username + "&password=" + password;
			var response = await client.GetAsync(url);
			var result = await response.Content.ReadAsStringAsync();
			return result;
		}

		public async Task<bool> LogoutAsync(string userid)
		{
			Clear();
			var url = baseUrl + "/?act=4&userid=" + userid;
			var response = await client.GetAsync(url);
			var result = await response.Content.ReadAsStringAsync();
			return result.Equals("OK");
		}

		public async Task<bool> RegisterAsycn(string username, string password)
		{
			Clear();
			var url = baseUrl + "/?act=4&username=" + username + "&password=" + password;
			var response = await client.GetAsync(url);
			var result = await response.Content.ReadAsStringAsync();
			return result.Equals("OK");
		}

		private async Task<string> GetAsync()
		{
			Uri uri = new Uri(url);
			string value = string.Empty;
			try
			{
				var response = await client.GetAsync(uri);
				value = await response.Content.ReadAsStringAsync();
			}
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
			catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
			{

			}
			return value;
		}
	}
	/* 1 获取列表
	 * 2 情绪识别
	 * 3 登录
	 * 4 注册*/

	enum Actions
	{
		get_list,
		post_setting
	}
}