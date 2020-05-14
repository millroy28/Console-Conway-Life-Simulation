 using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ConsoleLife
{
    class Cell
    {
        public bool IsAlive { get; set; }
        public int Pollution { get; set; }

        public Cell()
        {
            IsAlive = false;
            Pollution = 0;
        }

        public bool GetLifeStatus(int neighbors)
        {  
            Random randIntGen = new Random();
           
            if(neighbors < 2)
            {
                //SPONTANEOUS GENERATION  to do: move above or into a function above to return status
                int spontaneousGenerationThreshold = 20;
                if (randIntGen.Next(1, 10000)< spontaneousGenerationThreshold)
                {
                    return true;
                }
            }
            else
            {
                //TRADITIONAL CONWAY RULES:
                if (!IsAlive && neighbors == 3)
                {
                    //POLLUTION EFFECT As pollution in cell rises, it will be less likely for a cell to "get born"
                    if (randIntGen.Next(1, 10000) > Pollution)
                    {
                        return true;
                    }
                    else
                    {
                    return false;
                    }
                }
                else if (IsAlive && neighbors < 2)
                {
                    //POLLUTION EFFECT As pollution in cell rises, it will be more likely for a cell to spontaneously die
                    if (randIntGen.Next(1, 10000) > Pollution)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (IsAlive && neighbors > 3)
                {
                    //POLLUTION EFFECT As pollution in cell rises, it will be less likely for a cell to "get born"
                    if (randIntGen.Next(1, 10000) > Pollution)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (IsAlive)
                {
                    return true;
                }

            }
            return false;

        }

    }
}
