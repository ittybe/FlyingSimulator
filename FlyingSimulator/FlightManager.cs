using System;
using System.Collections.Generic;
using System.Text;

namespace Managering
{
    class FlightManager
    {
        public int Id{ get; set; }
        public int WeatherCorrection { get; private set; }

        public FlyingSimulator.FlyingSimulator Simulator { get; private set; }
        public FlightManager(FlyingSimulator.FlyingSimulator simulator, int id) 
        {
            Random r = new Random();
            WeatherCorrection = r.Next(-200, 200);
            Simulator = simulator;
            Id = id;
        }
        public int RecommendedHeight() 
        {
            int RecommendedHeight= 7*Simulator.MetersPerSec - WeatherCorrection;
            return RecommendedHeight;
        }
    }
}
