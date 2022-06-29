using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232H_WinFormApp
{
    public class IIC
    {
        public void IIC_BME280_Connect()
        {
            BME280 bme280 = new BME280();
            bme280.IIC_BME280_Connect();
        }
        public void IIC_SSD1306_Connect()
        {
            SSD1306 ssd1306 = new SSD1306();
            ssd1306.IIC_SSD1306_Connect();
        }
    }
}
