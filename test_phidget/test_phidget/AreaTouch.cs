using NetCoreAudio;
using Phidget22;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace test_phidget
{
    class AreaTouch
    {

        private List<string> short_track_array;
        private List<string> long_track_array;

        private int channel = 0;
        private List<VoltageInput> input;
        private bool run = false;
        
        private double trigger = 3;
        private int number = 0;
        private int short_touch = 0;
        private int long_touch = 0;
        private Int32 old_value = 0;
        private Int32 new_value = 0;
        private string path="";
        private int delay = 1000;
        private Blink blink;
        private bool init_done = false;
        private readonly object playerLock = new object();
        Player player = new Player();

        private void VoltageInput_VoltageChange(object sender, Phidget22.Events.VoltageInputVoltageChangeEventArgs e)
        {
            
            try
            {
                new_value = Environment.TickCount & Int32.MaxValue;
                Phidget22.VoltageInput evChannel = (Phidget22.VoltageInput)sender;
                Console.WriteLine("Voltage [" + evChannel.Channel + "]: " + e.Voltage);
                if (evChannel.Voltage > trigger)
                {
                    old_value = new_value;
                    blink.TurnOn(evChannel.Channel);
                }
                else
                {
                    int tmp = new_value - old_value;
                    Console.WriteLine("thread chanel " + evChannel.Channel + " time : " + tmp.ToString());
                    blink.TurnOff(evChannel.Channel);
                    if (tmp < delay)
                    {
                        if (evChannel.Channel < this.short_touch)
                        {
                            path = short_track_array[evChannel.Channel];
                            Console.WriteLine("short press");
                        }
                    }
                    else
                    {
                        if (evChannel.Channel < this.long_touch)
                        {
                            path = long_track_array[evChannel.Channel];
                            Console.WriteLine("long press");
                        }
                    }
                    if(run)
                    {
                        if (!player.Playing)
                        {
                            Console.WriteLine("player availlable");
                            player.Play(path);
                        }
                        else Console.WriteLine("player not availlable");
                    }
                    
                }
            }
            catch (PhidgetException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine("PhidgetException " + ex.ErrorCode + " (" + ex.Description + "): " + ex.Detail);
            }
            

        }

        public AreaTouch(int number)
        {
            this.number = number;
            this.long_touch = number;
            this.short_touch = number;
            input = new List<VoltageInput>();
            for (int i = 0; i < this.number; i++) input.Add(new VoltageInput());
            for (int i = 0; i < this.number; i++)
            {
                input[i].Channel = i;
                input[i].VoltageChange += VoltageInput_VoltageChange;
            }
            init_array();

        }
        public AreaTouch(int short_touch, int long_touch)
        {
            blink = new Blink(8);
            this.short_touch = short_touch;
            this.long_touch = long_touch;
            if (this.short_touch > this.long_touch) this.number = this.short_touch;
            else this.number = this.long_touch;
            Console.WriteLine(number.ToString());


            input = new List<VoltageInput>();
            for (int i = 0; i < this.number; i++) input.Add(new VoltageInput());
            for (int i = 0; i < this.number; i++)
            {
                input[i].Channel = i;
                input[i].VoltageChange += VoltageInput_VoltageChange;
            }
            init_array();


        }
        public void check_area()
        {
            
            Console.Write("try to connect...");
            
            
            try
            {

                for (int i = 0; i < this.number; i++)
                {
                    input[i].Close();
                    input[i].Open(5000);
                    input[i].VoltageChangeTrigger = 3;
                }
                Console.WriteLine("succeed");
                init_done = true;
            }
            catch (PhidgetException ex)
            {

                Console.WriteLine("failled");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine("PhidgetException " + ex.ErrorCode + " (" + ex.Description + "): " + ex.Detail);
                init_done =  false;
            }
            Thread.Sleep(1000);
            run = init_done;
            while (run) ;
            for (int i = 0; i < this.number; i++) input[i].Close();
        }

        private void init_array()
        {

            

            long_track_array = new List<string>();
            short_track_array = new List<string>();

            for (int i = 0; i < this.long_touch; i++)
            {
                this.long_track_array.Add("/home/pi/tracks/track_long_" + i.ToString() + ".wav");
            }
            
            for (int i = 0; i < this.short_touch; i++)
            {
                this.short_track_array.Add("/home/pi/tracks/track_short_" + i.ToString() + ".wav");
            }
        }
    }
}
