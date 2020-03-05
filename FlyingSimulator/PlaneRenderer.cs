using System;
using System.Collections.Generic;
using System.Text;
using ObjectUnits;

namespace Rendering
{
    class PlaneRenderer : IRenderer, IFrame
    {
        public Plane Plane{ get; set; }
        public string PlaneImage{ get; set; }

        public int LengthFrame { get; set; }

        public int HeightFrame { get; set; }

        public PlaneRenderer(Plane plane, int _LengthFrame, int _HeightFrame, string planeImage = "*") 
        {
            Plane = plane;
            PlaneImage = planeImage;
            LengthFrame = _LengthFrame;
            HeightFrame = _HeightFrame;
        }
        public void Render(int ShiftX = 0, int ShiftY = 0)
        {
            if (HeightFrame < Plane.Y)
                throw new ApplicationException($"{ToString()}: HeightFrame({HeightFrame}) cant be less than Plane Y({Plane.Y})");

            Console.SetCursorPosition(ShiftX, ShiftY + (HeightFrame - Plane.Y));
            Console.Write(PlaneImage);
        }
    }
}
