using System;
using System.Drawing;
using System.IO;
using System.Timers;
using static PictureMixer.ImageProcessTool;
namespace PictureMixer
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathC = @GetTime();
            Console.WriteLine("输入表层图片的路径");
            string pathA = Console.ReadLine();
            Console.WriteLine("输入底层图片的路径");
            string pathB = Console.ReadLine();
            string pathsave = pathC + ".png";
            Bitmap bitmap = new Bitmap(1, 1);
            DateTime time1 = new();
            time1 = TimeStopStart();
            bitmap = ToMixtureImage(pathA, pathB, bitmap);
            TimeStopStop(time1);
            try
            {
                bitmap.Save(pathsave);
            }
            catch (Exception e)
            {
                Console.WriteLine("保存失败"+e);
            }
        }

        private static string GetTime()
        {
            DateTime _dateTime =DateTime.Now;
            _dateTime.ToLocalTime();
            string pathC = _dateTime.Date + "" + _dateTime.Hour + _dateTime.Minute + _dateTime.Second + "";
            pathC = pathC.Replace('/','-');
            pathC = pathC.Replace(' ', '-');
            pathC = pathC = pathC.Replace(':', '-');
            return pathC;
        }

        static DateTime TimeStopStart()
        {
            DateTime beforDT = System.DateTime.Now;
            return beforDT;
        }
        static private void TimeStopStop(DateTime beforeDT)
        {
            DateTime _afterDT = System.DateTime.Now;
            TimeSpan ts = _afterDT.Subtract(beforeDT);
            Console.WriteLine("DateTime总共花费{0}s.", ts.TotalSeconds);
        }
    }
}

