using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTD2XX_NET;

namespace FT232H_WinFormApp
{
    public class FTDI_CommonFunction//FTDI製品を用いた開発に共通して使える部品をまとめた
    {
        public void Write_Code(List<byte> code,FTDI myFtdiDevice)//0x80 write 
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
    }
}
