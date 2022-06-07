#define FT232H                // Enable only one of these defines depending on your device type

// This code is provided as an example only and is not supported or guaranteed by FTDI
// It is the responsibility of the designer of the system incorporating any part of this code to ensure correct
// and safe operation of their overall system. By using this code, you agree that FTDI and its employees accept 
// no responsibility whatsoever for any consequences resulting from the use of this code. 

//参考：https://ftdichip.com/software-examples/code-examples/csharp-examples/　Example6
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FTD2XX_NET;//FTDI製品の利用のため必要
using System.Threading;


namespace FT232H_WinFormApp
{
    
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        /*
          public Form1()
        {
            InitializeComponent();//初期化
        }
         */
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    


}