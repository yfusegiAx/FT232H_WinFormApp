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
    public class SPI
    {

        public void SPI_BME280_Connect()
        {
            BME280 bme280 = new BME280();
            bme280.SPI_BME280_Connect();
        }
        public void SPI_SSD1306_Connect()
        {
            SSD1306 ssd1306 = new SSD1306();
            ssd1306.SPI_SSD1306_Connect();
        }
    }
}
