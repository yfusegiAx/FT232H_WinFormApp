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

        public void BME280_Calib(byte[] rawData)
        {
            ID = rawData[0xD0 - 0x88];
            dig_T1 = 0.0;
            dig_T2 = 0.0;
            dig_T3 = 0.0;
            dig_P1 = 0.0;
            dig_P2 = 0.0;
            dig_P3 = 0.0;
            dig_P4 = 0.0;
            dig_P5 = 0.0;
            dig_P6 = 0.0;
            dig_P7 = 0.0;
            dig_P8 = 0.0;
            dig_P9 = 0.0;
            if (ID == 0x60)
            {
                dig_H1 = 0.0;
                dig_H2 = 0.0;
                dig_H3 = 0.0;
                dig_H4 = 0.0;
                dig_H5 = 0.0;
                dig_H6 = 0.0;
            }
        }
        public void BME280_Calc(byte[] rawData)//ByteToString(readData.Skip(0x?? - 0x88).ToArray(), 3)で取得した生のデータをキャリブレーションできる形に変換
        {
            //F7~FEまでひとつなぎになっているデータをきりとる
            //readData[0xF7]~readData[0xFE]
           
            BME280_S32_t adc_T = rawData[0x02];// rawData[0xFA] + rawData[0xFB] + rawData[0xFC];
            BME280_S32_t adc_P = rawData[0x02];// rawData[0xF7] + rawData[0xF8] + rawData[0xF9];
            BME280_S32_t adc_H = rawData[0x02];// rawData[0xFD] + rawData[0xFE];
            Temprature = BME280_compensate_T_double(adc_T);
            Pressure = BME280_compensate_P_double(adc_P);
            Humidity = BME280_compensate_H_double(adc_H);
        }
       
        public double BME280_compensate_T_double(BME280_S32_t adc_T)
        {
            //adc_tは生の温度データ..BMEのアドレスのFA~FCの中身をつなぎ合わせたもの
            //dig_T1=(unsigned short) 
            //レジスタのアドレス:88h...dig_T1[7-0]   89h...dig_T1[15-8]
            //dig_T2=(signed short)
            //レジスタのアドレス:8Ah...dig_T2[7-0]   8Bh...dig_T2[15-8]
            //dig_T3=(signed short)
            //レジスタのアドレス:8Ch...dig_T3[7-0]   8Dh...dig_T3[15-8]
            double var1, var2, T;
            
            var1 = (((double)adc_T) / 16384.0 - ((double)dig_T1) / 1024.0) * ((double)dig_T2);
            var2 = ((((double)adc_T) / 131072.0 - ((double)dig_T1) / 8192.0) *
                 (((double)adc_T) / 131072.0 - ((double)dig_T1) / 8192.0)) * ((double)dig_T3);
            t_fine = (BME280_S32_t)(var1 + var2);
            T = (var1 + var2) / 5120.0;//人が理解できる温度データ
            return T;
        }

        //圧力(Pa)をdoubleとして返す「96386.2」の出力値は96386.2Pa=963.862hPa
        //adc_Pは生の圧力のデータ
        public double BME280_compensate_P_double(BME280_S32_t adc_P)
        {
            //adc_pは生の圧力データ..BMEのアドレスのFA~FCの中身をつなぎ合わせたもの
            //dig_P1=(unsigned short) 
            //レジスタのアドレス:8Eh...dig_P1[7-0]   8Fh...dig_P1[15-8]
            //dig_P2=(signed short)
            //レジスタのアドレス:90h...dig_P2[7-0]   91h...dig_P2[15-8]
            //dig_P3=(signed short)
            //レジスタのアドレス:92h...dig_P3[7-0]   93h...dig_P3[15-8]
            //dig_P4=(signed short)
            //レジスタのアドレス:94h...dig_P4[7-0]   95h...dig_P4[15-8]
            //dig_P5=(signed short)
            //レジスタのアドレス:96h...dig_P5[7-0]   97h...dig_P5[15-8]
            //dig_P6=(signed short)
            //レジスタのアドレス:98h...dig_P6[7-0]   99h...dig_P6[15-8]
            //dig_P7=(signed short)
            //レジスタのアドレス:9Ah...dig_P7[7-0]   9Bh...dig_P7[15-8]
            //dig_P8=(signed short)
            //レジスタのアドレス:9Ch...dig_P8[7-0]   9Dh...dig_P8[15-8]
            //dig_P9=(signed short)
            //レジスタのアドレス:9Eh...dig_P9[7-0]   9Fh...dig_P9[15-8]
            double var1, var2, p;
            
            var1 = ((double)t_fine / 2.0) - 64000.0;
            var2 = var1 * var1 * ((double)dig_P6) / 32768.0;
            var2 = var2 + var1 * ((double)dig_P5) * 2.0;
            var2 = (var2 / 4.0) + (((double)dig_P4) * 65536.0);
            var1 = (((double)dig_P3) * var1 * var1 / 524288.0 + ((double)dig_P2) * var1) / 524288.0;
            var1 = (1.0 + var1 / 32768.0) * ((double)dig_P1);
            if (var1 == 0.0)
            {
                return 0;//ゼロ除算
            }
            p = 1048576.0 - (double)adc_P;
            p = (p - (var2 / 4096.0)) * 6250.0 / var1;
            var1 = ((double)dig_P9) * p * p / 2147483648.0;
            var2 = p * ((double)dig_P8) / 32768.0;
            p = p + (var1 + var2 + ((double)dig_P7)) / 16.0;//人が理解できる圧力データ
            return p;
        }

        //湿度(%RH)をdoubleとして返す　「46.332」の出力値は、46.332%RHに相当
        public double BME280_compensate_H_double(BME280_S32_t adc_H)
        {
            //adc_Hは生の温度データ..BMEのアドレスのFA~FCの中身をつなぎ合わせたもの
            //dig_H1=(unsigned char) 
            //レジスタのアドレス:A1h...dig_H1[7-0]
            //dig_H2=(signed short)
            //レジスタのアドレス:E1h...dig_H2[7-0]   E2h...dig_H2[15-8]
            //dig_H3=(signed char)
            //レジスタのアドレス:E3h...dig_H3[7-0]
            //dig_H4=(sighed short)
            //レジスタのアドレス:E4h...E4h...dig_H4[11-4]  E4h...dig_H4[3-0]
            //dig_H5=(singed short)
            //レジスタのアドレス:E5h...dig_H5[3-0]  E5h...dig_H5[11-4]
            //dig_H6=(sighed char)
            //レジスタのアドレス:E7h...dig_H6[7-0] 
            double var_H;
           
            var_H = (((double)t_fine) - 76800.0);
            var_H = (adc_H - (((double)dig_H4) * 64.0 + ((double)dig_H5) / 16384.0 * var_H)) *
                  (((double)dig_H2) / 65536.0 * (1.0 + ((double)dig_H6) / 67108864.0 * var_H));
            var_H = var_H * (1.0 - ((double)dig_H1) * var_H / 524288.0);//人が理解できる湿度データ

            if (var_H > 100.0)
            {
                var_H = 100.0;
            }
            else if (var_H < 0.0)
            {
                var_H = 0.0;
            }
            return var_H;
        }
    }
}

