using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSCD371FinalProject
{
    public abstract class Shape
    {
        public class Block
        {
            public int Y { get; set; }
            public int X { get; set; }

            public Block(int x, int y)
            {
                this.Y = y;
                this.X = x;
            }

        }
        protected int rgb { get; set; }
        protected Block PivotBlock;
        public List<Block> AllBlocks;
        protected int DegreesRotated = 0;
        protected int LowestPoint = 0;
        protected bool IsBottomedOut = false;
        public Shape(int StartX)
        {
            StartX = CheckBounds(StartX);
            AllBlocks = new List<Block>();
            this.PivotBlock = new Block(0, 4);
            this.PopulateBlocks();
            this.rgb = 0;
        }

        public void ResetShape()
        {
            this.PivotBlock = new Block(0, 4);
            AllBlocks = new List<Block>();
            this.DegreesRotated = 0;
            this.LowestPoint = 0;
            this.IsBottomedOut = false;
            this.PopulateBlocks();
        }
        public List<Block> GetBlocks()
        {
            return this.AllBlocks;
        }

        public abstract void PopulateBlocks();
        public abstract void Rotate(TetrisGrid tetris);

        public int CheckBounds(int startx)
        {
            if (this is LineShape)
            {
                if (startx > 5)
                {
                    return 5;
                }
                else if (startx < 0)
                {
                    return 0;
                }
                return 4;
            }
            else if (this is LeftZed || this is TeeShape || this is LeftHook || this is Square)
            {
                if (startx > 6)
                {
                    return 6;
                }
                else if (startx < 0)
                {
                    return 0;
                }
                return 4;
            }
            else if (this is RightHook || this is RightZed)
            {
                if (startx > 9)
                {
                    return 9;
                }
                else if (startx < 4)
                {
                    return 4;
                }
                return startx;
            }
            else return 4;
        }

        public bool BottomedOut(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int lowestpointofgrid = grid.Length - 1;
            if (LowestPoint == lowestpointofgrid || this.IsBottomedOut)
            {
                this.IsBottomedOut = true;
                return true;
            }
            else if (LowestPoint < lowestpointofgrid)
            {
                List<Block> BottomBlocks = new List<Block>();
                List<int> BlockColumns = new List<int>();
                foreach (Block block in AllBlocks)
                {
                    BlockColumns.Add(block.Y);
                }
                foreach (Block block in AllBlocks)
                {
                    if (block.X == LowestPoint)
                    {
                        BottomBlocks.Add(block);
                    }
                    if (block.X != LowestPoint)
                    {
                        List<Block> listofthiscolumnsblock = new List<Block>();
                        foreach (Block b in AllBlocks)
                        {
                            if (b.Y == block.Y)
                            {
                                listofthiscolumnsblock.Add(b);
                            }
                        }
                        if (block.X == listofthiscolumnsblock.Max(Block => Block.X))
                        {
                            BottomBlocks.Add(block);
                        }
                    }
                }
                foreach (Block block in BottomBlocks)
                {
                        if ((block.X + 1 <= 17) && grid[block.X + 1][block.Y] != 0)
                        {
                            this.IsBottomedOut = true;
                            return true;
                        }
                }
            }
            return false;
        }
        public void Draw(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            foreach (Block block in AllBlocks)
            {
                if(block.X > 17 || block.X < 0)
                {
                    tetris.SetGrid(grid);
                    return;
                }
                if(block.Y > 9 || block.Y < 0)
                {
                    tetris.SetGrid(grid);
                    return;
                }
                grid[block.X][block.Y] = this.rgb;
            }
            tetris.SetGrid(grid);
        }
        public void UnDraw(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            foreach (Block block in AllBlocks)
            {
                if (block.X > 17 || block.X < 0)
                {
                    tetris.SetGrid(grid);
                    return;
                }
                if (block.Y > 9 || block.Y < 0)
                {
                    tetris.SetGrid(grid);
                    return;
                }
                grid[block.X][block.Y] = 0;
            }
            tetris.SetGrid(grid);
        }
        public void ShiftShapeDown(TetrisGrid tetris)
        {
            UnDraw(tetris);
            foreach (Block block in AllBlocks)
            {
                block.X++;
            }
            Draw(tetris);
            LowestPoint++;
        }
        public void ShiftRight(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int right = AllBlocks.Max(Block => Block.Y);
            int maxright = 9;  
            if (right >= maxright)
            {
                return;
            }
            foreach (Block block in AllBlocks)
            {
                if (right < 9 && ( grid[block.X][right + 1] != 0))
                {
                        return;
                }
            }
            UnDraw(tetris);
            foreach (Block block in AllBlocks)
            {
                block.Y++;
            }
            Draw(tetris);
        }
        public void ShiftLeft(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int min = AllBlocks.Min(Block => Block.Y);
            int minleft = 0;
            if (min <= minleft)
            {
                return;
            }
            foreach (Block block in AllBlocks)
            {
                if (min > 0 && (grid[block.X][min - 1] != 0))
                {
                        return;
                }
            }
            UnDraw(tetris);
            foreach (Block block in AllBlocks)
            {
                block.Y--;
            }
            Draw(tetris);
        }

    }
    public class Square : Shape
    {
        public Square(int StartX) : base(StartX)
        {
            StartX = CheckBounds(StartX);
            this.rgb = 1;
            
        }
        public override void PopulateBlocks()
        {
            this.AllBlocks.Add(PivotBlock);
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
            this.LowestPoint = PivotBlock.X + 1;
        }
        public override void Rotate(TetrisGrid tetris)
        {
            if (BottomedOut(tetris))
            {
                return;
            }          
            Draw(tetris);
        }
    }
    public class TeeShape : Shape
    {
        public TeeShape(int StartX) : base(StartX)
        {
            StartX = CheckBounds(StartX);
            this.rgb = 2;
        }
        public override void PopulateBlocks()
        {
            this.AllBlocks.Add(PivotBlock);
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 2));
            this.LowestPoint = PivotBlock.X + 1;
        }
        public override void Rotate(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int maxRight = grid[0].Length - 1, maxTop = grid.Length - 1;
            if (BottomedOut(tetris))
            {
                return;
            }
            UnDraw(tetris);
            switch (DegreesRotated)
            {
                case 0:
                    if (PivotBlock.X + 2 > maxTop)
                    {
                        PivotBlock.X -= 2;
                    }
                    else if (PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 2, PivotBlock.Y));
                    LowestPoint = PivotBlock.X + 2;
                    this.DegreesRotated += 90;
                    break;
                case 90:
                    if (PivotBlock.Y - 2 < 0)
                    {
                        PivotBlock.Y += 2;
                    }
                    else if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 2));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 180:
                    if (PivotBlock.X - 2 < 0)
                    {
                        PivotBlock.X += 2;
                    }
                    else if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 2, PivotBlock.Y));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 270:
                    if (PivotBlock.Y + 2 > maxRight)
                    {
                        PivotBlock.Y -= 2;
                    }
                    else if (PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    if(PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 2));
                    LowestPoint = PivotBlock.X + 1;
                    this.DegreesRotated = 0;
                    break;
            }
            Draw(tetris);
        }
    }
    public class RightZed : Shape
    {
        public RightZed(int StartX) : base(StartX)
        {
            StartX = CheckBounds(StartX);
            this.rgb = 3;
        }
        public override void PopulateBlocks()
        {
            this.AllBlocks.Add(PivotBlock);
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y - 1));
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y - 2));
            this.LowestPoint = PivotBlock.X + 1;
        }
        public override void Rotate(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int maxRight = grid[0].Length - 1, maxTop = grid.Length - 1;
            if (BottomedOut(tetris))
            {
                return;
            }
            UnDraw(tetris);
            switch (DegreesRotated)
            {
                case 0:
                    if (PivotBlock.X - 2 < 0)
                    {
                        PivotBlock.X += 2;
                    }
                    else if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 2, PivotBlock.Y - 1));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 90:
                    if (PivotBlock.Y + 2 > maxRight)
                    {
                        PivotBlock.Y -= 2;
                    }
                    else if (PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y + 2));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 180:
                    if (PivotBlock.X + 2 > maxTop)
                    {
                        PivotBlock.X -= 2;
                    }
                    else if (PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 2, PivotBlock.Y + 1));
                    LowestPoint = PivotBlock.X + 2;
                    this.DegreesRotated += 90;
                    break;
                case 270:
                    if (PivotBlock.Y - 2 < 0)
                    {
                        PivotBlock.Y += 2;
                    }
                    else if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 2));
                    LowestPoint = PivotBlock.X + 1;
                    this.DegreesRotated = 0;
                    break;
            }
            Draw(tetris);
        }
    }
    public class LeftZed : Shape
    {
        public LeftZed(int StartX) : base(StartX)
        {
            StartX = CheckBounds(StartX);
            this.rgb = 4;
        }
        public override void PopulateBlocks()
        {
            this.AllBlocks.Add(PivotBlock);
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 2));
            this.LowestPoint = PivotBlock.X + 1;
        }
        public override void Rotate(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int maxRight = grid[0].Length - 1, maxTop = grid.Length - 1;
            if (BottomedOut(tetris))
            {
                return;
            }
            UnDraw(tetris);
            switch (DegreesRotated)
            {
                case 0:
                    if (PivotBlock.X + 2 > maxTop)
                    {
                        PivotBlock.X -= 2;
                    }
                    else if (PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    if(PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 2, PivotBlock.Y - 1));
                    LowestPoint = PivotBlock.X + 2;
                    this.DegreesRotated += 90;
                    break;
                case 90:
                    if (PivotBlock.Y - 2 < 0)
                    {
                        PivotBlock.Y += 2;
                    }
                    else if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    if(PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y - 2));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 180:
                    if (PivotBlock.X - 2 < 0)
                    {
                        PivotBlock.X += 2;
                    }
                    else if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    if(PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 2, PivotBlock.Y + 1));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 270:
                    if (PivotBlock.Y + 2 > maxRight)
                    {
                        PivotBlock.Y -= 2;
                    }
                    else if (PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    if (PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 2));
                    LowestPoint = PivotBlock.X + 1;
                    this.DegreesRotated = 0;
                    break;
            }
            Draw(tetris);
        }
    }
    public class RightHook : Shape
    {
        public RightHook(int StartX) : base(StartX)
        {
            this.rgb = 5;
        }
        public override void PopulateBlocks()
        {
            this.AllBlocks.Add(PivotBlock);
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 2));
            this.LowestPoint = PivotBlock.X + 1;
        }
        public override void Rotate(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int maxRight = grid[0].Length - 1, maxTop = grid.Length - 1;
            if (BottomedOut(tetris))
            {
                return;
            }
            UnDraw(tetris);
            switch (DegreesRotated)
            {
                case 0:
                    if (PivotBlock.X - 2 < 0)
                    {
                        PivotBlock.X += 2;
                    }  
                    else if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X - 2, PivotBlock.Y));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 90:              
                    if (PivotBlock.Y + 2 > maxRight)
                    {
                        PivotBlock.Y -= 2;
                    }
                    else if (PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 2));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 180:
                    if (PivotBlock.X + 2 > maxTop)
                    {
                        PivotBlock.X -= 2;
                    }
                    if (PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X + 2, PivotBlock.Y));
                    LowestPoint = PivotBlock.X + 2;
                    this.DegreesRotated += 90;
                    break;
                case 270:
                    if (PivotBlock.Y - 2 < 0)
                    {
                        PivotBlock.Y += 2;
                    }
                    else if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }       
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 2));
                    LowestPoint = PivotBlock.X + 1;
                    this.DegreesRotated = 0;
                    break;
            }
            Draw(tetris);
        }
    }

    public class LeftHook : Shape
    {

        public LeftHook(int StartX) : base(StartX)
        {
            StartX = CheckBounds(StartX);
            this.rgb = 6;
        }
        public override void PopulateBlocks()
        {
            this.AllBlocks.Add(PivotBlock);
            this.AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 2));
            this.LowestPoint = PivotBlock.X + 1;
        }
        public override void Rotate(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int maxRight = grid[0].Length - 1, maxTop = grid.Length - 1;
            if (BottomedOut(tetris))
            {
                return;
            }
            UnDraw(tetris);
            switch (DegreesRotated)
            {
                case 0:
                    if (PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    else if (PivotBlock.X + 2 > maxTop)
                    {
                        PivotBlock.X -= 2;
                    }
                    else if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 2, PivotBlock.Y - 1));
                    LowestPoint = PivotBlock.X + 2;
                    this.DegreesRotated += 90;
                    break;
                case 90:
                    if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    else if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    else if (PivotBlock.Y - 2 < 0)
                    {
                        PivotBlock.Y += 2;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y - 2));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 180:
                    if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    else if (PivotBlock.X - 2 < 0)
                    {
                        PivotBlock.X += 2;
                    }                    
                    else if (PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X - 2, PivotBlock.Y + 1));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 270:
                    if (PivotBlock.Y + 2 > maxRight)
                    {
                        PivotBlock.Y -= 2;
                    }
                    else if (PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    else if (PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y + 2));
                    LowestPoint = PivotBlock.X + 1;
                    this.DegreesRotated = 0;
                    break;
            }
            Draw(tetris);
        }
    }
    public class LineShape : Shape
    {
        public LineShape(int StartX) : base(StartX)
        {
            StartX = CheckBounds(StartX);
            this.rgb = 7;
        }
        public override void PopulateBlocks()
        {
            this.AllBlocks.Add(PivotBlock);
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 2));
            this.AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 3));
        }
        public override void Rotate(TetrisGrid tetris)
        {
            int[][] grid = tetris.GetGrid();
            int maxRight = grid[0].Length - 1, maxTop = grid.Length - 1;
            if (BottomedOut(tetris))
            {
                return;
            }
            UnDraw(tetris);
            switch(DegreesRotated)
            {
                case 0:                   
                    if(PivotBlock.X + 1 > maxTop)
                    {
                        PivotBlock.X -= 3;
                    }
                    else if(PivotBlock.X + 2 > maxTop)
                    {
                        PivotBlock.X -= 2;
                    }
                    else if (PivotBlock.X + 3 > maxTop)
                    {
                        PivotBlock.X -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X + 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X + 2, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X + 3, PivotBlock.Y));
                    LowestPoint = PivotBlock.X + 3;
                    this.DegreesRotated += 90;
                    break;
                case 90:
                    if (PivotBlock.Y - 1 < 0)
                    {
                        PivotBlock.Y += 3;
                    }
                    else if (PivotBlock.Y - 2 < 0)
                    {
                        PivotBlock.Y += 2;
                    }
                    else if (PivotBlock.Y - 3 < 0)
                    {
                        PivotBlock.Y += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 1));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 2));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y - 3));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 180:
                    if (PivotBlock.X - 1 < 0)
                    {
                        PivotBlock.X += 3;
                    }
                    else if (PivotBlock.X - 2 < 0)
                    {
                        PivotBlock.X += 2;
                    }
                    else if (PivotBlock.X - 3 < 0)
                    {
                        PivotBlock.X += 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X - 1, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X - 2, PivotBlock.Y));
                    AllBlocks.Add(new Block(PivotBlock.X - 3, PivotBlock.Y));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated += 90;
                    break;
                case 270:
                    if (PivotBlock.Y + 1 > maxRight)
                    {
                        PivotBlock.Y -= 3;
                    }
                    else if (PivotBlock.Y + 2 > maxRight)
                    {
                        PivotBlock.Y -= 2;
                    }
                    else if (PivotBlock.Y + 3 > maxRight)
                    {
                        PivotBlock.Y -= 1;
                    }
                    AllBlocks.Clear();
                    AllBlocks.Add(PivotBlock);
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 1));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 2));
                    AllBlocks.Add(new Block(PivotBlock.X, PivotBlock.Y + 3));
                    LowestPoint = PivotBlock.X;
                    this.DegreesRotated = 0;
                    break;
            }
            Draw(tetris);
        }
    }

    public class TetrisGrid
    {

        int[][] grid { get; set; }

        public int CurrentScore { get; set; }
        Shape CurrentShape { get; set; }
        private List<Shape> shapes = new List<Shape>();
        private Queue<Shape> shapequeue = new Queue<Shape>();
        Shape NextShape { get; set; }
        public TetrisGrid(int dimX, int dimY)
        {

            Random r = new Random();
            shapes.Add(new LineShape(r.Next(2, 5)));
            shapes.Add(new Square(r.Next(2, 5)));
            shapes.Add(new TeeShape(r.Next(2, 5)));
            shapes.Add(new LeftHook(r.Next(2, 5)));
            shapes.Add(new LeftZed(r.Next(2, 5)));
            shapes.Add(new RightHook(r.Next(2, 5)));
            shapes.Add(new RightZed(r.Next(2, 5)));
            for (int i = 0; i < 100; i++)
            {
                shapequeue.Enqueue(shapes[r.Next(0, 6)]);
            }
            this.grid = new int[dimY][];
            for (int i = 0; i < dimY; i++)
            {
                this.grid[i] = new int[dimX];
            }
            CurrentShape = PollFromQueue();
            NextShape = PollFromQueue();
            RefreshGrid();
        }

        public Shape GetNextShape()
        {
            return this.NextShape;
        }

        public Shape GetCurrentShape()
        {
            return this.CurrentShape;
        }
        public void ShiftShapeAllDown()
        {
            while(!this.CurrentShape.BottomedOut(this))
            {
                this.UpdateGrid();
            }
        }
        public void ResetShapeList()
        {
            foreach (Shape s in shapequeue)
            {
                s.ResetShape();
            }
            foreach (Shape s in shapes)
            {
                s.ResetShape();
            }
        }
        public int[][] GetGrid() { return this.grid; }

        public Shape PollFromQueue()
        {
            Random r = new Random();
            Shape ret = shapequeue.Dequeue();
            shapequeue.Enqueue(shapes[r.Next(0, 6)]);
            return ret;
        }
        public void UpdateGrid()
        {
            CurrentScore += this.CheckForFullRowsAndShift();
            if (this.CurrentBottomedOut())
            {
                this.CurrentShape = null;
                ResetShapeList();
                this.CurrentShape = this.NextShape;
                this.NextShape = PollFromQueue();
                this.CurrentShape.Draw(this);
            }
            else if (this.CurrentShape != null)
            {
                this.ShiftShapeDown();
            }
        }
        public int CheckForFullRowsAndShift() // returns the amount of rows that were full, aka the scoring
        {
            List<int[]> list = new List<int[]>();
            int score = 0;
            foreach (int[] arr in this.grid)
            {
                if (!RowIsFull(arr))
                {
                    list.Add(arr);
                }
                else
                {
                    score++;
                }
            } // at this point we have a list of rows that aren't full, so excluding ones that are full
            if (score > 0 && this.CurrentShape.BottomedOut(this))
            {
                int[][] newarr = new int[18][];
                for (int ix = 0; ix < 18; ix++)
                {
                    newarr[ix] = new int[10];
                }
                int i = this.grid.Length - 1;
                int j = i - (list.Count() - 1);
                foreach (int[] arr in list)
                {
                    arr.CopyTo(newarr[j], 0);
                    j++;
                }
                this.SetGrid(newarr);
                return score;
            }
            else return 0;
        }

        private bool RowIsFull(int[] arr)
        {
            foreach (int i in arr)
            {
                if (i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void RefreshGrid()
        {
            CurrentShape.UnDraw(this);
            CurrentShape.Draw(this);
        }
        public void SetGrid(int[][] newGrid)
        {
            this.grid = newGrid;
        }

        public void Rotate()
        {
            CurrentShape.Rotate(this);
        }


        public bool CurrentBottomedOut()
        {
            return this.CurrentShape.BottomedOut(this);
        }

        public void GoNextShape(Shape s)
        {
            this.NextShape = s;
        }
        public void ShiftShapeDown()
        {
            CurrentShape.ShiftShapeDown(this);
        }
        public void ShiftRight()
        {
            CurrentShape.ShiftRight(this);
        }

        public void ShiftLeft()
        {
            CurrentShape.ShiftLeft(this);
        }
        public bool youlost()
        {
            return this.CurrentShape.AllBlocks.Max(Block => Block.X) == 0 && this.CurrentShape.BottomedOut(this);
        }
        public override string ToString()
        {
            string ret = "";
            if (grid != null && grid[0] != null)
            {
                for (int i = 0; i < grid.Length; i++)
                {
                    for (int j = 0; j < grid[i].Length; j++)
                    {
                        if (grid[i][j] != 0)
                            ret += " [ " + grid[i][j] + " ] ";
                        else
                            ret += " [  ] ";
                    }
                    ret += "\r\n";
                }
            }
            else
            {
                ret = "Cannot read the damn grid";
            }
            return ret;
        }
    }
}
