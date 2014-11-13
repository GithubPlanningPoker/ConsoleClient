using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanningPokerConsole
{
    public class CommandParser
    {
        public const string SERVER = "http://ghpp.brunothalmann.com";


        public void GameLoop()
        {
            Game g = null;
            while (g == null)
            {
                Console.Clear();
                header();
                Console.WriteLine("Commands:\ncreategame [username]\njoingame [gameid] [username]\njoinclipboard [username]\n\n");
                g = lobbyParse(Console.ReadLine());

            }
            while (true)
            {
                Console.Clear();

                header();

                Console.WriteLine();
                printTextInCenter("GAME: " + g.Id.Hash);
                Console.WriteLine();

                printDescription(g);

                Console.WriteLine();

                PrintVotes(g);

                Console.WriteLine("\n\n");

                if (g.Host)
                {                    
                    Console.WriteLine("Commands:\nvote [vote]\nclearvotes\ndescription [content]");
                }
                else
                    Console.WriteLine("Commands:\nvote [vote]");
                GameParse(Console.ReadLine(), g);
            }
        }

        private static void header()
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

        private static void printTextInCenter(string s, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            char[] text = s.ToArray();
            char[] line = new char[Console.WindowWidth];
            for (int i = 0; i < text.Count(); i++)
                line[((line.Count() / 2) - 1) - ((text.Count() / 2)) + i] = text[i];
            Console.Write(line);
            Console.ResetColor();
        }

        private static void printDescription(Game g)
        {
            Console.WriteLine("DESCRIPTION:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(g.Description);
            Console.ResetColor();
        }


        public Game lobbyParse(string input)
        {
            string[] s = input.Split(' ');

            Game game = null;

            switch (s[0])
            {
                case "creategame":
                    game = CreateGame(s[1]);
                    Console.WriteLine("Game successfully created with id {0}\nCopy to clipboard? y/n", game.Id.Hash);
                    if (Console.ReadLine() == "y")
                        Clipboard.SetText(game.Id.Hash);
                    break;
                case "joingame":
                    game = JoinGame(s[1], s[2]);
                    Console.WriteLine("Game successfully joined");
                    break;
                case "joinclipboard":
                    game = JoinGame(Clipboard.GetText(), s[1]);
                    break;
                case "":
                default:
                    printUnknowCommand();
                    break;
            }
            return game;
        }


        public void GameParse(string input, Game g)
        {
            string[] s = input.Split(' ');

            switch (s[0])
            {
                case "vote":
                    VoteTypes v = VoteValid(s[1]);
                    if (v != VoteTypes.Zero)
                        g.Vote(v);
                    else Console.WriteLine("Invalid vote");
                    break;
                case "clearvotes":
                    if (g.Host)
                        g.ClearVotes();
                    else Console.WriteLine("You need to be host of the game to do that");
                    break;
                case "description":
                    if (g.Host)
                        g.Description = String.Join(" ", s.ToList().GetRange(1, s.Count()-1));
                    break;
                case "":
                default:
                    printUnknowCommand();
                    break;
            }
        }

        private static void printUnknowCommand()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unknown command(Press any key to continue)");
            Console.ResetColor();
            Console.ReadLine();
        }

        private void PrintVotes(Game g)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int notVoted = g.Votes.Count(x => x.VoteType == null);
            int voted = g.Votes.Count();
            if (notVoted != 0)
            {                
                printTextInCenter("VOTES (" + (voted - notVoted) + "/" + voted + " have voted)", ConsoleColor.Green);
                foreach (var vote in g.Votes)
                {
                    if (vote.Name == g.User.Name)
                        Console.WriteLine("{0}: {1}", vote.Name, vote.VoteType);
                    Console.WriteLine("{0}: {1}", vote.Name, "***");
                }
                Console.ResetColor();
                return;
            }
            foreach (var vote in g.Votes)
                Console.WriteLine("{0}: {1}", vote.Name, vote.VoteType);
            Console.ResetColor();
        }

        private VoteTypes VoteValid(string vote)
        {
            VoteTypes vt;
            if (VoteTypesExtension.TryParse(vote, out vt))
                return vt;
            else
                return default(VoteTypes);
        }

        private Game JoinGame(string hash, string name)
        {
            return Game.JoinGame(SERVER, new Id(hash), name);
        }

        private Game CreateGame(string name)
        {
            return Game.CreateGame(SERVER, name);
        }
    }
}
