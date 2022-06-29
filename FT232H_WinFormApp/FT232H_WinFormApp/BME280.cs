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


    public class BME280
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

        public byte BME280_GetSlaveAddress(ref byte slaveaddress)
        {
            return slaveaddress;
        }

        public void SPI_BME280_Connect()
        {

        }
        public void IIC_BME280_Connect()
        {

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

