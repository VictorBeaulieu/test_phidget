using NetCoreAudio;
using Phidget22;
using System;
using System.Collections.Generic;
using System.Text;

namespace test_phidget
{
    class AreaTouch
    {

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
        /*
        private const string track = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test.wav";
        private const string track1 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test1.wav";
        private const string track2 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test2.wav";
        private const string track3 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test3.wav";
        private const string track4 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test4.wav";
        private const string track5 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test5.wav";
        private const string track6 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test6.wav";
        private const string track7 = "C:\\Users\\VictorBeaulieu\\Documents\\test_phidget\\tracks\\test7.wav";
        */
        
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

        private int channel = 0;
        private List<VoltageInput> input;
        private bool run = true;
        private bool play_track = false;
        private double trigger = 3;
        private int number = 0;
        private Int32 old_value = 0;
        private Int32 new_value = 0;
        private string path="";

       
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
                    
                    
                }
                else
                {
                    int tmp = new_value - old_value;
                    Console.WriteLine("thread chanel " + evChannel.Channel + " time : " + tmp.ToString());
                    switch (evChannel.Channel)
                    {
                        case 0:
                            if (tmp < 1000) path = track;
                            else path = track2;
                            break;
                        case 1:
                            if (tmp < 1000) path = track3;
                            else path = track4;
                            break;
                        case 2:
                            if (tmp < 1000) path = track4;
                            else path = track5;
                            break;
                        case 3:
                            if (tmp < 1000) path = track6;
                            else path = track7;
                            break;
                        case 4:
                            if (tmp < 1000) path = track8;
                            else path = track9;
                            break;
                        case 5:
                            if (tmp < 1000) path = track10;
                            else path = track11;
                            break;
                        case 6:
                            if (tmp < 1000) path = track12;
                            else path = track13;
                            break;
                        case 7:
                            if (tmp < 1000) path = track14;
                            else path = track15;
                            break;

                        default:
                            break;
                    }
                    play_track = true;
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
            input = new List<VoltageInput>();
            for (int i = 0; i < this.number; i++) input.Add(new VoltageInput());
            for (int i = 0; i < this.number; i++)
            {
                input[i].Channel = i;
                input[i].VoltageChange += VoltageInput_VoltageChange;
            }
            

        }


        public void check_area()
        {
            
            Console.Write("try to connect...");
            Player player = new Player();
            
            try
            {

                for (int i = 0; i < this.number; i++)
                {
                    input[i].Close();
                    input[i].Open(5000);
                    input[i].VoltageChangeTrigger = 3;
                }
                Console.WriteLine("succeed");
            }
            catch (PhidgetException ex)
            {

                Console.WriteLine("failled");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine("PhidgetException " + ex.ErrorCode + " (" + ex.Description + "): " + ex.Detail);
                run =  false;
            }

            while (run)
            {
                while (!play_track) ;

                player.Play(path);
                play_track = false;
            }
            for (int i = 0; i < this.number; i++) input[i].Close();
        }

    }
}
