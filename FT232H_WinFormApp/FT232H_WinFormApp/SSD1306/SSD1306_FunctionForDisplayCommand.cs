using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232H_WinFormApp
{
    //Displayに表示する手順
    /*
    set max ratio A8h,3Fh
    set display offset D3h,00h
    set display startline 40h
    set segment remap A0h/A1h
    set com output scan direction c0h/c8h
    set com pins hardware configuration DAh,02
    set contrast control 81h,7fh
    disable entire display on A4h 
    set normal display A6h
    set display clock divide And ratio oscillator frequency D5h,80h
    set display on/off 8D->14->AF/E

    first           page       remap
    page0(com0-7)   page0  page0(com63-56)
    page1(com8-15)  page1  page1(com55-48)
    page2(com16-23) page2  page2(com47-40)
    page3(com24-31) page3  page3(com39-32)
    page4(com32-39) page4  page4(com31-24)
    page5(com40-47) page5  page5(com23-16)
    page6(com48-55) page6  page6(com15-8)
    page7(com56-63) page7  page7(com7-0)

    seg0,seg1,...                   seg127
(re)seg127,seg126,...               seg0

     */
    public partial class SSD1306
    {
        /// ////////////////       共通            /////////////////////////////////
        
        public List<byte> databytes;//送るコマンドとデータbytes
 
        public void databytesAddRange(byte[] dataForSend)//ssd1306でII2通信時
        {
            for (int i = 0; i < dataForSend.Length; i++)
            {
                databytes.AddRange(new byte[] { 0x11, 0x00, 0x00, dataForSend[i]//set multiple ratio
                                  ,0x80, 0b11111100, 0b11111011//write
                                  ,0x22, 0x00//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
        }

        public byte SetAddress(byte b)
        {
            return b;
        }
        
        /// ////////////////switchで選択される関数/////////////////////////////////
        public List<byte> OnlyDisplayOnOff(byte b)//displayの電源を つける/消す だけのbyte配列を返す
        {
            databytes = new List<byte>();
            SetDisplayOnOff(b);
            return databytes;
        }
        public List<byte> DisplaySelectedPicture()//displayに選択した画像を表示する
        {
            databytes = new List<byte>();
            SetMultiplexRatio();
            SetDisplayOffset();
            SetDisplayStartline(0x40);
            SetSegmentRemap(0xA0);
            SetComOutputScanDirection(0xC0);
            SetComPinsHardwareConfiguration(0x0A);
            SetContrastControl(0x7F);
            DisableEntireDisplayOn(0xA4);
            SetNormalDisplay(0xA6);
            SetDisplayClockDivideAndRatioOscillatorFrequency(0x80);
            SetDisplayOnOff(0xAF);
            SetDisplay();
            return databytes;
        }
        public List<byte> DisplayWriteWords()//displayにGUIで入力した文字列を表示する
        {
            databytes = new List<byte>();
            return databytes;
        }
        public List<byte> DisplayBME280Data()//BME280で取得した値を表示する
        {
            databytes = new List<byte>();
            return databytes;
        }

        /// ////////////////switchで選択される関数で使う部品/////////////////////////////////

        // A8h,3Fh
        //128*64 display seg0~127 com0~64
        public void SetMultiplexRatio()//比率の設定
        {
            //multiplex modeは16~63まで　defaultは63
            //出力画面が一致しているCOMsignal(COM0~63)に応じて変わる
            //10****** A[5:0]の変化で16MUX~64MUXを表現
            //MUX=...A[5:0]+1 
            //b10111111をおくる =>0xBF
            byte[] dataForSend = new byte[] {0xA8,0xBF};
            databytesAddRange(dataForSend);
        }
        public void SetDisplayOffset()//offset..起点
        {
            //D3..command
            //2byte目にdisplay start lineを記述(com0 ~ com63)のいずれか
            //**A5 A4 A3 A2 A1 A0で0~63までの起点を設定
            byte[] dataForSend = new byte[] { 0xD3,0b000000};
            databytesAddRange(dataForSend);
        }
        public void SetDisplayStartline(byte b)//書き込み開始ライン
        {
            //0 1 X5 X4 X3 X2 X1 X0
            //0x40なら0からスタート 0=>COM0
            byte[] dataForSend = new byte[] { b };
            databytesAddRange(dataForSend);
        }

        public void SetSegmentRemap(byte b)
        {
            //A0..column address 0 =seg0
            //A1..column address 127 =seg0
            byte[] dataForSend = new byte[] {b};
            databytesAddRange(dataForSend);
        }

        public void SetComOutputScanDirection(byte b)
        {
            //1 1 0 0 X3 0 0 0
            //C0..normal mode X3..0
            //C8..remapped mode X3..1
           
            byte[] dataForSend = new byte[] {b};
            databytesAddRange(dataForSend);
        }

        public void SetComPinsHardwareConfiguration(byte b)
        {
            //pinを登録していく方向
            //DA..mode command
            //0 0 A5 A4 0 0 1 0
            //A4..0=sequential com pin configuration 
            //A4..1=alternative com pin configuration
            //A5..0=reset disable remap Left/Right remap
            //A5..1=enable remap Left/Right remap
            //00=0x0A 01=0x1A 10=0xAA 11=0xBA

            //c0/c8 ,A4,A5でpinの構成は8通り作れる
            byte[] dataForSend = new byte[] { 0xDA,b};
            databytesAddRange(dataForSend);
        }

        public void SetContrastControl(byte b)
        {
            //81..set contrast command
            //second byte..reset=7f
            //1 ~ 256までコントラストの度合を調節
            byte[] dataForSend = new byte[] { 0x81,b };
            databytesAddRange(dataForSend);
        }

        public void DisableEntireDisplayOn(byte b)
        {
            //A4..entire display off
            //A5..entire display on
            byte[] dataForSend = new byte[] {b};
            databytesAddRange(dataForSend);
        }

        public void SetNormalDisplay(byte b)
        {
            //   set normal display A6h
            //   set inverse display A7h
            byte[] dataForSend = new byte[] { b };
            databytesAddRange(dataForSend);
        }

        public void SetDisplayClockDivideAndRatioOscillatorFrequency(byte b)
        {
            //A[3:0] ..define the divide ratio of the display clock 
            //0000b=1 1 to 16
            //A[7:4]..set the ocillater freaquency 
            //1000b=reset 16paterns
            byte[] dataForSend = new byte[] { 0xD5,0x80};
            databytesAddRange(dataForSend);

        }
        public void SetDisplayOnOff(byte b)
        {
            //8D->14->AFで点灯　8D->14->AEで消灯
            byte[] dataForSend = new byte[] { 0x8D, 0x14,　b };
            databytesAddRange(dataForSend);
        }

        public void SetDisplay()
        {
            //8D->14->AFで点灯　8D->14->AEで消灯
            byte[] dataForSend = new byte[] { 0x81,0xFF};
            databytesAddRange(dataForSend);
        }


    }

}
