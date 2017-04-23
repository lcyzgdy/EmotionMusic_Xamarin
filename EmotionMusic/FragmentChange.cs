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
	class FragmentChange
	{
		FragmentTransaction transaction;
		Fragment[] fragments;
		int fragmentI;

		public FragmentChange()
		{
			fragments = new Fragment[4] { new SettingFragment(), new MainFragment(), new MineFragment(), new EmoFragment() };
			//mainFragment = new MainFragment();
			//mineFragment = new MineFragment();
			fragmentI = 1;
		}

		public void SetManager(FragmentTransaction fm)
		{
			transaction = fm;
		}

		public void Show()
		{
			//var transaction = FragmentManager.BeginTransaction();
			//transaction.Add(Resource.Id.MainLayout_Body, fragments[fragmentI]);
			transaction.Add(Resource.Id.MainLayout_Body, fragments[3]);
			transaction.Add(Resource.Id.MainLayout_Body, fragments[1]);
			transaction.Add(Resource.Id.MainLayout_Body, fragments[0]);
			transaction.Add(Resource.Id.MainLayout_Body, fragments[2]);
			transaction.Commit();
			fragmentI = 3;
		}

		public MainFragment MainFragment { get => fragments[1] as MainFragment; }
		public MineFragment MineFragment { get => fragments[2] as MineFragment; }
		public SettingFragment SettingFragment { get => fragments[0] as SettingFragment; }
		public EmoFragment EmoFragment { get => fragments[3] as EmoFragment; }

		public bool ToRight()
		{
			if (fragmentI >= 2) return false;
			fragmentI++;
			return true;
		}

		public bool ToLeft()
		{
			if (fragmentI <= 0) return false;
			fragmentI--;
			return true;
		}

		public void ShowMainFragment()
		{
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[1]);
			transaction.Commit();
			fragmentI = 1;
		}

		public void ShowMineFragment()
		{
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[2]);
			transaction.Commit();
			fragmentI = 2;
		}

		public void ShowSettingFragment()
		{
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[0]);
			transaction.Commit();
			fragmentI = 0;
		}

		public void ShowEmoFragment()
		{
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[3]);
			transaction.Commit();
			fragmentI = 3;
		}
	}
}