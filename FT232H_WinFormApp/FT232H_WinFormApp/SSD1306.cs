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
    public class SSD1306
    {
        public static bool PictureSettiing = false;
        //I2CでSSDと通信
        //ssd1306 app note 5/6参照
        //上から順に処理を記述
        /*
        Set MUX Ratio A8h,3Fh
        Set Display OffSet D3h,00h
        Set Display StartLine 40h
        Set Segment re-map A0h/A1h
        Set COM Output ScanDirection C0h/C8h
        Set COM Pins hardware configuration DAh,02
        Set Constrast Control 81h,7Fh
        Disable Entire Display On A4h
        Set Normal Display A6h
        Set Osc Frequency D5h,80h
        Enable charge pump regulator 8Dh,14h
        Display On AFh
         */

        /*
        p34 command descriptions
         */
        //file selectで表示したい画像の表示
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

        public void IIC_Connect()
        {
            if(PictureDisplay())
            {
                //画像を表示
            }
            else
            {
                //error
            }
        }

    }
}
