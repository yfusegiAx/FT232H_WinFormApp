using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTD2XX_NET;

namespace FT232H_WinFormApp
{
    public class FTDI_CommonFunction//FTDI製品を用いた開発にスレーブが異なる場合も共通して使える部品をまとめた.継承して使用する
    {
        public FTDI myFtdiDevice;

        public FTDI_CommonFunction() {
            myFtdiDevice = new FTDI();
        }

        public void Write_Code(List<byte> code)//0x80 only write 
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

        public void ResetPins(List<byte> op)//pinをすべてhighにする
        {
            //初期化 scl,sdaがhigh時 
            //0x80...output GPIO pin is lowbyte not output databytes
            //                         hex   value H/L    direction I/O     
            op.AddRange(new byte[] { 0x80, 0b11111111, 0b11111011 });
        }
        public void SetClock(List<byte> op)//86...clockの速度の調節
        {
            op.AddRange(new byte[] { 0x86, 0x0E, 0x00 });//400kbits/1cycleに設定
        }
        public void DownClock(List<byte> op)//clockを落とす
        {
            op.AddRange(new byte[] { 0x80, 0b11111100, 0b11111011 });
        }
       
    }
}
