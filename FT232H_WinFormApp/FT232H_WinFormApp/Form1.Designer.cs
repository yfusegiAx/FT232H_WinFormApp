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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.status_name_label = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Blue_value = new System.Windows.Forms.Label();
            this.Blue_label = new System.Windows.Forms.Label();
            this.Green_value = new System.Windows.Forms.Label();
            this.Green_label = new System.Windows.Forms.Label();
            this.Red_value = new System.Windows.Forms.Label();
            this.Red_label = new System.Windows.Forms.Label();
            this.Proximity_value = new System.Windows.Forms.Label();
            this.Proximity_label = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(59, 375);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "Initialize";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(226, 377);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 41);
            this.button2.TabIndex = 1;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(398, 377);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(109, 42);
            this.button3.TabIndex = 2;
            this.button3.Text = "Stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(73, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 179);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(43, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(236, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Hello World";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.status_name_label);
            this.groupBox2.Controls.Add(this.label_status);
            this.groupBox2.Location = new System.Drawing.Point(503, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 67);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Interface";
            // 
            // status_name_label
            // 
            this.status_name_label.AutoSize = true;
            this.status_name_label.Location = new System.Drawing.Point(110, 26);
            this.status_name_label.Name = "status_name_label";
            this.status_name_label.Size = new System.Drawing.Size(42, 15);
            this.status_name_label.TabIndex = 1;
            this.status_name_label.Text = "Closed";
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(52, 26);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(39, 15);
            this.label_status.TabIndex = 0;
            this.label_status.Text = "Status";
            this.label_status.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Blue_value);
            this.groupBox3.Controls.Add(this.Blue_label);
            this.groupBox3.Controls.Add(this.Green_value);
            this.groupBox3.Controls.Add(this.Green_label);
            this.groupBox3.Controls.Add(this.Red_value);
            this.groupBox3.Controls.Add(this.Red_label);
            this.groupBox3.Controls.Add(this.Proximity_value);
            this.groupBox3.Controls.Add(this.Proximity_label);
            this.groupBox3.Location = new System.Drawing.Point(501, 193);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 131);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sensors";
            // 
            // Blue_value
            // 
            this.Blue_value.AutoSize = true;
            this.Blue_value.Location = new System.Drawing.Point(98, 107);
            this.Blue_value.Name = "Blue_value";
            this.Blue_value.Size = new System.Drawing.Size(13, 15);
            this.Blue_value.TabIndex = 7;
            this.Blue_value.Text = "0";
            // 
            // Blue_label
            // 
            this.Blue_label.AutoSize = true;
            this.Blue_label.Location = new System.Drawing.Point(54, 107);
            this.Blue_label.Name = "Blue_label";
            this.Blue_label.Size = new System.Drawing.Size(30, 15);
            this.Blue_label.TabIndex = 6;
            this.Blue_label.Text = "Blue";
            // 
            // Green_value
            // 
            this.Green_value.AutoSize = true;
            this.Green_value.Location = new System.Drawing.Point(98, 77);
            this.Green_value.Name = "Green_value";
            this.Green_value.Size = new System.Drawing.Size(13, 15);
            this.Green_value.TabIndex = 5;
            this.Green_value.Text = "0";
            // 
            // Green_label
            // 
            this.Green_label.AutoSize = true;
            this.Green_label.Location = new System.Drawing.Point(54, 77);
            this.Green_label.Name = "Green_label";
            this.Green_label.Size = new System.Drawing.Size(38, 15);
            this.Green_label.TabIndex = 4;
            this.Green_label.Text = "Green";
            // 
            // Red_value
            // 
            this.Red_value.AutoSize = true;
            this.Red_value.Location = new System.Drawing.Point(98, 53);
            this.Red_value.Name = "Red_value";
            this.Red_value.Size = new System.Drawing.Size(13, 15);
            this.Red_value.TabIndex = 3;
            this.Red_value.Text = "0";
            // 
            // Red_label
            // 
            this.Red_label.AutoSize = true;
            this.Red_label.Location = new System.Drawing.Point(54, 53);
            this.Red_label.Name = "Red_label";
            this.Red_label.Size = new System.Drawing.Size(27, 15);
            this.Red_label.TabIndex = 2;
            this.Red_label.Text = "Red";
            // 
            // Proximity_value
            // 
            this.Proximity_value.AutoSize = true;
            this.Proximity_value.Location = new System.Drawing.Point(98, 30);
            this.Proximity_value.Name = "Proximity_value";
            this.Proximity_value.Size = new System.Drawing.Size(13, 15);
            this.Proximity_value.TabIndex = 1;
            this.Proximity_value.Text = "0";
            // 
            // Proximity_label
            // 
            this.Proximity_label.AutoSize = true;
            this.Proximity_label.Location = new System.Drawing.Point(54, 30);
            this.Proximity_label.Name = "Proximity_label";
            this.Proximity_label.Size = new System.Drawing.Size(57, 15);
            this.Proximity_label.TabIndex = 0;
            this.Proximity_label.Text = "Proximity";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TextBox textBox1;
        private Label status_name_label;
        private Label label_status;
        private Label Blue_value;
        private Label Blue_label;
        private Label Green_value;
        private Label Green_label;
        private Label Red_value;
        private Label Red_label;
        private Label Proximity_value;
        private Label Proximity_label;
    }
}