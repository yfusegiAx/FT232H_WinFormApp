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

        FTDI myFtdiDevice = new FTDI();//ftdi���i���g�����߂̃C���X�^���X����
        FTDI_CommonFunction commonFunction = new FTDI_CommonFunction();
        BME280 bme280 = new BME280();
        SSD1306 ssd1306 = new SSD1306();
        //driver�̒�`
        FTDI.FT_STATUS ftStatus;//�ʐM�\�ȏ��
        uint deviceCount = 0;

        // Create new instance of the FTDI device class
        //FTDI�f�o�C�X�N���X�̃C���X�^���X����
        //�f�o�C�X�N���X..usb�C���^�[�t�F�[�X���������Ă��鐻�i���
        public Form1()
        {       
            InitializeComponent();
            
            myFtdiDevice.OpenByIndex(0);//0�Ԗڂɐڑ������f�o�C�X�ɃA�N�Z�X

            // Update the Status text line
            if (ftStatus == FTDI.FT_STATUS.FT_OK)//�ڑ������f�o�C�X�̃X�e�[�^�X�̊m�F
            {
                status_value.Text = "Device Open";
            }
            else
            {
                status_value.Text = "Device NotFound";
            }
            Refresh();//Update���L�͈͂̍ĕ`�� �������x��
            //Update();//Forms��Controll�N���X�̊֐��@ �N���C�A���g�̈���̖��������ꂽ�̈悪�ĕ`�悳���
            Application.DoEvents();//System.WindowForms ���b�Z�[�W�L���[�Ɍ��݂���window���b�Z�[�W�����ׂď�������
            
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                return;//error �I��
            }

            myFtdiDevice.SetBitMode(0xFF,0x0);//���s�̃f�o�C�X���v�����ꂽ�f�o�C�X���[�h��Ή����Ă��Ȃ��Ƃ��Ƀf�t�H���g��UART,FIFO�ȊO�̃��[�h��ݒ肷��
            myFtdiDevice.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE);//setbitmode..(byte mask,byte bitmode)
            //FTDI.FT_BIT_MODES.FT_BIT_MODE_MPSSE=0x2
        }
      
        public string ByteToString(byte[] input, int num)
        {
            return $"0x{BitConverter.ToString(input, 0, num).Replace("-", " ")}";//binary->hex
        }

        private void SPIRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //SPI�ʐM���n�߂�
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
            //I2C�ʐM���n�߂�
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
            //BME280�ƒʐM����
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
            //SSD1306�ƒʐM����
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
            //�w�肵���ʐM�K�i�ƃf�o�C�X�ŒʐM���͂��߂�
            if (SPI_connect==true && BME280_connect==true)
            {
                bme280.SPI_BME280_Connect(myFtdiDevice);
                Templature_value.Text = $"{Math.Round(BME280_data[0], 3)}";//BME280�Ŏ擾�����l�̕\���F���x �L�����u���[�V������̒l
                Pressure_value.Text = $"{Math.Round(BME280_data[1] / 100.0, 3)}";//BME280�Ŏ擾�����l�̕\���F�C�� �L�����u���[�V������̒l
                Humidlity_value.Text = $"{Math.Round(BME280_data[2], 3)}";//BME280�Ŏ擾�����l�̕\���F���x �L�����u���[�V������̒l
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

        private void SlaveBME280RadioButton_CheckedChanged(object sender, EventArgs e)//slaveaddress��ݒ肷��
        {
            slaveAddress = 0x76;
            bme280.BME280_GetSlaveAddress(ref slaveAddress);
        }

        private void SlaveSSD1306RadioButton_CheckedChanged(object sender, EventArgs e)//slaveaddress��ݒ肷��
        {
            slaveAddress = (0x3C<<1 | 0b0);
            ssd1306.SSD1306_GetSlaveAddress(ref slaveAddress);
        }

        private void DisplayMode_comboBox_SelectedIndexChanged(object sender, EventArgs e)//displaymode��I�������Ƃ��̓���
        {
            DisplayModeSelected = true;
            DisplayMode = DisplayMode_comboBox.SelectedItem.ToString();
        }

    }
}