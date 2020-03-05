using System;
using System.Collections.Generic;
using System.Text;
using ObjectUnits;

namespace Rendering
{
    interface IFrame 
    {
        public int LengthFrame{ get; }
        public int HeightFrame { get; }
    }
    interface IRenderer 
    {
        // draw something in console
        void Render(int ShiftX = 0, int ShiftY = 0);
    }

    class MountainRenderer: IRenderer, IFrame
    {
        public Mountain Canvas { get; protected set; }
        public int HeightFrame{ get; }
        public int LengthFrame { get; }

        public MountainRenderer(int _CanvasHeight, int CanvasLength) 
        {
            HeightFrame = _CanvasHeight;
            LengthFrame = CanvasLength;
            Canvas = new Mountain(LengthFrame);
        }

        public void ShiftCanvasLeft(CharLocate FillingElem) 
        {
            for (int i = 1; i < Canvas.CharMountain.Length; i++) 
            {
                Canvas[i - 1] = Canvas[i];
            }
            Canvas[Canvas.CharMountain.Length - 1] = FillingElem;
        }
        public void ShiftCanvasLeft(Mountain FillingMount)
        {
            int CanvasLength = Canvas.CharMountain.Length;
            int FillingLength = FillingMount.Length;
            
            //if (ShiftTimes > CanvasLength)
            //    throw new ApplicationException($"{ToString()}: ShiftTimes({ShiftTimes}) cant be more than Canvas.CharMountain.Length({Canvas.CharMountain.Length})");
            //if (FillingArray.Length != ShiftTimes)
            //    throw new ApplicationException($"{ToString()}: FillingArray.Length({ShiftTimes}) not equals ShiftTimes({ShiftTimes})");
            
            for (int i = FillingLength; i < CanvasLength; i++)
            {
                Canvas[i - FillingLength] = Canvas[i];
            }
            for (int i = 0; i < FillingLength; i++) 
            {
                Canvas[i + (CanvasLength - FillingLength)] = FillingMount[i];
            }
        }
        public void ShiftCanvasLeft(CharLocate[] FillingMount)
        {
            int CanvasLength = Canvas.CharMountain.Length;
            int FillingLength = FillingMount.Length;

            if (FillingLength > CanvasLength)
                throw new ApplicationException($"CanvasLength({CanvasLength}) can't be less than FillingLength({FillingLength})");

            for (int i = FillingLength; i < CanvasLength; i++)
            {
                Canvas[i - FillingLength] = Canvas[i];
            }
            int index = 0;
            for (int i = CanvasLength - FillingLength; i < CanvasLength; i++)
            {
                Canvas[i] = FillingMount[index++];
            }
        }

        public void ShiftCanvasRight(CharLocate FillingElem)
        {
            throw new NotImplementedException();
            for (int i = Canvas.CharMountain.Length - 1; i > 0; i--)
            {
                Canvas[i] = Canvas[i - 1];
            }
            Canvas[0] = FillingElem;
        }
        public void ShiftCanvasRight(CharLocate[] FillingMount)
        {
            throw new NotImplementedException();
            int CanvasLength = Canvas.CharMountain.Length;
            int FillingLength = FillingMount.Length;

            if (FillingLength > CanvasLength)
                throw new ApplicationException($"CanvasLength({CanvasLength}) can't be less than FillingLength({FillingLength})");

            for (int i = Canvas.Length - FillingMount.Length; i > 0; i--)
            {
                Canvas[i] = Canvas[i - FillingMount.Length];
            }
            for (int i = 0; i < FillingLength; i++)
            {
                Canvas[i + (CanvasLength - FillingLength)] = FillingMount[i];
            }
        }
        public void Render(int ShiftX = 0, int ShiftY = 0) 
        {
            int MaxY = Canvas[Canvas.IndexMaxY()].Y;
            if (HeightFrame < MaxY)
                throw new ApplicationException($"{ToString()}: HeightFrame({HeightFrame}) cant be less than MaxY({MaxY})");
            // if mountain is less than CanvasHeight
            ShiftY += HeightFrame - MaxY;
            Canvas.Render(ShiftX, ShiftY);
        }
    }
}
