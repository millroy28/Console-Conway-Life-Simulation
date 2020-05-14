using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            int generation = 0;
            int neighbors = 0;
            int listPosition = 0;
            
            while (generation <= Generations)
            {
                for (int row = 1; row<=RowEnd; row++)
                {
                    for (int col = 1; col<=ColumnEnd; col++)
                    {
                        //DISPLAY Iteration formula: col + row-1*ColumnEnd -1 (to 0 index)
                        listPosition = col + ((row - 1) * ColumnEnd) - 1;
                        Console.SetCursorPosition(col, row);
                        if (field[listPosition].IsAlive)
                        {
                            Console.Write("█");
                            field[listPosition].Pollution++;    //Adds polution to each live cell
                        }
                        else
                        {
                            Console.Write("░");
                        }

                        neighbors = NeighborCount(listPosition, field);
                        field[listPosition].IsAlive = field[listPosition].GetLifeStatus(neighbors);

                        Console.SetCursorPosition(3, RowEnd + 1);
                        Console.Write($"Generation: {generation}");
                    }
                }

                generation++;
            }
            
        }

        public int NeighborCount(int listPosition, List<Cell> field)
        {
            int neighbors = 0;
            
            //upper left, center, right
            for (int h = -1; h <= 1; h++)
            {
                try //present to account for border. when out of range (top/bottom row) does not add to neighbors
                {
                    if(field[listPosition + h - ColumnEnd].IsAlive)
                    {
                        neighbors++;
                    }
                }
                catch 
                {
                    
                }
            }
            //same row, left, right
            for (int h = -1; h <= 1; h += 2)
            {
                try
                {
                    if (field[listPosition + h].IsAlive)
                    {
                        neighbors++;
                    }
                }
                catch
                {

                }
            }
            //lower left, center, right
            for (int h = -1; h <= 1; h++)
            {
                try
                {
                    if (field[listPosition + h + ColumnEnd].IsAlive)
                    {
                        neighbors++;
                    }
                } catch
                {

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
