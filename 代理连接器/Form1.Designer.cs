using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace 代理连接器
{
    partial class Form1
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
        /// </summary>
        private void InitializeComponent()
        {
            mainPanel = new Panel();
            lblIcon = new Label();
            _lblPingStatus = new Label();
            divider = new Panel();
            btnSetProxy = new Button();
            btnDisableProxy = new Button();
            _lblStatus = new Label();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.White;
            mainPanel.Controls.Add(lblIcon);
            mainPanel.Controls.Add(_lblPingStatus);
            mainPanel.Controls.Add(divider);
            mainPanel.Controls.Add(btnSetProxy);
            mainPanel.Controls.Add(btnDisableProxy);
            mainPanel.Controls.Add(_lblStatus);
            mainPanel.Location = new Point(50, 40);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(580, 345);
            mainPanel.TabIndex = 0;
            mainPanel.Paint += MainPanel_Paint;
            // 
            // lblIcon
            // 
            lblIcon.BackColor = Color.Transparent;
            lblIcon.Font = new Font("Segoe UI Emoji", 36F, FontStyle.Regular, GraphicsUnit.Point);
            lblIcon.Location = new Point(40, 58);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new Size(516, 181);
            lblIcon.TabIndex = 0;
            lblIcon.Text = "🔒";
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblPingStatus
            // 
            _lblPingStatus.AutoSize = true;
            _lblPingStatus.BackColor = Color.Transparent;
            _lblPingStatus.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            _lblPingStatus.ForeColor = Color.Gray;
            _lblPingStatus.Location = new Point(20, 18);
            _lblPingStatus.Name = "_lblPingStatus";
            _lblPingStatus.Size = new Size(100, 30);
            _lblPingStatus.TabIndex = 3;
            _lblPingStatus.Text = "● 未检测";
            // 
            // divider
            // 
            divider.BackColor = Color.FromArgb(230, 232, 235);
            divider.Location = new Point(40, 180);
            divider.Name = "divider";
            divider.Size = new Size(500, 1);
            divider.TabIndex = 3;
            // 
            // btnSetProxy
            // 
            btnSetProxy.BackColor = Color.FromArgb(13, 110, 253);
            btnSetProxy.Cursor = Cursors.Hand;
            btnSetProxy.FlatAppearance.BorderSize = 0;
            btnSetProxy.FlatAppearance.MouseDownBackColor = Color.FromArgb(10, 88, 202);
            btnSetProxy.FlatAppearance.MouseOverBackColor = Color.FromArgb(11, 94, 215);
            btnSetProxy.FlatStyle = FlatStyle.Flat;
            btnSetProxy.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnSetProxy.ForeColor = Color.White;
            btnSetProxy.Location = new Point(40, 242);
            btnSetProxy.Name = "btnSetProxy";
            btnSetProxy.Size = new Size(230, 46);
            btnSetProxy.TabIndex = 5;
            btnSetProxy.Text = "启用";
            btnSetProxy.UseVisualStyleBackColor = false;
            btnSetProxy.Click += BtnSetProxy_Click;
            btnSetProxy.MouseEnter += BtnSetProxy_MouseEnter;
            btnSetProxy.MouseLeave += BtnSetProxy_MouseLeave;
            // 
            // btnDisableProxy
            // 
            btnDisableProxy.BackColor = Color.White;
            btnDisableProxy.Cursor = Cursors.Hand;
            btnDisableProxy.FlatAppearance.BorderColor = Color.FromArgb(220, 53, 69);
            btnDisableProxy.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 235, 238);
            btnDisableProxy.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 245, 246);
            btnDisableProxy.FlatStyle = FlatStyle.Flat;
            btnDisableProxy.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnDisableProxy.ForeColor = Color.FromArgb(220, 53, 69);
            btnDisableProxy.Location = new Point(310, 242);
            btnDisableProxy.Name = "btnDisableProxy";
            btnDisableProxy.Size = new Size(230, 46);
            btnDisableProxy.TabIndex = 6;
            btnDisableProxy.Text = "禁用";
            btnDisableProxy.UseVisualStyleBackColor = false;
            btnDisableProxy.Click += BtnDisableProxy_Click;
            btnDisableProxy.MouseEnter += BtnDisableProxy_MouseEnter;
            btnDisableProxy.MouseLeave += BtnDisableProxy_MouseLeave;
            // 
            // _lblStatus
            // 
            _lblStatus.BackColor = Color.Transparent;
            _lblStatus.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            _lblStatus.ForeColor = Color.FromArgb(25, 135, 84);
            _lblStatus.Location = new Point(460, 16);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(100, 22);
            _lblStatus.TabIndex = 8;
            _lblStatus.Text = "● 已启用";
            _lblStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(674, 422);
            Controls.Add(mainPanel);
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "代理连接器 - Proxy Connector";
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            using (Pen borderPen = new Pen(Color.FromArgb(228, 230, 233)))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void InfoCard_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            using (Pen p = new Pen(Color.FromArgb(235, 237, 240)))
            {
                e.Graphics.DrawRectangle(p, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        #endregion

        private Label _lblStatus;
        private Label _lblPingStatus;
        private Button btnSetProxy;
        private Button btnDisableProxy;
        private Panel mainPanel;
        private Label lblIcon;
        private Panel divider;
    }
}
