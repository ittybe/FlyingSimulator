using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ObjectUnits
{
    struct CharLocate
    {
        public char Char{ get; set; }
        public int Y{ get; set; }
        public CharLocate(char _Char = ' ', int y = 0) 
        {
            Char = _Char;
            Y = y;
        }
        public static explicit operator char(CharLocate charLocate) 
        {
            return charLocate.Char;
        }
        public static explicit operator int(CharLocate charLocate)
        {
            return charLocate.Y;
        }
    }

    class Mountain : IEnumerable<CharLocate>, ICloneable, Rendering.IRenderer
    {
        public CharLocate[] CharMountain { get; protected set; }
        public int Length { get { return CharMountain.Length; } }
        public Mountain(int Length = 1) 
        {
            CharMountain = new CharLocate[Length];
            
        }
        public Mountain(int Length, char fillingChar, int fillingY )
        {
            CharMountain = new CharLocate[Length];
            Mountain.FillCharLocateArray(CharMountain, fillingChar, fillingY);
        }
        public int IndexMaxY() 
        {
            int index = 0;
            int tmp = CharMountain[0].Y;
            for (int i = 0; i < CharMountain.Length; i++)
            {
                if (tmp < CharMountain[i].Y) 
                {
                    tmp = CharMountain[i].Y;
                    index = i;
                }
            }
            return index;
        }
        static void FillCharLocateArray(CharLocate[]arr,char fillingChar, int fillingY) 
        {
            for (int i = 0; i < arr.Length; i++) 
            {
                arr[i].Char = fillingChar;
                arr[i].Y = fillingY;
            }
        }
        public void Render(int ShiftX = 0, int ShiftY = 0) 
        {
            CharLocate Highest = CharMountain[IndexMaxY()];
            for (int i = 0; i < CharMountain.Length; i++) 
            {
                CharLocate charLocate = CharMountain[i];

                Console.SetCursorPosition(ShiftX + i, ShiftY + (Highest.Y - charLocate.Y));
                Console.Write(charLocate.Char);
            }
        }
        public static Mountain Generate(int Height, int Length) 
        {
            if (Height > Length / 2)
                throw new ArgumentOutOfRangeException($"Height ({Height}) can't be more than Length/2 ({Length / 2})");
            
            Mountain result = new Mountain(Length);
            Random r = new Random();
            
            List<CharLocate> charMountain = new List<CharLocate>(result.CharMountain);
            
            int numOfPlates = 0;
            // last '_' char on the top demand +1 extra space
            Height--;
            for (int i = 0; i < Length; i++) 
            {
                // up hill case
                if (i < Height)
                    charMountain[i] = new CharLocate('/', i);
                // down hill case
                else if (i > Height && (Height * 2) - i >= 0)
                    charMountain[i] = new CharLocate('\\', (Height * 2) - i);
                // plate on mountain case
                else if (i == Height)
                    charMountain[i] = new CharLocate('_', i);
                else
                {
                    numOfPlates = Length - i;
                    charMountain.RemoveRange(i, numOfPlates);
                    break;
                }
            }

            // cover mountain with plates

            for (int i = 0; i < numOfPlates; i++)
            {
                int index = r.Next(0, charMountain.Count);

                CharLocate ShiftChar = charMountain[index];

                // if '\\' char we need to increase Y var (feature of console)
                int Y = ShiftChar.Char == '\\' ? ShiftChar.Y+1 : ShiftChar.Y;
                CharLocate charPlate = new CharLocate('_', Y);

                charMountain.Insert(index, charPlate);
            }
            result.CharMountain = charMountain.ToArray();

            return result;
        }
        static void Swap<T>(ref T A, ref T B)
        {
            T temp = A;
            A = B;
            B = temp;
        }
        
        public object Clone()
        {
            Mountain result = (Mountain)MemberwiseClone();
            result.CharMountain = (CharLocate[])CharMountain.Clone();
            return result;
        }

        IEnumerator<CharLocate> IEnumerable<CharLocate>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return new MountainEnumerator(this);
        }

        public CharLocate this[int index] 
        {
            get 
            {
                return CharMountain[index];
            }
            set 
            {
                CharMountain[index] = value;
            }
        }
    }
    class MountainEnumerator : IEnumerator<CharLocate>
    {
        private CharLocate[] mountain;
        private int index;

        public MountainEnumerator(Mountain _mountain) 
        {
            mountain = (CharLocate[])_mountain.CharMountain.Clone();
            index = -1;
        }

        object IEnumerator.Current => mountain[index];

        CharLocate IEnumerator<CharLocate>.Current => mountain[index];

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++index >= mountain.Length)
            {
                return false;
            }
            return true;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}