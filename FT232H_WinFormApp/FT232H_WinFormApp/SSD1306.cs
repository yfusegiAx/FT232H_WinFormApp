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
using System.Device.I2c;//I2Cインターフェースを使用するため
using System.Device.Gpio;//gpioを使う general purpose i/o
using Iot.Device.Ssd13xx.Commands;//ssd13xxを使う

namespace FT232H_WinFormApp
{

    public class SSD1306
    {
        public void SPI_SSD1306_Connect()
        {
            //SPIでSSD1306と通信するときの順序
        }
        public void IIC_SSD1306_Connect(FTDI myFtdiDevice)
        {
            //IICでSSD1306と通信するときの順序
            /*
               ****initialize****
               IIC_SetClock
               ResetPins
               IIC_StartCondition
               IIC_DownClock
               IIC_SetSA0
               IIC_DownClock
               IIC_SetAck

               ****control bytes send****
               IIC_DownClock
               IIC_SetControlBytes
               IIC_SetAck

               ****data bytes send****
               IIC_DownClock
               (ex)IIC_OnlyDisplayOn...databytesだけ別コンポーネント？
               IIC_SetAck

               ****stopCondition****
               IIC_SetStopCondition
             */
            List<byte> code = new List<byte>();
            IIC_SSD1306_Initialize(myFtdiDevice,code,slaveAddress);
        }


        public void IIC_SSD1306_Initialize(FTDI myFtdiDevice, List<byte> code, byte slaveAddress)
        {
            IIC_SetClock(code);
            ResetPins(code);
            IIC_StartCondition(code);
            IIC_DownClock(code);
            IIC_SetSA0(code,slaveAddress);
            IIC_DownClock(code);
            IIC_SetAck(code);
            Write_Code(code, myFtdiDevice);
        }

        public void IIC_SSD1306_SendControlBytes(FTDI myFtdiDevice, List<byte> code)
        {
            IIC_DownClock(code);
            IIC_SetControlBytes(code);
            IIC_SetAck(code);
            Write_Code(code, myFtdiDevice);
        }

        public void IIC_SSD1306_SendDataBytes(FTDI myFtdiDevice, List<byte> code)
        {
            IIC_DownClock(code);
            IIC_OnlyDisplayOn(code);
            IIC_SetAck(code);
            Write_Code(code, myFtdiDevice);
        }

        public void IIC_SSD1306_SendStopCondition(FTDI myFtdiDevice, List<byte> code)
        {
            IIC_SetStopCondition(code);
            Write_Code(code, myFtdiDevice);
        }

        public void IIC_SetClock(List<byte> code)//8C...IIC通信には必須 86...clockの速度の調節
        {
            code.AddRange(new byte[] { 0x8C, 0x86, 0x0E, 0x00 });//400kbits/1cycleに設定
        }

        public void ResetPins(List<byte> code)//pinをすべてhighにする
        {
            //初期化 scl,sdaがhigh時 
            //0x80...output GPIO pin is lowbyte not output databytes
            //                         hex   value H/L    direction I/O     
            code.AddRange(new byte[] { 0x80, 0b11111111, 0b11111011 });
        }

        public void IIC_StartCondition(List<byte> code)//start conditionを定義する
        {
            //scl=high,sda=low  
            code.AddRange(new byte[] { 0x80, 0b11111101, 0b11111011 });
        }

        public void IIC_DownClock(List<byte> code)//clockを落とす
        {
            code.AddRange(new byte[] { 0x80, 0b11111100, 0b11111011 });
        }

        public void IIC_SetSA0(List<byte> code, byte slaveAddress)//SA0を送る
        {
            //sa0(スレーブアドレス) + R/W#（read/write）を送る
            //sa0= 0111 101*          R/W#=0 =>01111010=>アドレスはFTDIにとっては0x7A　スレーブにとっては0x3D
            //sa0= 0111 100* (今回は) R/W#=0 =>01111000=>送るデータは0x78　スレーブにとっては0x3C
            //(0x3C << 1) | 0b0
            code.AddRange(new byte[] { 0x11, 0x00, 0x00, slaveAddress });//-VE write databyte output
        }

        public void IIC_SetControlBytes(List<byte> code)//controlBytesを送る
        {
            //co=0(dataのみ送る) + dc=0(command) +controlbyte=000000 =1000 0000 =>0x80
            code.AddRange(new byte[] { 0x11, 0x00, 0x00, 0x00 });
        }

        public void IIC_SetAck(List<byte> code)//ackを送る
        {
            //data output by recerver for ack signal
            code.AddRange(new byte[] { 0x22, 0x00 });//22...+VE data in bits
        }

        public void IIC_SetStopCondition(List<byte> code)
        {
            //先にsdaを上げる             //sda->scl
            code.AddRange(new byte[] { 0x80, 0b11111101, 0b11111011, 0x80, 0b11111111, 0b11111011 });
        }

        public void Write_Code(List<byte> code, FTDI myFtdiDevice)
        {
            uint written = 0;//実際に書かれたbyte数
            byte[] arrayCode = code.ToArray();//List<byte>->byte[]
            var ftStatus = myFtdiDevice.Write(arrayCode, arrayCode.Length, ref written);//データを送る　電位が変わる
            Debug.WriteLine($"{written}/{arrayCode.Length}");// 実際に書かれたbyte数/送りたいバイト数
            if (ftStatus != FTDI.FT_STATUS.FT_OK || written != arrayCode.Length)
            {
                Debug.WriteLine("Write_Code:Error : number of bytes error");
            }
        }

        //8D->14->AF
        public void IIC_OnlyDisplayOn(List<byte> code)//displayの電源をつけるだけのbyte配列を返す
        {
            byte[] commandForOnlyDisplayOn = new byte[] { 0x8D, 0x14, 0xAF };
            for (int i = 0; i < 3; i++)
            {
                code.AddRange(new byte[] { 0x11, 0x00, 0x00, commandForOnlyDisplayOn[i],
                                  ,0x80, 0b11111100, 0b11111011,//write
                                  ,0x22, 0x00,//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
        }
    }
}
