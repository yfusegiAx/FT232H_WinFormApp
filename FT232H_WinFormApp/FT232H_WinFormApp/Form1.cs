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
            var stopwatch = new Stopwatch();

       
            InitializeComponent();
            stopwatch.Start();//�����v�N���ɉ��b�����邩

            ftStatus = myFtdiDevice.OpenByIndex(0);//0�Ԗڂɐڑ������f�o�C�X�ɃA�N�Z�X

            //stopwatch.Stop();

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
            //
            myFtdiDevice.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);//setbitmode..(byte mask,byte bitmode)
            //FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE=0x2

           
            //Thread.Sleep(500);
            
            //�����v��10��_�ł�����e�X�g
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

        private void button2_Click(object sender, EventArgs e)
        {
            //start�{�^��
            //�ʐM�̊J�n
            //�N���b�N�𑗂�f�[�^������
            //  code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7����1�𑗂�

            byte sendData = 0x88;//����f�[�^
            uint readOnlyBufNum = 0;//�ǂݍ��ݗp�o�b�t�@
            byte[] code;
            uint written = 0;
            //List<byte> code_list = new List<byte>();//List���ƒx��

            //code_list.Count;
            //                          Value     Direction 
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };//pin�����Z�b�g..�ʐM�O�ɏ�Ԃ����Z�b�g�ł���
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            code = new byte[] { 0x80, 0b11110111, 0b11111011 };//adbus2��low�ɂ��邱�ƂŒʐM�������X���[�u��I���ł���悤�ɂȂ�
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��

            code = new byte[] { 0x80, 0b11110110, 0b11111011 };//adbus0=0�ɂ��� �N���b�N�𑗂邽��
            myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�d�ʂ��ς��


            byte[] ftdiData= new byte[] { 0x11,0x00,0x00, sendData };//data output buffer :+VE���ɃN���b�N�𑗂�A1byte�̃f�[�^�𑗂�AsendData�Ƃ����f�[�^�𑗂�
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//�N���b�N�����オ�� ��������
            
            ftdiData = new byte[] { 0x20, 0x77, 0x00};//data input buffer :-VE���ɃN���b�N�𑗂�A1byte�̃f�[�^�𑗂� ���̎��_�ł̓N���b�N�͏o�Ă��Ȃ�
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//�N���b�N������ �ǂݍ��݂̂��߂̃f�[�^�̑��M���N���� 
            Thread.Sleep(2);//FT232H����������̂�2�~���b�����邽�ߑ҂��Ă�����@100byte���炢�����

            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum)==FTDI.FT_STATUS.FT_OK)
            {
                code = new byte[] { 0x80, 0b11111110, 0b11111011 };
                myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�N���b�N����������
                code = new byte[] { 0x80, 0b11111111, 0b11111011 };
                myFtdiDevice.Write(code, code.Length, ref written);//�f�[�^�𑗂�@�N���b�N����������
                Templature_value.Text = "${@}";//BME280�Ŏ擾�����l�̕\���F���x
                Humidlity_value.Text = "${@}";//BME280�Ŏ擾�����l�̕\���F���x
                Hectpascal_value.Text = "${@}";//BME280�Ŏ擾�����l�̕\���F�C��
                Debug.WriteLine($"readonlybufnum={readOnlyBufNum}");
                byte[] readData = new byte[readOnlyBufNum];//�ǂݍ��񂾃f�[�^���i�[���邽�߂�byte�z��
                myFtdiDevice.Read(readData,readOnlyBufNum,ref readOnlyBufNum);//�����œǂݍ��� byte[] dataBuffer,uint numBytesToRead,ref uint numBytesRead
                                                                              //�f�[�^���i�[����o�b�t�@�A�f�o�C�X����v�����ꂽ�o�C�g���A���ۓǂݍ��܂��o�C�g��
                Debug.WriteLine($"readData = 0x{readData[0]:X02}");

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
                status_value.Text = "Driver not loaded";

                buttonInit.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
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
    }
}