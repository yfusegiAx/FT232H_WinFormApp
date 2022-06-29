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
        public static bool SPI_connect = false;
        public static bool IIC_connect = false;
        public static bool BME280_connect = false;
        public static bool SSD1306_connect = false;
        public static bool DisplayModeSelected = false;
        public static byte slaveAddress;
        public static double[] BME280_data = new double[3];
        public static string DisplayMode;

        FTDI myFtdiDevice = new FTDI();//ftdi製品を使うためのインスタンス生成
        FTDI_CommonFunction commonFunction = new FTDI_CommonFunction();
        BME280 bme280 = new BME280();
        SSD1306 ssd1306 = new SSD1306();
        //driverの定義
        FTDI.FT_STATUS ftStatus;//通信可能な状態
        uint deviceCount = 0;

        // Create new instance of the FTDI device class
        //FTDIデバイスクラスのインスタンス生成
        //デバイスクラス..usbインターフェースが所持している製品情報
        public Form1()
        {       
            InitializeComponent();
            
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
            
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                return;//error 終了
            }

            myFtdiDevice.SetBitMode(0xFF,0x0);//現行のデバイスが要求されたデバイスモードを対応していないときにデフォルトのUART,FIFO以外のモードを設定する
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

            if (rb.Checked)
            {
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

            if (rb.Checked)
            {
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

            if (rb.Checked)
            {
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

            if (rb.Checked)
            {
                BME280_connect = false;
                SSD1306_connect = true;
            }
        }

        private void DeviceConnect_Button_Click(object sender, EventArgs e)
        {
            //指定した通信規格とデバイスで通信をはじめる
            if (SPI_connect==true && BME280_connect==true)
            {
                bme280.SPI_BME280_Connect(myFtdiDevice);
                Templature_value.Text = $"{Math.Round(BME280_data[0], 3)}";//BME280で取得した値の表示：温度 キャリブレーション後の値
                Pressure_value.Text = $"{Math.Round(BME280_data[1] / 100.0, 3)}";//BME280で取得した値の表示：気圧 キャリブレーション後の値
                Humidlity_value.Text = $"{Math.Round(BME280_data[2], 3)}";//BME280で取得した値の表示：湿度 キャリブレーション後の値
            }
            else if (SPI_connect == true && SSD1306_connect == true && DisplayModeSelected == true)
            {
                ssd1306.SPI_SSD1306_Connect(myFtdiDevice);
            }
            else if (IIC_connect == true && BME280_connect == true)
            {
                bme280.IIC_BME280_Connect(myFtdiDevice);
            }
            else if (IIC_connect == true && SSD1306_connect == true && DisplayModeSelected == true)
            {
                ssd1306.IIC_SSD1306_Connect(myFtdiDevice,(0x3C<<1 | 0b0));
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

        private void SlaveBME280RadioButton_CheckedChanged(object sender, EventArgs e)//slaveaddressを設定する
        {
            slaveAddress = 0x76;
            bme280.BME280_GetSlaveAddress(ref slaveAddress);
        }

        private void SlaveSSD1306RadioButton_CheckedChanged(object sender, EventArgs e)//slaveaddressを設定する
        {
            slaveAddress = (0x3C<<1 | 0b0);
            ssd1306.SSD1306_GetSlaveAddress(ref slaveAddress);
        }

        private void DisplayMode_comboBox_SelectedIndexChanged(object sender, EventArgs e)//displaymodeを選択したときの動作
        {
            DisplayModeSelected = true;
            DisplayMode = DisplayMode_comboBox.SelectedItem.ToString();
        }

    }
}