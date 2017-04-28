using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robotic_Project;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Robotic_Project
{
    class Animations
    {
        //Global Variables
        MainWindow mw = (MainWindow)System.Windows.Application.Current.MainWindow;
        UIElement[] GaugeBar1 = new UIElement[10];
        UIElement[] GaugeBar2 = new UIElement[10];

        public void Initialize()
        {
            //FOR GAUGE 1
            GaugeBar1[0] = mw.GaugeBar_1;
            GaugeBar1[1] = mw.GaugeBar_2;
            GaugeBar1[2] = mw.GaugeBar_3;
            GaugeBar1[3] = mw.GaugeBar_4;
            GaugeBar1[4] = mw.GaugeBar_5;
            GaugeBar1[5] = mw.GaugeBar_6;
            GaugeBar1[6] = mw.GaugeBar_7;
            GaugeBar1[7] = mw.GaugeBar_8;
            GaugeBar1[8] = mw.GaugeBar_9;
            GaugeBar1[9] = mw.GaugeBar_10;


            //FOR GAUGE 2
            GaugeBar2[0] = mw.GaugeBar_11;
            GaugeBar2[1] = mw.GaugeBar_12;
            GaugeBar2[2] = mw.GaugeBar_13;
            GaugeBar2[3] = mw.GaugeBar_14;
            GaugeBar2[4] = mw.GaugeBar_15;
            GaugeBar2[5] = mw.GaugeBar_16;
            GaugeBar2[6] = mw.GaugeBar_17;
            GaugeBar2[7] = mw.GaugeBar_18;
            GaugeBar2[8] = mw.GaugeBar_19;
            GaugeBar2[9] = mw.GaugeBar_20;
        }

        public void doGaugeAnimation(double currentNumber, int selectedGauge)
        {
            var GAUGE = new UIElement[10];
            if (selectedGauge == 1)
            {
                GAUGE = GaugeBar1;
            }
            else
            {
                GAUGE = GaugeBar2;
            }

            int num = Convert.ToInt32(currentNumber);


            if (num == 10)
            {

            }

            if (num > 9.5 || num == 0)
            {
                Storyboard sb = mw.FindResource("SelectedGaugeUP") as Storyboard;
                Storyboard.SetTarget(sb, GAUGE[0]);
                sb.Begin();

                for (int i = 0; i <= 9; i++)
                {
                    if (i != num)
                    {
                        sb = mw.FindResource("SelectedGaugeDOWN") as Storyboard;
                        Storyboard.SetTarget(sb, GAUGE[i]);
                        sb.Begin();
                        //mw.LogMessage(i.ToString() + " down");
                    }
                }
            }
            else
            {
                Storyboard sb = mw.FindResource("SelectedGaugeDOWN") as Storyboard;
                Storyboard.SetTarget(sb, GAUGE[0]);
                sb.Begin();



                sb = mw.FindResource("SelectedGaugeUP") as Storyboard;
                Storyboard.SetTarget(sb, GAUGE[num]);
                sb.Begin();

                for (int i = 0; i <= 9; i++)
                {
                    if (i != num)
                    {
                        sb = mw.FindResource("SelectedGaugeDOWN") as Storyboard;
                        Storyboard.SetTarget(sb, GAUGE[i]);
                        sb.Begin();
                        //mw.LogMessage(i.ToString() + " down");
                    }
                }
            }
        }

        public void RotateNeedle(double newValue, UIElement _Object)
        {
            var currentAngle = mw.CurrentRotationAngle(_Object);


            var dist = Math.Abs(mw.DegreeToDoubleValue(currentAngle) - newValue);
            var time = (int)(dist * 100000 / 750);
            var Animation = new DoubleAnimation(currentAngle, mw.DoubleValueToDegree(newValue), new Duration(new TimeSpan(0, 0, 0, 0, time)));
            var rT = new RotateTransform();
            _Object.RenderTransform = rT;
            //Animation.RepeatBehavior = Timeline.RepeatBehaviorProperty(1);
            rT.BeginAnimation(RotateTransform.AngleProperty, Animation);
        }

        public void RandomNeedleMovement(UIElement NeedleOBJ)
        {
            var randomNewValue = (new Random().NextDouble() * (9 - 0) + 0);
            RotateNeedle(randomNewValue, NeedleOBJ);
            mw.LogMessage("New Random Value: " + randomNewValue.ToString("0.0"));
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }



    }
}
