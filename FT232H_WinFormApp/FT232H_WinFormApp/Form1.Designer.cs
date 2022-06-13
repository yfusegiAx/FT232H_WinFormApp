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

namespace FT232H_WinFormApp
{
    public partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonInit = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.status_value = new System.Windows.Forms.Label();
            this.status_label = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Pressure_value = new System.Windows.Forms.Label();
            this.hectopascal_label = new System.Windows.Forms.Label();
            this.Humidlity_value = new System.Windows.Forms.Label();
            this.Humidlity_label = new System.Windows.Forms.Label();
            this.Templature_value = new System.Windows.Forms.Label();
            this.Templature_label = new System.Windows.Forms.Label();
            this.Templature_degrees = new System.Windows.Forms.Label();
            this.Humidity_percent = new System.Windows.Forms.Label();
            this.hectopascal_hPa = new System.Windows.Forms.Label();
            this.CommunicationMethod = new System.Windows.Forms.GroupBox();
            this.ComMethod = new System.Windows.Forms.Label();
            this.SPIRadioButton = new System.Windows.Forms.RadioButton();
            this.I2CRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.CommunicationMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(59, 375);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(100, 42);
            this.buttonInit.TabIndex = 0;
            this.buttonInit.Text = "Initialize";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(226, 377);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(105, 41);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(398, 377);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(109, 42);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(73, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 316);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SSD1306";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.status_value);
            this.groupBox2.Controls.Add(this.status_label);
            this.groupBox2.Location = new System.Drawing.Point(506, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 67);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Interface";
            // 
            // status_value
            // 
            this.status_value.AutoSize = true;
            this.status_value.Location = new System.Drawing.Point(110, 26);
            this.status_value.Name = "status_value";
            this.status_value.Size = new System.Drawing.Size(42, 15);
            this.status_value.TabIndex = 1;
            this.status_value.Text = "Closed";
            // 
            // status_label
            // 
            this.status_label.AutoSize = true;
            this.status_label.Location = new System.Drawing.Point(52, 26);
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(39, 15);
            this.status_label.TabIndex = 0;
            this.status_label.Text = "Status";
            this.status_label.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.hectopascal_hPa);
            this.groupBox3.Controls.Add(this.Humidity_percent);
            this.groupBox3.Controls.Add(this.Templature_degrees);
            this.groupBox3.Controls.Add(this.Pressure_value);
            this.groupBox3.Controls.Add(this.hectopascal_label);
            this.groupBox3.Controls.Add(this.Humidlity_value);
            this.groupBox3.Controls.Add(this.Humidlity_label);
            this.groupBox3.Controls.Add(this.Templature_value);
            this.groupBox3.Controls.Add(this.Templature_label);
            this.groupBox3.Location = new System.Drawing.Point(506, 179);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 169);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BME280 ";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // Pressure_value
            // 
            this.Pressure_value.AutoSize = true;
            this.Pressure_value.Location = new System.Drawing.Point(160, 127);
            this.Pressure_value.Name = "Pressure_value";
            this.Pressure_value.Size = new System.Drawing.Size(13, 15);
            this.Pressure_value.TabIndex = 5;
            this.Pressure_value.Text = "0";
            // 
            // hectopascal_label
            // 
            this.hectopascal_label.AutoSize = true;
            this.hectopascal_label.Location = new System.Drawing.Point(52, 127);
            this.hectopascal_label.Name = "hectopascal_label";
            this.hectopascal_label.Size = new System.Drawing.Size(70, 15);
            this.hectopascal_label.TabIndex = 4;
            this.hectopascal_label.Text = "hectopascal";
            // 
            // Humidlity_value
            // 
            this.Humidlity_value.AutoSize = true;
            this.Humidlity_value.Location = new System.Drawing.Point(158, 77);
            this.Humidlity_value.Name = "Humidlity_value";
            this.Humidlity_value.Size = new System.Drawing.Size(13, 15);
            this.Humidlity_value.TabIndex = 3;
            this.Humidlity_value.Text = "0";
            // 
            // Humidlity_label
            // 
            this.Humidlity_label.AutoSize = true;
            this.Humidlity_label.Location = new System.Drawing.Point(54, 77);
            this.Humidlity_label.Name = "Humidlity_label";
            this.Humidlity_label.Size = new System.Drawing.Size(59, 15);
            this.Humidlity_label.TabIndex = 2;
            this.Humidlity_label.Text = "Humidlity";
            // 
            // Templature_value
            // 
            this.Templature_value.AutoSize = true;
            this.Templature_value.Location = new System.Drawing.Point(158, 30);
            this.Templature_value.Name = "Templature_value";
            this.Templature_value.Size = new System.Drawing.Size(13, 15);
            this.Templature_value.TabIndex = 1;
            this.Templature_value.Text = "0";
            // 
            // Templature_label
            // 
            this.Templature_label.AutoSize = true;
            this.Templature_label.Location = new System.Drawing.Point(54, 30);
            this.Templature_label.Name = "Templature_label";
            this.Templature_label.Size = new System.Drawing.Size(65, 15);
            this.Templature_label.TabIndex = 0;
            this.Templature_label.Text = "Templature";
            // 
            // Templature_degrees
            // 
            this.Templature_degrees.AutoSize = true;
            this.Templature_degrees.Location = new System.Drawing.Point(177, 30);
            this.Templature_degrees.Name = "Templature_degrees";
            this.Templature_degrees.Size = new System.Drawing.Size(19, 15);
            this.Templature_degrees.TabIndex = 6;
            this.Templature_degrees.Text = "℃";
            // 
            // Humidity_percent
            // 
            this.Humidity_percent.AutoSize = true;
            this.Humidity_percent.Location = new System.Drawing.Point(179, 77);
            this.Humidity_percent.Name = "Humidity_percent";
            this.Humidity_percent.Size = new System.Drawing.Size(17, 15);
            this.Humidity_percent.TabIndex = 7;
            this.Humidity_percent.Text = "%";
            // 
            // hectopascal_hPa
            // 
            this.hectopascal_hPa.AutoSize = true;
            this.hectopascal_hPa.Location = new System.Drawing.Point(179, 127);
            this.hectopascal_hPa.Name = "hectopascal_hPa";
            this.hectopascal_hPa.Size = new System.Drawing.Size(27, 15);
            this.hectopascal_hPa.TabIndex = 8;
            this.hectopascal_hPa.Text = "hPa";
            // 
            // CommunicationMethod
            // 
            this.CommunicationMethod.Controls.Add(this.I2CRadioButton);
            this.CommunicationMethod.Controls.Add(this.SPIRadioButton);
            this.CommunicationMethod.Controls.Add(this.ComMethod);
            this.CommunicationMethod.Location = new System.Drawing.Point(506, 114);
            this.CommunicationMethod.Name = "CommunicationMethod";
            this.CommunicationMethod.Size = new System.Drawing.Size(248, 59);
            this.CommunicationMethod.TabIndex = 6;
            this.CommunicationMethod.TabStop = false;
            this.CommunicationMethod.Text = "CommunicationMethod";
            // 
            // ComMethod
            // 
            this.ComMethod.AutoSize = true;
            this.ComMethod.Location = new System.Drawing.Point(52, 28);
            this.ComMethod.Name = "ComMethod";
            this.ComMethod.Size = new System.Drawing.Size(55, 15);
            this.ComMethod.TabIndex = 0;
            this.ComMethod.Text = "Method :";
            // 
            // SPIRadioButton
            // 
            this.SPIRadioButton.AutoSize = true;
            this.SPIRadioButton.Location = new System.Drawing.Point(110, 26);
            this.SPIRadioButton.Name = "SPIRadioButton";
            this.SPIRadioButton.Size = new System.Drawing.Size(41, 19);
            this.SPIRadioButton.TabIndex = 1;
            this.SPIRadioButton.TabStop = true;
            this.SPIRadioButton.Text = "SPI";
            this.SPIRadioButton.UseVisualStyleBackColor = true;
            this.SPIRadioButton.CheckedChanged += new System.EventHandler(this.SPIRadioButton_CheckedChanged);
            // 
            // I2CRadioButton
            // 
            this.I2CRadioButton.AutoSize = true;
            this.I2CRadioButton.Location = new System.Drawing.Point(154, 26);
            this.I2CRadioButton.Name = "I2CRadioButton";
            this.I2CRadioButton.Size = new System.Drawing.Size(41, 19);
            this.I2CRadioButton.TabIndex = 2;
            this.I2CRadioButton.TabStop = true;
            this.I2CRadioButton.Text = "I2C";
            this.I2CRadioButton.UseVisualStyleBackColor = true;
            this.I2CRadioButton.CheckedChanged += new System.EventHandler(this.I2CRadioButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CommunicationMethod);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonInit);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.CommunicationMethod.ResumeLayout(false);
            this.CommunicationMethod.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button buttonInit;
        private Button buttonStart;
        private Button buttonStop;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label status_value;
        private Label status_label;
        private Label Pressure_value;
        private Label hectopascal_label;
        private Label Humidlity_value;
        private Label Humidlity_label;
        private Label Templature_value;
        private Label Templature_label;
        private Label hectopascal_hPa;
        private Label Humidity_percent;
        private Label Templature_degrees;
        private GroupBox CommunicationMethod;
        private Label ComMethod;
        private RadioButton I2CRadioButton;
        private RadioButton SPIRadioButton;
    }
}