
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
        private static Int32 old_value = 0;
        private static int delay = 0;
        private static Int32 new_value = 0;
        private static Player player;
        private static bool blink_run = true;
        private static bool playable = true;
        private static bool short_touch = true;


        private static void VoltageInput_VoltageChange(object sender, Phidget22.Events.VoltageInputVoltageChangeEventArgs e)
        {

            
            new_value = Environment.TickCount & Int32.MaxValue;
            Phidget22.VoltageInput evChannel = (Phidget22.VoltageInput)sender;
            Console.WriteLine("Voltage [" + evChannel.Channel + "]: " + e.Voltage);

            if (e.Voltage >= 3) old_value = new_value;
            else
            {
                Console.WriteLine("playable : " + playable.ToString());
                if (playable)
                {
                    
                    delay = (new_value - old_value);
                    if (delay < DELAY) short_touch = true;
                    else short_touch = false;
                    Console.WriteLine("delay : " + delay.ToString());
                    Console.WriteLine("short_touch : " + short_touch.ToString());
                    string path = "";
                    switch (evChannel.Channel)
                    {
                        case 0:
                            if(short_touch) path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test.wav";
                            //else path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test1.wav";
                            break;

                        case 1:
                            if (short_touch) path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test2.wav";
                            //path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test3.wav";
                            break;
                        case 2:
                            if (short_touch) path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test4.wav";
                            // else path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test5.wav";
                            break;
                        case 3:
                            if (short_touch) path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test6.wav";
                            //else path = "C:\\Users\\Victor\\Documents\\test_phidget\\tracks\\test7.wav";
                            break;

                        default:
                            path = "";
                            break;
                    }
                    if (path.Length > 0) player.Play(path);
                    
                    /*
                    if (delay > 1000) player.Play("/home/pi/tracks/test.wav");
                    else player.Play("/home/pi/tracks/test2.wav");*/
                }

            }

            
        }
        private static void blink_task()
        {
            int cpt = FIRST_LED;
            bool increase = true;
            List<DigitalOutput> output = new List<DigitalOutput>();
            for (int i = 0; i < 8; i++) output.Add(new DigitalOutput());


            for (int i = FIRST_LED; i < LAST_LED; i++)
            {
                output[i].Channel = i;
                output[i].Open(5000);
            }
            while (blink_run)
            {
                for (int i = FIRST_LED; i < LAST_LED; i++)
                {
                    if(cpt!=i) output[i].DutyCycle = 0;
                    else output[cpt].DutyCycle = 1;
                }
                Thread.Sleep(50);
                if (increase)
                {
                    cpt++;
                    if (cpt > LAST_LED-2)
                    {                        
                        increase = false;
                    }
                }
                else
                {
                    cpt--;
                    if (cpt < FIRST_LED+1) increase = true;
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
            Thread blink_thread = new Thread(new ThreadStart(blink_task));
            blink_thread.Start();

            List<VoltageInput> input = new List<VoltageInput>();
            for (int i = 0; i < 8; i++) input.Add(new VoltageInput());
            for (int i = 0; i < 8; i++)
            {
                input[i].Channel = i;
                
                input[i].VoltageChange += VoltageInput_VoltageChange;
                input[i].Open(5000);
                input[i].VoltageChangeTrigger = 3;
            }

            playable = true;
            player.PlaybackFinished += OnPlaybackFinished;

            Console.ReadLine();
            Console.WriteLine("stop blink");


            blink_run = false;
            


    
            Console.WriteLine("stop test1");
            
            Console.WriteLine("stop test3");
            Console.WriteLine("stop program");
            for (int i = 0; i < 8; i++)
            {

                input[i].Close();
            }
        }

        private static void OnPlaybackFinished(object sender, EventArgs e)
        {
            Console.WriteLine("Playback finished");
            playable = true;
        }


    }
}
