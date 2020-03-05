using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectUnits
{
    class Plane
    {
        public delegate void StateHandler(int CurrentState);

        public event StateHandler SpeedChangeEvent;

        public event StateHandler YChangeEvent;

        public Plane(int _y, int _speed)
        {
            Y = _y;
            Speed = _speed;
        }

        private int y;

        public int Y
        {
            get { return y; }
            set 
            {
                y = value;
                YChangeEvent?.Invoke(y);
            }
        }
        private int speed;

        public int Speed
        {
            get { return speed; }
            set 
            { 
                if(value >= 0)
                    speed = value;
                SpeedChangeEvent?.Invoke(speed);
            }
        }


    }
}
