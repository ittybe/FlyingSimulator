using System;
using System.Collections.Generic;
using System.Text;
using FlyingSimulator;
using System.Threading;
namespace ControlPanel
{
    interface IChecker 
    {
        void StartChecking();
    }
    class PlaneStateChecker: IChecker
    {
        public delegate void StateHandler();

        public event StateHandler PlaneCrush;

        public int FallingSpeed { get; set; }

        public FlyingSimulator.FlyingSimulator Simulator{ get; protected set; }

        public PlaneStateChecker(FlyingSimulator.FlyingSimulator simulator, int _FallingSpeed = 500) 
        {
            Simulator = simulator;
            FallingSpeed = _FallingSpeed;
        }

        public void CheckPlaneCrush()
        {
            bool PlaneCrushedIntoMountain = Simulator.Plane.Y <= Simulator.FrameRenderer.MountainRenderer.Canvas[0].Y;
            //bool PlaneCrushedLowSpeed = Simulator.Plane.Y == 0 && Simulator.MetersPerSec < FallingSpeed;
            if (PlaneCrushedIntoMountain)
            {
                PlaneCrush?.Invoke();
            }
        }

        public void CheckPlaneFalling() 
        {
            bool IsFallingSpeed = Simulator.MetersPerSec < FallingSpeed;
            if (IsFallingSpeed) 
            {
                Simulator.Plane.Y--;
                Thread.Sleep(Simulator.MetersPerSec);
            }
        }
        public void StartChecking()
        {
            while (true) 
            {
                CheckPlaneCrush();
                CheckPlaneFalling();
                if (Simulator.IsPlaneCrushed)
                    break;
            }
        }
    }
}
