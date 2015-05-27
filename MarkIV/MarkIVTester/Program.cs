using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkIV.CoffeeMaker;
using MarkIV.Devices.Concrete;
using System.Reactive.Linq;
using MarkIV.Devices;

namespace MarkIVTester
{
    class Program
    {
        public static MarkIV.CoffeeMaker.MarkIV machine;
           
        static void Main(string[] args)
        {
            PlateSensor plateSensor = new PlateSensor();
            machine =
                new MarkIV.CoffeeMaker.MarkIV(
                    new Button(),
                    new Light(),
                    plateSensor,
                    new Boiler(plateSensor),
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
            if (command.Equals("pressbutton") || command.Equals("p"))
                machine.BrewButton.Press(null);
            if (command.Equals("removepot") || command.Equals("r"))
                machine.PlateSensor.RemovePot();
            if (command.Equals("putemptypot") || command.Equals("e"))
                machine.PlateSensor.PutEmptyPot();
            if (command.Equals("putnonemptypot") || command.Equals("n"))
                machine.PlateSensor.PutNonEmptyPot();
            if (command.Equals("refillboiler") || command.Equals("b"))
                machine.Boiler.Refill();
            if (command.Equals("status") || command.Equals("s"))
                Console.WriteLine( machine.GetStatusAsString() );
            if (command.Equals("(c)ls") || command.Equals("c"))
            {
                Console.Clear();
                DisplayHelp();
            }
            
            DisplayPrompt();
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Usage Instructions");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("?                : Print This Help");
            Console.WriteLine("(P)ressButton    : Start Brewing");
            Console.WriteLine("(R)emovePot      : Remove Pot");
            Console.WriteLine("Put(E)mptyPot    : Put Empty Pot");
            Console.WriteLine("Put(N)onEmptyPot : Put NON Empty Pot");
            Console.WriteLine("Refill(B)oiler   : Fills the boiler");
            Console.WriteLine("(S)tatus         : Prints Status");
            Console.WriteLine("CTRL+C           : Ends the simulation");
        }

        static IEnumerable<string> ReadInput()
        {
            while (true)
                yield return Console.ReadLine();
        }

    }
}
