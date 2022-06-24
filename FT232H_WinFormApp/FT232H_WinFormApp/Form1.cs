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
       
        //driver�̒�`
        FTDI.FT_STATUS ftStatus;//�ʐM�\�ȏ��

        uint deviceCount = 0;

        // Create new instance of the FTDI device class
        //FTDI�f�o�C�X�N���X�̃C���X�^���X����
        //�f�o�C�X�N���X..usb�C���^�[�t�F�[�X���������Ă��鐻�i���


        FTDI myFtdiDevice = new FTDI();

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {       
            InitializeComponent();
            //�f�o�C�X�̓o�^
            /*
            bool DeviceInit = false;
            buttonInit.Enabled = false;

            try
            {
                ftStatus = myFtdiDevice.GetNumberOfDevices(ref deviceCount);//�ڑ��\�ȃf�o�C�X�̐��𐔂���A�Ԃ�l��FT_STATUS

            }
            catch
            {
                status_value.Text = "Driver not loaded";

                buttonInit.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
            }

            myFtdiDevice.OpenByIndex(0);//0�Ԗڂɐڑ������f�o�C�X�ɃA�N�Z�X

            // Update the Status text line
            if (ftStatus == FTDI.FT_STATUS.FT_OK)//�ڑ������f�o�C�X�̃X�e�[�^�X�̊m�F
            {
                status_value.Text = "Open";
            }
            else
            {
                status_value.Text = "No Device Found";
            }
            Refresh();//Update���L�͈͂̍ĕ`�� �������x��
            //Update();//Forms��Controll�N���X�̊֐��@ �N���C�A���g�̈���̖��������ꂽ�̈悪�ĕ`�悳���
            Application.DoEvents();//System.WindowForms ���b�Z�[�W�L���[�Ɍ��݂���window���b�Z�[�W�����ׂď�������
                                   //���s�[���V�����t�H�[���̐����[���C�x���g�̏���
            */


            ftStatus = myFtdiDevice.OpenByIndex(0);//0�Ԗڂɐڑ������f�o�C�X�ɃA�N�Z�X
            //myFtdiDevice.GetNumberOfDevices(ref deviceCount);//deviceCount�B�BPC�Ɛڑ��ł���f�o�C�X�̐�
            status_value.Text = ftStatus.ToString();
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                return;//error �I��
            }

            myFtdiDevice.SetBitMode(0xFF,0x0);//���s�̃f�o�C�X���v�����ꂽ�f�o�C�X���[�h��Ή����Ă��Ȃ��Ƃ��Ƀf�t�H���g��UART,FIFO�ȊO�̃��[�h��ݒ肷��
            //setbitmode..(byte mask,byte bitmode) //0xFF..���ׂďo�� handle��c#�ł͕s�v
            //bitmode 0=reset 
            //bit���}�X�N���遁bit�𕢂��B��
            
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

        private void SSD1306_Button_Click(object sender, EventArgs e)
        {
            SSD1306 ssd1306 =new SSD1306();
            ssd1306.IIC_Connect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newval"></param>
        /// <param name="oldval"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        //�t�B���^�������ꍇ�Ɏ�������֐�
�@�@�@�@/*
        double MMA(double newval, double  oldval, int N)
        {
            return ((N - 1) * oldval + newval)/ N;
        }
       */

        private void BMP280_Button_Click(object sender, EventArgs e)
        {
            //SPI�ʐM��BMP�Ƃ���肷��
            //  code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7����1�𑗂�
           
            byte sendData = 0x88;//����f�[�^
            uint readOnlyBufNum = 0;//�ǂݍ��ݗp�o�b�t�@

            uint written = 0;
            byte[] code,readData;
            /*
            //0x80 output
            //Value     Direction 
            ///// �ʐM�J�n�@�@////
            code = new byte[] { 0x86, 4, 0 };//pin�����Z�b�g..�ʐM�O�ɏ�Ԃ����Z�b�g�ł��� 0x80...output lowbyte
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

             code = new byte[] { 0x80, 0b11111111, 0b11111011 };//pin�����Z�b�g..�ʐM�O�ɏ�Ԃ����Z�b�g�ł��� 0x80...output lowbyte
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            code = new byte[] { 0x80, 0b11110111, 0b11111011 };//adbus2��low�ɂ��邱�ƂŒʐM�������X���[�u��I���ł���悤�ɂȂ�
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            code = new byte[] { 0x80, 0b11110110, 0b11111011 };//adbus0=0�ɂ��� �N���b�N�𑗂邽��
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            //�ǂݍ��ݑO�̐ݒ�
            //code = new byte[] { 0x11, 0x01,0x00,0x60,0xB6};//BME�ւ�reset�̖��� E0��B6�𑗂� -VE�̂Ƃ�data��output����
            //myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            //�ݒ���������Ă���ǂݍ��݊J�n

            //���x�̐ݒ�@�ݒ肵�Ȃ��ƃX�L�b�v�����
            code = new byte[] { 0x11, 0x01, 0x00, 0x72, 0x01 };
            //0(write) F2(11110010) value(00000001) :over sampling x1==>01110010 00000001=>0x72��0x01���������݂�����o�C�g�z��
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς�� �f�[�^�ʂ�write�ŏ��߂Ē�`����

            //���x�@���́@���[�h�̐ݒ�
            code = new byte[] { 0x11, 0x01, 0x00, 0x74, 0x27 };//0(�������݃��[�h) F4�Ɂ@value���������݂�����o�C�g�z��
            //0 11110100 00100111==>0x74 ��0x27����������
            //001 001 11 ==>0x27 ���x�f�[�^�̃I�[�o�[�T���v�����Ox1 ���̓f�[�^�̃I�[�o�[�T���v�����Ox1 �ʏ탂�[�h:11 (�X���[�v��00 ������01,10)
            myFtdiDevice.Write(code, code.Length, ref readOnlyBufNum);//�N���b�N�����オ�� ���߂��������݁@

            /////  �ʐM�@/////
            byte[] ftdiData = new byte[] { 0x11, 0x00, 0x00, sendData };//data output buffer :+VE���ɃN���b�N�𑗂�A1byte�̃f�[�^�𑗂�AsendData�Ƃ����f�[�^�𑗂邽�߂�byte�z��
            //0x88�Ƃ����f�[�^�𑗂��ftdi���̓f�[�^�𑗂��Ă��邪BME��0x88�Ƃ����A�h���X��ǂ܂��Ăق����ƔF�m
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//�N���b�N�����オ�� ���߂��������݁@

            ftdiData = new byte[] { 0x20, 0x79, 0x00 };//data input buffer :-VE���ɃN���b�N�𑗂�A���̎��_�ł̓N���b�N�͏o�Ă��Ȃ� 0x0078+1�ǂݎ��ݒ�
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//�N���b�N������  �ǂݎ�������@�Ƃ�������
            //�����ł�ftdi�̃o�b�t�@�ɓǂݎ�����f�[�^�������Ă����� �܂��f�[�^�͎Q�Ƃ��Ă��Ȃ�

            /////   �ʐM�I��   /////
            code = new byte[] { 0x80, 0b11111110, 0b11111011 };//cs���P�ɂȂ�@�X���[�u�Ƃ̂����̏I��
            myFtdiDevice.Write(code, code.Length, ref written);//
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };//reset �̂��߂̔z��
            myFtdiDevice.Write(code, code.Length, ref written);//
           
            Thread.Sleep(100);//FT232H����������̂�2�~���b�����邽�ߑ҂��Ă�����@100byte���炢�����

            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum) == FTDI.FT_STATUS.FT_OK)
            {
                Debug.WriteLine($"readonlybufnum={readOnlyBufNum}");
                readData = new byte[readOnlyBufNum];//�ǂݍ��񂾃f�[�^���i�[���邽�߂�byte�z�� new byte��malloc���Ă���
                myFtdiDevice.Read(readData, readOnlyBufNum, ref readOnlyBufNum);//�����œǂݍ��� byte[] dataBuffer,uint numBytesToRead,ref uint numBytesRead
                                                                                //�f�[�^���i�[����o�b�t�@�A�f�o�C�X����v�����ꂽ�o�C�g���A���ۓǂݍ��܂��o�C�g��
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
            BME280 bme280=new BME280();//�C���X�^���X����
            bme280.BME280_Calib(readData);//ID��Ԃ� dig..�̒l�̏����� 0x60���Ԃ��Ă��Ȃ��Ǝ��x�͓ǂݎ��Ȃ� �Ȃ�60���Ԃ��Ă���?
            Thread.Sleep(10);//FT232H����������̂�2�~���b�����邽�ߑ҂��Ă�����@100byte���炢�����
            bme280.BME280_Calc(readData.Skip(0xF7 - 0x88).ToArray() );//���X0x88����X�^�[�g����Ƃ����0xF7-0x88�Ԗڂ܂ŋN�_���X�L�b�v
            Templature_value.Text = $"{Math.Round(bme280.Temprature,3)}";//BME280�Ŏ擾�����l�̕\���F���x �L�����u���[�V������̒l
            Humidlity_value.Text = $"{Math.Round(bme280.Humidity,3)}";//BME280�Ŏ擾�����l�̕\���F���x �L�����u���[�V������̒l
            Pressure_value.Text = $"{Math.Round(bme280.Pressure/100.0,3)}";//BME280�Ŏ擾�����l�̕\���F�C�� �L�����u���[�V������̒l
            */
            //SSD�̓���m�F

            //�C���X�^���X�̔j��?
            

            //����bit�z��
            //1
            //������ scl,sda��high�� 
            //0x80...output GPIO pin is lowbyte not output databytes
            //                  hex   value H/L    direction I/O     
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            //2
            //scl=high,sda=low  start condition���`����
            byte[] startConditionCode = new byte[] { 0x80, 0b11111101, 0b11111011 };
            myFtdiDevice.Write(startConditionCode, startConditionCode.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��
          
            //3
            //sa0(�X���[�u�A�h���X) + R/W#�iread/write�j�𑗂�
            //sa0= 0111 101*          R/W#=0 =>01111010=>�A�h���X��FTDI�ɂƂ��Ă�0x7A�@�X���[�u�ɂƂ��Ă�0x3D
            //sa0= 0111 100* (�����) R/W#=0 =>01111000=>����f�[�^��0x78�@�X���[�u�ɂƂ��Ă�0x3C
            byte[] sa0Code = new byte[] { 0x11, 0x00, 0x00, (0x3C << 1) | 0b0 };//-VE write databyte output


            //4
            //data output by recerver for ack signal
            byte[] ackCode = new byte[] { 0x33, 0x00, 1 };//-VE read bits 1�𑗂�
           
            //5
            //Co(continuation bit) + D/C#(data/command select bit) + ControlByte + ACK + DataByte + ACK�𑗂�
            //����ۂ͏�L�����g���G���f�B�A��(ack,databyte,ack,control,dc,co)
            //Co=0�̂Ƃ��̓f�[�^�o�C�g��������Ȃ�
            //0x12..only �{VE bits output
            //0x36..in -VE out +VE bits output
            //0x11�ł܂Ƃ߂��f�[�^�����
            //co=1(data��command������) + dc=0(command) +controlbyte=000000 =1000 0000 =>0x08
            byte[] controlCode = new byte[] {0x11,0x00,0x00,0x80};
            //ack+databyte
            //displayOn�̃R�}���h�𑗂�
            //Display On AFh
            //AFh:D/C Hex D7 D6 D5 D4 D3 D2 D1 D0
            //    0   AF  1  0  1  0  1  1  1  1
            byte[] commandCode = new byte[] {0x11,0x00,0x00,0xAF};
            //combine
            byte[] combineDataCode=new byte[sa0Code.Length+ackCode.Length+controlCode.Length+ackCode.Length+commandCode.Length+ackCode.Length];//0x80+0x00+0xAF+0x00 
            sa0Code.CopyTo(combineDataCode,0);
            ackCode.CopyTo(combineDataCode, sa0Code.Length);
            controlCode.CopyTo(combineDataCode, ackCode.Length + sa0Code.Length);
            ackCode.CopyTo(combineDataCode, ackCode.Length + sa0Code.Length + controlCode.Length);
            commandCode.CopyTo(combineDataCode, ackCode.Length + sa0Code.Length + controlCode.Length + ackCode.Length);
            ackCode.CopyTo(combineDataCode, ackCode.Length + sa0Code.Length+ controlCode.Length + ackCode.Length + commandCode.Length);
            myFtdiDevice.Write(combineDataCode, combineDataCode.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            //6
            //stop condition
            byte[] stopConditionCode = new byte[] { 0x80, 0b11111111, 0b11111011 };
            myFtdiDevice.Write(stopConditionCode, stopConditionCode.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��
        }
        public string ByteToString(byte[] input, int num)
        {
            return $"0x{BitConverter.ToString(input, 0, num).Replace("-", " ")}";
        }

        private void AppEnd_Button_Click(object sender, EventArgs e)
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
                status_value.Text = "Driver not loaded";

                SSD1306_Button.Enabled = false;
                BMP280_Button.Enabled = false;
                AppEnd_Button.Enabled = true;
            }
            Thread.Sleep(1000);
            Application.Exit();//�A�v���P�[�V�����̏I��

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void SPIRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //SPI�ʐM���n�߂�

        }

        private void I2CRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //I2C�ʐM���n�߂�
        }

        private void Hectpascal_value_Click(object sender, EventArgs e)
        {

        }
        
        private void FileSelectButton_Click(object sender, EventArgs e)
        {
            
        }

    }
}