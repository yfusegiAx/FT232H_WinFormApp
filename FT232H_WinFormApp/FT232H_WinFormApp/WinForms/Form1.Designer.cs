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
            this.DeviceConnect_Button = new System.Windows.Forms.Button();
            this.AppEnd_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DisplayMode_comboBox = new System.Windows.Forms.ComboBox();
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
            this.IICRadioButton = new System.Windows.Forms.RadioButton();
            this.SPIRadioButton = new System.Windows.Forms.RadioButton();
            this.ComMethod = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.SSD1306_RadioButton = new System.Windows.Forms.RadioButton();
            this.BME280_RadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.SlaveSSD1306RadioButton = new System.Windows.Forms.RadioButton();
            this.SlaveBME280RadioButton = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.CommunicationMethod.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // DeviceConnect_Button
            // 
            this.DeviceConnect_Button.Location = new System.Drawing.Point(212, 378);
            this.DeviceConnect_Button.Name = "DeviceConnect_Button";
            this.DeviceConnect_Button.Size = new System.Drawing.Size(105, 41);
            this.DeviceConnect_Button.TabIndex = 1;
            this.DeviceConnect_Button.Text = "Device Connect";
            this.DeviceConnect_Button.UseVisualStyleBackColor = true;
            this.DeviceConnect_Button.Click += new System.EventHandler(this.DeviceConnect_Button_Click);
            // 
            // AppEnd_Button
            // 
            this.AppEnd_Button.Location = new System.Drawing.Point(369, 377);
            this.AppEnd_Button.Name = "AppEnd_Button";
            this.AppEnd_Button.Size = new System.Drawing.Size(109, 42);
            this.AppEnd_Button.TabIndex = 2;
            this.AppEnd_Button.Text = "APP END";
            this.AppEnd_Button.UseVisualStyleBackColor = true;
            this.AppEnd_Button.Click += new System.EventHandler(this.AppEnd_Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.DisplayMode_comboBox);
            this.groupBox1.Location = new System.Drawing.Point(73, 162);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(129, 186);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SSD1306";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 102);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(109, 23);
            this.textBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "DisplayMode select";
            // 
            // DisplayMode_comboBox
            // 
            this.DisplayMode_comboBox.FormattingEnabled = true;
            this.DisplayMode_comboBox.Items.AddRange(new object[] {
            "OnlyDisplayOn",
            "OnlyDisplayOff",
            "DisplaySelectedPicture",
            "DisplayWriteWords",
            "DisplayBME280 Data"});
            this.DisplayMode_comboBox.Location = new System.Drawing.Point(2, 45);
            this.DisplayMode_comboBox.Name = "DisplayMode_comboBox";
            this.DisplayMode_comboBox.Size = new System.Drawing.Size(121, 23);
            this.DisplayMode_comboBox.TabIndex = 0;
            this.DisplayMode_comboBox.SelectedIndexChanged += new System.EventHandler(this.DisplayMode_comboBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.status_value);
            this.groupBox2.Controls.Add(this.status_label);
            this.groupBox2.Location = new System.Drawing.Point(506, 40);
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
            this.status_label.Location = new System.Drawing.Point(34, 26);
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(42, 15);
            this.status_label.TabIndex = 0;
            this.status_label.Text = "Status:";
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
            this.groupBox3.Location = new System.Drawing.Point(208, 162);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(270, 186);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BME280 ";
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
            this.CommunicationMethod.Controls.Add(this.IICRadioButton);
            this.CommunicationMethod.Controls.Add(this.SPIRadioButton);
            this.CommunicationMethod.Controls.Add(this.ComMethod);
            this.CommunicationMethod.Location = new System.Drawing.Point(506, 130);
            this.CommunicationMethod.Name = "CommunicationMethod";
            this.CommunicationMethod.Size = new System.Drawing.Size(266, 59);
            this.CommunicationMethod.TabIndex = 6;
            this.CommunicationMethod.TabStop = false;
            this.CommunicationMethod.Text = "CommunicationStandard";
            // 
            // IICRadioButton
            // 
            this.IICRadioButton.AutoSize = true;
            this.IICRadioButton.Location = new System.Drawing.Point(185, 28);
            this.IICRadioButton.Name = "IICRadioButton";
            this.IICRadioButton.Size = new System.Drawing.Size(38, 19);
            this.IICRadioButton.TabIndex = 2;
            this.IICRadioButton.TabStop = true;
            this.IICRadioButton.Text = "IIC";
            this.IICRadioButton.UseVisualStyleBackColor = true;
            this.IICRadioButton.CheckedChanged += new System.EventHandler(this.IICRadioButton_CheckedChanged);
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
            this.ComMethod.Location = new System.Drawing.Point(34, 26);
            this.ComMethod.Name = "ComMethod";
            this.ComMethod.Size = new System.Drawing.Size(60, 15);
            this.ComMethod.TabIndex = 0;
            this.ComMethod.Text = "Standard :";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.SSD1306_RadioButton);
            this.groupBox4.Controls.Add(this.BME280_RadioButton);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(506, 207);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(266, 59);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ControlDevice";
            // 
            // SSD1306_RadioButton
            // 
            this.SSD1306_RadioButton.AutoSize = true;
            this.SSD1306_RadioButton.Location = new System.Drawing.Point(185, 26);
            this.SSD1306_RadioButton.Name = "SSD1306_RadioButton";
            this.SSD1306_RadioButton.Size = new System.Drawing.Size(69, 19);
            this.SSD1306_RadioButton.TabIndex = 2;
            this.SSD1306_RadioButton.TabStop = true;
            this.SSD1306_RadioButton.Text = "SSD1306";
            this.SSD1306_RadioButton.UseVisualStyleBackColor = true;
            this.SSD1306_RadioButton.CheckedChanged += new System.EventHandler(this.SSD1306_RadioButton_CheckedChanged);
            // 
            // BME280_RadioButton
            // 
            this.BME280_RadioButton.AutoSize = true;
            this.BME280_RadioButton.Location = new System.Drawing.Point(111, 26);
            this.BME280_RadioButton.Name = "BME280_RadioButton";
            this.BME280_RadioButton.Size = new System.Drawing.Size(68, 19);
            this.BME280_RadioButton.TabIndex = 1;
            this.BME280_RadioButton.TabStop = true;
            this.BME280_RadioButton.Text = "BMP280";
            this.BME280_RadioButton.UseVisualStyleBackColor = true;
            this.BME280_RadioButton.CheckedChanged += new System.EventHandler(this.BME280_RadioButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device :";
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(73, 40);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(129, 107);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Clock Setting";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.SlaveSSD1306RadioButton);
            this.groupBox6.Controls.Add(this.SlaveBME280RadioButton);
            this.groupBox6.Location = new System.Drawing.Point(212, 40);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(266, 107);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Slave Address";
            // 
            // SlaveSSD1306RadioButton
            // 
            this.SlaveSSD1306RadioButton.AutoSize = true;
            this.SlaveSSD1306RadioButton.Location = new System.Drawing.Point(23, 52);
            this.SlaveSSD1306RadioButton.Name = "SlaveSSD1306RadioButton";
            this.SlaveSSD1306RadioButton.Size = new System.Drawing.Size(102, 19);
            this.SlaveSSD1306RadioButton.TabIndex = 1;
            this.SlaveSSD1306RadioButton.TabStop = true;
            this.SlaveSSD1306RadioButton.Text = "0x3C(SSD1306)";
            this.SlaveSSD1306RadioButton.UseVisualStyleBackColor = true;
            this.SlaveSSD1306RadioButton.CheckedChanged += new System.EventHandler(this.SlaveSSD1306RadioButton_CheckedChanged);
            // 
            // SlaveBME280RadioButton
            // 
            this.SlaveBME280RadioButton.AutoSize = true;
            this.SlaveBME280RadioButton.Location = new System.Drawing.Point(23, 27);
            this.SlaveBME280RadioButton.Name = "SlaveBME280RadioButton";
            this.SlaveBME280RadioButton.Size = new System.Drawing.Size(128, 19);
            this.SlaveBME280RadioButton.TabIndex = 0;
            this.SlaveBME280RadioButton.TabStop = true;
            this.SlaveBME280RadioButton.Text = "0x76/0x77(BME280)";
            this.SlaveBME280RadioButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.SlaveBME280RadioButton.UseVisualStyleBackColor = true;
            this.SlaveBME280RadioButton.CheckedChanged += new System.EventHandler(this.SlaveBME280RadioButton_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(48, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.CommunicationMethod);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.AppEnd_Button);
            this.Controls.Add(this.DeviceConnect_Button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.CommunicationMethod.ResumeLayout(false);
            this.CommunicationMethod.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Button DeviceConnect_Button;
        private Button AppEnd_Button;
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
        private RadioButton IICRadioButton;
        private RadioButton SPIRadioButton;
        private GroupBox groupBox4;
        private RadioButton SSD1306_RadioButton;
        private RadioButton BME280_RadioButton;
        private Label label1;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private RadioButton SlaveSSD1306RadioButton;
        private RadioButton SlaveBME280RadioButton;
        private Label label2;
        private ComboBox DisplayMode_comboBox;
        private TextBox textBox1;
        private Button button1;
    }
}