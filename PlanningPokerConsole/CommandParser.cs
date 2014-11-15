﻿using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace PlanningPokerConsole
{
    public class CommandParser
    {
        public const string SERVER = "http://ghpp.brunothalmann.com";
        //public const string SERVER = "http://ghpp.mikaelec.com/api";
        //public const string SERVER = "http://localhost:52450";

        public CommandParser()
        {
            //File.Create("description.txt");
        }

        public void GameLoop()
        {
            Game game = null;
            while (game == null)
            {
                Console.Clear();
                ConsoleGraphics.Header();
                ConsoleGraphics.PrintLobbyCommands();
                ConsoleGraphics.PrintInputString();
                game = lobbyParse(Console.ReadLine());

            }
            while (true)
            {
                Console.Clear();

                ConsoleGraphics.Header();

                Console.WriteLine();
                ConsoleGraphics.PrintTextInCenter("GAME: " + game.Id.Hash);
                Console.WriteLine();

                ConsoleGraphics.PrintTitle(game);
                ConsoleGraphics.PrintDescription(game);

                Console.WriteLine();

                ConsoleGraphics.PrintVotes(game);

                Console.WriteLine("\n\n");

                ConsoleGraphics.PrintGameCommands(game);
                ConsoleGraphics.PrintInputString();
                GameParse(Console.ReadLine(), game);
            }
        }

        public Game lobbyParse(string input)
        {
            string[] s = input.Split(' ');

            Game game = null;

            if (s.Length == 1)
            {
                ConsoleGraphics.PrintUnknowCommand();
                GameLoop();
            }

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
                    ConsoleGraphics.PrintUnknowCommand();
                    break;
            }
            return game;
        }

        public void GameParse(string input, Game game)
        {
            string[] s = input.Split(' ');

            switch (s[0])
            {
                case "vote":
                    VoteTypes v = VoteValid(s[1]);
                    if (v != VoteTypes.Zero)
                        game.Vote(v);
                    else Console.WriteLine("Invalid vote");
                    break;
                case "clearvotes":
                    if(game.Host)
                        game.ClearVotes();
                    else
                        Console.WriteLine("You are not the host.");
                    break;
                case "kick":
                    if (game.Host)
                        game.Kick(s[1]);
                    else
                        Console.WriteLine("You are not the host.");
                    break;
                case "title":
                    game.Title = String.Join(" ", s.ToList().GetRange(1, s.Length-1));
                    break;
                case "description":
                    changeDescription(game);
                    break;
                case "publish":
                    GithubIssues gi = new GithubIssues();
                    gi.Login("343e6297fe72a7769c7539781fca7191f7b08439");
                    gi.PostIssue(game.Title, game.Description, s[1], s[2]);
                    break;
                case "":
                default:
                    ConsoleGraphics.PrintUnknowCommand();
                    break;
            }
        }

        private void changeDescription(Game g)
        {
            var p = Process.Start("description.txt");
            p.WaitForExit();
            if (p.HasExited)
                g.Description = File.ReadAllText("description.txt");
        }

        private Game JoinGame(string hash, string name)
        {
            return Game.JoinGame(SERVER, new Id(hash), name);
        }

        private Game CreateGame(string name)
        {
            return Game.CreateGame(SERVER, name);
        }

        private VoteTypes VoteValid(string vote)
        {
            VoteTypes vt;
            if (VoteTypesExtension.TryParse(vote, out vt))
                return vt;
            else
                return default(VoteTypes);
        }
    }
}
