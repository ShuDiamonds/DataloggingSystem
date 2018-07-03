using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

// 追加


//Audio
using NAudio.Wave;


namespace KinectV2
{
    public partial class MainWindow : Window
    {
        #region FFT関係
        // 波形サイズ
        int size = 4096/4;
        // サンプリング周波数
        int fs = 4000;

        // FFT用データ
        double[] reFFT;
        double[] imFFT;

        /*
        // FFT
        public static void FFT(double[] inputRe, double[] inputIm, out double[] outputRe, out double[] outputIm, int bitSize)
        {
            int dataSize = 1 << bitSize;
            int[] reverseBitArray = BitScrollArray(dataSize);

            outputRe = new double[dataSize];
            outputIm = new double[dataSize];

            // バタフライ演算のための置き換え
            for (int i = 0; i < dataSize; i++)
            {
                outputRe[i] = inputRe[reverseBitArray[i]];
                outputIm[i] = inputIm[reverseBitArray[i]];
            }

            // バタフライ演算
            for (int stage = 1; stage <= bitSize; stage++)
            {
                int butterflyDistance = 1 << stage;
                int numType = butterflyDistance >> 1;
                int butterflySize = butterflyDistance >> 1;

                double wRe = 1.0;
                double wIm = 0.0;
                double uRe = System.Math.Cos(System.Math.PI / butterflySize);
                double uIm = -System.Math.Sin(System.Math.PI / butterflySize);

                for (int type = 0; type < numType; type++)
                {
                    for (int j = type; j < dataSize; j += butterflyDistance)
                    {
                        int jp = j + butterflySize;
                        double tempRe = outputRe[jp] * wRe - outputIm[jp] * wIm;
                        double tempIm = outputRe[jp] * wIm + outputIm[jp] * wRe;
                        outputRe[jp] = outputRe[j] - tempRe;
                        outputIm[jp] = outputIm[j] - tempIm;
                        outputRe[j] += tempRe;
                        outputIm[j] += tempIm;
                    }
                    double tempWRe = wRe * uRe - wIm * uIm;
                    double tempWIm = wRe * uIm + wIm * uRe;
                    wRe = tempWRe;
                    wIm = tempWIm;
                }
            }
        }
        // ビットを左右反転した配列を返す
        private static int[] BitScrollArray(int arraySize)
        {
            int[] reBitArray = new int[arraySize];
            int arraySizeHarf = arraySize >> 1;

            reBitArray[0] = 0;
            for (int i = 1; i < arraySize; i <<= 1)
            {
                for (int j = 0; j < i; j++)
                    reBitArray[j + i] = reBitArray[j] + arraySizeHarf;
                arraySizeHarf >>= 1;
            }
            return reBitArray;
        }
        */

        #endregion

        //記録する変数の定義
        int[] FFT_y_i = new int[2] { 1, 1 };
        double[] FFT_Y = new double[2] { 0, 0 };
        /*
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // 32bitで最大値1.0fにする
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);

                float sample32 = sample / 32768f;
                ProcessSample(sample32);
            }
        }

        List<double> _recorded = new List<double>(); // 音声データ

        private void ProcessSample(float sample)
        {
            var windowsize = size;
            _recorded.Add(sample);
            if (_recorded.Count == windowsize)
            {
                // FFT用データ
                double[] FFTIn = new double[size];
                double[] FFTInIm = new double[size];
                // 窓関数後データ
                double[] data = new double[size];
                // 波形生成
                for (int i = 0; i < size; i++)
                {
                    FFTInIm[i] = 0.0;
                }

                FFTIn = _recorded.Take(size).ToArray();

                // FFT
                FFT(FFTIn, FFTInIm, out reFFT, out imFFT, (int)Math.Log(size, 2));
                bool FFTBigFlag = false, FFTSmallFlag = false;
                for (int i = 0; i < size / 2; i++)
                {
                    if (i > 0)
                    {
                        //FFTの横軸の周波数の単位、分解能についてはここを見る
                        //URL　http://detail.chiebukuro.yahoo.co.jp/qa/question_detail/q1350561093
                        //FFTの分解能（ひと間隔）はfs/sizeでももとまる。また、横軸の最大値は、サンプリング周波数の半分（サンプリング定理）となる。
                        float FFTResolution = ((float)fs / size);
                        float FFT_x = (float)i * FFTResolution;
                        double FFT_y = Math.Sqrt(reFFT[i] * reFFT[i] + imFFT[i] * imFFT[i]);

                        //現在のxの範囲は、0から2000Hzまで測定できる
                        if (200 < FFT_x && FFT_x < 1000)
                        {
                            //ノイズ処理
                            if (4 < FFT_y && FFT_y < 7)
                            {
                                FFTSmallFlag = true;
                                //FFT_y_i[0]++;
                                //FFT_Y[0] = FFT_y + FFT_Y[0];
                            }
                        }
                        if (200 < FFT_x && FFT_x < 1000)
                        {
                            //ノイズ処理
                            if (20 < FFT_y)
                            {
                                FFTBigFlag = true;
                                //FFT_y_i[1]++;
                                //FFT_Y[1] = FFT_y + FFT_Y[1];
                            }
                        }
                        //FFTResult[i] = new DataPoint(x, y);
                    }
                }
                if (FFTSmallFlag)
                {
                    FFT_y_i[0]++;
                }
                if (FFTBigFlag)
                {
                    FFT_y_i[1]++;
                }
                //FFT_Y[0] = FFT_Y[0] / FFT_y_i[0];
                ////FFT_Y[1] = FFT_Y[1] / FFT_y_i[1];
                //label.Content = "FFT"+ FFT_y_i[0].ToString()+ "\n";
                //label.Content += FFT_y_i[1].ToString();
                //line1.ItemsSource = DftIn;
                //line2.ItemsSource = FFTResult.Take(FFTResult.Count() / 2);


                _recorded.Clear();
            }
        }
        */

    }
}
