using System;
using System.Diagnostics;
// 追加

using System.IO; //File.ReadAllLines
using System.Windows;
using System.Windows.Media;

namespace KinectV2
{
    public partial class MainWindow : Window
    {

        string filename = "";
        string filepath = "./data/";


        public void FirstDataLogging()
        {
            // 現在の日付と時刻を取得する
            DateTime dtNow = DateTime.Now;

            //ディレクトリの作成
            filepath = "./output" + dtNow.ToLongDateString() + dtNow.ToString("tthh時mm分ss秒") + "/";
            Directory.CreateDirectory(filepath);

            //filename = filepath+"output" + dtNow.ToLongDateString() + dtNow.ToString("tthh時mm分ss秒") + ".csv";
            filename = $"{filepath}output.csv";

            //取得した日付と時刻を表示する
            Console.WriteLine(filename);
            FileSave(filename,
                "TimeStamp",
                 "clickCNTL",
                    "clickCNTR",
                    "clickCNTM",
                    "MouseDisplacement",
                    "MouseSpeedMax",
                    "MouseWheelAmount",
                    "KeyTypeCNT",
                    "KeyTypeDelCNT",
                    "KeyTypeBackCNT",
                    "KeyTypeEnterCNT",
                    "Curvature",
                    "AppName",
                    //FFT関係
                    "FFT0",
                    "FFT1",

                    //イスのデータ
                    "Compass",
                    "Posture_RightLeft",
                    "Posture_Rear",
                    "Posture_Front",
                    "Sag",
                    "Rotation",
                    "SittingStatus",
                      //メモ
                      "Memo"
                );

            //Resume 04/18

            #region UIDATAファイル書き込み new
            //System.Console.WriteLine(LabelBox.ToString() + ".csv");
            FirstUIDataLogging();
            #endregion


            return;
        }

        

        Boolean FirstUIData = false;
        

        void dt_Tick(object sender, EventArgs e)
        {
            //残り時間の更新
            ButtonUIupdate();
            //シリアルポートの状態を反映
            if (serialPort.IsOpen)
            {
                ConnectStatus.Content = "Opened";
                ConnectStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                if (ChairData_cnt==0)
                {
                    ConnectStatus.Content = "受信できていません";
                    ConnectStatus.Foreground = new SolidColorBrush(Color.FromRgb(255,0,0));
                }
            } else
            {
                ConnectStatus.Content = "Closed";
            }
            

            Process ProcessName = GetActiveProcess();
            //Console.WriteLine(ProcessName.ProcessName);
            AppName = ProcessName.ProcessName;

            #region　平均化

            //マウスとキーボードは自動的に平均化されているのでいらない

            //FFTによる音については自動で平均化されているのでいらない。

            //イスの平均化
            for (int i = 0; i < ChairData.Length - 1; i++)
            {
                ChairData[i] = ChairData[i]/ ChairData_cnt;
            }
            


            #endregion

            DateTime dtNow = DateTime.Now;
            Console.WriteLine();

            FileSave(filename,
                       dtNow.ToString(),
                     //WinAPI
                     clickCNTL.ToString(),
                      clickCNTR.ToString(),
                      clickCNTM.ToString(),
                      MouseDisplacement.ToString(),
                      MouseSpeedMax.ToString(),
                      MouseWheelAmount.ToString(),
                      KeyTypeCNT.ToString(),
                      KeyTypeDelCNT.ToString(),
                      KeyTypeBackCNT.ToString(),
                      KeyTypeEnterCNT.ToString(),
                      Math.Round(Curvature,1).ToString(),
                      AppName,
                      //FFT関係
                      FFT_y_i[0].ToString(),
                      FFT_y_i[1].ToString(),

                      //ここからイスのデータ
                      Math.Round(ChairData[0],0).ToString(),
                      Math.Round(ChairData[1], 2).ToString(),
                      Math.Round(ChairData[2], 2).ToString(),
                      Math.Round(ChairData[3], 2).ToString(),
                      Math.Round(ChairData[4], 1).ToString(),
                      Math.Round(ChairData[5], 2).ToString(),
                      Math.Round(ChairData[6], 2).ToString(),
                      //メモ
                      memotextbox.Text
                 );


            #region デバッグ

            //String tmp = Convert.ToString(p.X) + "," + Convert.ToString(p.Y);

            ////Beep(262, 500);  // ド
            ////SetCursorPos(100,100);
            //// マウス左クリックがあれば・・・
            //int x = GetAsyncKeyState((int)System.Windows.Forms.Keys.LButton);
            //if ((x & 1) != 0)
            //{
            //    tmp += " !";
            //}
            //TB_Log.Text = tmp + Environment.NewLine + TB_Log.Text;

            //WinAPI
            //labelContext.Content = "clickCNTL:" + clickCNTL.ToString() + "\n";
            //labelContext.Content += "clickCNTR:" + clickCNTR.ToString() + "\n";
            //labelContext.Content += "MouseDisplacement:" + MouseDisplacement.ToString() + "\n";
            //labelContext.Content += "MouseSpeedMax:" + MouseSpeedMax.ToString() + "\n";
            //labelContext.Content += "MouseWheelAmount:" + MouseWheelAmount.ToString() + "\n";
            //labelContext.Content += "KeyTypeCNT:" + KeyTypeCNT.ToString() + "\n";
            //labelContext.Content += "Curvature:" + Curvature.ToString() + "\n";
            //labelContext.Content += "AppName:" + AppName + "\n";



            //顔のデータ
            //labelContext.Content = "LM_JawOpen: " + LM_JawOpen.ToString("0.00") + "\n";
            //labelContext.Content += "LM_LeftcheekPuff: " + LM_LeftcheekPuff.ToString("0.00") + "\n";
            //labelContext.Content += "LM_JawOpen: " + LM_LipCornerDepressorRight.ToString("0.00") + "\n";
            //labelContext.Content += "LM_LipCornerDepressorRight: " + LM_LipCornerDepressorLeft.ToString("0.00") + "\n";
            //labelContext.Content += "LM_LipCornerPullerRight: " + LM_LipCornerPullerRight.ToString("0.00") + "\n";
            //labelContext.Content += "LM_LipCornerPullerLeft: " + LM_LipCornerPullerLeft.ToString("0.00") + "\n";
            //labelContext.Content += "LM_RightcheekPuff: " + LM_RightcheekPuff.ToString("0.00") + "\n";
            //labelContext.Content += "LM_LeftcheekPufff: " + LM_LeftcheekPufff.ToString("0.00") + "\n";
            //labelContext.Content += "LM_RighteyebrowLowerer: " + LM_RighteyebrowLowerer.ToString("0.00") + "\n";
            //labelContext.Content += "LM_LefteyebrowLowerer: " + LM_LefteyebrowLowerer.ToString("0.00") + "\n";





            ////ここで表情筋の情報を表示
            //label.Content = ("RightEyebrow:" + faceAlignment.AnimationUnits[FaceShapeAnimations.RighteyebrowLowerer].ToString("0.00") + "\n");
            //label.Content += ("RightcheekPuff:" + faceAlignment.AnimationUnits[FaceShapeAnimations.RightcheekPuff].ToString("0.00") + "\n");
            //label.Content += ("JawOpen:" + faceAlignment.AnimationUnits[FaceShapeAnimations.JawOpen].ToString("0.00") + "\n");
            //label.Content += ("JawSlideRight:" + faceAlignment.AnimationUnits[FaceShapeAnimations.JawSlideRight].ToString("0.00") + "\n");
            //label.Content += ("LipCornerPullerRight:" + faceAlignment.AnimationUnits[FaceShapeAnimations.LipCornerPullerRight].ToString("0.00") + "\n");
            //label.Content += ("RightLipCornerDepressor:" + faceAlignment.AnimationUnits[FaceShapeAnimations.LipCornerDepressorRight].ToString("0.00") + "\n");
            //label.Content += ("LipStretcherRight:" + faceAlignment.AnimationUnits[FaceShapeAnimations.LipStretcherRight].ToString("0.00") + "\n");
            //label.Content += ("LipPucker:" + faceAlignment.AnimationUnits[FaceShapeAnimations.LipPucker].ToString("0.00") + "\n");

            #endregion


            #region 取得するデータを格納してる変数の初期化
            dtNow = DateTime.Now;
            Console.WriteLine(dtNow.ToLongDateString() + dtNow.ToString("tthh時mm分ss秒"));
            //WINAPI関係の初期化
            clickCNTL = 0;
            clickCNTR = 0;
            clickCNTM = 0;
            MouseDisplacement = 0;
            MouseSpeedMax = 0;
            MouseWheelAmount = 0;
            KeyTypeCNT = 0;
            KeyTypeDelCNT = 0;
            KeyTypeBackCNT = 0;
            KeyTypeEnterCNT = 0;
            PreCurvature = 0;
            Curvature = 0;


            //FFT関係
            FFT_y_i[0] = 0;
            FFT_y_i[1] = 0;

            //イスのデータの初期化
            for(int i = 0; i < ChairData.Length - 1; i++)
                {
                ChairData[i] = 0;
            }
            ChairData_cnt = 0;

            #endregion






        }

        

        


        

        void FileSave(string FILENAME, params string[] SaveData)
        {
            try
            {
                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(FILENAME, true, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    string DataText = "";
                    foreach (string Element in SaveData)
                    {
                        DataText = DataText + Element + ",";
                    }
                    sw.WriteLine(DataText);

                }
            }
            catch
            {

            }


            return;
        }

    }
}
