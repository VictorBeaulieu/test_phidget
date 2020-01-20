
using System;
using System.IO;
using System.Threading;



namespace test_phidget
{

    class Program
    {
        private static bool mainRun = false;


        static int Main(string[] args)
        {

            int res = 0;

            if (File.Exists("/media/usb/info.txt"))
            {
                mainRun = false;
                string[] config = File.ReadAllLines("/media/usb/info.txt");
                try
                {
                    for (int i = 0; i < Int16.Parse(config[1]); i++)
                    {
                        File.Copy("/media/usb/tracks/track_short_" + (1 + i).ToString() + ".wav", "/home/pi/tracks_2/track_short_" + (1 + i).ToString() + ".wav", true);
                    }
                    for (int i = 0; i < Int16.Parse(config[1]); i++)
                    {
                        File.Copy("/media/usb/tracks/track_long_" + (1 + i).ToString() + ".wav", "/home/pi/tracks/track_long_" + (1 + i).ToString() + ".wav", true);
                    }
                }
                catch (Exception)
                {
                    res = -1;
                    throw;
                }


            }
            else mainRun = true;

            if(mainRun)
            {
                Blink blink = new Blink(20);
                Thread blink_thread = new Thread(new ThreadStart(blink.k200_task));
                blink_thread.Start();


                AreaTouch area = new AreaTouch(4);
                Thread newThread = new Thread(new ThreadStart(area.check_area));
                newThread.Start();
                while (true) ;
            }          




            Console.WriteLine("stop program");
            return res;
        }



    }
}
