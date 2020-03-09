using System;
using System.Collections.Generic;
using System.Text;

namespace Rendering
{
    class InfoRenderer: IRenderer
    {
        public string InfoToRender{ get; set; }
        
        public InfoRenderer(string infoToRender = "")
        {
            InfoToRender = infoToRender;
        }
        public void Render(int ShiftX = 0, int ShiftY = 0)
        {
            string[] SplitedInfo = InfoToRender.Split('\n');
            int LineNumber = 0;
            foreach (var line in SplitedInfo)
            {
                Console.SetCursorPosition(ShiftX, ShiftY + LineNumber++);
                Console.Write(line);
            }
        }
    }

}
