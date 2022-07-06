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
    using BME280_S32_t = System.Int32;//int型32bitのユーザー定義型を宣言


    public class BME280:FTDI_CommonFunction
    {
        BME280_S32_t t_fine;
        double dig_T1 = 0.0;
        double dig_T2 = 0.0;
        double dig_T3 = 0.0;
        double dig_P1 = 0.0;
        double dig_P2 = 0.0;
        double dig_P3 = 0.0;
        double dig_P4 = 0.0;
        double dig_P5 = 0.0;
        double dig_P6 = 0.0;
        double dig_P7 = 0.0;
        double dig_P8 = 0.0;
        double dig_P9 = 0.0;
        double dig_H1 = 0.0;
        double dig_H2 = 0.0;
        double dig_H3 = 0.0;
        double dig_H4 = 0.0;
        double dig_H5 = 0.0;
        double dig_H6 = 0.0;

        public double Temprature;
        public double Humidity;
        public double Pressure;
        public byte ID;
        public uint readOnlyBufNum = 0;//読み込み用バッファ

        public byte BME280_GetSlaveAddress()
        {
            return 0;
        }
        //読み込み順は温度ー＞圧力ー＞湿度
        
        public void SPI_BME280_Connect()
        {
            List<byte> code = new List<byte>();
            List<byte> checkbytes = new List<byte>();
            byte[] codebytes;
            byte[] readData;
            //code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7から1を送る
            uint written = 0;
            byte sendData = 0x88;//送るデータ
            uint readOnlyBufNum = 0;//読み込み用バッファ
            code.AddRange( new byte[] { 0x86, 0b00000100, 0b11111011 });//pinをリセット..通信前に状態をリセットできる 0x80...output lowbyte
            code.AddRange( new byte[] { 0x80, 0b11111111, 0b11111011 });//pinをリセット..通信前に状態をリセットできる 0x80...output lowbyte
            code.AddRange( new byte[] { 0x80, 0b11110111, 0b11111011 });//adbus2をlowにすることで通信したいスレーブを選択できるようになる
            code.AddRange( new byte[] { 0x80, 0b11110110, 0b11111011 });//adbus0=0にする クロックを送るため
            code.AddRange( new byte[] { 0x11, 0x01, 0x00, 0x72, 0x01 });//0(write) F2(11110010) value(00000001) :over sampling x1==>01110010 00000001=>0x72に0x01を書き込みをするバイト配列
            Write_Code(code);

            checkbytes.AddRange(new byte[] { 0x11, 0x01, 0x00, 0x74, 0x27 });//0(書き込みモード) F4に　valueを書き込みをするバイト配列
            codebytes = checkbytes.ToArray();
            myFtdiDevice.Write(codebytes, codebytes.Length, ref readOnlyBufNum);//クロック立ち上がる 命令を書き込み　

            code.AddRange(new byte[] { 0x11, 0x00, 0x00, sendData });//data output buffer :+VE時にクロックを送る、1byteのデータを送る、sendDataというデータを送るためのbyte配列
            Write_Code(code);

            checkbytes.AddRange(new byte[] { 0x20, 0x79, 0x00 });//data input buffer :-VE時にクロックを送る、この時点ではクロックは出ていない 0x0078+1読み取る設定
            codebytes=checkbytes.ToArray();
            myFtdiDevice.Write(codebytes, codebytes.Length, ref readOnlyBufNum);//クロック立ち上がる 命令を書き込み　

            code.AddRange(new byte[] { 0x80, 0b11111110, 0b11111011 });//csが１になる　スレーブとのやり取りの終了
            code.AddRange(new byte[] { 0x80, 0b11111111, 0b11111011 });//reset のための配列
            Write_Code(code);

            Thread.Sleep(100);//FT232Hが反応するのに2ミリ秒かかるため待ってあげる　100byteくらいが上限
            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum) == FTDI.FT_STATUS.FT_OK)
            {
                Debug.WriteLine($"readonlybufnum={readOnlyBufNum}");
                readData = new byte[readOnlyBufNum];//読み込んだデータを格納するためのbyte配列 new byteでmallocしている
                myFtdiDevice.Read(readData, readOnlyBufNum, ref readOnlyBufNum);//ここで読み込む byte[] dataBuffer,uint numBytesToRead,ref uint numBytesRead
                                                                                //データを格納するバッファ、デバイスから要求されたバイト数、実際読み込まれるバイト数
                if (readOnlyBufNum <= 0x78)
                {
                    Debug.WriteLine($"readData = NO DATA");
                    return;
                }
                else
                {
                    Debug.WriteLine($"readData = 0x{readData[0]:X02}");
                }
            }
            else
            {
                return;
            }
            BME280_Calib(readData);//IDを返す dig..の値の初期化 0x60が返ってこないと湿度は読み取れない なぜ60が返ってくる?
            Thread.Sleep(10);//FT232Hが反応するのに2ミリ秒かかるため待ってあげる　100byteくらいが上限
            BME280_Calc(readData.Skip(0xF7 - 0x88).ToArray());//元々0x88からスタートするところを0xF7-0x88番目まで起点をスキップ
        }

        public void SPI_BME280_Connect(FTDI myFtdiDevice)
        {
            //SPIでBME280と通信するときの順序
            //SPIでBME280と通信するときの順序
            //SPI通信でBMPとやり取りする
            byte[] code, readData;
            //code = new byte[] { 0x80, 0b11111111, 0xFF };//adbus0~adbus7から1を送る
            uint written = 0;
            byte sendData = 0x88;//送るデータ
            uint readOnlyBufNum = 0;//読み込み用バッファ
            //0x80 output
            //Value     Direction 
            ///// 通信開始　　////
            code = new byte[] { 0x86, 0b00000100, 0b11111011 };//pinをリセット..通信前に状態をリセットできる 0x80...output lowbyte
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };//pinをリセット..通信前に状態をリセットできる 0x80...output lowbyte
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる
            code = new byte[] { 0x80, 0b11110111, 0b11111011 };//adbus2をlowにすることで通信したいスレーブを選択できるようになる
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる
            code = new byte[] { 0x80, 0b11110110, 0b11111011 };//adbus0=0にする クロックを送るため
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる
            //読み込み前の設定
            //code = new byte[] { 0x11, 0x01,0x00,0x60,0xB6};//BMEへのresetの命令 E0にB6を送る -VEのときdataをoutputする
            //myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる
            //設定を完了してから読み込み開始
            //湿度の設定　設定しないとスキップされる
            code = new byte[] { 0x11, 0x01, 0x00, 0x72, 0x01 };
            //0(write) F2(11110010) value(00000001) :over sampling x1==>01110010 00000001=>0x72に0x01を書き込みをするバイト配列
            myFtdiDevice.Write(code, code.Length, ref written);//データを送る　電位が変わる データ量はwriteで初めて定義する
            //温度　圧力　モードの設定
            code = new byte[] { 0x11, 0x01, 0x00, 0x74, 0x27 };//0(書き込みモード) F4に　valueを書き込みをするバイト配列
            //0 11110100 00100111==>0x74 に0x27を書き込む
            //001 001 11 ==>0x27 温度データのオーバーサンプリングx1 圧力データのオーバーサンプリングx1 通常モード:11 (スリープが00 強制が01,10)
            myFtdiDevice.Write(code, code.Length, ref readOnlyBufNum);//クロック立ち上がる 命令を書き込み　
            /////  通信　/////
            byte[] ftdiData = new byte[] { 0x11, 0x00, 0x00, sendData };//data output buffer :+VE時にクロックを送る、1byteのデータを送る、sendDataというデータを送るためのbyte配列
            //0x88というデータを送るとftdi側はデータを送っているがBMEは0x88というアドレスを読ませてほしいと認知
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック立ち上がる 命令を書き込み　
            ftdiData = new byte[] { 0x20, 0x79, 0x00 };//data input buffer :-VE時にクロックを送る、この時点ではクロックは出ていない 0x0078+1読み取る設定
            myFtdiDevice.Write(ftdiData, ftdiData.Length, ref readOnlyBufNum);//クロック下がる  読み取りをしろ　という命令
            //ここではftdiのバッファに読み取ったデータが入っている状態 まだデータは参照していない
            /////   通信終了   /////
            code = new byte[] { 0x80, 0b11111110, 0b11111011 };//csが１になる　スレーブとのやり取りの終了
            myFtdiDevice.Write(code, code.Length, ref written);//
            code = new byte[] { 0x80, 0b11111111, 0b11111011 };//reset のための配列

            Thread.Sleep(100);//FT232Hが反応するのに2ミリ秒かかるため待ってあげる　100byteくらいが上限
            if (myFtdiDevice.GetRxBytesAvailable(ref readOnlyBufNum) == FTDI.FT_STATUS.FT_OK)
            {
                Debug.WriteLine($"readonlybufnum={readOnlyBufNum}");
                readData = new byte[readOnlyBufNum];//読み込んだデータを格納するためのbyte配列 new byteでmallocしている
                myFtdiDevice.Read(readData, readOnlyBufNum, ref readOnlyBufNum);//ここで読み込む byte[] dataBuffer,uint numBytesToRead,ref uint numBytesRead
                                                                                //データを格納するバッファ、デバイスから要求されたバイト数、実際読み込まれるバイト数
                if (readOnlyBufNum <= 0x78)
                {
                    Debug.WriteLine($"readData = NO DATA");
                    return;
                }
                else
                {
                    Debug.WriteLine($"readData = 0x{readData[0]:X02}");
                }
            }
            else
            {
                return;
            }
            BME280_Calib(readData);//IDを返す dig..の値の初期化 0x60が返ってこないと湿度は読み取れない なぜ60が返ってくる?
            Thread.Sleep(10);//FT232Hが反応するのに2ミリ秒かかるため待ってあげる　100byteくらいが上限
            BME280_Calc(readData.Skip(0xF7 - 0x88).ToArray());//元々0x88からスタートするところを0xF7-0x88番目まで起点をスキップ

        }
        public void IIC_BME280_Connect(FTDI myFtdiDevice)
        {
            //bus0=scl,bus1=sda
            //IICでBME280と通信するときの順序
        }

        

        public void BME280_Calib(byte[] rawData)
        {
            ID = rawData[0xD0 - 0x88];
            //0x88から順にdig_T1[7-0],dig_T2[15-8],,と格納されているので値を正しく代入していく
            dig_T1 = ((UInt16)rawData[0x89 - 0x88]<<8)  | rawData[0x88 - 0x88];
            dig_T2 = (Int16)(rawData[0x8B - 0x88] << 8 | rawData[0x8A - 0x88]);
            dig_T3 = (Int16)(rawData[0x8D - 0x88] << 8 | rawData[0x8C - 0x88]);
            dig_P1 = (((UInt16)rawData[0x8F - 0x88] << 8)| rawData[0x8E - 0x88]);
            dig_P2 = (Int16)(((Int16)rawData[0x91 - 0x88] << 8) | rawData[0x90 - 0x88]);
            dig_P3 = (Int16)(((Int16)rawData[0x93 - 0x88] << 8) | rawData[0x92 - 0x88]);
            dig_P4 = (Int16)(rawData[0x95 - 0x88] << 8 | rawData[0x94 - 0x88]);
            dig_P5 = (Int16)(rawData[0x97 - 0x88] << 8 | rawData[0x96 - 0x88]);
            dig_P6 = (Int16)(rawData[0x99 - 0x88] << 8 | rawData[0x98 - 0x88]);
            dig_P7 = (Int16)(rawData[0x9B - 0x88] << 8 | rawData[0x9A - 0x88]);
            dig_P8 = (Int16)(rawData[0x9D - 0x88] << 8 | rawData[0x9C - 0x88]);
            dig_P9 = (Int16)(rawData[0x9F - 0x88] << 8 | rawData[0x9E - 0x88]);
            if (ID == 0x60)
            {
                dig_H1 = (Byte)rawData[0xA1 - 0x88];
                dig_H2 = ((Int16)rawData[0xE2 - 0x88] << 8) | rawData[0xE1-0x88];
                dig_H3 = (Byte)rawData[0xE3 - 0x88];
                dig_H4 = ((Int16)rawData[0xE4 - 0x88] )<<4 | (0x0f & rawData[0xE5 - 0x88]);//[0xE5][3-0]だけを足したいので上位4bitをマスク
                
                //dig_H5 = ((Int16)rawData[0xE5 - 0x88] & 0xF0)<<4 | rawData[0xE6 - 0x88];//[0xE5][7-4]だけを足したいので下位4bitをマスク
                dig_H5 = (Int16)(rawData[0xE5 - 0x88] & 0xF0)>>4 | rawData[0xE6 - 0x88]<<4;//[0xE5][7-4]だけを足したいので下位4bitをマスク
                dig_H6 = (SByte)rawData[0xE7 - 0x88];

                /*
                0x0F & 0x73  = 0x03
                0x0F & (0x73 >> 4) = 0x07
                00001111 & (0111)=>00000111
                 */
            }
            else
            {
                Debug.WriteLine($"ID is not 0x60 but 0x{ID:X02}");
            }
        }
        public void BME280_Calc(byte[] rawData)//ByteToString(readData.Skip(0x?? - 0x88).ToArray(), 3)で取得した生のデータをキャリブレーションできる形に変換
        {
            //F7~FEまでひとつなぎになっているデータをきりとる
            //readData[0xF7]~readData[0xFE]
            BME280_S32_t adc_P = ((rawData[0] <<8 | rawData[1]) <<4) | rawData[2]>>4;// rawData[0xF7] + rawData[0xF8] + rawData[0xF9];
            BME280_S32_t adc_T = ((rawData[3] << 8 | rawData[4]) << 4) | rawData[5] >> 4; // rawData[0xFA] + rawData[0xFB] + rawData[0xFC];
            BME280_S32_t adc_H = rawData[6]<<8 | rawData[7];// rawData[0xFD] + rawData[0xFE];
            Temprature = BME280_compensate_T_double(adc_T);
            Pressure = BME280_compensate_P_double(adc_P);
            Humidity = BME280_compensate_H_double(adc_H);
        }


       
        // 温度を℃の倍精度で返します。 「51.23」の出力値は、51.23℃に相当します。
        // t_fineは、グローバル値として細かい温度値を持ちます。
        double BME280_compensate_T_double(BME280_S32_t adc_T)
        {
            double var1, var2, T;
            var1 = (((double)adc_T) / 16384.0 - ((double)dig_T1) / 1024.0) * ((double)dig_T2);
            var2 = ((((double)adc_T) / 131072.0 - ((double)dig_T1) / 8192.0) *
            (((double)adc_T) / 131072.0 - ((double)dig_T1) / 8192.0)) * ((double)dig_T3);
            t_fine = (BME280_S32_t)(var1 + var2);
            T = (var1 + var2) / 5120.0;
            return T;
        }
        // 圧力（Pa）をdoubleとして返します。 「96386.2」の出力値は、96386.2Pa = 963.862hPaに相当します。
        double BME280_compensate_P_double(BME280_S32_t adc_P)
        {
            double var1, var2, p;
            var1 = ((double)t_fine / 2.0) - 64000.0;
            var2 = var1 * var1 * ((double)dig_P6) / 32768.0;
            var2 = var2 + var1 * ((double)dig_P5) * 2.0;
            var2 = (var2 / 4.0) + (((double)dig_P4) * 65536.0);
            var1 = (((double)dig_P3) * var1 * var1 / 524288.0 + ((double)dig_P2) * var1) / 524288.0;
            var1 = (1.0 + var1 / 32768.0) * ((double)dig_P1);
            if (var1 == 0.0)
            {
                return 0; // ゼロ除算による例外を避ける。
            }
            p = 1048576.0 - (double)adc_P;
            p = (p - (var2 / 4096.0)) * 6250.0 / var1;
            var1 = ((double)dig_P9) * p * p / 2147483648.0;
            var2 = p * ((double)dig_P8) / 32768.0;
            p = p + (var1 + var2 + ((double)dig_P7)) / 16.0;
            return p;
        }
        // 湿度（%RH）をdoubleとして返します。 「46.332」の出力値は、46.332%RHに相当します。
        double BME280_compensate_H_double(BME280_S32_t adc_H)
        {
            double var_H;
            var_H = (((double)t_fine) - 76800.0);
            var_H = (adc_H - (((double)dig_H4) * 64.0 + ((double)dig_H5) / 16384.0 * var_H)) *
            (((double)dig_H2) / 65536.0 * (1.0 + ((double)dig_H6) / 67108864.0 * var_H *
            (1.0 + ((double)dig_H3) / 67108864.0 * var_H)));
            var_H = var_H * (1.0 - ((double)dig_H1) * var_H / 524288.0);
            if (var_H > 100.0)
                var_H = 100.0;
            else if (var_H < 0.0)
                var_H = 0.0;
            return var_H;
        }

    }
}

