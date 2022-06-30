using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232H_WinFormApp
{
    public partial class SSD1306
    {
        //8D->14->AF
        public List<byte> OnlyDisplayOn()//displayの電源をつけるだけのbyte配列を返す
        {
            byte[] commandForOnlyDisplayOn = new byte[] { 0x8D, 0x14, 0xAF };
            List<byte> databytes = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                databytes.AddRange(new byte[] { 0x11, 0x00, 0x00, commandForOnlyDisplayOn[i]
                                  ,0x80, 0b11111100, 0b11111011//write
                                  ,0x22, 0x00//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
            return databytes;
        }
        //8D->14->AE
        public List<byte> OnlyDisplayOff()//displayの電源を消すだけのbyte配列を返す
        {
            byte[] commandForOnlyDisplayOn = new byte[] { 0x8D, 0x14, 0xAE };
            List<byte> databytes = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                databytes.AddRange(new byte[] { 0x11, 0x00, 0x00, commandForOnlyDisplayOn[i]
                                  ,0x80, 0b11111100, 0b11111011//write
                                  ,0x22, 0x00//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
            return databytes;
        }
        public List<byte> DisplaySelectedPicture()//displayに選択した画像を表示する
        {
            byte[] commandForOnlyDisplayOn = new byte[] { 0x8D, 0x14, 0xAF };
            List<byte> databytes = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                databytes.AddRange(new byte[] { 0x11, 0x00, 0x00, commandForOnlyDisplayOn[i]
                                  ,0x80, 0b11111100, 0b11111011//write
                                  ,0x22, 0x00//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
            return databytes;
        }
        public List<byte> DisplayWriteWords()//displayにGUIで入力した文字列を表示する
        {
            byte[] commandForOnlyDisplayOn = new byte[] { 0x8D, 0x14, 0xAF };
            List<byte> databytes = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                databytes.AddRange(new byte[] { 0x11, 0x00, 0x00, commandForOnlyDisplayOn[i]
                                  ,0x80, 0b11111100, 0b11111011//write
                                  ,0x22, 0x00//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
            return databytes;
        }
        public List<byte> DisplayBME280Data()//BME280で取得した値を表示する
        {
            byte[] commandForOnlyDisplayOn = new byte[] { 0x8D, 0x14, 0xAF };
            List<byte> databytes = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                databytes.AddRange(new byte[] { 0x11, 0x00, 0x00, commandForOnlyDisplayOn[i]
                                  ,0x80, 0b11111100, 0b11111011//write
                                  ,0x22, 0x00//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
            return databytes;
        }
    }

}
