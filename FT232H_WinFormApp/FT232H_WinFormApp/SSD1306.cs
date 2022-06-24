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
        public static bool PictureSettiing = false;
  
        //表示したい画像の選択
        public bool PictureDisplay()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "picture";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
            }
            //dialog のokを押した後でbitmap->表示
            //form1のpicturebox1に表示
            Bitmap src = new Bitmap(filePath);
            //Bitmap dst = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);//RGB24bitにする
            //Graphics gfx = Graphics.FromImage(dst);

            //gfx.DrawImage(src, 0, 0);
            //dst.Save("test.bmp", ImageFormat.Bmp);

            //gfx.Dispose();
            //src.Dispose();
            //dst.Dispose();
            PictureSettiing = true;
            return PictureSettiing;
        }

        public void IIC_Connect()//I2C Connectボタンを押すと呼び出される
        {
            if(PictureDisplay())
            {
                //画像を表示
                MessageBox.Show("PictureDisplay():true");

                //通信開始

                //i2cはsda(serial data)とscl(serial clock)だけで通信ができる
                //実際にはVccも必要なのでVcc,GND,SDA,SCLの４本でやり取りする
                //デバイスの複数接続が可能なためI2Cアドレスが個別に振られている

                /*
                ft232h側..
                adbus0->vcc
                adbus1->NC
                adbus2->cl
                adbus3->sda
                */

                /*
                power ON時の推奨順序
                VddをON
                ->RES#ピンを、時間幅が少なくとも3us以上の設定でlowにする
                ->さらに最低3us待ったあとでVccをOnにする
                ->この状態にしてから画面をOnにするためのコマンドAFhを送る。このあとSEG/COMは100msにONになる。
                 */

                byte[] code;
                  //送るbit配列
                  //1
                  //初期化 scl,sdaがhigh時 
                  //0x80...output GPIO pin is lowbyte not output databytes
                  //                  hex   value H/L    direction I/O     
                  code = new byte[] { 0x80, 0b11111111, 0b11111011};
                  //myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

                  //2
                  //scl=high,sda=low  start conditionを定義する
                  code = new byte[] { 0x80, 0b11111101, 0b11111011};
                //myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

                //3
                //sa0(スレーブアドレス) + R/W#（read/write）を送る
                //sa0= 0111 101*          R/W#=0 =>01111010=>アドレスはFTDIにとっては0x7A　スレーブにとっては0x3D
                //sa0= 0111 100* (今回は) R/W#=0 =>01111000=>送るデータは0x78　スレーブにとっては0x3C
                //0x10 databytes output when +VE
                //0x7A=0111101 + 0  上位7bitをスレーブはデータとして受取り,最上位に0がつくから0x3C
                code = new byte[] { 0x11, 0x00, 0x00, (0x3C << 1) | 0b0 };//-VE write databyte output

                //0x10...data output when out click edge "+VE" (no read)
                //code = new byte[] { 0x11, 0b01111000, 0b11111011 };
                //myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる

                //4
                //data output by recerver for ack signal
                byte[] ackCode = new byte[] { 0x33, 0x00, 0x00 };//-VE read bits 
                byte[] newCode = new byte[code.Length + ackCode.Length];
                code.CopyTo(newCode, 0);
                ackCode.CopyTo(newCode, code.Length);

                //5
                //Co(continuation bit) + D/C#(data/command select bit) + ControlByte + ACK + DataByte + ACKを送る
                //Co=0のときはデータバイトしか送らない
                //0x12..only ＋VE bits output
                //0x36..in -VE out +VE bits output

                //////////////////////////////////////////////////////////////
                //画面に表示するまでの処理順序
                //Set Multiplex Ratio A8h,3Fh //multiplex=多重化　ratio=比
                //A8h:D/C Hex D7 D6 D5 D4 D3 D2 D1 D0
                //    0   A8  1  0  1  0  1  0  0  0         

                //Set Display OffSet D3h,00h //offset=先頭からの距離
                //D3h:D/C Hex D7 D6 D5 D4 D3 D2 D1 D0
                //    0   D3  1  1  0  1  0  0  1  1    

                //Set Display StartLine 40h 
                //40h:D/C Hex D7 D6 D5 D4 D3 D2 D1 D0
                //    0   D3  0  1  x5 x4 x3 x2 x1 x0 

                //Set Segment re-map A0h/A1h 
                //A0/A1h:D/C Hex    D7 D6 D5 D4 D3 D2 D1 D0
                //       0   A0/A1  1  0  1  0  0  0  0  x[0]
                //A0h,x[0]=0b..seg0にカラムアドレスの0が入る(RESET)
                //A1h,x[0]=1b..seg0にカラムアドレスの127が入る

                //Set COM Output ScanDirection C0h/C8h
                //C0/C8h:D/C Hex    D7 D6 D5 D4 D3 D2 D1 D0
                //       0   C0/C8  1  1  0  0  X3  0  0 0
                //c0h: x[3]=0b,normalmode scan(RESET)scan from COM0 to COM[N-1]
                //c8h: x[3]=1b,normalmode scan(RESET)scan from COM[N-1] to COM0

                //Set COM Pins hardware configuration DAh,02
                //DAh: D/C Hex D7 D6 D5 D4 D3 D2 D1 D0
                //     0   DA  1  1  0  1  1  0  1  0

                //Set Constrast Control 81h,7Fh
                //81h:D/C Hex D7 D6 D5 D4 D3 D2 D1 D0
                //    0   81  1  0  0  0  0  0  0  1

                //Disable Entire Display On A4h
                //A4/A5h:D/C Hex    D7 D6 D5 D4 D3 D2 D1 D0
                //       0   A4/A5  1  0  1  0  0  1  0  x[0]
                //A4h:x[0]=0b:display on (RESET)
                //A5h:x[0]=1b:display on entiredisplay (ignore RAM content)


                //Set Normal/inverse Display A6h/A7h
                //A6/A7h:D/C Hex    D7 D6 D5 D4 D3 D2 D1 D0
                //       0   A6/A7  1  0  1  0  0  1  1  x[0]
                //A6h:x[0]=0b:display off 
                //A6h:x[0]=1b:display on 
                //A7h:x[0]=0b:display on 
                //A7h:x[0]=1b:display off 


                //Set Oscilater Frequency(発振周波数) D5h,80h
                //発振周波数＝振動子を発振回路に組み込んで動作させている状態での実際の周波数
                //D5/A[7:0]h:D/C Hex    D7 D6 D5 D4 D3 D2 D1 D0
                //           0   D5     1  1  0  1  0  1  0  1
                //           0   A[7:0] A7 A6 A5 A4 A3 A2 A1 A0
                //Enable charge pump regulator 8Dh,14h


                //Display On AFh
                //AFh:D/C Hex D7 D6 D5 D4 D3 D2 D1 D0
                //    0   AF  1  0  1  0  1  1  1  x[0]
                //x[0]=1b:display on in normal mode
                //x[0]=0b:display off in normal mode
                //////////////////////////////////////////////////////////////


            }
            else
            {
                //error
                MessageBox.Show("PictureDisplay() => false");
            }
        }

    }
}
