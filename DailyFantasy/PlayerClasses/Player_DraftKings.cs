using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DailyFantasy.PlayerClasses.Player_DraftKings;

namespace DailyFantasy.PlayerClasses
{
    public class Player_DraftKings
    { 
        public string Position { get; set; }
        public string Name { get; set;}
        public string ID { get; set; }
        public int Salary { get; set; }
        public decimal AvgPointsPerGame { get; set; }

        internal static List<Player_DraftKings> GenerateLineup(Player_DraftKings quarterback, Player_DraftKings runningback1, Player_DraftKings runningback2, Player_DraftKings widereceiver1, Player_DraftKings widereceiver2, Player_DraftKings widereceiver3,Player_DraftKings tightend, Player_DraftKings flexes, Player_DraftKings defenses)
        {
            return new List<Player_DraftKings>() { quarterback, runningback1, runningback2, widereceiver1, widereceiver2, widereceiver3, flexes, tightend, defenses };
        }

        public override string ToString()
        {
            return (Position +","+","+Name+","+","+Salary+","+AvgPointsPerGame);
        }

        public static bool UnderSalaryCap(List<Player_DraftKings> lineup, int salaryCap)
        {
            int salary = 0;
            foreach (Player_DraftKings player in lineup)
            {
                salary += player.Salary;
            }
            if (salary > salaryCap) return false;
            return true;
        }
        public static bool ValidLineup(List<Player_DraftKings> lineup)
        {
            decimal pointsperGame = 0;

            for (int i = 0; i < lineup.Count; i++)
            {
                if (lineup[i].Name == "Dontrell Hilliard")
                    return false;
                for (int j = i+1; j<lineup.Count; j++)
                {
                    if (lineup[i].ID == lineup[j].ID)
                        return false;
                }
                pointsperGame += lineup[i].AvgPointsPerGame;   
            }
            return true;
        }

        internal static Player_DraftKings GetPlayer(List<Player_DraftKings> players, string name)
        {
            return players.Where(x => x.Name == name).First();
        }
        internal static List<Player_DraftKings> Eliminate_Players(List<Player_DraftKings> consideredPlayers)
        {
            List<Player_DraftKings> players = new List<Player_DraftKings>();
            List<int> Positions_To_Eliminate = new List<int>();
            for (int i = 0; i< consideredPlayers.Count-1; i++)
            {
                for (int j=i; j<consideredPlayers.Count; j++)
                {
                    if (consideredPlayers[i].AvgPointsPerGame >= consideredPlayers[j].AvgPointsPerGame && consideredPlayers[i].Salary < consideredPlayers[j].Salary)
                    {
                        Positions_To_Eliminate.Add(j);
                    }
                    if (consideredPlayers[j].AvgPointsPerGame >= consideredPlayers[i].AvgPointsPerGame && consideredPlayers[j].Salary < consideredPlayers[i].Salary)
                    {
                        Positions_To_Eliminate.Add(i);
                    }
                }
            }

            for (int z = 0; z<consideredPlayers.Count; z++)
            {
                if (!Positions_To_Eliminate.Contains(z))
                {
                    players.Add(consideredPlayers[z]);
                }
            }
            return players;

        }

        internal static List<Player_DraftKings> Eliminate_Injuries(List<Player_DraftKings> potentialplayers)
        {
            List<Player_DraftKings> players = new List<Player_DraftKings>();
            List<string> injuredPlayerNames = new List<String>()
            {
                "Greg Dulcich",
                "Jahan Dotson",
                "Laviska Shenault Jr.",
                "Joe Flacco",
                "Carson Wentz",
                "Sammy Watkins",
                "Sterling Shepard"
            };
            foreach(Player_DraftKings player in potentialplayers)
            {
                if (!injuredPlayerNames.Contains(player.Name)) 
                {
                    players.Add(player); 
                }
            }
            return players;
            
        }
    }
    public class Lineup : IComparable
    {
        public List<Player_DraftKings> players { get; set; }
        public decimal avgPointsPerGame { get; set; }

        public Lineup(List<Player_DraftKings> players)
        {
            this.avgPointsPerGame = 0;
            this.players = players;
            foreach (var player in players)
                this.avgPointsPerGame += player.AvgPointsPerGame;
        }


        public int CompareTo(object? other)
        {
            Lineup otherLineup = (Lineup)other;
            if (avgPointsPerGame > otherLineup.avgPointsPerGame) return -1;
            else if (avgPointsPerGame == otherLineup.avgPointsPerGame) return 0;
            else return 1;
        }

        public override string ToString()
        {
            string returnString = "";
            foreach (var player in players)
            {
                returnString += player.Name + ",";
            }
            returnString += avgPointsPerGame;
            return returnString;
        }

        public static List<List<Player_DraftKings>> GenerateValidLineups(List<Player_DraftKings> quarterBacks,List<Player_DraftKings> tightEnds, List<Player_DraftKings> runningBacks, List<Player_DraftKings> wideReceivers, List<Player_DraftKings> flex, Player_DraftKings defense,int salaryCap)
        {
            List<List<Player_DraftKings>> lineups = new List<List<Player_DraftKings>>();
            for (int t = 0; t < tightEnds.Count; t++)
            {
                for (int x = 0;  x < runningBacks.Count; x++)
                {
                    for (int y = x + 1;  y < runningBacks.Count; y++)
                    {
                        for (int i =0; i<wideReceivers.Count-2; i++)
                        {
                            for (int j = i + 1; j < wideReceivers.Count-1; j++)
                            {
                                for (int k = j + 1; k < wideReceivers.Count; k++)
                                {
                                    for (int q = 0; q < quarterBacks.Count; q++)
                                    {

                                        for (int f = flex.Count - 1; f >= 0 && x < flex.Count; f--)
                                        {

                                            List<Player_DraftKings> potentialLineup = Player_DraftKings.GenerateLineup(quarterBacks[q], runningBacks[x], runningBacks[y], wideReceivers[i], wideReceivers[j], wideReceivers[k], tightEnds[t], flex[f], defense);

                                            if (Player_DraftKings.UnderSalaryCap(potentialLineup, salaryCap))
                                            {
                                                if (Player_DraftKings.ValidLineup(potentialLineup))
                                                {
                                                    lineups.Add(potentialLineup);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            return lineups;
        }
    }
}
