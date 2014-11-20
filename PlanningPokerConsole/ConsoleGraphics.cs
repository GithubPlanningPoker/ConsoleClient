using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace PlanningPokerConsole
{
    public static class ConsoleGraphics
    {
        public static void Header()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write("#");
            char[] edges = new char[Console.WindowWidth];
            edges[0] = '#';
            edges[edges.Count() - 1] = '#';
            Console.Write(edges);
            char[] title = "GitHub Planning Poker".ToArray();
            char[] titleWithEdges = new char[edges.Count()];
            Array.Copy(edges, titleWithEdges, edges.Count());
            for (int i = 0; i < title.Count(); i++)
                titleWithEdges[((titleWithEdges.Count() / 2) - 1) - ((title.Count() / 2)) + i] = title[i];
            for (int i = 0; i < titleWithEdges.Count(); i++)
            {
                if (i > 0 && i < titleWithEdges.Count() - 1)
                    Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(titleWithEdges[i]);
                Console.ResetColor();
            }
            Console.Write(edges);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write("#");
        }

        public static void PrintTextInCenter(string s, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            char[] text = s.ToArray();
            char[] line = new char[Console.WindowWidth];
            for (int i = 0; i < text.Count(); i++)
                line[((line.Count() / 2) - 1) - ((text.Count() / 2)) + i] = text[i];
            Console.Write(line);
            Console.ResetColor();
        }

        public static void PrintDescription(Game g)
        {
            Console.WriteLine("DESCRIPTION:");
            Console.WriteLine(g.Description);
            Console.WriteLine();
        }

        public static void PrintTitle(Game g)
        {
            Console.WriteLine("TITLE:");
            Console.WriteLine(g.Title);
            Console.WriteLine();
        }

        public static void PrintUnknowCommand()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unknown command(Press any key to continue)");
            Console.ResetColor();
            Console.ReadLine();
        }

        public static void PrintVotes(Game g, VoteTypes clientVote)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int notVoted = g.Votes.Count(x => x.HasVoted == false);
            int votes = g.Votes.Count();
            PrintTextInCenter("VOTES (" + (votes - notVoted) + "/" + votes + " have voted)", ConsoleColor.Green);
            
            if(clientVote != default(VoteTypes))
                Console.WriteLine("{0}: {1}", g.User.Name, clientVote);
            
            foreach (var vote in g.Votes)
            {
                if (vote.Name == g.User.Name)
                    continue;
                if (notVoted == 0)
                {
                    Console.WriteLine("{0}: {1}", vote.Name, vote.VoteType);
                    continue;
                }
                Console.WriteLine("{0}: {1}", vote.Name, "Not Voted");
            }
            Console.ResetColor();
            return;
        }

        public static void PrintLobbyCommands()
        {
            Console.WriteLine(printCommandsHeader() + printCreateGameCommand() + printJoinGameCommand() + printJoinClipboardCommand());
        }

        public static void PrintGameCommands(Game g)
        {
            if (g.Host)
                Console.WriteLine(printCommandsHeader() + printVoteCommand() + printTitleCommand() + printDescriptionCommand() + printPublishCommand() + printClearVotesCommand() + printKickCommand());
            else
                Console.WriteLine(printCommandsHeader() + printVoteCommand() + printTitleCommand() + printDescriptionCommand() + printPublishCommand());
        }

        public static void PrintInputString()
        {
            Console.Write(">> ");
        }

        #region PrintHelpers
        private static string printCommandsHeader()
        {
            return "\nCommands:\n";
        }

        private static string printCommands(string command, string options)
        {
            return command + "\t" + options + "\n";
        }

        private static string printVoteCommand()
        {
            return printCommands("vote", "[vote]");
        }

        private static string printDescriptionCommand()
        {
            return printCommands("description", "");
        }

        private static string printKickCommand()
        {
            return printCommands("kick", "[username]");
        }

        private static string printTitleCommand()
        {
            return printCommands("title", "[content]");
        }

        private static string printClearVotesCommand()
        {
            return printCommands("clearvotes", "");
        }

        private static string printJoinClipboardCommand()
        {
            return printCommands("joinclipboard", "[username]");
        }

        private static string printJoinGameCommand()
        {
            return printCommands("joingame", "[gameid] [username]");
        }

        private static string printCreateGameCommand()
        {
            return printCommands("creategame", "[username]");
        }

        private static string printPublishCommand()
        {
            return printCommands("publish", "[name] [repo]");
        }
        #endregion
    }
}
