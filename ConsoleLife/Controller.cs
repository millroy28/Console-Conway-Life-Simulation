using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace ConsoleLife
{
    class Controller
    {
        public int ColumnEnd { get; set; }
        public int RowEnd { get; set; }
        public int Generations { get; set; }

        public Controller(int _columnEnd, int _rowEnd, int _generations)
        {
            ColumnEnd = _columnEnd;
            RowEnd = _rowEnd;
            Generations = _generations;
        }
        public Controller(int _generations)
        {
            ColumnEnd = 50;
            RowEnd = 30;
            Generations = _generations;
        }
        public Controller() //Default Constructor = default grid parameters
        {
            ColumnEnd = 50;
            RowEnd = 30;
            Generations = 1000;
        }

        public List<Cell> CreateEnvironment()
        {
            //Creates and returns a list of cells based on grid parameters
            List<Cell> field = new List<Cell>();
            int totalCells = ColumnEnd * RowEnd;
            for (int i = 0; i < totalCells; i++)
            {
                field.Add(new Cell());
            }
            return field;
        }

        public void DisplayAndCount(List<Cell> field)
        {   
            //Break this into easier to understand methods - it's too unwiedly now
            int generation = 0;
            int neighbors = 0;
            int listPosition = 0;
            int population = 0;
            float pollutionTotal = 0;
            float averageCellPollution = 0;
            int fieldCount = field.Count;
            int populationPeak = 0;
            float averageCellPollutionPeak = 0;
            int populationPeakGeneration = 0;
            int averageCellPollutionPeakGeneration = 0;
            int peakCounter = 0;
            int populationDipPostPeak = 999999;
            int populationDipPostPeakGeneration = 9999;
            int previousPopPeak = 0;


            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(3, RowEnd + 1);
            Console.Write($"Generation:\tPopulation:\tAve Pollution Per Cell\tAvg Pollution Effect Percentage");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(3, RowEnd + 3);
            Console.Write($"Max Population:  Max Pop Gen:  Max Avg Cell Pollution:  MACP Gen:   Pop Min Post Peak: PMPP Gen:");
            Console.ResetColor();
            while (generation <= Generations)
            {

                for (int row = 1; row<=RowEnd; row++)
                {
                    for (int col = 1; col<=ColumnEnd; col++)
                    {
                        bool prevAlive = field[listPosition].IsAlive;
                        neighbors = NeighborCount(listPosition, field);
                        field[listPosition].IsAlive = field[listPosition].GetLifeStatus(neighbors);
                        //DISPLAY Iteration formula: col + row-1*ColumnEnd -1 (to 0 index)
                        if(prevAlive == field[listPosition].IsAlive)
                        {
                            if (field[listPosition].IsAlive)
                            {                          
                                field[listPosition].Pollution += 5;    //Adds polution to each live cell
                                population++;
                            }
                            else
                            {
                              
                                if (field[listPosition].Pollution > 1)
                                {
                                    field[listPosition].Pollution -= 1;    //Removes polution to each vacant cell
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(col, row);
                            if (field[listPosition].IsAlive)
                            {
                                Console.Write("█");
                                field[listPosition].Pollution += 5;    //Adds polution to each live cell
                                population++;
                            }
                            else
                            {
                                Console.Write("░");
                                if (field[listPosition].Pollution > 1)
                                {
                                    field[listPosition].Pollution -= 1;    //Removes polution to each vacant cell
                                }
                            }
                        }
                        listPosition = col + ((row - 1) * ColumnEnd) - 1;
                    }
                }

                foreach(Cell cell in field)
                {
                    pollutionTotal += cell.Pollution;
                    averageCellPollution = pollutionTotal / fieldCount;                    
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(3, RowEnd + 2);
                Console.Write(generation);
                Console.SetCursorPosition(20, RowEnd + 2);
                Console.Write("         ");
                Console.SetCursorPosition(20, RowEnd + 2);
                Console.Write(population);
                Console.SetCursorPosition(35, RowEnd + 2);
                Console.Write("              ");
                Console.SetCursorPosition(35, RowEnd + 2);
                Console.Write(averageCellPollution);
                Console.SetCursorPosition(60, RowEnd + 2);
                Console.Write("       ");
                Console.SetCursorPosition(60, RowEnd + 2);
                Console.Write((averageCellPollution / field[0].PollutionDenom).ToString("P"));
                
                if (population > populationPeak)
                {
                    populationPeak = population;
                    populationPeakGeneration = generation;
                }
                if (averageCellPollution > averageCellPollutionPeak)
                {
                    averageCellPollutionPeak = averageCellPollution;
                    averageCellPollutionPeakGeneration = generation;
                }
                
                if (populationPeak != previousPopPeak)
                {
                    previousPopPeak = populationPeak;
                    peakCounter = 0;
                } else
                {
                    previousPopPeak = populationPeak;
                    peakCounter++;
                }

                if (peakCounter > 500 && population < populationDipPostPeak && populationPeak > 500)
                {
                    populationDipPostPeak = population;
                    populationDipPostPeakGeneration = generation;
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(3, RowEnd + 4);
                Console.Write(populationPeak);
                Console.SetCursorPosition(20, RowEnd + 4);
                Console.Write("         ");
                Console.SetCursorPosition(20, RowEnd + 4);
                Console.Write(populationPeakGeneration);
                Console.SetCursorPosition(35, RowEnd + 4);
                Console.Write("              ");
                Console.SetCursorPosition(35, RowEnd + 4);
                Console.Write((averageCellPollutionPeak / field[0].PollutionDenom).ToString("P"));
                Console.SetCursorPosition(60, RowEnd + 4);
                Console.Write("       ");
                Console.SetCursorPosition(60, RowEnd + 4);
                Console.Write(averageCellPollutionPeakGeneration);
                Console.SetCursorPosition(73, RowEnd + 4);
                Console.Write("       ");
                Console.SetCursorPosition(73, RowEnd + 4);
                Console.Write(populationDipPostPeak);
                Console.SetCursorPosition(93, RowEnd + 4);
                Console.Write("       ");
                Console.SetCursorPosition(93, RowEnd + 4);
                Console.Write(populationDipPostPeakGeneration);
                Console.ResetColor();

                averageCellPollution = 0;
                pollutionTotal = 0;
                population = 0;
                generation++;
            }
            
        }

        public int NeighborCount(int listPosition, List<Cell> field)
        {
            int neighbors = 0;
            
            //upper left, center, right
            for (int h = -1; h <= 1; h++)
            {
                 if(listPosition + h - ColumnEnd >= 0)
                {
                    if(field[listPosition + h - ColumnEnd].IsAlive)
                    {
                        neighbors++;
                    }
                }
            }
            //same row, left, right
            for (int h = -1; h <= 1; h += 2)
            {
                 if(listPosition + h >= 0 && listPosition + h < RowEnd * ColumnEnd)
                {
                    if (field[listPosition + h].IsAlive)
                    {
                        neighbors++;
                    }
                }
               
            }
            //lower left, center, right
            for (int h = -1; h <= 1; h++)
            {
                 if(listPosition + h + ColumnEnd < RowEnd * ColumnEnd)
                    if (field[listPosition + h + ColumnEnd].IsAlive)
                    {
                        neighbors++;
                    }
               
            }
            return neighbors;
        }

        public void StartLife()
        {
            List<Cell> field = new List<Cell>();
            field = CreateEnvironment();
            DisplayAndCount(field);
        }
    }
}
