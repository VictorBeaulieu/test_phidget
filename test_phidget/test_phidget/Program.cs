
using System;
using System.IO;
using System.Threading;



namespace test_phidget
{

    class Program
    {
        private static bool mainRun = false;
        private static int number_short_touch;
        private static int number_long_touch;


        static int Main(string[] args)
        {

            int res = 0;
            
            if (File.Exists("/media/usb/info.txt"))
            {
                Blink blink = new Blink(20);
                Thread blink_thread = new Thread(new ThreadStart(blink.loading_task));
                blink_thread.Start();
                mainRun = false;
                
                try
                {
                    for (int i = 0; i < number_short_touch; i++)
                    {
                        File.Copy("/media/usb/tracks/track_short_" + i.ToString() + ".wav", "/home/pi/tracks/track_short_" + i.ToString() + ".wav", true);
                    }
                    for (int i = 0; i < number_long_touch; i++)
                    {
                        File.Copy("/media/usb/tracks/track_long_" + i.ToString() + ".wav", "/home/pi/tracks/track_long_" + i.ToString() + ".wav", true);
                    }
                    File.Copy("/media/usb/info.txt", "/home/pi/info.txt", true);
                    blink.blink_run = false;
                    mainRun = true;
                }
                catch (Exception)
                {
                    res = -1;
                    throw;
                }


            }
            else mainRun = true;
            
            if (mainRun)
            {
                string[] config = File.ReadAllLines("/home/pi/info.txt");
                Console.WriteLine(config[0]);
                Console.WriteLine(config[1]);
                Console.WriteLine(config[2]);
                number_short_touch = Int16.Parse(config[1]);
                number_long_touch = Int16.Parse(config[2]);

                
                AreaTouch area = new AreaTouch(number_short_touch, number_long_touch);
                Thread newThread = new Thread(new ThreadStart(area.check_area));
                newThread.Start();
                while (true) ;
            }          

    


            Console.WriteLine("stop program");
            return res;
        }



    }
}
