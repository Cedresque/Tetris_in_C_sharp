using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    public class Board
   {
        private int rows;
        private int columns;

        private int score;
        public int Score
        {
            get { return score; }
        }

        private int lines_filled;
        public int Lines_filled
        {
            get { return lines_filled; }
        }

        private bool game_over;
        public bool Game_Over
        {
            get { return game_over; }
        }
        private Gamepiece current_Gamepiece;
        private Label[,] block_Controls;

        static private Brush noBrush = Brushes.Transparent;
        static private Brush grayBrush = Brushes.Gray;

        public Board(Grid TetrisGrid)
        {
            rows = TetrisGrid.RowDefinitions.Count;
            columns = TetrisGrid.ColumnDefinitions.Count;
            score = 0;
            lines_filled = 0;
            game_over = false;

            block_Controls = new Label[columns, rows];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    block_Controls[i, j] = new Label();
                    block_Controls[i, j].Background = noBrush;
                    block_Controls[i, j].BorderBrush = grayBrush;
                    block_Controls[i, j].BorderThickness = new Thickness(1, 1, 1, 1);
                    Grid.SetColumn(block_Controls[i, j], i);
                    Grid.SetRow(block_Controls[i, j], j);
                    TetrisGrid.Children.Add(block_Controls[i, j]);
                }
            }

            current_Gamepiece = new Gamepiece();
            DrawCurrentGamepiece();
        }
        private void DrawCurrentGamepiece()
        {
            Point position = current_Gamepiece.Current_Postion;
            Point[] shape = current_Gamepiece.Current_Shape;
            Brush color = current_Gamepiece.Current_Colour;
            int actual_x;
            int actual_y;
            foreach ( Point punkt in shape)
            {
                actual_x = (int)(punkt.X + position.X) + ((columns/2)-1);
                actual_y = (int)(punkt.Y + position.Y) + 2;
                block_Controls[actual_x, actual_y].Background = color;
            }

        }

        private void EraseCurrentGamepiece()
        {
            Point position = current_Gamepiece.Current_Postion;
            Point[] shape = current_Gamepiece.Current_Shape;
            int actual_x;
            int actual_y;
            foreach ( Point punkt in shape)
            {
                actual_x = (int)(punkt.X + position.X) + ((columns/2)-1);
                actual_y = (int)(punkt.Y + position.Y) + 2;
                block_Controls[actual_x, actual_y].Background = noBrush;
            }
        }

        private void CheckRows()
        {
            bool full;
            for(int i = rows - 1; i > 0; i--)
            {
                full = true;
                for (int j = 0; j< columns; j++)
                {
                    if(block_Controls[j,i].Background == noBrush)
                    {
                        full = false;
                    }
                }
                if (full)
                {
                    RemoveRow(i);
                    score += 100;
                    lines_filled += 1;
                    CheckCertainRow(i, 2);
                }
            }
        }
        private void CheckCertainRow(int i, int level)
        {
            bool full = true;
            for(int j = 0; j < columns; j++)
            {
                if(block_Controls[j,i].Background == noBrush)
                {
                    full = false;
                }
            }
            if (full)
            {
                RemoveRow(i);
                score += (100 * level);
                lines_filled += 1;
                CheckCertainRow(i, level + 1);
            }
        }

        private void RemoveRow(int row)
        {
            for(int i = row; i > 2; i--)
            {
                for(int j = 0; j < columns; j++)
                {
                    block_Controls[j, i].Background = block_Controls[j, i - 1].Background;
                }
            }
        }

        public void MoveCurrentGamepieceLeft()
        {
            Point position = current_Gamepiece.Current_Postion;
            Point[] shape = current_Gamepiece.Current_Shape;
            bool move = true;
            EraseCurrentGamepiece();
            int actual_x;
            int actual_y;
            foreach ( Point punkt in shape)
            {
                actual_x = (int)(punkt.X + position.X) + ((columns/2)-1);
                actual_y = (int)(punkt.Y + position.Y) + 2;
                if((actual_x)-1< 0)
                {
                    move = false;
                }else if (block_Controls[actual_x-1,actual_y].Background != noBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                current_Gamepiece.MoveLeft();
            }
            DrawCurrentGamepiece();
        }
        public void MoveCurrentGamepieceRight()
        {
            Point position = current_Gamepiece.Current_Postion;
            Point[] shape = current_Gamepiece.Current_Shape;
            bool move = true;
            EraseCurrentGamepiece();
            int actual_x;
            int actual_y;
            foreach ( Point punkt in shape)
            {
                actual_x = (int)(punkt.X + position.X) + ((columns/2)-1);
                actual_y = (int)(punkt.Y + position.Y) + 2;
                if((actual_x)+1>= columns)
                {
                    move = false;
                }else if (block_Controls[actual_x+1,actual_y].Background != noBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                current_Gamepiece.MoveRight();
            }
            DrawCurrentGamepiece();
        }
        public void MoveCurrentGamepieceDown()
        {
            Point position = current_Gamepiece.Current_Postion;
            Point[] shape = current_Gamepiece.Current_Shape;
            bool move = true;
            EraseCurrentGamepiece();
            int actual_x;
            int actual_y;
            foreach ( Point punkt in shape)
            {
                actual_x = (int)(punkt.X + position.X) + ((columns/2)-1);
                actual_y = (int)(punkt.Y + position.Y) + 2;
                if((actual_y)+1 >= rows)
                {
                    move = false;
                }else if (block_Controls[actual_x,actual_y+1].Background != noBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                current_Gamepiece.MoveDown();
            }
            DrawCurrentGamepiece();
            if (!move)
            {
                CheckRows();
                current_Gamepiece = new Gamepiece();
                position = current_Gamepiece.Current_Postion;
                shape = current_Gamepiece.Current_Shape;
                foreach( Point punkt in shape)
                {
                    actual_x = (int)(punkt.X + position.X) + ((columns/2)-1);
                    actual_y = (int)(punkt.Y + position.Y) + 2;
                    if(block_Controls[actual_x,actual_y].Background != noBrush)
                    {
                        game_over = true;
                    }
                }
            
            }
        }
        public void RotateCurrentGamepiece()
        {
            Point position = current_Gamepiece.Current_Postion;
            Point[] shape = current_Gamepiece.Current_Shape;
            bool move = true;
            EraseCurrentGamepiece();
            current_Gamepiece.Rotate();
            int actual_x;
            int actual_y;
            foreach(Point punkt in shape)
            {
                actual_x = (int)(punkt.X + position.X) + ((columns/2)-1);
                actual_y = (int)(punkt.Y + position.Y) + 2;
                if(actual_x < 0 || actual_x >= columns || actual_y + 2  >= rows)
                {
                    move = false;
                }
                else if(block_Controls[actual_x,actual_y].Background != noBrush)
                {
                    move = false;
                }
            }
            if (!move)
            {
                current_Gamepiece.Rotate();
                current_Gamepiece.Rotate();
                current_Gamepiece.Rotate();
            }
            DrawCurrentGamepiece();
        }
    }
    public class Gamepiece
    {
        //Field and Property for Position
        private Point current_Position;
        public Point Current_Postion
        {
            get { return current_Position; }
        }
        
        //Field and Property for Shape
        private Point[] current_Shape;
        public Point[] Current_Shape
        {
            get { return current_Shape; }
        }

        //Field and Property for Colour
        private Brush current_Colour;
        public Brush Current_Colour
        {
            get { return current_Colour; }
        }

        // if piece is Würfel it should not rotate
        private bool rotate;

        //initializing a new Gamepiece
        public Gamepiece()
        {
            //if needed rotate will be set false in SetRandomnShape
            rotate = true;
            current_Position = new Point(0, 0);
            current_Colour = Brushes.Transparent;
            current_Shape = SetRandomnShape();
        }

        //Methods to move Gamepieces
        public void MoveLeft()
        {
            current_Position.X -= 1;
        }
        public void MoveRight()
        {
            current_Position.X += 1;
        }
        public void MoveDown()
        {
            current_Position.Y += 1;
        }
        public void Rotate()
        {
            if (rotate)
            {
                for (int i = 0; i < current_Shape.Length; i++)
                {
                    double x = current_Shape[i].X;
                    current_Shape[i].X = current_Shape[i].Y * -1;
                    current_Shape[i].Y = x;
                }
            }
        }
        //Set Shape for new Gamepiece and assign the correct Colour
        private Point[] SetRandomnShape()
        {
            Random rand = new Random();
            switch (rand.Next() % 7)
            {
                //i-Stein
                case 0:
                    current_Colour = Brushes.Cyan;
                    return new Point[]
                    {
                        new Point(-1,0),
                        new Point(0,0),
                        new Point(1,0),
                        new Point(2,0)
                    };
                //j-Stein
                case 1:
                    current_Colour = Brushes.Blue;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(1,0),
                        new Point(2,0),
                        new Point(2,-1)
                    };
                //l-stein
                case 2:
                    current_Colour = Brushes.Orange;
                    return new Point[]
                   {
                        new Point(0,0),
                        new Point(1,0),
                        new Point(2,0),
                        new Point(2,1)
                   };
                //Würfel
                case 3:
                    current_Colour = Brushes.Yellow;
                    rotate = false;
                    return new Point[]
                    {
                        new Point(-1,-1),
                        new Point(-1,0),
                        new Point(0,-1),
                        new Point(0,0)
                    };
                //s-Stein
                case 4:
                    current_Colour = Brushes.Green;
                    return new Point[]
                    {
                        new Point(-1,1),
                        new Point(-1,0),
                        new Point(0,0),
                        new Point(0,-1)
                    };
                //t-Stein
                case 5:
                    current_Colour = Brushes.Purple;
                    return new Point[]
                    {
                        new Point(-1,0),
                        new Point(0,0),
                        new Point(1,0),
                        new Point(0,1)
                    };
                //Z-Stein
                case 6:
                    current_Colour = Brushes.Red;
                    return new Point[]
                    {
                        new Point(-1,-1),
                        new Point(-1,0),
                        new Point(0,0),
                        new Point(0,1)
                    };  
                default:
                    return null;
            }
        }
    }
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        Board MyBoard;
        public MainWindow()
        {
            InitializeComponent();
        }
        void MainWindow_Initilized(object sender, EventArgs e)
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Game_Tick);
            Timer.Interval = new TimeSpan(0,0,0,0,400);
            GameStart();
        }
        private void GameStart()
        {
            MainGrid.Children.Clear();
            MyBoard = new Board(MainGrid);
            Timer.Start();
        }
        void Game_Tick(object sender, EventArgs e)
        {
            Scores.Content = MyBoard.Score.ToString("000000000000");
            Lines.Content = MyBoard.Lines_filled.ToString("000000000000");
            MyBoard.MoveCurrentGamepieceDown();
            if (MyBoard.Game_Over)
            {
                Timer.Stop();
            }
        }
        private void GamePause()
        {
            if (Timer.IsEnabled)
            {
                Timer.Stop();
            }
            else
            {
                Timer.Start();
            }
            
        }
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (Timer.IsEnabled)
                    {
                        MyBoard.MoveCurrentGamepieceLeft();
                    }
                    break;
                case Key.Right:
                    if (Timer.IsEnabled)
                    {
                        MyBoard.MoveCurrentGamepieceRight();
                    }
                    break;
                case Key.Down:
                    if (Timer.IsEnabled)
                    {
                        MyBoard.MoveCurrentGamepieceDown();
                    }
                    break;
                case Key.Up:
                    if (Timer.IsEnabled)
                    {
                        MyBoard.RotateCurrentGamepiece();
                    }
                    break;
                case Key.R:
                    GameStart();
                    break;
                case Key.P:
                    GamePause();
                    break;
                default:
                    break;
            }
        }
    }
}
