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
        public static bool SPI_connect = false;
        public static bool IIC_connect = false;
        public static bool BME280_connect = false;
        public static bool SSD1306_connect = false;




        FTDI myFtdiDevice = new FTDI();//ftdi製品を使うためのインスタンス生成

        public Form1()
        {       
            InitializeComponent();
            //デバイスの登録
            
            bool DeviceInit = false;
            
            try
            {
                ftStatus = myFtdiDevice.GetNumberOfDevices(ref deviceCount);//接続可能なデバイスの数を数える、返り値はFT_STATUS
            }
            catch
            {
                status_value.Text = "Driver not loaded";
            }

            myFtdiDevice.OpenByIndex(0);//0番目に接続したデバイスにアクセス

            // Update the Status text line
            if (ftStatus == FTDI.FT_STATUS.FT_OK)//接続したデバイスのステータスの確認
            {
                status_value.Text = "Device Open";
            }
            else
            {
                status_value.Text = "Device NotFound";
            }
            Refresh();//Updateより広範囲の再描画 ただし遅い
            //Update();//FormsのControllクラスの関数　 クライアント領域内の無効化された領域が再描画される
            Application.DoEvents();//System.WindowForms メッセージキューに現在あるwindowメッセージをすべて処理する
                                   //実行ー＞新しいフォームの生成ー＞イベントの処理
            
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
      
        public string ByteToString(byte[] input, int num)
        {
            return $"0x{BitConverter.ToString(input, 0, num).Replace("-", " ")}";//binary->hex
        }

        private void SPIRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //SPI通信を始める
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                MessageBox.Show("Please select communication standard");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                SPI_connect = true;
                IIC_connect = false;
            }
        }

        private void IICRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //I2C通信を始める
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                MessageBox.Show("Please select communication standard");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                SPI_connect = false;
                IIC_connect = true;
            }
        }

        private void BME280_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //BME280と通信する
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                MessageBox.Show("Please select device for communication");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                BME280_connect = true;
                SSD1306_connect = false;
            }
        }

        private void SSD1306_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //SSD1306と通信する
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                MessageBox.Show("Please select device for communication");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                BME280_connect = false;
                SSD1306_connect = true;
            }
        }

        private void DeviceConnect_Button_Click(object sender, EventArgs e)
        {
            //指定した通信規格とデバイスで通信をはじめる
            SPI spi = new SPI();
            IIC iic = new IIC();

            if (SPI_connect==true && BME280_connect==true)
            {
                spi.SPI_BME280_Connect();
            }
            else if (SPI_connect == true && SSD1306_connect == true)
            {
                spi.SPI_SSD1306_Connect();
            }
            else if (IIC_connect == true && BME280_connect == true)
            {
                iic.IIC_BME280_Connect();
            }
            else if (IIC_connect == true && SSD1306_connect == true)
            {
                iic.IIC_SSD1306_Connect();
            }
            else
            {
                MessageBox.Show("connect error");
            }
        }

        private void AppEnd_Button_Click(object sender, EventArgs e)
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

                DeviceConnect_Button.Enabled = false;
                AppEnd_Button.Enabled = true;
            }
            Thread.Sleep(1000);
            Application.Exit();//アプリケーションの終了
        }
    }
}