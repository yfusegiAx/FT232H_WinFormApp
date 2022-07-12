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
using System.Text.RegularExpressions;

namespace FT232H_WinFormApp
{
    public partial class Form1 : Form
    {
        public bool SPI_connect = false;
        public bool IIC_connect = false;
        public bool BME280_connect = false;
        public bool SSD1306_connect = false;
        public bool DisplayModeSelected = false;

        public FTDI myFtdiDevice = new FTDI();//ftdi製品を使うためのインスタンス生成 publicの場合はvarで宣言できない
        public FTDI_CommonFunction commonFunction = new FTDI_CommonFunction();
        public BME280 bme280 = new BME280();
        public SSD1306 ssd1306;
        FTDI.FT_STATUS ftStatus;//デバイスの接続状況を取得する
        uint deviceCount = 0;

        public Form1()
        {       
            InitializeComponent();

            ssd1306 = new SSD1306(myFtdiDevice, 0x3C << 1);

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
            var rb = sender as RadioButton;
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
            var rb = sender as RadioButton;
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
            RadioButton? rb = sender as RadioButton;
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
            RadioButton? rb = sender as RadioButton;
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
                //connect関数と値を取得する関数は分けたほうがいい
                //bme280.SPI_BME280_Connect(myFtdiDevice);
                bme280.SPI_BME280_Connect(myFtdiDevice);

                Templature_value.Text = $"{Math.Round(bme280.Temprature, 3)}";//BME280で取得した値の表示：温度 キャリブレーション後の値
                Pressure_value.Text = $"{Math.Round(bme280.Pressure / 100.0, 3)}";//BME280で取得した値の表示：気圧 キャリブレーション後の値
                Humidlity_value.Text = $"{Math.Round(bme280.Humidity, 3)}";//BME280で取得した値の表示：湿度 キャリブレーション後の値
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
                ssd1306.IIC_SSD1306_Connect(ssd1306.DisplayMode);
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
            //slaveAddress = 0x76;
            bme280.BME280_GetSlaveAddress();
        }

        private void SlaveSSD1306RadioButton_CheckedChanged(object sender, EventArgs e)//slaveaddressを設定する
        {
            //slaveAddress = (0x3C<<1 | 0b0);
            //ssd1306.SSD1306_GetSlaveAddress();
        }

        private void DisplayMode_comboBox_SelectedIndexChanged(object sender, EventArgs e)//displaymodeを選択したときの動作
        {
            DisplayModeSelected = true;
            ssd1306.DisplayMode = DisplayMode_comboBox.SelectedItem;
        }
        /// <summary>
        /// ulongデータを、ビッグエンディアンで、そのままdataリストに追加していく。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public int AddListDataData(List<byte> data, ulong p)//0x 00 01 02 03 04 05 06 07 08
        {
            int datNum = 0;
            for (int i = 7; i >= 0; i--)
            {
                int shift = (i << 3);
                if (p >= (0x01UL << shift))
                {
                    data.Add((byte)((p & (0xFFUL << shift)) >> shift));
                    datNum++;
                }
            }
            return datNum;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<byte> data=new List<byte>();
            var texts = Regex.Replace(textBox1.Text, "[^0-9a-fA-F ]", "").Split(' ');

            foreach (var t in texts)
            {
                if (t != "" && t.Length < 17)
                {
                    ulong ut = Convert.ToUInt64(t, 16);
                    if (ut == 0u)
                        data.Add(0);
                    else
                        AddListDataData(data, ut);
                }

            }
            Console.WriteLine(data);
        }
    }
}