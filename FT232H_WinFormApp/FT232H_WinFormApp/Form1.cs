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
        //driver�̒�`
        FTDI.FT_STATUS ftStatus;//�ʐM�\�ȏ��

        //FT232H�ɖ��߂��o�����߂̐錾

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
        //FTDI�f�o�C�X�N���X�̃C���X�^���X����
        //�f�o�C�X�N���X..usb�C���^�[�t�F�[�X���������Ă��鐻�i���

        //label1:interface��"status"
        //label2:interface��status�\��
        //label3:interface��"proximity"
        //label_status:interface��"status"
        //label5:interface��"Red"
        //label6:interface��Red�̒l
        //label7:interface��"Green"
        //label8:interface��Green�̒l
        //label9:interface��"Blue"
        //label10:interface��Blue�̒l

        FTDI myFtdiDevice = new FTDI();

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            var stopwatch = new Stopwatch();

            uint written = 0;
       
            InitializeComponent();
            stopwatch.Start();//�����v�N���ɉ��b�����邩

            ftStatus = myFtdiDevice.OpenByIndex(0);//0�Ԗڂɐڑ������f�o�C�X�ɃA�N�Z�X

            //stopwatch.Stop();

            //myFtdiDevice.GetNumberOfDevices(ref deviceCount);//deviceCount�B�BPC�Ɛڑ��ł���f�o�C�X�̐�
            status_name_label.Text = ftStatus.ToString();
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                return;//error �I��
            }
            myFtdiDevice.SetBitMode(0xFF,0x0);//���s�̃f�o�C�X���v�����ꂽ�f�o�C�X���[�h��Ή����Ă��Ȃ��Ƃ��Ƀf�t�H���g��UART,FIFO�ȊO�̃��[�h��ݒ肷��
            //setbitmode..(byte mask,byte bitmode) //0xFF..���ׂďo�� handle��c#�ł͕s�v
            //bitmode 0=reset 
            //bit���}�X�N���遁bit�𕢂��B��
            //
            myFtdiDevice.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);//setbitmode..(byte mask,byte bitmode)
            //FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE=0x2

            byte[] code;
            //List<byte> code_list = new List<byte>();//List���ƒx��

            //code_list.Count;

            code = new byte[] { 0x80, 0b11111111, 0xFF };
            myFtdiDevice.Write(code, code.Length, ref written);
            //Thread.Sleep(500);
            /*
            for (int i = 0; i < 10; i++)
            {

                code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7����1�𑗂�
                myFtdiDevice.Write(code, code.Length, ref written);
                Thread.Sleep(500);
                Debug.WriteLine("code.length="+code.Length);//code.length�m�F�@
                Debug.WriteLine("written=" + written);//written�m�F�@
               

                code = new byte[] { 0x80, 0b11111110, 0xFF };//3byte�������܂�� //adbus0~adbus6�͂P�@adbus7��0 adbus0�����ڑ����Ă���ꍇ�͓_�ł���
                myFtdiDevice.Write(code, code.Length, ref written);//�������ރo�C�g�z��@�f�o�C�X�ɏ������܂��o�C�g���@���ۃf�o�C�X�ɏ������܂��o�C�g��

                Thread.Sleep(500);
            }
            */

            //myFtdiDevice.Write(new byte[] {0x80},1,ref written);
           
            /*
           code = new byte[] { 0x8d, 0x86, 0xa1, 0x1a, 0x20, 0x6D, 0x00 };
           myFtdiDevice.Write(code, code.Length, ref written);

           code = new byte[] { 0x80, 0b11111111, 0xFF };
           myFtdiDevice.Write(code, code.Length, ref written);
            
            code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7����1�𑗂�
            myFtdiDevice.Write(code, code.Length, ref written);

            uint bufnum = 0;

            byte[] buf = new byte[bufnum];
            myFtdiDevice.GetRxBytesAvailable(ref bufnum);//public GetRxBytesAvailable(uint32 &RxQueue) //�ǂݍ��݂̂��߂ɗ��p�\�ȃo�C�g��
            //read�ŏ��߂ăJ�E���g���n�܂�H
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
            //initialize�{�^��
            //�f�o�C�X�̓o�^
            bool DeviceInit = false;
            buttonInit.Enabled = false;

            try
            {
                ftStatus = myFtdiDevice.GetNumberOfDevices(ref deviceCount);//�ڑ��\�ȃf�o�C�X�̐��𐔂���A�Ԃ�l��FT_STATUS
            }
            catch
            {
                label_status.Text = "Driver not loaded";

                buttonInit.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
            }

            myFtdiDevice.OpenByIndex(0);//0�Ԗڂɐڑ������f�o�C�X�ɃA�N�Z�X

            // Update the Status text line
            if (ftStatus == FTDI.FT_STATUS.FT_OK)//�ڑ������f�o�C�X�̃X�e�[�^�X�̊m�F
            {
                DeviceOpen = true;
                label_status.Text = "Open";
            }
            else
            {
                DeviceOpen = false;
                label_status.Text = "No Device Found";
            }
            Refresh();
            //Update();//Forms��Controll�N���X�̊֐��@ �N���C�A���g�̈���̖��������ꂽ�̈悪�ĕ`�悳���
            Application.DoEvents();//System.WindowForms ���b�Z�[�W�L���[�Ɍ��݂���window���b�Z�[�W�����ׂď�������
                                   //���s�[���V�����t�H�[���̐����[���C�x���g�̏���

           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //start�{�^��
            //�ʐM�̊J�n
            //�N���b�N�𑗂�f�[�^������
            //  code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7����1�𑗂�
            byte sendData = 0x00;//����f�[�^
            uint readOnlyBufNum = 0;//�ǂݍ��ݗp�o�b�t�@

             byte[] ftdiData= new byte[] { 0x10,0x00,0x00,sendData };//data output buffer :+VE���ɃN���b�N�𑗂�A0~1byte�̃f�[�^�𑗂�AsendData�Ƃ����f�[�^�𑗂�
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//�N���b�N�����オ�� ��������

            ftdiData = new byte[] { 0x24, 0x00, 0x00};//data input buffer :-VE���ɃN���b�N�𑗂�A0~1byte�̃f�[�^�𑗂�
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//�N���b�N������ �ǂݍ���

            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum)==FTDI.FT_STATUS.FT_OK)
            {
                Proximity_value.Text = "read clear!";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //stop�{�^��
            //�ʐM�I��
            myFtdiDevice.Close();//openByIndex�̋t
            try
            {
                ftStatus = myFtdiDevice.GetNumberOfDevices(ref deviceCount);//�ڑ��\�ȃf�o�C�X�̐��𐔂���A�Ԃ�l��FT_STATUS
            }
            catch
            {
                label_status.Text = "Driver not loaded";

                buttonInit.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
            }

        }
    }
}