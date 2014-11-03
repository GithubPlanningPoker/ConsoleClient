﻿using System;
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
                Console.WriteLine("Commands:\ncreategame [username]\njoingame [gameid] [username]\njoinclipboard [username]\n\n");
                g = lobbyParse(Console.ReadLine());

            }
            while (true)
            {
                Console.Clear();

                Console.WriteLine("GAME {0}\n\n", g.Id.Hash);

                printDescription(g);

                Console.WriteLine("\n");

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

        private static void printDescription(Game g)
        {
            Console.WriteLine("DESCRIPTION:");
            Console.ForegroundColor = ConsoleColor.Blue;
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
            int notVoted = g.Votes.Count(x => x.Value == null);
            int voted = g.Votes.Count();
            if (notVoted != 0)
            {
                foreach (var vote in g.Votes)
                {
                    if (vote.Key == g.User)
                        Console.WriteLine("{0}: {1}", vote.Key, vote.Value);
                    Console.WriteLine("{0}: {1}", vote.Key, "***");
                }
                Console.WriteLine(voted-notVoted + "/" + voted + " have voted.");
                Console.ResetColor();
                return;
            }
            foreach (var vote in g.Votes)
                Console.WriteLine("{0}: {1}", vote.Key, vote.Value);
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
