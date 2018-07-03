using System;
using System.Windows;
// 追加

using System.IO.Ports;
namespace KinectV2
{
    public partial class MainWindow : Window
    {
        SerialPort serialPort = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);

        private void setSerialComboBox()
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                portselectComboBox.Items.Add(portName);
            }

        }


        //double ChairData1 = 0, ChairData2 = 0, ChairData3 = 0;
        double[] ChairData = new double[12];
        int ChairData_cnt = 0;
        //DateTime NowNow, NowNow2;

        private void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
        {
            // NowNow2 = NowNow;

            //// 現在の日付と時刻を取得する
            //NowNow = DateTime.Now;

            //var minu = NowNow - NowNow2;
            //Console.WriteLine("serial time"+minu.ToString());
            System.Threading.Thread.Sleep(10);  //同期させるため
            try
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();

                //Console.WriteLine("indata:"+indata);
                //Console.WriteLine("Data Received:");
                var splitedData = indata.Split('\t');
                //Console.Write(splitedData[0]);
                //splitedDataは3個の予定
                //6個のデータの内容は、xyzの加速度と角加速度の順番
                float[] SerialData = new float[12];
                for (int i = 0; i < splitedData.Length - 1; i++)//splitedData.Length-1のマイナス1は最後の改行をなくすため
                {
                    SerialData[i] = float.Parse(splitedData[i]);
                    ChairData[i] += SerialData[i];
                    //Console.WriteLine(i.ToString()+":"+SerialData[i].ToString());
                }

                
                ChairData_cnt++;

            }
            catch
            {

            }


        }

        private void SerialProtOpenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                string portName = portselectComboBox.SelectedItem.ToString();
                serialPort.PortName = portName;
                serialPort.BaudRate = int.Parse(BaudRateeComboBox.SelectedItem.ToString());
                serialPort.Open();
                Console.WriteLine("SerialPort is Opend");
                ConnectStatus.Content = "Opened";

            }
            catch
            {
                MessageBox.Show(e.Source.ToString());
            }


        }
        private void Sendbutton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {

            }

        }

        private void BoardrateComboBox_DropDownOpened(object sender, EventArgs e)
        {
            // ボーレートを毎回取得して表示するために表示の度にリストをクリアする
            BaudRateeComboBox.Items.Clear();

            // ボーレートを出力する
            BaudRateeComboBox.Items.Add("115200"); //デフォルトなのでこれを最初にもってくる

            BaudRateeComboBox.Items.Add("9600");
            BaudRateeComboBox.Items.Add("14400");
            BaudRateeComboBox.Items.Add("19200");
            BaudRateeComboBox.Items.Add("28800");
            BaudRateeComboBox.Items.Add("38400");
            BaudRateeComboBox.Items.Add("57600");
            BaudRateeComboBox.Items.Add("76800");
            BaudRateeComboBox.Items.Add("115200");
            BaudRateeComboBox.Items.Add("230400");
            BaudRateeComboBox.Items.Add("460800");
        }



    }
}
