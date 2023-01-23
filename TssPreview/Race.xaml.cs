using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.Windows.Shapes.Path;

namespace TssPreview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Race : Window
    {
        public Game GameState;

        float GridSize = 8;

        bool IsRaceOpened = false;

        readonly DispatcherTimer timer;

        private DateTime oldTime;

        bool isPlay = false;

        float BoatSize
        {
            get
            {
                return GridSize * (float)BoatScaleSlider.Value;
            }
        }

        public Race(string path)
        {
            InitializeComponent();

            Load(path);
            UpdateTitle(path);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(100);
            timer.Tick += Timer_Tick;

            BoatScaleSlider.ValueChanged += BoatScaleSlider_ValueChanged;
        }

        private void UpdateTitle(string path)
        {
            Title = string.Format("\"{1}\" TSS Preview {0}", App.Version, System.IO.Path.GetFileName(path));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (slider.Value < slider.Maximum)
            {
                slider.Value += 0.0000005 * (DateTime.Now.Ticks - oldTime.Ticks);
            }
            else
            {
                isPlay = false;
                UpdateTimerData();
                timer.Stop();
            }

            oldTime = DateTime.Now;
        }

        private void Resize()
        {
            if (IsRaceOpened)
            {
                if (GameContainer.ActualHeight / GameState.Height < GameContainer.ActualWidth / GameState.Width)
                {
                    GridSize = (float)GameContainer.ActualHeight / GameState.Height;
                }
                else
                {
                    GridSize = (float)GameContainer.ActualWidth/ GameState.Width;
                }
                canvas.Width = GridSize * GameState.Width;
                canvas.Height = GridSize * GameState.Height;
            }
        }

        public void Load(string path)
        {
            try
            {
                GameState = Game.Load(path);
                IsRaceOpened = true;
                slider.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateGame()
        {
            canvas.Children.Clear();

            if (IsRaceOpened)
            {
                foreach (Mark mark in GameState.Marks)
                {
                    canvas.Children.Add(DrawMark(mark));
                }
                for (int i = 0; i < GameState.Boats.Length; i++)
                {
                    Boat boat = GameState.Boats[i];
                    canvas.Children.Add(DrawTrack(boat));
                    canvas.Children.Add(DrawBoat(boat, (float)slider.Value));
                }
            }
        }

        public void UpdateWind()
        {
            wind.Children.Clear();
            if (IsRaceOpened)
            {
                wind.Clip = new RectangleGeometry(new Rect(-1, -1, wind.ActualWidth + 2, wind.ActualHeight + 0.5));

                int len = GameState.TurnCount + 5;
                string data = string.Format("M {2} {1} L {0}, {1}", wind.ActualWidth, wind.ActualHeight + 50, wind.ActualWidth / 2);
                double gridY = wind.ActualHeight / len;
                Path newElem = new Path();
                double width = wind.ActualWidth;
                double scaleX = width / 200;
                wind.Children.Add(newElem);
                for (int i = 0; i < len; i++)
                {
                    data += string.Format("L {0}, {1} ", (GameState.Wind[i % GameState.Wind.Length] * 5 + 100) * scaleX, wind.ActualHeight - i * gridY);
                    data += string.Format("L {0}, {1} ", (GameState.Wind[(i + 1) % GameState.Wind.Length] * 5 + 100) * scaleX, wind.ActualHeight - i * gridY);
                    if (i == (int)slider.Value + 1)
                    {
                        Rectangle newRect = new Rectangle();
                        newRect.Width = wind.ActualWidth;
                        newRect.Height = gridY;
                        newRect.Margin = new Thickness(0, wind.ActualHeight - i * gridY, 0, 0);
                        newRect.Fill = new SolidColorBrush(Color.FromArgb(64, 0, 0, 255));
                        newRect.RadiusY = 2;
                        newRect.RadiusX = 2;
                        wind.Children.Add(newRect);
                    }
                }
                data += string.Format("L {0} 0 L {1} 0", (GameState.Wind[(len) % GameState.Wind.Length] * 5 + 100) * scaleX,
                                      wind.ActualWidth / 2);
                data += "Z";
                newElem.Data = Geometry.Parse(data);
                newElem.Stroke = Brushes.Black;
                newElem.Fill = new SolidColorBrush(Color.FromArgb(64, 255, 0, 0));

                windRotate.Angle = GameState.Wind[((int)slider.Value + 1) % GameState.Wind.Length] * 2;
            }
        }

        public void Update()
        {
            if (IsRaceOpened)
            {
                slider.Maximum = GameState.TurnCount - 1;
                windRotate.Angle = 0;

                UpdatePlayers();
                Resize();
                UpdateGame();
                UpdateWind();
            }
        }
        
        private void UpdatePlayers()
        {
            players.ItemsSource = GameState.Boats;
        }

        private UIElement DrawBoat(Boat boat, float turnNow)
        {
            float size = BoatSize;
            Ellipse rv = new Ellipse();
            rv.Width = size;
            rv.Height = size;
            rv.Fill = new SolidColorBrush(boat.GetColor());
            Point pt1 = boat.Turns[(int)turnNow].Points[0];

            Point pt2 = pt1;
            if (turnNow < boat.Turns.Length - 1)
            {
                pt2 = boat.Turns[(int)turnNow + 1].Points[0];
            }

            Point pt = new Point(pt1.X + (pt2.X - pt1.X) * (turnNow - (int)turnNow), pt1.Y + (pt2.Y - pt1.Y) * (turnNow - (int)turnNow));

            rv.Margin = new Thickness(pt.X * GridSize - size / 2, pt.Y * GridSize - size / 2, 0, 0);
            return rv;
        }

        public UIElement DrawMark(Mark mark)
        {
            Ellipse rv = new Ellipse();
            rv.Width = GridSize;
            rv.Height = GridSize;
            rv.Fill = Brushes.Red;
            rv.Margin = new Thickness(mark.X * GridSize - GridSize / 2, mark.Y * GridSize - GridSize / 2, 0, 0);
            return rv;
        }

        public UIElement DrawTrack(Boat boat)
        {
            Path rv = new Path();
            string data = "M ";
            data += boat.Turns[0].Points[0].X * GridSize + "," + boat.Turns[0].Points[0].Y * GridSize + " ";

            foreach (Story turn in boat.Turns)
            {
                foreach (Point pt in turn.Points)
                {
                    data += "L " + pt.X * GridSize + " " + pt.Y * GridSize + " ";
                }
            }

            rv.Data = Geometry.Parse(data);
            rv.Stroke = new SolidColorBrush(boat.GetColor());

            return rv;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Update();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Game";
            dialog.DefaultExt = ".tss";
            dialog.Filter = "Image files (*.tss, *.tssrace)|*.tss;*.tssrace";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                Load(dialog.FileName);
                Update();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Update();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            isPlay = !isPlay;

            if (isPlay)
            {
                timer.Start();
                oldTime = DateTime.Now;
            }
            else
            {
                slider.Value = Math.Floor(slider.Value + 1);
                timer.Stop();
            }

            UpdateTimerData();
        }

        private void UpdateTimerData()
        {
            playIcon.Visibility = (!isPlay) ? Visibility.Visible : Visibility.Collapsed;
            pauseIcon.Visibility = (isPlay) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            slider.Value = Math.Round(slider.Value);
            Update();
        }

        private void BoatScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Update();
        }

        private void OpenTSSWeb(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://tss.boats",
                UseShellExecute = true
            });
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            else if (e.Key == Key.Left) 
            {
                slider.Value = (int)slider.Value - 1;
            }
            else if (e.Key == Key.Right) 
            {
                slider.Value = (int)slider.Value + 1;
            }
        }
    }
}
