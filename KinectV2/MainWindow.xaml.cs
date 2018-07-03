using System;
using System.Windows;
using System.Windows.Controls;


using System.Windows.Threading;     //timer
// 追加

using System.IO.Ports;


//Audio
//using NAudio.Wave;

namespace KinectV2
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {


        private DispatcherTimer dt, Notification_dt;



        public MainWindow()
        {

            this.DataContext = this;
            InitializeComponent();


            #region FFTの関係の初期化
            /*
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var deviceInfo = WaveIn.GetCapabilities(i);
                Console.WriteLine(String.Format("Audio Device {0}: {1}, {2} channels",
                    i, deviceInfo.ProductName, deviceInfo.Channels));
            }


            WaveIn waveIn = new WaveIn()
            {
                //ここでマイクの選択
                DeviceNumber = 0, // Default
            };
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.WaveFormat = new WaveFormat(sampleRate: fs, channels: 1);
            waveIn.StartRecording();
            */
            #endregion


            //シリアルポート関係
            setSerialComboBox();
            // 改行コードまでUIに反映されないよう、システムの改行コードを設定しておく
            serialPort.NewLine = Environment.NewLine;
            //serialPort.ReadTimeout = 500;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);


            // タイマーの生成
            dt = new DispatcherTimer(DispatcherPriority.Normal);
            dt.Interval = TimeSpan.FromSeconds(float.Parse(textBox.Text));
            dt.Tick += dt_Tick;
            dt.Start();
            // タイマー2の生成
            dt2 = new DispatcherTimer(DispatcherPriority.Normal);
            dt2.Interval = TimeSpan.FromSeconds(0.05);
            dt2.Tick += dt_Tick2;
            dt2.Start();
            // 通知タイマーの生成
            Notification_dt = new DispatcherTimer(DispatcherPriority.Normal);
            Notification_dt.Interval = TimeSpan.FromMinutes(15);
            Notification_dt.Tick += Notification_dt_Tick;
            Notification_dt.Start();




        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //1回位目のデータの項目などを書く
            FirstDataLogging();

        }



        private void button_ClickRevise(object sender, RoutedEventArgs e)
        {
            //textBox.Text.
            dt.Interval = TimeSpan.FromSeconds(float.Parse(textBox.Text));
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {


            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("SerialPort is Closed");
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }




    }
}
