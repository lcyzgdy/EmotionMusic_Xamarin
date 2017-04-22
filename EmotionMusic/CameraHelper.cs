using Android.Graphics;
using Java.IO;

namespace EmotionMusic
{
    class CameraHelper
    {
        public static File _file;
        public static File _dir;
#pragma warning disable CS0649 // 从未对字段“CameraHelper.bitmap”赋值，字段将一直保持其默认值 null
        public static Bitmap bitmap;
#pragma warning restore CS0649 // 从未对字段“CameraHelper.bitmap”赋值，字段将一直保持其默认值 null

        public static void CreateDirectoryForPictures()
        {
            _dir = new File(
                 Android.OS.Environment.GetExternalStoragePublicDirectory(
                     Android.OS.Environment.DirectoryPictures), "Emotions");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        public static Bitmap LoadAndResizeBitmap(string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);
            return resizedBitmap;
        }
    }
}