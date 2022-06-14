#define FT232H

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTD2XX_NET;
using System.Threading;
using System.Diagnostics;

namespace FT232H_WinFormApp
{
    public partial class Form1 : Form
    {
       
        //driverの定義
        FTDI.FT_STATUS ftStatus;//通信可能な状態

        uint deviceCount = 0;

        // Create new instance of the FTDI device class
        //FTDIデバイスクラスのインスタンス生成
        //デバイスクラス..usbインターフェースが所持している製品情報


        FTDI myFtdiDevice = new FTDI();

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            var stopwatch = new Stopwatch();

       
            InitializeComponent();
            stopwatch.Start();//ランプ起動に何秒かかるか

            ftStatus = myFtdiDevice.OpenByIndex(0);//0番目に接続したデバイスにアクセス

            //stopwatch.Stop();

            //myFtdiDevice.GetNumberOfDevices(ref deviceCount);//deviceCount。。PCと接続できるデバイスの数
            status_value.Text = ftStatus.ToString();
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                return;//error 終了
            }
           
            myFtdiDevice.SetBitMode(0xFF,0x0);//現行のデバイスが要求されたデバイスモードを対応していないときにデフォルトのUART,FIFO以外のモードを設定する
            //setbitmode..(byte mask,byte bitmode) //0xFF..すべて出力 handleはc#では不要
            //bitmode 0=reset 
            //bitをマスクする＝bitを覆い隠す
            //
            myFtdiDevice.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);//setbitmode..(byte mask,byte bitmode)
            //FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE=0x2

           
            //Thread.Sleep(500);
            
            //ランプを10回点滅させるテスト
            /*
            for (int i = 0; i < 10; i++)
            {

                code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7から1を送る
                myFtdiDevice.Write(code, code.Length, ref written);
                Thread.Sleep(500);
                Debug.WriteLine("code.length="+code.Length);//code.length確認　
                Debug.WriteLine("written=" + written);//written確認　
               

                code = new byte[] { 0x80, 0b11111110, 0xFF };//3byte書き込まれる //adbus0~adbus6は１　adbus7は0 adbus0だけ接続している場合は点滅する
                myFtdiDevice.Write(code, code.Length, ref written);//書き込むバイト配列　デバイスに書き込まれるバイト数　実際デバイスに書き込まれるバイト数

                Thread.Sleep(500);
            }
            */

            //myFtdiDevice.Write(new byte[] {0x80},1,ref written);
           
            /*
           code = new byte[] { 0x8d, 0x86, 0xa1, 0x1a, 0x20, 0x6D, 0x00 };
           myFtdiDevice.Write(code, code.Length, ref written);

           code = new byte[] { 0x80, 0b11111111, 0xFF };
           myFtdiDevice.Write(code, code.Length, ref written);
            
            code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7から1を送る
            myFtdiDevice.Write(code, code.Length, ref written);

            uint bufnum = 0;

            byte[] buf = new byte[bufnum];
            myFtdiDevice.GetRxBytesAvailable(ref bufnum);//public GetRxBytesAvailable(uint32 &RxQueue) //読み込みのために利用可能なバイト数
            //readで初めてカウントが始まる？
            if (bufnum >= 0)
            {
                Debug.WriteLine("bufnum=" + $"{bufnum}");
            }
           */
            


            //myFtdiDevice.Read(buf, 1,ref written);

        }
      

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //initializeボタン
            //デバイスの登録
            bool DeviceInit = false;
            buttonInit.Enabled = false;

            try
            {
                ftStatus = myFtdiDevice.GetNumberOfDevices(ref deviceCount);//接続可能なデバイスの数を数える、返り値はFT_STATUS

            }
            catch
            {
                status_value.Text = "Driver not loaded";

                buttonInit.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
            }

            myFtdiDevice.OpenByIndex(0);//0番目に接続したデバイスにアクセス

            // Update the Status text line
            if (ftStatus == FTDI.FT_STATUS.FT_OK)//接続したデバイスのステータスの確認
            {
                status_value.Text = "Open";
            }
            else
            {
                status_value.Text = "No Device Found";
            }
            Refresh();//Updateより広範囲の再描画 ただし遅い
            //Update();//FormsのControllクラスの関数　 クライアント領域内の無効化された領域が再描画される
            Application.DoEvents();//System.WindowForms メッセージキューに現在あるwindowメッセージをすべて処理する
                                   //実行ー＞新しいフォームの生成ー＞イベントの処理

           

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newval"></param>
        /// <param name="oldval"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        //フィルタが無い場合に実装する関数
　　　　/*
        double MMA(double newval, double  oldval, int N)
        {
            return ((N - 1) * oldval + newval)/ N;
        }
       */

        private void button2_Click(object sender, EventArgs e)
        {
            //startボタン
            //通信の開始
            //クロックを送りデータも送る
            //  code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7から1を送る

            byte sendData = 0x88;//送るデータ
            uint readOnlyBufNum = 0;//読み込み用バッファ
            byte[] code;
            uint written = 0;
            //List<byte> code_list = new List<byte>();//Listだと遅い

            //code_list.Count;
            //                          Value     Direction 
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };//pinをリセット..通信前に状態をリセットできる
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

            code = new byte[] { 0x80, 0b11110111, 0b11111011 };//adbus2をlowにすることで通信したいスレーブを選択できるようになる
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

            code = new byte[] { 0x80, 0b11110110, 0b11111011 };//adbus0=0にする クロックを送るため
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる


            byte[] ftdiData= new byte[] { 0x11,0x00,0x00, sendData };//data output buffer :+VE時にクロックを送る、1byteのデータを送る、sendDataというデータを送る
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック立ち上がる 書き込み
            
            ftdiData = new byte[] { 0x20, 0x77, 0x00};//data input buffer :-VE時にクロックを送る、1byteのデータを送る この時点ではクロックは出ていない
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック下がる 読み込みのためのデータの送信が起きる 
            Thread.Sleep(2);//FT232Hが反応するのに2ミリ秒かかるため待ってあげる　100byteくらいが上限

            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum)==FTDI.FT_STATUS.FT_OK)
            {
                code = new byte[] { 0x80, 0b11111110, 0b11111011 };
                myFtdiDevice.Write(code, code.Length, ref written);//データを送る　クロックが発生する
                code = new byte[] { 0x80, 0b11111111, 0b11111011 };
                myFtdiDevice.Write(code, code.Length, ref written);//データを送る　クロックが発生する
                Templature_value.Text = "${@}";//BME280で取得した値の表示：温度
                Humidlity_value.Text = "${@}";//BME280で取得した値の表示：湿度
                Hectpascal_value.Text = "${@}";//BME280で取得した値の表示：気圧
                Debug.WriteLine($"readonlybufnum={readOnlyBufNum}");
                byte[] readData = new byte[readOnlyBufNum];//読み込んだデータを格納するためのbyte配列
                myFtdiDevice.Read(readData,readOnlyBufNum,ref readOnlyBufNum);//ここで読み込む byte[] dataBuffer,uint numBytesToRead,ref uint numBytesRead
                                                                              //データを格納するバッファ、デバイスから要求されたバイト数、実際読み込まれるバイト数
                Debug.WriteLine($"readData = 0x{readData[0]:X02}");

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //stopボタン
            //通信終了
            myFtdiDevice.Close();//openByIndexの逆
            try
            {
                ftStatus = myFtdiDevice.GetNumberOfDevices(ref deviceCount);//接続可能なデバイスの数を数える、返り値はFT_STATUS
            }
            catch
            {
                status_value.Text = "Driver not loaded";

                buttonInit.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
            }
            Thread.Sleep(1000);
            Application.Exit();//アプリケーションの終了

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void SPIRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //SPI通信を始める

        }

        private void I2CRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //I2C通信を始める
        }
    }
}