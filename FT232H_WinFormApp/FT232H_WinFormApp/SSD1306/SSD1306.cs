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
    public partial class SSD1306:FTDI_CommonFunction
    {
        public byte slaveAddress;
        public object DisplayMode;

        public SSD1306(FTDI ft, byte sa) {
            if(ft != null)
            {
                myFtdiDevice = ft;
                slaveAddress = sa;
            }
        }

        public byte SSD1306_SetSlaveAddress()
        {
            return slaveAddress;
        }
        public void SPI_SSD1306_Connect(FTDI myFtdiDevice)
        {
            //SPIでSSD1306と通信するときの順序
        }
        public void IIC_SSD1306_Connect(object mode)
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
            //List<byte> addData = new List<byte>();//displaymodeを複数


            List<byte> code = new List<byte>();
            IIC_Initialize(code);
            IIC_SendControlBytes(code);
            IIC_SendDataBytes(code);
            IIC_SendStopCondition(code);
            Write_Code(code);
        }


        /////////SSD1306でIIC通信をする際に使うプロパティ ///////////

        public void IIC_Initialize(List<byte> op)
        {
            IIC_SetClock(op);
            ResetPins(op);
            IIC_StartCondition(op);
            DownClock(op);
            IIC_SetSA0(op, slaveAddress);
            DownClock(op);
            IIC_SetAck(op);
        }

        public void IIC_SendControlBytes(List<byte> op)
        {
            DownClock(op);
            IIC_SetControlBytes(op);
            IIC_SetAck(op);
        }

        public void IIC_SendDataBytes(List<byte> op)
        {
            DownClock(op);
            IIC_SetDataBytes(op, SwitchCommandForSSD1306Display(DisplayMode));
            IIC_SetAck(op);
        }

        public void IIC_SendStopCondition(List<byte> op)
        {
            DownClock(op);
            IIC_SetStopCondition(op);
        }
        public void IIC_SetClock(List<byte> op)//8C...IIC通信には必須 86...clockの速度の調節
        {
            op.AddRange(new byte[] { 0x8C, 0x86, 0x0E, 0x00 });//400kbits/1cycleに設定
        }
        public void IIC_StartCondition(List<byte> op)//start conditionを定義する
        {
            //scl=high,sda=low  
            op.AddRange(new byte[] { 0x80, 0b11111101, 0b11111011 });
        }

        public void IIC_SetSA0(List<byte> op, byte slaveAddress)//SA0を送る
        {
            //sa0(スレーブアドレス) + R/W#（read/write）を送る
            //sa0= 0111 101*          R/W#=0 =>01111010=>アドレスはFTDIにとっては0x7A　スレーブにとっては0x3D
            //sa0= 0111 100* (今回は) R/W#=0 =>01111000=>送るデータは0x78　スレーブにとっては0x3C
            //(0x3C << 1) | 0b0
            op.AddRange(new byte[] { 0x11, 0x00, 0x00, slaveAddress });//-VE write databyte output
        }

        public void IIC_SetControlBytes(List<byte> op)//controlBytesを送る
        {
            //co=0(dataのみ送る) + dc=0(command) +controlbyte=000000 =1000 0000 =>0x80
            op.AddRange(new byte[] { 0x11, 0x00, 0x00, 0x00 });
        }
        public void IIC_SetDataBytes(List<byte> op, List<byte>bytes)//dataBytesを送る
        {
            bytes.ToArray();
            op.AddRange(bytes);
        }

        public void IIC_SetAck(List<byte> op)//ackを送る
        {
            //data output by recerver for ack signal
            op.AddRange(new byte[] { 0x22, 0x00 });//22...+VE data in bits
        }

        public void IIC_SetStopCondition(List<byte> op)
        {
            //先にsdaを上げる             //sda->scl
            op.AddRange(new byte[] { 0x80, 0b11111101, 0b11111011, 0x80, 0b11111111, 0b11111011 });
        }

        public List<byte> SwitchCommandForSSD1306Display(object DisplayMode)
        {
            List<byte> bytes = new List<byte>();
            switch (DisplayMode)
            {
                case "OnlyDisplayOn":
                    bytes = OnlyDisplayOnOff(0xAF);
                    break;
                case "OnlyDisplayOff":
                    bytes = OnlyDisplayOnOff(0xAE);
                    break;
                case "DisplaySelectedPicture":
                    bytes = DisplaySelectedPicture();
                    break;
                case "DisplayWriteWords":
                    bytes = DisplayWriteWords();
                    break;
                case "DisplayBME280 Data":
                    bytes = DisplayBME280Data();
                    break;
                default:
                    MessageBox.Show("error : display mode");
                    break;
            }
            return bytes;
        }

    }
}
