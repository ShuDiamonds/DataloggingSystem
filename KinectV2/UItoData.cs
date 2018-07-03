using System;
using System.Windows;
using System.Windows.Controls;
// 追加
namespace KinectV2
{
    public partial class MainWindow : Window
    {
        int questionnaireCNT = 0;
        int questionnaireCNT_correctNUM = 0;
        DateTime ButtonLastTime = DateTime.Now;
        private void UIDataSaveButton_Click(object sender, RoutedEventArgs e)
        {
            UIDataSave();
            ButtonUIupdate();
            ButtonLastTime = DateTime.Now;
        }


        public void UIDataSave()
        {
            if (FirstUIData == false)
            {
                FirstUIDataLogging();
            }

            //ラジオボタンの取得
            #region スタックパネル内のラジオボタンの状態の取得

            string Q1ans = "", Q2ans = "", Q3ans = "", Q4ans = "";
            foreach (RadioButton rb in Q1panel.Children)
            {
                if ((bool)rb.IsChecked)
                {
                    Q1ans = rb.Name.ToString();
                    //rb.IsChecked = false;
                    //Console.WriteLine("Q1の回答は"+Q1ans[4]);
                    break;
                }
            }

            foreach (RadioButton rb in Q2panel.Children)
            {
                if ((bool)rb.IsChecked)
                {
                    Q2ans = rb.Name.ToString();
                    //rb.IsChecked = false;
                    break;
                }
            }
            foreach (RadioButton rb in Q3panel.Children)
            {
                if ((bool)rb.IsChecked)
                {
                    Q3ans = rb.Name.ToString();
                    //rb.IsChecked = false;
                    break;
                }
            }
            foreach (RadioButton rb in Q4panel.Children)
            {
                if ((bool)rb.IsChecked)
                {
                    Q4ans = rb.Name.ToString();
                    //rb.IsChecked = false;
                    break;
                }
            }

            if (Q1ans == "")
            {
                MessageBox.Show("Q1が入力されていません");
                return;
            }
            if (Q2ans == "")
            {
                MessageBox.Show("Q2が入力されていません");
                return;
            }
            if (Q3ans == "")
            {
                MessageBox.Show("Q3が入力されていません");
                return;
            }
            if (Q4ans == "")
            {
                MessageBox.Show("Q4が入力されていません");
                return;
            }
            #endregion

            DateTime dtNow = DateTime.Now;
            FileSave(filepath + "Questionnaire" + ".csv",
                       dtNow.ToString(),
                       Q1ans[4].ToString(),
                       Q2ans[4].ToString(),
                       Q3ans[4].ToString(),
                       Q4ans[4].ToString()
                       );

            questionnaireCNT++;
            return;
        }


        public void FirstUIDataLogging()
        {


            FirstUIData = true;

            //初回QuestionnaireUIdataLoggingデータの保存


            FileSave(filepath + "Questionnaire" + ".csv",
                       "TimeStamp",
                       "Q1",
                       "Q2",
                       "Q3",
                       "Q4"
                       );

            return;

        }

        


    }
}
