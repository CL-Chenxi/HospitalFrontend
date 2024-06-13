using System;
using System.Collections.Generic;
using System.Linq;

namespace RadioV2
{
    public class Test
    {
        static void Main()
        {

            //test for RadioStation
            RadioStation station = new RadioStation("Classic FM", "Music", 101.1);
            Console.WriteLine(station.ToString());
            Console.WriteLine();
            RadioStation station1 = new RadioStation("Classic FM", "News", 99.8);           
            Console.WriteLine(station1.ToString());
            Console.WriteLine();
            RadioStation station2 = new RadioStation("Classic FM", "General", 92.9);
            Console.WriteLine(station2.ToString());
            Console.WriteLine();

            //test for RadioPlayer
            RadioPlayerApp app = new RadioPlayerApp();

            // Display all radio stations
            app.DisplayStations();

            // Play a specific station
            app.PlayStation("RTE 2FM");

            // Pause a specific station
            app.PauseStation("FM 104");

            // Like a specific station
            app.LikeStation("Newstalk");


            // Display favorite stations
            Console.WriteLine("Favorite Stations:");
            foreach (var sta in app.FavoriteStations)
            {
                Console.WriteLine(sta.ToString());
            }

            // Use the indexer to get stations by genre
            Console.WriteLine("Music Stations:");
            var musicStations = app["Music"];
            foreach (var stationName in musicStations)
            {
                Console.WriteLine(stationName);
            }
        }

       



    }
    
}
