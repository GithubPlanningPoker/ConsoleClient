using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningPokerConsole
{
    public class CommandParser
    {
        public const string SERVER = "http://ghpp.mikaelec.com";
        public Game lobbyParse(string input)
        {
            string[] s = input.Split(' ');

            Game game = null;

            while (game == null)
            {
                switch (s[0])
                {
                    case "creategame":
                        game = CreateGame(s[1]);
                        break;
                    case "joingame":
                        game = JoinGame(s[1], s[2]);
                        break;
                    default:
                        break;
                }
            }

            return game;
        }

        public void GameParse(string input, Game g)
        {
            string[] s = input.Split(' ');

            while (true)
            {
                try
                {
                    switch (s[0])
                    {
                        case "vote":

                            VoteTypes v = VoteValid(s[1]);
                            g.Vote(v);
                            break;
                        case "clearvotes":
                            if (g.Host)
                                g.ResetGame();
                            else throw new ArgumentException("You are not the host");
                            break;
                        default:
                            break;
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }

        private VoteTypes VoteValid(string vote)
        {
            VoteTypes vt;


            if (VoteTypesExtension.TryParse(vote, out vt))
                return vt;
            else
            {
                Console.WriteLine("Not valid vote");
                throw new ArgumentException("Not valid vote");
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
