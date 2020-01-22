using Phidget22;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace test_phidget
{
    class Blink
    {
        private int SPEED = 30;
        private const int FIRST_LED = 0;
        private const int LAST_LED = 8;
        private List<DigitalOutput> output;
        public bool blink_run = true;

        public Blink(int speed)
        {
            this.SPEED = speed;/*
            output = new List<DigitalOutput>();
            for (int i = 0; i < 8; i++) output.Add(new DigitalOutput());
            for (int i = 0; i < 8; i++) output[i].Channel = i;
            for (int i = 0; i < 8; i++)
            {
                output[i].Close();
                output[i].Open(5000);
            }*/
        }

        private void Open()
        {
            output = new List<DigitalOutput>();
            for (int i = 0; i < 8; i++) output.Add(new DigitalOutput());
            for (int i = 0; i < 8; i++) output[i].Channel = i;
            for (int i = 0; i < 8; i++)
            {
                output[i].Close();
                output[i].Open(5000);
            }
        }
        private void Close()
        {
            
            for (int i = 0; i < 8; i++) output[i].Open(5000);
        }

        public void k200_task()
        {

            this.Open();
            blink_run = true;
            while (blink_run)
            {

                for (int i = FIRST_LED; i < LAST_LED; i++)
                {
                    for (int j = FIRST_LED; j < LAST_LED; j++) output[j].DutyCycle = 0;
                    output[i].DutyCycle = 1;
                    Thread.Sleep(SPEED);
                }
                for (int i = LAST_LED-1; i > FIRST_LED; i--)
                {
                    for (int j = FIRST_LED; j < LAST_LED; j++) output[j].DutyCycle = 0;
                    output[i].DutyCycle = 1;
                    Thread.Sleep(SPEED);
                }
            }
            this.Close();
        }

        public void loading_task()
        {

            this.Open();
            blink_run = true;
            while (blink_run)
            {

                for (int i = FIRST_LED; i < LAST_LED; i++)
                {
                    output[i].DutyCycle = 1;
                    Thread.Sleep(100);
                }
                for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 0;
            }
            for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 0;
            Thread.Sleep(SPEED);
            for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 1;
            Thread.Sleep(SPEED);
            for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 0;
            Thread.Sleep(SPEED);
            for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 1;
            Thread.Sleep(SPEED);
            for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 0;
            Thread.Sleep(SPEED);
            for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 1;
            Thread.Sleep(SPEED);
            for (int i = FIRST_LED; i < LAST_LED; i++) output[i].DutyCycle = 0;
            this.Close();
        }
    }
}
