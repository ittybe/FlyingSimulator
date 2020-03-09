using System;
using System.Collections.Generic;
using System.Text;
using FlyingSimulator;


namespace Rendering
{
    class FrameRenderer : IRenderer, IFrame
    {
        public FlyingSimulator.FlyingSimulator Simulator { get; protected set; }
        public int HeightFrame{ get; protected set; }
        public int LengthFrame{ get; protected set; }

        public MountainRenderer MountainRenderer{ get; private set; }
        public PlaneRenderer PlaneRenderer { get; private set; }
        public InfoRenderer InfoRenderer{ get; private set; }
        public FrameRenderer(FlyingSimulator.FlyingSimulator simulator, int _Height, int _Length) 
        {
            Simulator = simulator;
            HeightFrame = _Height;
            LengthFrame = _Length;
            MountainRenderer = new MountainRenderer(HeightFrame, LengthFrame);
            PlaneRenderer = new PlaneRenderer(Simulator.Plane, LengthFrame, HeightFrame);
            InfoRenderer = new InfoRenderer("");
        }
        
        public void Render(int ShiftX = 0, int ShiftY = 0)
        {
            PlaneRenderer.Render(ShiftX, ShiftY);
            MountainRenderer.Render(ShiftX, ShiftY);
            InfoRenderer.Render(ShiftX, ShiftY + HeightFrame+1);
        }
    }
}
