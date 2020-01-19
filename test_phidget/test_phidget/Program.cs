
using System;
using System.Threading;



namespace test_phidget
{

    class Program
    {
        
        
        
        static int Main(string[] args)
        {

            int res = 0;
            
            
            if (true)
            {
                Blink blink = new Blink(20);
                Thread blink_thread = new Thread(new ThreadStart(blink.k200_task));
                blink_thread.Start();
                
                
                AreaTouch area = new AreaTouch(4);
                Thread newThread = new Thread(new ThreadStart(area.check_area));
                newThread.Start();
                while (true) ;




                Console.WriteLine("stop program");
            }
            return res;
        }



    }
}
