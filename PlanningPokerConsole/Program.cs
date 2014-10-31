using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningPokerConsole
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            CommandParser cp = new CommandParser();
            cp.GameLoop();
        }
    }
}
