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
    set display on/off 8D->14->AF/E ->light on

    Set Memory Addressing Mode 20h ->B0h~B7h 00h~07h
    Set Column Address         21h
    Set Page Address           22h

    ***option***
    Horizontal Scroll Setup 26h/27
    Continuous Vertical and Horizontal Scroll Setup 29h/2Ah
    Activate/Deactivate Scroll            2E/2Fh
    Set Vertical Scroll Area   A3h 

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
    public partial class SSD1306 : FTDI_CommonFunction
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
            SetMemoryAddressingMode();
            SetColumnAddress();


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

        
        public void SetMemoryAddressingMode()
        {
            //20h
            /*ssd1306には3種類のアドレスモードがある
              1:page addressing mode 
              2:horizon addressing mode
              3:vertical addressing mode
             このうち一つを選ぶ
             COL=graphic display data RAM column
             B0h~B7hコマンドで目的の表示位置のpage start addressを設定
             00h~07hコマンドで下位ビット->10h~1Fhで上位ビット、
             書き込みを開始するカラムのアドレス（ページアドレス）を設定
                         */
            byte[] dataForSend = new byte[] { 0x20,0xB2 };
            databytesAddRange(dataForSend);
        }

        public void SetColumnAddress()
        {
            //21h
            /*
             3byte
            カラムのstart address,end address,現在のR/W address
            表示するRAMデータのR/Wの設定
            20ｈの続きの記述?
            カラムの最後まで行ったときにまた最初に戻れる？
             */
            byte[] dataForSend = new byte[] {0x00,0xFF,0x21};
            databytesAddRange(dataForSend);
        }

        public void SetPageAddress()
        {
            //22h
            byte[] dataForSend = new byte[] { 0x00, 0xFF, 0x22 };
            databytesAddRange(dataForSend);
        }
        //**** option ****
        public void HorizontalScrollSetup()
        {
            //26h/27
            //上昇するスクローリングのパラメータを構成する複数の連続したバイトで構成される
            //スクローリングのstartpage,stoppage,scrolling speedを決めることが出来る
            //hirizontal scroll はright/left
            byte[] dataForSend = new byte[] {0x26, 0x00, 0x00, 0x00 };
            databytesAddRange(dataForSend);
        }
        public void ContinuousVerticalAndHorizontalScrollSetup()
        {
            //29h/2Ah
            /*６bytesで構成される
            継続的に垂直方向に変化するパラメータ、スクローリング開始ページ、終了ページ、スクロールスピード、
            垂直スクローリングの開始地点を決める
            すべてのバイト配列を合わせるとhorizontalかつverticalなスクローリングができる
            E[5:0]を０にするとhirizontalスクローリングだけ可能になる
            */

        }

        public void ActivateOrDeactivateScroll(byte b)
        {
            //2Fh => on 2E => off
            byte[] dataForSend = new byte[] {b};
            databytesAddRange(dataForSend);
        }
        public void SetVerticalScrollArea()
        {
            //A3h
            //連続した３bytesで垂直方向のスクローリングする範囲を決める
            byte[] dataForSend = new byte[] {0x00,0x00,0x00};
            databytesAddRange(dataForSend);
        }
        public void SetDataForDisplayOn()
        {
            //oleに表示する内容について記述
        }

    }

}
/*
 private void print_oled2(List<byte> hfData, uint LineFrom, uint LineTo)
        {
            var rangeData = new List<byte>();
            rangeData.Add(0x00);//control byte
            tool.AddListDataData(rangeData, 0x200021007F);
            I2C_Send(rangeData);

            rangeData = new List<byte>();
            rangeData.Add(0x00);//control byte
            tool.AddListDataData(rangeData, 0x220000u | (LineFrom << 8) | LineTo);//upper font 
            I2C_Send(rangeData);

            var opFontData = new List<byte>();
            opFontData.Add(0x40);//control byte
            opFontData.AddRange(hfData);// lower font()
            I2C_Send(opFontData);
        }
        public void print_oled(List<byte> hfData, uint LineFrom, uint LineTo, uint from, uint to)
        {
            var rangeData = new List<byte>();
            rangeData.Add(0x00);//control byte
            tool.AddListDataData(rangeData, 0x2000210000u| (from << 8) | to);
            I2C_Send(rangeData);

            rangeData = new List<byte>();
            rangeData.Add(0x00);//control byte
            tool.AddListDataData(rangeData, 0x220000u | (LineFrom << 8) | LineTo);//upper font 
            I2C_Send(rangeData);

            var opFontData = new List<byte>();
            opFontData.Add(0x40);//control byte
            opFontData.AddRange(hfData);// lower font()
            I2C_Send(opFontData);
        }
         /// <summary>
        /// String からフォント画像作成、出力。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="LineRow"></param>
        /// <returns></returns>
        public Bitmap print_oledBmp(string text, uint LineRow)
        {
            var bfont = BmpFont(text);
            var fData = BitmapToRawBit(bfont);

            var listfData = fData.ToList();
            listfData.Insert(0, 0x40);//コントロールバイトの追加。
            print_oledfontOneLineLeft(listfData, LineRow,0);
            return bfont;
        }
        // <summary>
        /// フォント画像(主に128x8px)を2値化して、byte配列に変換する。
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public byte[] BitmapToRawBit(Bitmap image)
        {
            var thsByte = new byte[(image.Width * image.Height / 8)];
            for (int y = 0; y < image.Height; y++)
            {
                int thpos = 0;
                int byteH = (y / 8);
                Color color;
                for (int x = 0; x < image.Width; x++)
                {
                    thpos = (int)(byteH * image.Width + x);
                    color = image.GetPixel(x, y);
                    thsByte[thpos] += (byte)(color.R > thres ? 0x01 << (y % 8) : 0x00);//B
                    //Console.WriteLine($"{x},{y},{0x01 << (y % 8):x2}");
                }
            }
            return thsByte;
        }

 */