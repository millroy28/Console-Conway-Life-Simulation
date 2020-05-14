using System;

namespace ConsoleLife
{
    class Program
    {
        //int columnEnd = 100;
        //int rowEnd = 30;
        //int spontaneousGenerationRate = 5; //Random number between 1/1000, if below this #, spontaneous life

        static void Main(string[] args)
        {
            Console.SetWindowSize(100,40);
            Controller run = new Controller(98,37,100000);
            run.StartLife();
            
            
        }
    }
}
