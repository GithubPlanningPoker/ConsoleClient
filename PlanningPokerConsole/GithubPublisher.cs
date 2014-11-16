using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace PlanningPokerConsole
{
    public class GithubPublisher
    {
        private GithubIssues githubIssues;
        public GithubPublisher(string githubToken)
        {
            githubIssues = new GithubIssues();
            githubIssues.Login(githubToken);
        }

        public void PostIssue(Game game, string user, string repo)
        {
            githubIssues.PostIssue("[" + game.VoteResultEstimate() + "] " + game.Title, game.Description, user, repo);
        }
    }
}
