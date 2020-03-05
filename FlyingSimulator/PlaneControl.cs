using System;
using System.Collections.Generic;
using System.Text;
using ObjectUnits;
using FlyingSimulator;
using System.Threading;

namespace ControlPanel
{
    interface IKeyBoardListrener
    {
        void Listen();
    }
    class PlaneControl : IKeyBoardListrener
    {
        public FlyingSimulator.FlyingSimulator Simulator{ get; set; }
        public int ChangeSpeedArrow{ get; set; }
        public bool IsCrushed { get; set; }
        public PlaneControl(FlyingSimulator.FlyingSimulator simulator, int _ChangeSpeedArrow = 100) 
        {
            Simulator = simulator;
            ChangeSpeedArrow = _ChangeSpeedArrow;
        }
        public void Listen()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: 
                        {
                            if (Simulator.Plane.Y < Simulator.FrameRenderer.HeightFrame)
                                Simulator.Plane.Y++;
                        }break;
                    case ConsoleKey.DownArrow:
                        {
                            if (Simulator.Plane.Y > 0)
                                Simulator.Plane.Y--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            Simulator.MetersPerSec += ChangeSpeedArrow;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            Simulator.MetersPerSec -= ChangeSpeedArrow;
                        }
                        break;
                    default:
                        break;
                }
                if (Simulator.IsPlaneCrushed)
                    break;
            }
        }
    }
}