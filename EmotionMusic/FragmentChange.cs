﻿using System;
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
			fragments = new Fragment[3] { new SettingFragment(), new EmoFragment(), new MainFragment()};
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
			//transaction.Add(Resource.Id.MainLayout_Body, fragments[3]);;
			transaction.SetTransition(FragmentTransit.FragmentFade);
			transaction.Add(Resource.Id.MainLayout_Body, fragments[1]);
			transaction.Add(Resource.Id.MainLayout_Body, fragments[0]);
			transaction.Add(Resource.Id.MainLayout_Body, fragments[2]);
			transaction.Hide(fragments[0]);
			transaction.Hide(fragments[1]);
			transaction.Hide(fragments[2]);
			transaction.Show(fragments[1]);
			transaction.Commit();
			fragmentI = 1;
		}

		public MainFragment MainFragment { get => fragments[1] as MainFragment; }
		//public MineFragment MineFragment { get => fragments[2] as MineFragment; }
		public SettingFragment SettingFragment { get => fragments[0] as SettingFragment; }
		public EmoFragment EmoFragment { get => fragments[2] as EmoFragment; }

		public bool ToRight()
		{
			if (fragmentI >= 2) return false;
			transaction.SetCustomAnimations(Resource.Animation.fragment_slide_right_in, Resource.Animation.fragment_slide_left_out);
			//transaction.SetTransition(FragmentTransit.FragmentFade);
			transaction.Hide(fragments[fragmentI]);
			fragmentI++;
			transaction.Show(fragments[fragmentI]);
			transaction.Commit();
			return true;
		}

		public bool ToLeft()
		{
			if (fragmentI <= 0) return false;
			transaction.SetCustomAnimations(Resource.Animation.fragment_slide_left_in, Resource.Animation.fragment_slide_right_out);
			//transaction.SetTransition(FragmentTransit.FragmentFade);
			transaction.Hide(fragments[fragmentI]);
			fragmentI--;
			transaction.Show(fragments[fragmentI]);
			transaction.Commit();
			return true;
		}

		public void ShowMainFragment()
		{
			//transaction.SetCustomAnimations(Resource.Animation.fragment_slide_right_in, Resource.Animation.fragment_slide_left_out, Resource.Animation.fragment_slide_left_in, Resource.Animation.fragment_slide_right_out);
			//transaction.SetTransition(FragmentTransit.FragmentFade);
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[2]);
			transaction.Commit();
			fragmentI = 2;
		}

		public void ShowMineFragment()
		{
			//transaction.SetCustomAnimations(Resource.Animation.fragment_slide_right_in, Resource.Animation.fragment_slide_left_out, Resource.Animation.fragment_slide_left_in, Resource.Animation.fragment_slide_right_out);
			//transaction.SetTransition(FragmentTransit.FragmentFade);
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[2]);
			transaction.Commit();
			fragmentI = 2;
		}

		public void ShowSettingFragment()
		{
			//transaction.SetCustomAnimations(Resource.Animation.fragment_slide_right_in, Resource.Animation.fragment_slide_left_out, Resource.Animation.fragment_slide_left_in, Resource.Animation.fragment_slide_right_out);
			//transaction.SetTransition(FragmentTransit.FragmentFade);
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[0]);
			transaction.Commit();
			fragmentI = 0;
		}

		public void ShowEmoFragment()
		{
			//transaction.SetCustomAnimations(Resource.Animation.fragment_slide_right_in, Resource.Animation.fragment_slide_left_out, Resource.Animation.fragment_slide_left_in, Resource.Animation.fragment_slide_right_out);
			//transaction.SetTransition(FragmentTransit.FragmentFade);
			transaction.Hide(fragments[fragmentI]);
			transaction.Show(fragments[1]);
			transaction.Commit();
			fragmentI = 1;
		}
	}
}