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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.status_value = new System.Windows.Forms.Label();
            this.status_label = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.hectopascal_hPa = new System.Windows.Forms.Label();
            this.Humidity_percent = new System.Windows.Forms.Label();
            this.Templature_degrees = new System.Windows.Forms.Label();
            this.Pressure_value = new System.Windows.Forms.Label();
            this.Pressure_label = new System.Windows.Forms.Label();
            this.Humidlity_value = new System.Windows.Forms.Label();
            this.Humidlity_label = new System.Windows.Forms.Label();
            this.Templature_value = new System.Windows.Forms.Label();
            this.Templature_label = new System.Windows.Forms.Label();
            this.CommunicationMethod = new System.Windows.Forms.GroupBox();
            this.I2CRadioButton = new System.Windows.Forms.RadioButton();
            this.SPIRadioButton = new System.Windows.Forms.RadioButton();
            this.ComMethod = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.CommunicationMethod.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(59, 375);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(100, 42);
            this.buttonInit.TabIndex = 0;
            this.buttonInit.Text = "SSD1306 Connect";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(212, 378);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(105, 41);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "BMP280 Connect";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(369, 377);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(109, 42);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "App END";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(73, 162);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 186);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SSD1306";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(65, 49);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(121, 107);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.status_value);
            this.groupBox2.Controls.Add(this.status_label);
            this.groupBox2.Location = new System.Drawing.Point(506, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 67);
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
            this.groupBox3.Controls.Add(this.Pressure_label);
            this.groupBox3.Controls.Add(this.Humidlity_value);
            this.groupBox3.Controls.Add(this.Humidlity_label);
            this.groupBox3.Controls.Add(this.Templature_value);
            this.groupBox3.Controls.Add(this.Templature_label);
            this.groupBox3.Location = new System.Drawing.Point(506, 269);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(266, 169);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BME280 ";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // hectopascal_hPa
            // 
            this.hectopascal_hPa.AutoSize = true;
            this.hectopascal_hPa.Location = new System.Drawing.Point(202, 127);
            this.hectopascal_hPa.Name = "hectopascal_hPa";
            this.hectopascal_hPa.Size = new System.Drawing.Size(27, 15);
            this.hectopascal_hPa.TabIndex = 8;
            this.hectopascal_hPa.Text = "hPa";
            // 
            // Humidity_percent
            // 
            this.Humidity_percent.AutoSize = true;
            this.Humidity_percent.Location = new System.Drawing.Point(202, 77);
            this.Humidity_percent.Name = "Humidity_percent";
            this.Humidity_percent.Size = new System.Drawing.Size(17, 15);
            this.Humidity_percent.TabIndex = 7;
            this.Humidity_percent.Text = "%";
            // 
            // Templature_degrees
            // 
            this.Templature_degrees.AutoSize = true;
            this.Templature_degrees.Location = new System.Drawing.Point(202, 30);
            this.Templature_degrees.Name = "Templature_degrees";
            this.Templature_degrees.Size = new System.Drawing.Size(19, 15);
            this.Templature_degrees.TabIndex = 6;
            this.Templature_degrees.Text = "℃";
            // 
            // Pressure_value
            // 
            this.Pressure_value.AutoSize = true;
            this.Pressure_value.Location = new System.Drawing.Point(130, 127);
            this.Pressure_value.Name = "Pressure_value";
            this.Pressure_value.Size = new System.Drawing.Size(13, 15);
            this.Pressure_value.TabIndex = 5;
            this.Pressure_value.Text = "0";
            this.Pressure_value.Click += new System.EventHandler(this.Hectpascal_value_Click);
            // 
            // Pressure_label
            // 
            this.Pressure_label.AutoSize = true;
            this.Pressure_label.Location = new System.Drawing.Point(52, 127);
            this.Pressure_label.Name = "Pressure_label";
            this.Pressure_label.Size = new System.Drawing.Size(51, 15);
            this.Pressure_label.TabIndex = 4;
            this.Pressure_label.Text = "Pressure";
            // 
            // Humidlity_value
            // 
            this.Humidlity_value.AutoSize = true;
            this.Humidlity_value.Location = new System.Drawing.Point(130, 77);
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
            this.Templature_value.Location = new System.Drawing.Point(130, 30);
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
            // CommunicationMethod
            // 
            this.CommunicationMethod.Controls.Add(this.I2CRadioButton);
            this.CommunicationMethod.Controls.Add(this.SPIRadioButton);
            this.CommunicationMethod.Controls.Add(this.ComMethod);
            this.CommunicationMethod.Location = new System.Drawing.Point(506, 114);
            this.CommunicationMethod.Name = "CommunicationMethod";
            this.CommunicationMethod.Size = new System.Drawing.Size(266, 59);
            this.CommunicationMethod.TabIndex = 6;
            this.CommunicationMethod.TabStop = false;
            this.CommunicationMethod.Text = "CommunicationMethod";
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
            // ComMethod
            // 
            this.ComMethod.AutoSize = true;
            this.ComMethod.Location = new System.Drawing.Point(52, 28);
            this.ComMethod.Name = "ComMethod";
            this.ComMethod.Size = new System.Drawing.Size(55, 15);
            this.ComMethod.TabIndex = 0;
            this.ComMethod.Text = "Method :";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton1);
            this.groupBox4.Controls.Add(this.radioButton2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(506, 183);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(266, 59);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ControllDevice";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(185, 26);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(69, 19);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "SSD1306";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(111, 26);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(68, 19);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "BMP280";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device :";
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(73, 40);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(416, 107);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Clock Setting";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
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
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.CommunicationMethod.ResumeLayout(false);
            this.CommunicationMethod.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private Label Pressure_label;
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
        private GroupBox groupBox4;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label1;
        private PictureBox pictureBox1;
        private GroupBox groupBox5;
    }
}