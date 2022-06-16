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
            InitializeComponent();

            ftStatus = myFtdiDevice.OpenByIndex(0);//0番目に接続したデバイスにアクセス

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
            
            myFtdiDevice.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);//setbitmode..(byte mask,byte bitmode)
            //FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE=0x2

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

            uint written = 0;
            byte[] readData;
            //0x80 output
            //Value     Direction 
            ///** 通信開始　　**//
            byte[] code = new byte[] { 0x80, 0b11111111, 0b11111011 };//pinをリセット..通信前に状態をリセットできる 0x80...output lowbyte
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

            code = new byte[] { 0x80, 0b11110111, 0b11111011 };//adbus2をlowにすることで通信したいスレーブを選択できるようになる
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

            code = new byte[] { 0x80, 0b11110110, 0b11111011 };//adbus0=0にする クロックを送るため
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

            ///**  通信　**///
            byte[] ftdiData = new byte[] { 0x11, 0x00, 0x00, sendData };//data output buffer :+VE時にクロックを送る、1byteのデータを送る、sendDataというデータを送るためのbyte配列
            //0x88というデータを送るとftdi側はデータを送っているがBMEは0x88というアドレスを読ませてほしいと認知
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック立ち上がる 命令を書き込み　

            ftdiData = new byte[] { 0x20, 0x79, 0x00 };//data input buffer :-VE時にクロックを送る、この時点ではクロックは出ていない 0x0078+1読み取る設定
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック下がる  読み取りをしろ　という命令
            //ここではftdiのバッファに読み取ったデータが入っている状態 まだデータは参照していない

            ///**   通信終了   **///
            code = new byte[] { 0x80, 0b11111110, 0b11111011 };//csが１になる　スレーブとのやり取りの終了
            myFtdiDevice.Write(code, code.Length, ref written);//
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };//reset のための配列
            myFtdiDevice.Write(code, code.Length, ref written);//
           
            Thread.Sleep(10);//FT232Hが反応するのに2ミリ秒かかるため待ってあげる　100byteくらいが上限

            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum) == FTDI.FT_STATUS.FT_OK)
            {
                Debug.WriteLine($"readonlybufnum={readOnlyBufNum}");
                readData = new byte[readOnlyBufNum];//読み込んだデータを格納するためのbyte配列 new byteでmallocしている
                myFtdiDevice.Read(readData, readOnlyBufNum, ref readOnlyBufNum);//ここで読み込む byte[] dataBuffer,uint numBytesToRead,ref uint numBytesRead
                                                                                //データを格納するバッファ、デバイスから要求されたバイト数、実際読み込まれるバイト数
                if (readOnlyBufNum <= 0x78)
                {
                    Debug.WriteLine($"readData = NO DATA");
                    return;
                }
                else
                {
                    Debug.WriteLine($"readData = 0x{readData[0]:X02}");
                }

            }
            else {
                return;
            }
            //今回(6/16)はBMEとだけ通信するのでBMEだけと通信する前提でインスタンス生成
            BME280 bme280=new BME280();//インスタンス生成
            bme280.BME280_Calib(readData);//IDを返す dig..の値の初期化 0x60が返ってこないと湿度は読み取れない なぜ60が返ってくる?
            bme280.BME280_Calc(readData.Skip(0xF7 - 0x88).ToArray() );//元々0x88からスタートするところを0xF7-0x88番目まで起点をスキップ
            Templature_value.Text = $"{bme280.Temprature}";//BME280で取得した値の表示：温度 キャリブレーション後の値
            Humidlity_value.Text = $"{bme280.Humidity}";//BME280で取得した値の表示：湿度 キャリブレーション後の値
            Pressure_value.Text = $"{bme280.Pressure}";//BME280で取得した値の表示：気圧 キャリブレーション後の値

        }
        public string ByteToString(byte[] input, int num)
        {
            return $"0x{BitConverter.ToString(input, 0, num).Replace("-", " ")}";
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

        private void Hectpascal_value_Click(object sender, EventArgs e)
        {

        }
    }
}