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
        FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;//通信可能な状態

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
        uint devcount = 0;

        //###################################################################################################################################
        //###################################################################################################################################
        //##################                          Main Application Layer                                            #####################
        //###################################################################################################################################
        //###################################################################################################################################

        // Create new instance of the FTDI device class
        //FTDIデバイスクラスのインスタンス生成
        //デバイスクラス..usbインターフェースが所持している製品情報
        FTDI myFtdiDevice = new FTDI();

        public Form1()
        {
            uint written = 0;
            uint bufnum = 0;
            InitializeComponent();
            myFtdiDevice.OpenByIndex(0);//0番目に接続したデバイスにアクセス
            myFtdiDevice.SetBitMode(0xFF,0x0);//setbitmode..(byte mask,byte bitmode) //0xFF..すべて出力 handleはc#では不要
            //bitmode 0=reset 
            myFtdiDevice.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);//setbitmode..(byte mask,byte bitmode)
            //FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE=0x2

            byte[] code;
            //List<byte> code_list = new List<byte>();//Listだと遅い

            //code_list.Count;
            
            //code = new byte[] { 0x80, 0b11111111, 0xFF };
            //myFtdiDevice.Write(code, code.Length, ref written);
           
            code = new byte[] { 0x80, 0b11110001, 0xFF };//3byte書き込まれる
           
            myFtdiDevice.Write(code, code.Length, ref written);//書き込むバイト配列　デバイスに書き込まれるバイト数　実際デバイスに書き込まれるバイト数
          
            //myFtdiDevice.Write(new byte[] {0x80},1,ref written);
            Debug.WriteLine("code.length="+code.Length);//code.length確認　
            Debug.WriteLine("written=" + written);//written確認　
            /*
           code = new byte[] { 0x8d, 0x86, 0xa1, 0x1a, 0x20, 0x6D, 0x00 };
           myFtdiDevice.Write(code, code.Length, ref written);

           code = new byte[] { 0x80, 0b11111111, 0xFF };
           myFtdiDevice.Write(code, code.Length, ref written);
           */
            myFtdiDevice.GetRxBytesAvailable(ref bufnum);

            byte[] buf = new byte[bufnum];


            myFtdiDevice.Read(buf, 1,ref written);

        }
        //label1:interfaceの"status"
        //label2:interfaceのstatus表示
        //label3:interfaceの"proximity"
        //label4:interfaceの"status"
        //label5:interfaceの"Red"
        //label6:interfaceのRedの値
        //label7:interfaceの"Green"
        //label8:interfaceのGreenの値
        //label9:interfaceの"Blue"
        //label10:interfaceのBlueの値


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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //startボタン
            //通信の開始
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //stopボタン
            //通信終了
        }
    }
}