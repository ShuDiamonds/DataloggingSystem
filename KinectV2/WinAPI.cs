using System;
using System.Windows;
using System.Diagnostics;


using System.Windows.Threading;     //timer
// 追加
using System.Runtime.InteropServices;
namespace KinectV2
{
    public partial class MainWindow : Window
    {

        #region WinAPI関係のクラス宣言
        /// Win32API の extern 宣言クラス

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);



        /// アクティブなプロセスを取得する
        /// アクティブプロセス
        public static Process GetActiveProcess()
        {
            // アクティブなウィンドウハンドルの取得
            IntPtr hWnd = GetForegroundWindow();
            int id;
            // ウィンドウハンドルからプロセスIDを取得
            GetWindowThreadProcessId(hWnd, out id);
            Process process = Process.GetProcessById(id);
            return process;
        }

        [DllImport("kernel32.dll")]
        private extern static bool Beep(uint dwFreq, uint dwDuration);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool mouse_event(
        ulong dwFlags,// 移動とクリックのオプション
        int dx,              // 水平位置または移動量
        int dy,              // 垂直位置または移動量
        int dwData,          // ホイールの移動
        int dwExtraInfo  // アプリケーション定義の情報
            );



        #region DLL Import

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);



        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(int vKey);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        #endregion

        #endregion

        #region WinAPIからとるデータの定義
        int clickCNTL = 0, clickCNTR = 0, MouseDisplacement = 0, MouseSpeedMax = 0,
            MouseWheelAmount = 0, KeyTypeCNT = 0, KeyTypeEnterCNT = 0, KeyTypeDelCNT = 0,
            KeyTypeBackCNT = 0, clickCNTM = 0;
        float PreCurvature = 0, Curvature = 0;
        POINT p, PrePoint = new POINT(0, 0);
        string AppName;
        #endregion

        private DispatcherTimer dt2;

        //マウスとキーボードの監視をタイマーで行う
        void dt_Tick2(object sender, EventArgs e)
        {
            //ここからキーボード
            #region キーボード監視



            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.LButton)) != 0)
            {
                //マウスボタンが押されている
                Console.WriteLine("MouseL ON\n");
                clickCNTL++;
            }
            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.RButton)) != 0)
            {
                //マウスボタンが押されている
                Console.WriteLine("MouseR ON\n");
                clickCNTR++;
            }
            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.MButton)) != 0)
            {
                //マウスボタンが押されている
                Console.WriteLine("MouseM ON\n");
                clickCNTM++;
            }
            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.Up)) != 0
                || (1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.Down)) != 0)
            {
                //マウスホイールが押されている
                Console.WriteLine("Mouse VolumeUPDown\n");
            }
            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.A)) != 0
                || (1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.I)) != 0
                || (1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.U)) != 0
                || (1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.E)) != 0
                || (1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.O)) != 0
                )
            {
                //Aキーが押されている
                Console.WriteLine("AIUEO\n");
                KeyTypeCNT++;
            }

            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.Delete)) != 0)
            {
                Console.WriteLine("Delete\n");
                KeyTypeDelCNT++;
            }

            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.Back)) != 0)
            {
                Console.WriteLine("Back\n");
                KeyTypeBackCNT++;
            }

            if ((1 & GetAsyncKeyState((int)System.Windows.Forms.Keys.Enter)) != 0)
            {
                Console.WriteLine("Enter\n");
                KeyTypeEnterCNT++;
            }

            #endregion

            //ここまでキーボード

            #region ここからマウスの操作量の取得

            if (GetCursorPos(out p))
            {
                //マウスの移動量の合計
                MouseDisplacement += Math.Abs(p.X - PrePoint.X) + Math.Abs(p.Y - PrePoint.Y);
                //マウスの最大移動量
                if (MouseSpeedMax < Math.Sqrt(Math.Pow(Math.Abs(p.X - PrePoint.X), 2)
                    + Math.Pow(Math.Abs(p.Y - PrePoint.Y), 2)))
                {
                    MouseSpeedMax = (int)Math.Sqrt(Math.Pow(Math.Abs(p.X - PrePoint.X), 2)
                    + Math.Pow(Math.Abs(p.Y - PrePoint.Y), 2));
                }
                //カーソルの移動の湾曲度
                //カーソルの動き方について直線的か湾曲的かについて情報を定義したい
                //変位のｘｙ成分から角度を算出、それを微分したら物の絶対値の総和をとる。


                float tmp = (float)Math.Atan2(p.Y - PrePoint.Y, p.X - PrePoint.X);
                if (p.Y - PrePoint.Y == 0 && p.X - PrePoint.X == 0) tmp = 0;
                Curvature += Math.Abs(tmp - PreCurvature);
                PreCurvature = tmp;


                PrePoint = p;

            }

            #endregion
            #region デバッグ
            ////String tmp = Convert.ToString(p.X) + "," + Convert.ToString(p.Y);

            //////Beep(262, 500);  // ド
            //////SetCursorPos(100,100);
            ////// マウス左クリックがあれば・・・
            ////int x = GetAsyncKeyState((int)System.Windows.Forms.Keys.LButton);
            ////if ((x & 1) != 0)
            ////{
            ////    tmp += " !";
            ////}
            ////TB_Log.Text = tmp + Environment.NewLine + TB_Log.Text;

            //labelContext.Content = "clickCNT:" + clickCNT.ToString() + "\n";
            //labelContext.Content += "MouseDisplacement:" + MouseDisplacement.ToString() + "\n";
            //labelContext.Content += "MouseSpeedMax:" + MouseSpeedMax.ToString() + "\n";
            //labelContext.Content += "MouseWheelAmount:" + MouseWheelAmount.ToString() + "\n";
            //labelContext.Content += "KeyTypeCNT:" + KeyTypeCNT.ToString() + "\n";
            //labelContext.Content += "Curvature:" + Curvature.ToString() + "\n";
            //labelContext.Content += "AppName:" + AppName + "\n";
            #endregion




        }
    }
}
