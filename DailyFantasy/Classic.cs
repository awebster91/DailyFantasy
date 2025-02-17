using CsvHelper;
using DailyFantasy.PlayerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyFantasy
{
    internal class Classic
    {
        public static void WriteClassicLineups(string filename)
        {



            List<Player_DraftKings> tightEnds = new List<Player_DraftKings>();
            List<Player_DraftKings> runningBacks = new List<Player_DraftKings>();
            List<Player_DraftKings> wideReceivers = new List<Player_DraftKings>();
            List<Player_DraftKings> flex = new List<Player_DraftKings>();
            List<Player_DraftKings> quarterBacks = new List<Player_DraftKings>();


            StreamReader reader = new StreamReader(filename);
            CsvParser parser = new CsvParser(reader, System.Globalization.CultureInfo.CurrentCulture);
            CsvReader csv = new CsvReader(parser);
            List<Player_DraftKings> potentialplayers = csv.GetRecords<Player_DraftKings>().ToList();
            List<Player_DraftKings> players = Player_DraftKings.Eliminate_Injuries(potentialplayers);
            foreach (var player in players)
            {
                if (player.AvgPointsPerGame >= (decimal)6)
                {
                    switch (player.Position)
                    {
                        case "TE":
                                flex.Add(player);
                                tightEnds.Add(player);
                            break;
                        case "WR":
                             flex.Add(player);
                             wideReceivers.Add(player);
                            // code block
                            break;
                        case "RB":
                            flex.Add(player);
                            runningBacks.Add(player);
                            break;
                        case "QB":
                            quarterBacks.Add(player);
                            break;
                        default:
                            // code block
                            break;
                    }
                }

            }
            quarterBacks = Player_DraftKings.Eliminate_Players(quarterBacks);
            flex = Player_DraftKings.Eliminate_Players(flex);
            runningBacks =  Player_DraftKings.Eliminate_Players(runningBacks);
            wideReceivers = Player_DraftKings.Eliminate_Players(wideReceivers);
            tightEnds = Player_DraftKings.Eliminate_Players(tightEnds);
            Player_DraftKings defense = Player_DraftKings.GetPlayer(players, "Jets ");
            List<List<Player_DraftKings>> lineups = Lineup.GenerateValidLineups(quarterBacks,tightEnds, runningBacks, wideReceivers, flex, defense, 50000);



            List<Lineup> lineupswithPoints = new List<Lineup>();
            foreach (var lineup in lineups)
            {
                lineupswithPoints.Add(new Lineup(lineup));
            }
            lineupswithPoints.Sort();

            StreamWriter write = new StreamWriter("C:\\FFLineUP\\" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".csv");
            for (int i = 0; i< lineupswithPoints.Count&&i<20000; i++)
            {
                write.WriteLine(lineupswithPoints[i]);
            }
            write.Flush();
            write.Close();
        }
    }
}
