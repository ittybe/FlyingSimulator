using System;
using System.Collections.Generic;
using System.Text;
using FlyingSimulator;


namespace Rendering
{
    class FrameRenderer : IRenderer, IFrame
    {
        public int HeightFrame{ get; protected set; }
        public int LengthFrame{ get; protected set; }

        public MountainRenderer MountainRenderer{ get; private set; }
        public PlaneRenderer PlaneRenderer { get; private set; }

        public FrameRenderer(ObjectUnits.Plane plane, int _Height, int _Length) 
        {
            HeightFrame = _Height;
            LengthFrame = _Length;
            MountainRenderer = new MountainRenderer(HeightFrame, LengthFrame);
            PlaneRenderer = new PlaneRenderer(plane, LengthFrame, HeightFrame);
        }

        public void Render(int ShiftX = 0, int ShiftY = 0)
        {
            PlaneRenderer.Render(ShiftX, ShiftY);
            MountainRenderer.Render(ShiftX, ShiftY);
        }
    }
}
