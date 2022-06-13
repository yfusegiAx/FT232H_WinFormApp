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
        //###################################################################################################################################
        //###################################################################################################################################
        //##################                                      Definitions                                           #####################
        //###################################################################################################################################
        //###################################################################################################################################
        //driverの定義
        FTDI.FT_STATUS ftStatus;//通信可能な状態

        //FT232Hに命令を出すための宣言

        // ###### I2C Library defines ######
        const byte I2C_Dir_SDAin_SCLin = 0x00;
        const byte I2C_Dir_SDAin_SCLout = 0x01;
        const byte I2C_Dir_SDAout_SCLout = 0x03;
        const byte I2C_Dir_SDAout_SCLin = 0x02;
        const byte I2C_Data_SDAhi_SCLhi = 0x03;
        const byte I2C_Data_SDAlo_SCLhi = 0x01;
        const byte I2C_Data_SDAlo_SCLlo = 0x00;
        const byte I2C_Data_SDAhi_SCLlo = 0x02;

        // MPSSE clocking commands
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_IN = 0x24;
        const byte MSB_RISING_EDGE_CLOCK_BYTE_IN = 0x20;
        const byte MSB_FALLING_EDGE_CLOCK_BYTE_OUT = 0x11;
        const byte MSB_DOWN_EDGE_CLOCK_BIT_IN = 0x26;
        const byte MSB_UP_EDGE_CLOCK_BYTE_IN = 0x20;
        const byte MSB_UP_EDGE_CLOCK_BYTE_OUT = 0x10;
        const byte MSB_RISING_EDGE_CLOCK_BIT_IN = 0x22;
        const byte MSB_FALLING_EDGE_CLOCK_BIT_OUT = 0x13;

        // Clock
        const uint ClockDivisor = 49;      //          = 199;// for 100KHz
        // Sending and receiving
        static uint NumBytesToSend = 0;
        static uint NumBytesToRead = 0;
        uint NumBytesSent = 0;
        static uint NumBytesRead = 0;
        static byte[] MPSSEbuffer = new byte[500];
        static byte[] InputBuffer = new byte[500];
        static byte[] InputBuffer2 = new byte[500];
        static uint BytesAvailable = 0;
        static bool I2C_Ack = false;
        static byte AppStatus = 0;
        static byte I2C_Status = 0;
        public bool Running = true;
        static bool DeviceOpen = false;
        // GPIO
        static byte GPIO_Low_Dat = 0;
        static byte GPIO_Low_Dir = 0;
        static byte ADbusReadVal = 0;
        static byte ACbusReadVal = 0;

        // ###### Proximity sensor defines ######
        static byte Command = 0x00;
        static byte[] ProxData = new byte[500];
        static UInt16 ProxiValue = 0;
        static double ProxiValueD = 0;
        public const byte VCNL40x0_ADDRESS = 0x13;//0x13 is 7 bit address, 0x26 is 8bit address
        // registers
        public const byte REGISTER_COMMAND = 0x80;
        public const byte REGISTER_ID = 0x81;
        public const byte REGISTER_PROX_RATE = 0x82;
        public const byte REGISTER_PROX_CURRENT = 0x83;
        public const byte REGISTER_AMBI_PARAMETER = 0x84;
        public const byte REGISTER_AMBI_VALUE = 0x85;
        public const byte REGISTER_PROX_VALUE = 0x87;
        public const byte REGISTER_INTERRUPT_CONTROL = 0x89;
        public const byte REGISTER_INTERRUPT_LOW_THRES = 0x8a;
        public const byte REGISTER_INTERRUPT_HIGH_THRES = 0x8c;
        public const byte REGISTER_INTERRUPT_STATUS = 0x8e;
        public const byte REGISTER_PROX_TIMING = 0xf9;
        // Bits in the registers defined above
        public const byte COMMAND_SELFTIMED_MODE_ENABLE = 0x01;
        public const byte COMMAND_PROX_ENABLE = 0x02;
        public const byte COMMAND_AMBI_ENABLE = 0x04;
        public const byte COMMAND_MASK_PROX_DATA_READY = 0x20;
        public const byte PROX_MEASUREMENT_RATE_31 = 0x04;
        public const byte AMBI_PARA_AVERAGE_32 = 0x05; // DEFAULT
        public const byte AMBI_PARA_AUTO_OFFSET_ENABLE = 0x08; // DEFAULT enable
        public const byte AMBI_PARA_MEAS_RATE_2 = 0x10; // DEFAULT
        public const byte INTERRUPT_THRES_SEL_PROX = 0x00;
        public const byte INTERRUPT_THRES_ENABLE = 0x02;
        public const byte INTERRUPT_COUNT_EXCEED_1 = 0x00; // DEFAULT

        // ###### Colour sensor defines ######
        public const byte COLOR_ADDRESS = 0x29;
        public const byte _ENABLE = 0x80;                   //Enablestatusandinterrupts
        public const byte _ATIME = 0x81;                    //RGBCADCtime
        public const byte _CONTROL = 0x8F;                  //Gaincontrolregister
        public const byte _GAIN_x4 = 0x01;
        public const byte _GAIN_x16 = 0x10;
        public const byte _GAIN_x60 = 0x11;
        static byte Global_Red = 0;
        static byte Global_Green = 0;
        static byte Global_Blue = 0;
        uint deviceCount = 0;

        //###################################################################################################################################
        //###################################################################################################################################
        //##################                          Main Application Layer                                            #####################
        //###################################################################################################################################
        //###################################################################################################################################

        // Create new instance of the FTDI device class
        //FTDIデバイスクラスのインスタンス生成
        //デバイスクラス..usbインターフェースが所持している製品情報

        //label1:interfaceの"status"
        //label2:interfaceのstatus表示
        //label3:interfaceの"proximity"
        //label_status:interfaceの"status"
        //label5:interfaceの"Red"
        //label6:interfaceのRedの値
        //label7:interfaceの"Green"
        //label8:interfaceのGreenの値
        //label9:interfaceの"Blue"
        //label10:interfaceのBlueの値

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
                DeviceOpen = true;
                status_value.Text = "Open";
            }
            else
            {
                DeviceOpen = false;
                status_value.Text = "No Device Found";
            }
            Refresh();//Updateより広範囲の再描画 ただし遅い
            //Update();//FormsのControllクラスの関数　 クライアント領域内の無効化された領域が再描画される
            Application.DoEvents();//System.WindowForms メッセージキューに現在あるwindowメッセージをすべて処理する
                                   //実行ー＞新しいフォームの生成ー＞イベントの処理

           

        }

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
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };//adbus0~adbus7にすべてフラグを立てている
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　クロックが発生する

            code = new byte[] { 0x80, 0b11110011, 0b11111011 };//
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　クロックが発生する

            code = new byte[] { 0x80, 0b11110000, 0b11111011 };//
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　クロックが発生する


            byte[] ftdiData= new byte[] { 0x11,0x00,0x00, sendData };//data output buffer :+VE時にクロックを送る、1byteのデータを送る、sendDataというデータを送る
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック立ち上がる 書き込み
            
            ftdiData = new byte[] { 0x20, 0x77, 0x00};//data input buffer :-VE時にクロックを送る、1byteのデータを送る この時点ではクロックは出ていない

            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック下がる 読み込みのためのデータの送信が起きる readonlybufnumには読み込めたバイト数が格納されている?
            Thread.Sleep(10);//FT232Hが反応するのに2ミリ秒かかるため待ってあげる　100byteくらいが上限

            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum)==FTDI.FT_STATUS.FT_OK)
            {
                Templature_value.Text = "${@}";//BME280で取得した値の表示：温度
                Humidlity_value.Text = "${@}";//BME280で取得した値の表示：湿度
                Hectpascal_value.Text = "${@}";//BME280で取得した値の表示：気圧
                Debug.WriteLine($"readonlybufnum={readOnlyBufNum}");
                byte[] readData = new byte[readOnlyBufNum];//読み込んだデータを格納するためのbyte配列
                myFtdiDevice.Read(readData,readOnlyBufNum,ref readOnlyBufNum);//ここで読み込む？ byte dataBuffer,uint numBytesToRead,ref uint numBytesRead
                                                                              //デバイスから読み込まれたデータを移植されたバイト配列(ここにデータが入る、空でいい),デバイスから要求されたバイト数,実際読み込まれるバイト数
                Debug.WriteLine($"readData = 0x{readData[0]:X02}");

            }

            code = new byte[] { 0x80, 0b11111110, 0b11111011 };
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　クロックが発生する
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　クロックが発生する

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

        }

        private void I2CRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}