using System;
using System.Collections.Generic;
using System.Text;
using ObjectUnits;
using Rendering;
using ControlPanel;
using System.Threading;

namespace FlyingSimulator
{
    class FlyingSimulator : IRenderer
    {
        public bool IsPlaneCrushed { get; protected set; }
        public Thread RenderingThread { get; protected set; }
        public Thread PlaneControlThread { get; protected set; }
        public Thread PlaneStateCheckThread { get; set; }
        
        public int FPS { get; set; }

        public double MetersPerShift { get; set; }


        private double shiftsPerFrame;
        public double ShiftsPerFrame
        {
            get { return shiftsPerFrame; }
            set { shiftsPerFrame = value; }
        }

        private double framesPerShift;
        public double FramesPerShift
        {
            get { return framesPerShift; }
            set { framesPerShift = value; }
        }

        public int MetersPerSec
        {
            get { return Plane.Speed; }
            set
            {
                Plane.Speed = value;
                FramesPerShift = FPS * MetersPerShift / MetersPerSec;
                ShiftsPerFrame = 1 / FramesPerShift;
            }
        }

        public FrameRenderer FrameRenderer { get; protected set; }
        public Plane Plane { get; protected set; }
        public PlaneControl PlaneControlPanel { get; protected set; }
        public PlaneStateChecker PlaneStateChecker { get; protected set; }
        
        public FlyingSimulator(int _Height, int _Length, int _FPS = 120, double _MetersPerShift = 10)
        {
            FPS = _FPS;
            MetersPerShift = _MetersPerShift;
            int StartSpeed = 500;
            Plane = new Plane(_Height/2, StartSpeed);
            FrameRenderer = new FrameRenderer(this, _Height, _Length);
            PlaneControlPanel = new PlaneControl(this);
            PlaneStateChecker = new PlaneStateChecker(this, StartSpeed);
        }
        protected string GetInfoToRender()
        {
            string info = "";
            info += $"Plane Speed : {Plane.Speed, 10}\n";
            info += $"Plane Height: {Plane.Y, 10}\n";
            return info;
        }

        public void Render(int ShiftX = 0, int ShiftY = 0)
        {
            /// MOUNTAIN ANIMATION
            Random r = new Random();
            int LengthMountain = r.Next(FrameRenderer.LengthFrame / 2, FrameRenderer.LengthFrame);
            int HeightMountain = r.Next(1, LengthMountain / 3);

            IEnumerator<CharLocate> buffer = (IEnumerator<CharLocate>)Mountain.Generate(HeightMountain, LengthMountain).GetEnumerator();
            buffer.MoveNext();
            List<CharLocate> filling = new List<CharLocate>();
            long FramesCount = 0;
            while (true)
            {
                //Console.WriteLine($"{ShiftsPerFrame} , {FramesPerShift} , {MetersPerSec}");
                //Thread.Sleep(100);
                FramesCount++;
                bool IsShift;
                if ((int)FramesPerShift < 2)
                    IsShift = FramesCount % 2 == 0;
                else
                    IsShift = FramesCount % (int)FramesPerShift == 0;
                if (IsShift)
                {
                    for (int i = 0; i < ShiftsPerFrame; i++)
                    {
                        filling.Add(buffer.Current);
                        if (!buffer.MoveNext())
                        {
                            LengthMountain = r.Next(FrameRenderer.LengthFrame / 2, FrameRenderer.LengthFrame);
                            HeightMountain = r.Next(1, LengthMountain / 2);

                            buffer = (IEnumerator<CharLocate>)Mountain.Generate(HeightMountain, LengthMountain).GetEnumerator();
                            buffer.MoveNext();
                        }
                    }
                    // processing info
                    FrameRenderer.MountainRenderer.ShiftCanvasLeft(filling.ToArray());
                    filling.Clear();
                    FrameRenderer.InfoRenderer.InfoToRender = GetInfoToRender();

                    Console.Clear();
                    FrameRenderer.Render(ShiftX, ShiftY);
                    if (IsPlaneCrushed)
                        break;
                }
                else
                {
                    Thread.Sleep(1000 / FPS);
                }
            }
        }

        
        public void Stop()
        {
            // NOT SUPPORTED ON MY PLATFORM

            //PlaneControlThread?.Abort();
            //RenderingThread?.Abort();
        }
        public void Start()
        {
            PlaneControlThread = new Thread(PlaneControlPanel.Listen);
            RenderingThread = new Thread(() => Render(4, 5));
            PlaneStateCheckThread = new Thread(PlaneStateChecker.StartChecking);
            
            PlaneStateChecker.PlaneCrush += () => IsPlaneCrushed = true;

            PlaneControlThread.Start();
            RenderingThread.Start();
            PlaneStateCheckThread.Start();
        }
    }
}
