using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Linq;

namespace Lab_2_First_App
{
    public partial class MainWindow : Window
    {
        static bool b = true;
        static DispatcherTimer dT;
        static int Radius = 20;
        static int PointCount = 5;
        static Polygon myPolygon = new Polygon();
        static List<Ellipse> EllipseArray = new List <Ellipse>();
        static PointCollection pC = new PointCollection();

        public MainWindow()
        {
            dT = new DispatcherTimer();

            InitializeComponent();
            InitPoints();
            InitPolygon();

            dT = new DispatcherTimer();
            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, 1000);            
        }

        private void InitPoints()
        {
            Random rnd = new Random();
            pC.Clear();
            EllipseArray.Clear();

            for (int i = 0; i < PointCount; i++)
            {
                Point p = new Point();

                p.X = rnd.Next(Radius, (int)(0.75*MainWin.Width)-3*Radius);
                p.Y = rnd.Next(Radius, (int)(0.90*MainWin.Height-3*Radius));                
                pC.Add(p);
            }

            for (int i = 0; i < PointCount; i++)
            { 
                Ellipse el = new Ellipse();

                el.StrokeThickness = 1;
                el.Height = el.Width = Radius;
                el.Stroke = Brushes.DarkBlue;
                if (i == 0)
                    el.Fill = Brushes.Red;
                else 
                    el.Fill = Brushes.LightBlue;

                EllipseArray.Add(el); 
            }            
        }

        private void InitPolygon()
        {
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;            
            myPolygon.StrokeThickness = 1;            
        }

        private void PlotPoints()
        {            
            for (int i=0; i<PointCount; i++)
            {
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius/2);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius/2);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }


        private void PlotWay(int [] BestWayIndex)
        {
            PointCollection Points = new PointCollection();

            for (int i = 0; i < BestWayIndex.Length; i++)
                Points.Add(pC[BestWayIndex[i]]);

            myPolygon.Points = Points;
            MyCanvas.Children.Add(myPolygon);
        }

        private void VelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
                        
            dT.Interval = new TimeSpan(0,0,0,0,Convert.ToInt16(item.Content));
        }

        private void StopStart_Click(object sender, RoutedEventArgs e)
        {
            if (dT.IsEnabled)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
                Alg_Type.IsEnabled = true;
            }
            else
            {                
                NumElemCB.IsEnabled = false;
                Alg_Type.IsEnabled = false;
                dT.Start();
            }
        }

        private void NumElemCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
            m = true;

            PointCount = Convert.ToInt32(item.Content);
            InitPoints();
            InitPolygon();
        }

        private void Alg_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            b = Alg_Type.SelectedIndex == 0;
            m = true;
        }

        private void OneStep(object sender, EventArgs e)
        {
            MyCanvas.Children.Clear();
            //InitPoints();
            PlotPoints();
            if (b)
                PlotWay(Zhad());
            else
                PlotWay(Genetic());
        }

        static bool m = true;
        static PointCollection[] roads = new PointCollection[6];
        private int[] Genetic()
        {
            Random rnd = new Random();
            int[] way = new int[PointCount];
            PointCollection[] temp = new PointCollection[3];
            List<PointCollection> cloneTemp = new List<PointCollection>();
            int cross = rnd.Next(1, PointCount - 2);

            if (m)
            {
                for (int i = 0; i < 6; i++)
                {
                    int k = PointCount;
                    PointCollection copy = pC.Clone();
                    roads[i] = new PointCollection();
                    for (int j = 0; j < PointCount; j++)
                    {
                        Point p = new Point();
                        p = copy[rnd.Next(0, k)];
                        copy.Remove(p);
                        roads[i].Add(p);
                        k--;
                    }
                }
                m = false;
            }

            for (int i = 0; i < 3; i++)
                temp[i] = new PointCollection();

            for (int i = 0; i < 6; i+=2)
            {
                for (int j = 0; j < PointCount * 2; j++)
                {
                    if (j <= cross)
                        temp[i / 2].Add(roads[i][j]);
                    else if (j > cross && j < PointCount)
                    {
                        if (!temp[i / 2].Contains(roads[i + 1][j]))
                            temp[i / 2].Add(roads[i + 1][j]);
                    }
                    else
                    {
                        if (j == PointCount)
                            j += cross + 1;
                        if (!temp[i / 2].Contains(roads[i][j - PointCount]))
                            temp[i / 2].Add(roads[i][j - PointCount]);
                    }
                }
            }


            for (int i = 0; i < 3; i++)
            {
                if (rnd.Next(10) > 1)
                {
                    int obr = rnd.Next(1, PointCount);
                    int probr = rnd.Next(obr);
                    Point prev = new Point(0, 0), next;
                    for (int j = probr; j <= obr; j++)
                    {
                        if (j == probr)
                        {
                            prev = temp[i][probr];
                            temp[i][j] = temp[i][obr];
                        }
                        else
                        {
                            next = temp[i][j];
                            temp[i][j] = prev;
                            prev = next;
                        }
                    }
                }
                else if (rnd.Next(10) > 1)
                {
                    int obr = rnd.Next(1, PointCount);
                    int probr = rnd.Next(obr);
                    PointCollection tem = new PointCollection();
                    for (int j = probr; j <= obr; j++)
                        tem.Add(temp[i][j]);

                    for (int j = probr; j <= obr; j++)
                    {
                        int t = rnd.Next(tem.Count());
                        temp[i][j] = tem[t];
                        tem.RemoveAt(t);
                    }
                }
            }

            (double, int)[] distances = new (double, int)[9];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < PointCount; j++)
                {
                    distances[i].Item2 = i;
                    if (j == PointCount - 1)
                        distances[i].Item1 += Math.Sqrt(Math.Pow(roads[i][j].X - roads[i][0].X, 2) + Math.Pow(roads[i][j].Y - roads[i][0].Y, 2));
                    else
                        distances[i].Item1 += Math.Sqrt(Math.Pow(roads[i][j].X - roads[i][j + 1].X, 2) + Math.Pow(roads[i][j].Y - roads[i][j + 1].Y, 2));
                    if (i < 3)
                    {
                        distances[i + 6].Item2 = i + 6;
                        if (j == PointCount - 1)
                            distances[i + 6].Item1 += Math.Sqrt(Math.Pow(temp[i][j].X - temp[i][0].X, 2) + Math.Pow(temp[i][j].Y - temp[i][0].Y, 2));
                        else
                            distances[i + 6].Item1 += Math.Sqrt(Math.Pow(temp[i][j].X - temp[i][j + 1].X, 2) + Math.Pow(temp[i][j].Y - temp[i][j + 1].Y, 2));
                    }
                }
            }

            distances = distances.OrderBy(x => x.Item1).ToArray();

            if (distances[1].Item2 < 6)
                for (int j = 0; j < PointCount; j++)
                    way[j] = pC.IndexOf(roads[distances[1].Item2][j]);
            else
                for (int j = 0; j < PointCount; j++)
                    way[j] = pC.IndexOf(temp[distances[1].Item2 - 6][j]);

            for (int i = 0; i < 6; i++)
                if (distances[i].Item2 >= 6)
                    cloneTemp.Add(temp[distances[i].Item2 - 6]);

            int n = 0;
            for (int i = 6; i < 9; i++)
            {  
                if (distances[i].Item2 < 6)
                {
                    roads[distances[i].Item2].Clear();
                    roads[distances[i].Item2] = cloneTemp[n].Clone();
                    n++;
                }
            }

            foreach (PointCollection item in temp)
                item.Clear();
            cloneTemp.Clear();

            return way;
        }
        private int[] Zhad()
        {
            int counter = 0;
            double min = 0;
            Point t = new Point(0, 0); 
            int[] way = new int[PointCount];
            List<(Point, int)> temp = new List<(Point, int)>();

            way[0] = 0;
            foreach (Point point in pC)
            {
                if (pC.IndexOf(point) == 0)
                    t = point;
                temp.Add((point, pC.IndexOf(point)));
            }

            for (int i = 1; i < PointCount; i++)
            {
                int c = 0;
                foreach (var point in temp)
                {
                    if (point.Item1 != t)
                    {
                        c++;
                        if (c == 1)
                        {
                            min = Math.Sqrt(Math.Pow(t.X - point.Item1.X, 2) + Math.Pow(t.Y - point.Item1.Y, 2));
                            counter = point.Item2;
                        }
                        else
                        {
                            double f = min;
                            double k = Math.Sqrt(Math.Pow(t.X - point.Item1.X, 2) + Math.Pow(t.Y - point.Item1.Y, 2));
                            min = Math.Min(min, k);
                            if (min != f)
                                counter = point.Item2;
                        }
                    }
                }
                foreach (var point in temp)
                    if (point.Item2 == way[i-1])
                    {
                        temp.Remove(point);
                        break;
                    }

                way[i] = counter;
                foreach (var point in temp)
                    if (point.Item2 == way[i])
                        t = point.Item1;
            }

            return way;
        }
    }
}