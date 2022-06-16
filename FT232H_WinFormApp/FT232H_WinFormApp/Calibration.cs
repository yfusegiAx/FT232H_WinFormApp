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
        double BME280_compensate_T_double(BME280_S32_t adc_T)
        {
            //adc_tは生の温度データ
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
                return 0;//ゼロ除算
            }
            p = 1048576.0 - (double)adc_P;
            p = (p - (var2 / 4096.0)) * 6250.0 / var1;
            var1 = ((double)dig_P9) * p * p / 2147483648.0;
            var2 = p * ((double)dig_P8) / 32768.0;
            p = p + (var1 + var2 + ((double)dig_P7)) / 16.0;
            return p;
        }

        //湿度(%RH)をdoubleとして返す　「46.332」の出力値は、46.332%RHに相当
        double BME280_compensate_H_double(BME280_S32_t adc_H)
        {
            double var_H;

            var_H = (((double)t_fine) - 76800.0);
            var_H = (adc_H - (((double)dig_H4) * 64.0 + ((double)dig_H5) / 16384.0 * var_H)) *
                  (((double)dig_H2) / 65536.0 * (1.0 + ((double)dig_H6) / 67108864.0 * var_H)));
            var_H = var_H * (1.0 - ((double)dig_H1) * var_H / 524288.0);

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
