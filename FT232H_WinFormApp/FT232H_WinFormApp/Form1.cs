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
            
            bool DeviceInit = false;

            try
            {
                ftStatus = myFtdiDevice.GetNumberOfDevices(ref deviceCount);//�ڑ��\�ȃf�o�C�X�̐��𐔂���A�Ԃ�l��FT_STATUS

            }
            catch
            {
                status_value.Text = "Driver not loaded";

                
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
      

       
        private void DeviceConnect_Button_Click(object sender, EventArgs e)
        {
           
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

                DeviceConnect_Button.Enabled = false;
                AppEnd_Button.Enabled = true;
            }
            Thread.Sleep(1000);
            Application.Exit();//�A�v���P�[�V�����̏I��

        }


        private void SPIRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //SPI�ʐM���n�߂�

        }

        private void IICRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //I2C�ʐM���n�߂�
        }

    }
}