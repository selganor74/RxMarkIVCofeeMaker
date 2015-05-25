using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkIV.CoffeeMaker;
using MarkIV.Devices.Concrete;
using System.Reactive.Linq;

namespace MarkIVTester
{
    class Program
    {
        public static MarkIV.CoffeeMaker.MarkIV machine;
           
        static void Main(string[] args)
        {
            machine =
                new MarkIV.CoffeeMaker.MarkIV(
                    new Button(),
                    new Light(),
                    new PlateSensor(),
                    new Boiler(),
                    new PlateHeater(),
                    new ReliefValve()
                );

            DisplayHelp();
            DisplayPrompt();
            foreach (string cmd in ReadInput()) {
                ProcessCommand(cmd);
            }

           
        }

        private static void DisplayPrompt()
        {
            Console.Write("> ");
        }

        static void ProcessCommand(string command)
        {
            command = command.ToLower();

            if (command.Equals("?"))
                DisplayHelp();
            if (command.Equals("pressbutton"))
                machine.BrewButton.Press(null);
            if (command.Equals("removepot"))
                machine.PlateSensor.RemovePot();
            if (command.Equals("putemptypot"))
                machine.PlateSensor.PutEmptyPot();
            if (command.Equals("putnonemptypot"))
                machine.PlateSensor.PutNonEmptyPot();
            if (command.Equals("refillboiler"))
                machine.Boiler.Refill();
            if (command.Equals("status"))
                Console.WriteLine( machine.GetStatusAsString() );
            DisplayPrompt();
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Usage Instructions");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("?              : Print This Help");
            Console.WriteLine("pressbutton    : Start Brewing");
            Console.WriteLine("removepot      : Remove Pot");
            Console.WriteLine("putemptypot    : Put Empty Pot");
            Console.WriteLine("putnonemptypot : Put Empty Pot");
            Console.WriteLine("refillboiler   : Fills the boiler");
            Console.WriteLine("status         : Prints Status");
            Console.WriteLine("CTRL+C         : Ends the simulation");
        }

        static IEnumerable<string> ReadInput()
        {
            while (true)
                yield return Console.ReadLine();
        }

    }
}
