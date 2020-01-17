
using System;
using System.Threading.Tasks;
using Phidget22;
using Phidget22.Events;
using NetCoreAudio;
using System.Threading;
using System.Collections.Generic;



namespace test_phidget
{

    class Program
    {
        private const int DELAY = 1000;
        private const int FIRST_LED = 0;
        private const int LAST_LED = 8;


        /*
        private const string track = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test.wav";
        private const string track1 = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test1.wav";
        private const string track2 = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test2.wav";
        private const string track3 = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test3.wav";
        private const string track4 = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test4.wav";
        private const string track5 = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test5.wav";
        private const string track6 = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test6.wav";
        private const string track7 = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test7.wav";

        */

        private const string track = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test.wav";
        private const string track1 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test1.wav";
        private const string track2 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test2.wav";
        private const string track3 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test3.wav";
        private const string track4 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test4.wav";
        private const string track5 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test5.wav";
        private const string track6 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test6.wav";
        private const string track7 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test7.wav";
        /*
        private const string track = "/home/pi/tracks/test.wav";
        private const string track1 = "/home/pi/tracks/test1.wav";
        private const string track2 = "/home/pi/tracks/test2.wav";
        private const string track3 = "/home/pi/tracks/test3.wav";
        private const string track4 = "/home/pi/tracks/test4.wav";
        private const string track5 = "/home/pi/tracks/test5.wav";
        private const string track6 = "/home/pi/tracks/test6.wav";
        private const string track7 = "/home/pi/tracks/test7.wav";
        private const string track8 = "/home/pi/tracks/test8.wav";
        private const string track9 = "/home/pi/tracks/test9.wav";
        private const string track10 = "/home/pi/tracks/test10.wav";
        private const string track11 = "/home/pi/tracks/test11.wav";
        private const string track12 = "/home/pi/tracks/test12.wav";
        private const string track13 = "/home/pi/tracks/test13.wav";
        private const string track14 = "/home/pi/tracks/test14.wav";
        private const string track15 = "/home/pi/tracks/test15.wav";

        */

        private static Int32 old_value = 0;
        private static int delay = 0;
        private static Int32 new_value = 0;
        private static Player player;
        private static bool blink_run = true;
        private static bool blink_stop = false;
        private static bool playable = true;
        private static bool short_touch = true;
        private static bool init = true;

        private static List<DigitalOutput> output;

        private static void VoltageInput_VoltageChange(object sender, Phidget22.Events.VoltageInputVoltageChangeEventArgs e)
        {

            
            new_value = Environment.TickCount & Int32.MaxValue;
            Phidget22.VoltageInput evChannel = (Phidget22.VoltageInput)sender;
            Console.WriteLine("Voltage [" + evChannel.Channel + "]: " + e.Voltage);
            if(!init)
            {
                if (e.Voltage >= 3)
                {
                    old_value = new_value;
                    blink_stop = true;
                    for (int i = FIRST_LED; i < LAST_LED; i++)
                    {
                        if (evChannel.Channel != i) output[i].DutyCycle = 0;
                        else output[evChannel.Channel].DutyCycle = 1;
                    }
                }

                else
                {
                    Console.WriteLine("playable : " + playable.ToString());
                    blink_stop = false;
                    for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 0;
                    

                    delay = (new_value - old_value);
                    if (delay < DELAY) short_touch = true;
                    else short_touch = false;


                    Console.WriteLine("delay : " + delay.ToString());
                    Console.WriteLine("short_touch : " + short_touch.ToString());
                    string path = "";
                    switch (evChannel.Channel)
                    {
                        case 0:
                            if (short_touch) path = track;
                            //else path = track1;
                            break;

                        case 1:
                            if (short_touch) path = track2;
                            //path = track3;
                            break;
                        case 2:
                            if (short_touch) path = track4;
                            // else path = track5;
                            break;
                        case 3:
                            if (short_touch) path = track6;
                            //else path = track7;
                            break;

                        default:
                            path = "";
                            break;
                    }
                    if (path.Length > 0) player.Play(path);
                }
            
                    
                    /*
                    if (delay > 1000) player.Play("/home/pi/tracks/test.wav");
                    else player.Play("/home/pi/tracks/test2.wav");*/
                

            }

            
        }
        private static void blink_task()
        {
            int cpt = FIRST_LED;
            bool increase = true;
            
            while (blink_run)
            {
                if (!blink_stop)
                { 
                    for (int i = FIRST_LED; i < LAST_LED; i++)
                    {
                        if (cpt != i) output[i].DutyCycle = 0;
                        else output[cpt].DutyCycle = 1;
                    }
                Thread.Sleep(50);
                if (increase)
                {
                    cpt++;
                    if (cpt > LAST_LED - 2)
                    {
                        increase = false;
                    }
                }
                else
                {
                    cpt--;
                    if (cpt < FIRST_LED + 1) increase = true;
                }
                }
            }
            for (int i = FIRST_LED; i < LAST_LED; i++)
            {
              
                output[i].Close();
            }
        }

       
        static void Main(string[] args)
        {
                       
            player = new Player();
            output = new List<DigitalOutput>();
            List<VoltageInput> input = new List<VoltageInput>();
            for (int i = 0; i < 8; i++) input.Add(new VoltageInput());
            for (int i = 0; i < 8; i++)
            {
                input[i].Channel = i;
                
                input[i].VoltageChange += VoltageInput_VoltageChange;
                input[i].Open(5000);
                input[i].VoltageChangeTrigger = 3;
            }
            for (int i = 0; i < 8; i++) output.Add(new DigitalOutput());
            for (int i = FIRST_LED; i < LAST_LED; i++)
            {
                output[i].Channel = i;
                output[i].Open(5000);
            }

            Thread blink_thread = new Thread(new ThreadStart(blink_task));
            blink_thread.Start();



            player.PlaybackFinished += OnPlaybackFinished;
            Thread.Sleep(3000);
            init = false;
            Console.ReadLine();
            Console.WriteLine("stop blink");


            blink_run = false;          


            Console.WriteLine("stop program");
            for (int i = 0; i < 8; i++)
            {

                input[i].Close();
            }
        }

        private static void OnPlaybackFinished(object sender, EventArgs e)
        {
            Console.WriteLine("Playback finished");
            
        }


    }
}
