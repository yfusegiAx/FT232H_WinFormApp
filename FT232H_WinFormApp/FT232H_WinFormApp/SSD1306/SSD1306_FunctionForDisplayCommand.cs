using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232H_WinFormApp
{
    public partial class SSD1306
    {
        //////////////  IIC    FUNCTION   //////////////////////////////////////////////////////////////////////

        //8D->14->AF
        public List<byte> IIC_OnlyDisplayOn()//displayの電源をつけるだけのbyte配列を返す
        {
            byte[] commandForOnlyDisplayOn = new byte[] { 0x8D, 0x14, 0xAF };
            List<byte> databytes = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                databytes.AddRange(new byte[] { 0x11, 0x00, 0x00, commandForOnlyDisplayOn[i]
                                  ,0x80, 0b11111100, 0b11111011//write
                                  ,0x22, 0x00//+VE data in bits ack
                                  ,0x80, 0b11111100, 0b11111011 });//write
            }
            return databytes;
        }
    }
}
