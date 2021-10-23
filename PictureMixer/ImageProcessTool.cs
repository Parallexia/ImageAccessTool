using System;
using System.Drawing;

namespace PictureMixer
{
    public class ImageProcessTool {
        public struct SizeOfImage
        {
            internal long width;
            internal long height;
        }
        /*public static void ImportImage(string str)
        {
            try
            {
                Bitmap bitmap = new Bitmap(str);
            }
            catch (Exception)
            {

                Console.WriteLine("你输入的路径不正确"); ;
            }

        }
*/
        public static SizeOfImage GetImageSize(Bitmap bitmap)
        {
            SizeOfImage size;
            try
            {
                size.width = bitmap.Width;
                size.height = bitmap.Height;
                return size;
            }
            catch
            {
                Console.WriteLine("没有成功读取图片的属性");
                size.width = 0;
                size.height = 0;
                return size;
            }
        }
        
        public static Bitmap ChangeImageScale(long x, long y, Bitmap bitmap)
        {
            int width, width1;
            int height, height1;
            width1 = bitmap.Width;
            height1 = bitmap.Height;
            try
            {
                width = (int)x;
                height = (int)y;

            }
            catch (Exception)
            {
                Console.WriteLine("图片大小变化未成功");
                return bitmap;
            }
            Bitmap newBitmap = new Bitmap(bitmap, width, height);
            width = bitmap.Width;
            height = bitmap.Height;
            Console.WriteLine("x轴变化倍数:" + (newBitmap.Width / width));
            Console.WriteLine("Y轴变化倍数:" + (newBitmap.Height / height));
            return newBitmap;
        }
        public static Bitmap ChangeImageScale(int x, int y, Bitmap bitmap)
        {
            int width,width1;
            int height,height1;
            width1 = bitmap.Width;
            height1 = bitmap.Height;
            try
            {
                width = (int)x;
                height = (int)y;

            }
            catch (Exception)
            {
                Console.WriteLine("图片大小变化未成功");
                return bitmap;
            }
            bitmap.SetResolution(width, height);
            Console.WriteLine("x轴变化倍数:" + (width1 / width));
            Console.WriteLine("Y轴变化倍数:" + (height1 / height));
            return bitmap;
        }
        //只修改了这个
        public static Bitmap ChangeImageScale(SizeOfImage sizeofImage, Bitmap bitmap)
        {
            Console.WriteLine(bitmap.Tag);
            int width, width1;
            int height, height1;
            width1 = bitmap.Width;
            height1 = bitmap.Height;
            try
            {
                width = (int)sizeofImage.width;
                height = (int)sizeofImage.height;

            }
            catch (Exception)
            {
                Console.WriteLine("图片大小变化未成功");
                return bitmap;
            }
            Size size = new Size();
            size.Width = width;
            size.Height = height;
            bitmap = new Bitmap(bitmap,size);
            double x = (width / (width1 + 0.0));
            double y = (height / (height1 + 0.0));
            Console.WriteLine("x轴变化倍数:" + x);
            Console.WriteLine("Y轴变化倍数:" + y);
            return bitmap;
        }
        public static Bitmap ChangeImageScale(int x, Bitmap bitmap)
        {
            bitmap = ChangeImageScale(x, x,bitmap);
            return bitmap;
        }
        public static Bitmap ToGray(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    //利用公式计算灰度值
                    int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }
        public static Bitmap CreateMixedImage(Bitmap bitmapLayer, Bitmap bitmapBelow,Bitmap bitmap)
        {
            //Color piexlLayer, piexlBelow;
            SizeOfImage layerSize = GetImageSize(bitmapLayer);//缩略图打开时
            SizeOfImage belowSize = GetImageSize(bitmapBelow);//原图打开时
            int alpha, grey, greyBelow, greyLayer;

            if ((layerSize.height * layerSize.width) > (belowSize.height * belowSize.width))
            { bitmapBelow = ChangeImageScale(layerSize, bitmapBelow); }
            else
            { bitmapLayer = ChangeImageScale(belowSize, bitmapLayer); }
            layerSize = GetImageSize(bitmapLayer);//缩略图打开时

            bitmap = ChangeImageScale(layerSize,bitmap);


            for (int height = 1; height < layerSize.height; height++)
            {
                for (int width = 1; width < layerSize.width; width++)
                {
                    //piexlBelow = bitmapBelow.GetPixel(width, height);
                    //piexlLayer = bitmapLayer.GetPixel(width, height);

                    greyBelow = (short)
                         (bitmapBelow.GetPixel(width, height).R * 0.3
                        + bitmapBelow.GetPixel(width, height).G * 0.59
                        + bitmapBelow.GetPixel(width, height).B * 0.11);
                    greyBelow = (int)((greyBelow/256F)*128); 
                    greyLayer = (short)
                         (bitmapLayer.GetPixel(width, height).R * 0.3
                        + bitmapLayer.GetPixel(width, height).G * 0.59
                        + bitmapLayer.GetPixel(width, height).B * 0.11);

                    greyLayer = (int)((greyLayer/256F)*128 + 127);
                    //a(mix) = 1 - r1 + r2

                    alpha = 255 - (greyLayer - greyBelow);
                    //r(mix) = r(2)/a(mix)

                    float greyfloat =  255 * (greyBelow/(alpha+0.01F));
                    grey = (int)Math.Ceiling(greyfloat);
                    Color color = Color.FromArgb(alpha, grey, grey, grey);
                    try
                    {
                        bitmap.SetPixel(width, height, color);
                    }
                    catch 
                    {
                        Console.WriteLine("图片生成错误");
                    }
                }
            }


            return bitmap;
        }
        public static Bitmap ToMixtureImage(string pathLayer,string pathBelow,Bitmap bitmap)
        {


            {
                Bitmap bitmapA = new Bitmap(pathLayer);
                Bitmap bitmapB = new Bitmap(pathBelow);
                bitmap = CreateMixedImage(bitmapA, bitmapB, bitmap);
                return bitmap;
            }

        }
    }
}
