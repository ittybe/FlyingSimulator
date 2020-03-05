using System; 
using System.Collections.Generic;
using System.Diagnostics;
using ObjectUnits;
using Rendering;
namespace FlyingSimulator
{
    // TODO срандомь 10 колекций в 1 массив и доставай их по необзодимости
    // так же shiftCanvasLeft() +4 на 4 позиции сразу для отображения скорости 
    class Program
    {
        static void Main(string[] args)
        {

            //Plane plane = new Plane(10, 100);

            //Console.WriteLine(plane.Y);
            //plane.Y++;
            //Console.WriteLine(plane.Y);
            int hei = 50;
            int len = 80;

            FlyingSimulator flyingSimulator = new FlyingSimulator(hei, len);
            try
            {
                flyingSimulator.Start();
            }
            catch (ApplicationException exp) 
            {
                Console.WriteLine(exp.Message);
            }
            finally
            {
                flyingSimulator.Stop();
            }
#if RE
            Random r = new Random();
            int len = 60;
            int height = 10;

            MountainRenderer MountainRenderer = new MountainRenderer(height, len);
            //MountainRenderer.BufferMountain = (IEnumerator<CharLocate>)Mountain.Generate(10, 50).GetEnumerator();
            IEnumerator<CharLocate> buffer = (IEnumerator<CharLocate>)Mountain.Generate(height, len).GetEnumerator();
            
            CharLocate FillingElem;

            const int MetersPerShift = 100;
            
            int fps = 24;

            int MetersPerSecond = 300;
            int ShiftsPerSecond = MetersPerSecond / MetersPerShift;
            int FramesPerShift = fps / ShiftsPerSecond;

            long FramesCount = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int counter = 0;
            while (true)
            {
                FramesCount++;
                if (FramesCount % FramesPerShift == 0)
                {
                    if (buffer.MoveNext())
                        FillingElem = buffer.Current;
                    else
                    {
                        //buffer.Reset()
                        int _height = r.Next(height / 2, height);
                        int _len = r.Next(_height*2, len);
                        buffer = (IEnumerator<CharLocate>)Mountain.Generate(_height, _len).GetEnumerator();
                        buffer.MoveNext();
                        FillingElem = buffer.Current;
                    }
                    
                    MountainRenderer.ShiftCanvasLeft(FillingElem);
                    Console.Clear();
                    MountainRenderer.Render(0, 10);
                    counter++;
                    if (counter == ShiftsPerSecond)
                    {
                        stopwatch.Stop();
                        break;
                    }
                }
                else 
                {
                    System.Threading.Thread.Sleep(1000 / fps);
                }
            }
            Console.Clear();
            Console.WriteLine("Elapsed Time milisec is {0}", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"Shifts per sec {ShiftsPerSecond}");
            Console.WriteLine($"Counter is {counter}");
            Console.WriteLine($"MetersPerSecond: {MetersPerSecond}");
            Console.WriteLine($"ShiftsPerSecond : {ShiftsPerSecond }");
            Console.WriteLine($"FramesPerShift: {FramesPerShift}");
            //int СoefficientAlign = ((int)stopwatch.ElapsedMilliseconds - 1000) / fps;
            int СoefficientAlign = 0;
            Console.WriteLine($"1000 / fps - СoefficientAlign : {1000 / fps - СoefficientAlign }");
            Console.WriteLine($"1000 / fps : {1000 / fps }");

            Console.WriteLine($"СoefficientAlign is {СoefficientAlign}");


            Console.ReadKey();
            while (true)
            {
                FramesCount++;
                if (FramesCount % FramesPerShift == 0)
                {
                    if (buffer.MoveNext())
                        FillingElem = buffer.Current;
                    else
                    {
                        //buffer.Reset()
                        int _height = r.Next(height / 2, height);
                        int _len = r.Next(_height * 2, len);
                        buffer = (IEnumerator<CharLocate>)Mountain.Generate(_height, _len).GetEnumerator();
                        buffer.MoveNext();
                        FillingElem = buffer.Current;
                    }

                    MountainRenderer.ShiftCanvasLeft(FillingElem);
                    Console.Clear();
                    MountainRenderer.Render(0, 10);
                }
                else
                {
                    int milisec = 1000 / fps - СoefficientAlign;
                    if (milisec < 0)
                        milisec = 3;
                    System.Threading.Thread.Sleep(milisec);
                }
            }
#endif
        }
    }
}
