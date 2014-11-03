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
        public const string SERVER = "http://ghpp.mikaelec.com";


        public void GameLoop()
        {
            Game g = null;
            while (g == null)
            {
                Console.WriteLine("Commands:\ncreategame [name]\njoingame [gameid] [name]\njoinclipboard [name]\n\n");
                g = lobbyParse(Console.ReadLine());

            }
            while (true)
            {
                Console.Clear();

                Console.WriteLine("GAME {0}\n\n", g.Id.Hash);
                PrintVotes(g);

                Console.WriteLine("\n\n");

                if (g.Host)
                {
                    
                    Console.WriteLine("Commands:\nvote [vote]\nclearvotes");
                }
                else
                    Console.WriteLine("Commands:\nvote [vote]");
                GameParse(Console.ReadLine(), g);
            }
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
                default:
                    Console.WriteLine("Unknown command");
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
                        g.ResetGame();
                    else Console.WriteLine("You need to be host of the game to do that");
                    break;
                case "":
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }

        private void PrintVotes(Game g)
        {
            var votes = g.Votes;

            foreach (var vote in votes)
            {
                Console.WriteLine("{0}: {1}", vote.Key, vote.Value);
            }
        }

        private VoteTypes VoteValid(string vote)
        {
            VoteTypes vt;


            if (VoteTypesExtension.TryParse(vote, out vt))
                return vt;
            else
            {
                return default(VoteTypes);
            }


        }

        private Game JoinGame(string hash, string name)
        {
            return Game.JoinGame(SERVER, new Id(hash), name);
        }

        private Game CreateGame(string name)
        {
            return Game.CreateGame(SERVER, name);
        }


        public void Parse(string input)
        {

        }

    }
}
