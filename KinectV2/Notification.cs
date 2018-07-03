using System;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using System.Collections.Generic;



using System.Windows.Threading;     //timer
using System.Runtime.InteropServices;

namespace KinectV2
{
    public partial class MainWindow : Window
    {
        
        private System.Media.SoundPlayer player = null;
        string SoundFile = @"Sound/plucky.wav";

        void Notification_dt_Tick(object sender, EventArgs e)
        {
            if (SoundCheckBox.IsChecked == true)
            {
                PlaySound();
            }
            questionnaireCNT_correctNUM++;

            ButtonUIupdate();
            //mp3 url:https://notificationsounds.com/notification-sounds


        }

        

        public void ButtonUIupdate()
        {

            var lefttime = DateTime.Now - ButtonLastTime;
            //lefttime = new TimeSpan(0, 0, 15, 0)- lefttime;
            UIDataSaveButton.Content = "保存 " + questionnaireCNT.ToString() + "/" + questionnaireCNT_correctNUM.ToString()
                +"    経過時間: "+ lefttime.ToString(@"hh\:mm\:ss");

            return;
        }

        private void PlaySound()
        {
            player = new System.Media.SoundPlayer(SoundFile);
            player.Play();
        }

        private void StopSound()
        {
            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
        }

    }


}