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
using Android.Gestures;

namespace EmotionMusic
{
	public class GestureListView : ListView
	{
		Context context;
		GestureDetector gestureDetector;
		IOnFlingListener mListener;

		/*
		 * 设置左右滑动监听
		 * */
		public void SetOnFlingListener(IOnFlingListener listener)
		{
			mListener = listener;
			gestureDetector = new GestureDetector(context, new Gesture(context, mListener));
		}

		public GestureListView(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
			this.context = context;
		}

		public GestureListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			this.context = context;
		}

		public GestureListView(Context context) :
			base(context)
		{
			this.context = context;
		}

		public override bool OnTouchEvent(MotionEvent ev)
		{

			if (gestureDetector.OnTouchEvent(ev))
				return true;//当左右滑动时自己处理
			return base.OnTouchEvent(ev);
		}

		/*
		 * 滑动监听
		 * */
		public class Gesture : GestureDetector.IOnGestureListener
		{
			Context context;
			IOnFlingListener mListener;

			IntPtr IJavaObject.Handle => throw new NotImplementedException();

			public Gesture(Context context, IOnFlingListener listener)
			{
				this.context = context;
				this.mListener = listener;
			}

			public bool OnDown(MotionEvent e)
			{
				return false;
			}

			public void OnShowPress(MotionEvent e)
			{

			}

			public bool OnSingleTapUp(MotionEvent e)
			{
				return false;
			}

			public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
			{
				return false;
			}

			public void OnLongPress(MotionEvent e)
			{

			}


			public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX,
					float velocityY)
			{
				if (Math.Abs(e1.GetX() - e2.GetX()) > Math.Abs(e1.GetY()
						- e2.GetY()))
				{//当左右滑动距离大于上下滑动距离时才认为是左右滑
				 // 左滑
					if (e1.GetX() - e2.GetX() > 100)
					{
						mListener.OnLeftFling();
						return true;
					}
					// 右滑
					else if (e1.GetX() - e2.GetX() < -100)
					{
						mListener.OnRightFling();
						return true;
					}
				}
				return true;
			}
			
			public void Dispose()
			{
				//throw new NotImplementedException();
			}
		}

		/*
		 * 左右滑动时调用的监听接口
		 * */
		public interface IOnFlingListener
		{
			void OnLeftFling();
			void OnRightFling();
		}
	}
}