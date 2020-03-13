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

        public event StateHandler PlaneLanded;

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
            bool PlaneLanding = Simulator.Plane.Y == 0;
            // Simulator.FrameRenderer.Simulator
            string CharsMountain = @"/_\";
            
            bool PlaneInMountain = CharsMountain.IndexOf(Simulator.FrameRenderer.MountainRenderer.Canvas[0].Char) >= 0 && Simulator.Plane.Y == 0;
            //bool PlaneCrushedLowSpeed = Simulator.Plane.Y == 0 && Simulator.MetersPerSec < FallingSpeed;
            if ((PlaneCrushedIntoMountain && !PlaneLanding) || PlaneInMountain)
            {
                PlaneCrush?.Invoke();
                Simulator.IsSimulationStoped = true;
            }
        }

        public void CheckPlaneFalling() 
        {
            bool IsFallingSpeed = Simulator.MetersPerSec < FallingSpeed;
            bool IsLanded = Simulator.Plane.Y == 0;
            if (IsFallingSpeed && !IsLanded)
            {
                Simulator.Plane.Y--;
                Thread.Sleep(Simulator.MetersPerSec);
            }
        }
        public void CheckPlaneLanding() 
        {
            bool IsLanded = Simulator.Plane.Y == 0 && Simulator.PassedDistance >= Simulator.Distance;
            if (IsLanded) 
            {
                PlaneLanded?.Invoke();
                Simulator.IsSimulationStoped = true;
            }
        }
        public void StartChecking()
        {
            while (true) 
            {
                CheckPlaneCrush();
                CheckPlaneFalling();
                CheckPlaneLanding();
                if (Simulator.IsSimulationStoped)
                    break;
            }
        }
    }
}
