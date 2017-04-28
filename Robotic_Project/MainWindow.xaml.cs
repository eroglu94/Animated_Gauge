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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using System.IO.Ports;
using System.Threading;

namespace Robotic_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        public void metroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LogMessage("Program Started!");
            StartTimer();
            Animations temp_Ah = new Animations();
            temp_Ah.Initialize();
            ah = temp_Ah;
        }

        DispatcherTimer Timer = new DispatcherTimer();
        public void StartTimer()
        {
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            Timer.Start();
        }
        public async void Timer_Tick(object sender, EventArgs e)
        {
            await doUIRefresh();
        }

        Animations ah = new Animations();

        public async Task doUIRefresh()
        {
            //Refresh Gauge 1
            var angle = CurrentRotationAngle(needle);
            double num = DegreeToDoubleValue(angle);
            tbxClockValue_1.Text = num.ToString("0.0");
            if (Convert.ToDouble(tbxClockValue_1.Text) >= 10)
            {
                tbxClockValue_1.Text = "9.9";
            }
            ah.doGaugeAnimation(num, 1);


            //Refresh Gauge 2
            angle = CurrentRotationAngle(needle_2);
            num = DegreeToDoubleValue(angle);
            tbxClockValue_2.Text = num.ToString("0.0");
            if (Convert.ToDouble(tbxClockValue_2.Text) >= 10)
            {
                tbxClockValue_2.Text = "9.9";
            }
            ah.doGaugeAnimation(num, 2);


        }

        public double CurrentRotationAngle(UIElement deneme)
        {
            double angle = 0.0;

            Transform transform = deneme.RenderTransform;

            if (transform is TransformGroup)
            {
                TransformGroup tg = (TransformGroup)deneme.RenderTransform;
                foreach (Transform t in tg.Children)
                    if (t is RotateTransform)
                    {
                        RotateTransform r = (RotateTransform)t;
                        angle += r.Angle;
                    }
            }

            if (transform is RotateTransform)
            {
                angle = ((RotateTransform)transform).Angle;
            }

            return angle;

        }

        public double DegreeToDoubleValue(double Angle)
        {
            if (Angle == 360)
            {
                return 0;
            }
            else
            {
                return Angle / 36;
            }

        }

        public double DoubleValueToDegree(double Value)
        {
            if (Value == 0)
            {
                return 360;
            }
            else
            {
                return Value * 36;
            }

        }

        public void button_Click(object sender, RoutedEventArgs e)
        {
            if (tbxGauge1.Text != "")
            {
                if (Convert.ToDouble(tbxGauge1.Text) >= 0 && Convert.ToDouble(tbxGauge1.Text) <= 9.999)
                {
                    var value = Convert.ToDouble(tbxGauge1.Text);
                    ah.RotateNeedle(value, needle);
                    LogMessage("New Manual Value: " + value);
                    tbxGauge1.Text = "";
                }
                else
                {
                    LogMessage("Enter Only '0' to '9'");
                }

            }
            else
            {
                ah.RandomNeedleMovement(needle);
            }

        }

        public void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (tbxGauge2.Text != "")
            {
                if (Convert.ToDouble(tbxGauge2.Text) >= 0 && Convert.ToDouble(tbxGauge2.Text) <= 9.999)
                {
                    var value = Convert.ToDouble(tbxGauge2.Text);
                    ah.RotateNeedle(value, needle_2);
                    LogMessage("New Manual Value: " + value);
                    tbxGauge2.Text = "";
                }
                else
                {
                    LogMessage("Enter Only '0' to '9'");
                }
            }
            else
            {
                ah.RandomNeedleMovement(needle_2);
            }
        }

        public void LogMessage(string Message, string type = "Default")
        {
            var currentTime = DateTime.Now;
            var content = "[" + currentTime.ToLongTimeString() + "]" + "  " + Message + "\n";
            richTextBox.AppendText(content);
            richTextBox.ScrollToEnd();
        }

        private void toggleSwitchButton_Checked(object sender, RoutedEventArgs e)
        {
            tbxbluetoothState.Text = "ONLINE";
            tbxbluetoothState.Foreground = Brushes.LimeGreen;
            LogMessage("Bluetooth ONLINE");

        }

        private void toggleSwitchButton_Unchecked(object sender, RoutedEventArgs e)
        {
            tbxbluetoothState.Text = "OFFLINE";
            tbxbluetoothState.Foreground = Brushes.Gray;
            LogMessage("Bluetooth OFFLINE");
        }

        public SerialPort serialPort = new SerialPort();
        int counter = 0;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            while (serialPort.IsOpen)
            {
                // WRITE THE INCOMING BUFFER TO CONSOLE
                while (serialPort.BytesToRead > 0)
                {
                    LogMessage(Convert.ToChar(serialPort.ReadChar()).ToString());
                }
                // SEND
                serialPort.WriteLine("PC counter: " + (counter++));
                Thread.Sleep(500);
            }
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            serialPort.BaudRate = 9600;
            serialPort.PortName = "COM8"; // Set in Windows
            serialPort.Open();

            Arduino_Timer.Tick += new EventHandler(Arduino_Timer_Tick);
            Arduino_Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            Arduino_Timer.Start();

        }

        private async void Arduino_Timer_Tick(object sender, EventArgs e)
        {
            await ArduinoData();
        }

        DispatcherTimer Arduino_Timer = new DispatcherTimer();
        private async Task ArduinoData()
        {
            while (serialPort.IsOpen)
            {
                // WRITE THE INCOMING BUFFER TO CONSOLE
                while (serialPort.BytesToRead > 0)
                {
                    LogMessage(Convert.ToChar(serialPort.ReadChar()).ToString());
                }
                // SEND
                serialPort.WriteLine("PC counter: " + (counter++));

                //await Task.Delay(500);
            }
        }
    }
}

