using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CSCD371FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
   
    public partial class PlayWindow : Window
    { 
        private SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        private int ScrollRate { get; set; }
        private int hiScore { get; set; }
        private double Speed { get; set; }
        private TetrisGrid tg;
        private double currentinterval = 0.5;
        private int hiscore;
        private int currentLevel;
        DispatcherTimer timer = new DispatcherTimer();

        public PlayWindow()
        {      
            InitializeComponent();
            tg = new TetrisGrid(10, 18);
            UpdateCanvas();
            timer.Interval = TimeSpan.FromSeconds(currentinterval);
            timer.Tick += Update;
            this.currentLevel = 1;
            GetHighScore();
        }
        public void GetHighScore()
        {
            try
            {
                if (!File.Exists("hiscore.txt"))
                {
                    File.Create("hiscore.txt");
                    using (StreamWriter sw = new StreamWriter("hiscore.txt"))
                    {
                        sw.WriteLine("0");
                        sw.Close();
                    }
                }
                using (StreamReader sr = new StreamReader("hiscore.txt"))
                {
                    String s = sr.ReadLine();
                    this.hiscore = int.Parse(s);
                }
            }
            catch(Exception e)
            {
                this.hiscore = 0;
            }
            this.hiscorebox.Text = "High Score: " + this.hiscore;
        }
        public void SetHighScore(int newhighscore)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("hiscore.txt"))
                {
                    sw.WriteLine("" + newhighscore);
                    sw.Close();
                }
            }
            catch(Exception e)
            {
                return;
            }
            this.hiscorebox.Text = "High Score: " + this.tg.CurrentScore;
        }
        public PlayWindow(string text) : this()
        {
            LabelName.Content = text + "!";
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Left:
                    this.tg.ShiftLeft(); 
                    this.UpdateCanvas();
                    break;
                case Key.Right:
                    this.tg.ShiftRight();
                    this.UpdateCanvas();
                    break;
                case Key.LeftCtrl:
                    this.tg.Rotate(); 
                    this.UpdateCanvas();
                    break;
                case Key.Enter:
                    this.tg.ShiftShapeAllDown();
                    this.UpdateCanvas();
                    break;
                default: break;
            }
            
        }
        private Color IntToColor(int color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }
        private void UpdateCanvas()
        {
            int[][] grid = this.tg.GetGrid();
            this.tg.RefreshGrid();
            CanvasGrid.Children.Clear();
            double gWidth = CanvasGrid.Width / 10;
            double gHeight = CanvasGrid.Height / 18;
            int norows = grid.Length;
            int nocols = grid[0].Length;
            for (int y = 0; y < norows; y++)
            {
                for (int x = 0; x < nocols; x++)
                {
                    double left = (x * gWidth);
                    double top = (y * gHeight);
                    Rectangle current = new Rectangle();               
                    if (grid[y][x] != 0)
                    {
                        switch(grid[y][x])
                        {
                            case 1:
                                mySolidColorBrush = new SolidColorBrush(Colors.Yellow);   
                                break;
                            case 2:
                                mySolidColorBrush = new SolidColorBrush(Colors.Orange);

                                break;
                            case 3:
                                mySolidColorBrush = new SolidColorBrush(Colors.Purple);

                                break;
                            case 4:
                                mySolidColorBrush = new SolidColorBrush(Colors.Red);

                                break;
                            case 5:
                                mySolidColorBrush = new SolidColorBrush(Colors.White);

                                break;
                            case 6:
                                mySolidColorBrush = new SolidColorBrush(Colors.RoyalBlue);

                                break;
                            case 7:
                                mySolidColorBrush = new SolidColorBrush(Colors.Green);

                                break;
                            default:
                                break;
                        }                        
                    }
                    else
                    mySolidColorBrush = new SolidColorBrush(Colors.DarkGray);
                    current.Stroke = new SolidColorBrush(Colors.Black);
                    current.Fill = mySolidColorBrush;
                    current.Width = gWidth;
                    current.Height = gHeight;
                    current.SetValue(Canvas.LeftProperty, left);
                    current.SetValue(Canvas.TopProperty, top);
                    this.CanvasGrid.Children.Add(current);   
                }
            }
        }
        private void Shiftbutton_Click(object sender, RoutedEventArgs e)
        {
            this.tg.ShiftShapeAllDown();
        }
       
        private void Rotatebutton_Click(object sender, RoutedEventArgs e)
        {
            this.tg.Rotate();
            this.UpdateCanvas();
        }

        private void Shift_Right_Click(object sender, RoutedEventArgs e)
        {
            this.tg.ShiftRight();
            this.UpdateCanvas();
        }
        private void Shift_Left_Click(object sender, RoutedEventArgs e)
        {
            this.tg.ShiftLeft();
            this.UpdateCanvas();
        }

        private void Button_Menu(object sender, RoutedEventArgs e)
        {
           
            MainWindow mWind = new MainWindow();
            this.timer.Stop();
            mWind.Show();
            this.Close();
        }

        private void Button_Play(object sender, RoutedEventArgs e)
        {           
            timer.Start();
            this.pauseButt.IsEnabled = true;
            this.playButt.IsEnabled = false;
            this.Save.IsEnabled = false;
            this.Save.Foreground = new SolidColorBrush(Colors.DarkGray);
            this.playButt.Foreground = new SolidColorBrush(Colors.DarkGray);
            this.pauseButt.Foreground = new SolidColorBrush(Colors.Red);
        }
        public void Update(object sender, EventArgs e)
        {
            if (this.tg.youlost())
            {
                if(this.tg.CurrentScore > this.hiscore)
                {
                    this.SetHighScore(this.tg.CurrentScore);
                }
                this.timer.Stop();
                this.tg = new TetrisGrid(10, 18);
                this.UpdateCanvas();
                this.scorebox.Text = "Current Score: 0";
                this.playButt.IsEnabled = true;
                this.pauseButt.IsEnabled = false;
                this.Save.IsEnabled = false;
                this.Save.Foreground = new SolidColorBrush(Colors.DarkGray);
                this.playButt.Foreground = new SolidColorBrush(Colors.Red);
                this.pauseButt.Foreground = new SolidColorBrush(Colors.DarkGray);
                this.msgbox.Text = "You lost!";
                return;
            }
            this.tg.UpdateGrid();
            this.UpdateCanvas();

            Shape next = this.tg.GetNextShape();
            if(next is LineShape)
            {
                this.nextUp.Source = new BitmapImage(new Uri("line.jpg", UriKind.RelativeOrAbsolute));
            }
            if (next is Square)
            {
                this.nextUp.Source = new BitmapImage(new Uri("square.jpg", UriKind.RelativeOrAbsolute));
            }
            if (next is LeftHook)
            {
                this.nextUp.Source = new BitmapImage(new Uri("righthook.png", UriKind.RelativeOrAbsolute));
            }
            if (next is RightHook)
            {
                this.nextUp.Source = new BitmapImage(new Uri("righthook.png", UriKind.RelativeOrAbsolute));
            }
            if (next is LeftZed)
            {
                this.nextUp.Source = new BitmapImage(new Uri("sideS.png", UriKind.RelativeOrAbsolute));
            }
            if (next is RightZed)
            {
                this.nextUp.Source = new BitmapImage(new Uri("sideS.png", UriKind.RelativeOrAbsolute));
            }
            if (next is TeeShape)
            {
                this.nextUp.Source = new BitmapImage(new Uri("pyramid.png", UriKind.RelativeOrAbsolute));
            }

            this.scorebox.Text = "Current Score: " + this.tg.CurrentScore;
            if (this.tg.CurrentScore >= 10 && this.tg.CurrentScore % 10 == 0)
            {
                this.tg.CurrentScore += 5;
                this.currentinterval *= 0.75;
                this.currentLevel++;
                timer.Interval = TimeSpan.FromSeconds(currentinterval);
            }
            this.msgbox.Text = "Level " + this.currentLevel;
        }
        private void Button_Pause(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.playButt.IsEnabled = true;
            this.pauseButt.IsEnabled = false;
           // this.Save.IsEnabled = true;
           // this.Save.Foreground = new SolidColorBrush(Colors.Red);
            this.playButt.Foreground = new SolidColorBrush(Colors.Red);
            this.pauseButt.Foreground = new SolidColorBrush(Colors.DarkGray);      
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
