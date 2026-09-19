using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net;

namespace 代理连接器
{
    public partial class Form1 : Form
    {
        private NotifyIcon _trayIcon;
        private ContextMenuStrip _trayMenu;
        private bool _allowExit = false;

        public Form1()
        {
            InitializeComponent();

            SetupTray();
        }

        private void BtnSetProxy_MouseEnter(object sender, EventArgs e)
        {
            try { btnSetProxy.BackColor = Color.FromArgb(11, 94, 215); } catch { }
        }

        private void BtnSetProxy_MouseLeave(object sender, EventArgs e)
        {
            try { btnSetProxy.BackColor = Color.FromArgb(13, 110, 253); } catch { }
        }

        private void BtnDisableProxy_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                btnDisableProxy.BackColor = Color.FromArgb(220, 53, 69);
                btnDisableProxy.ForeColor = Color.White;
            }
            catch { }
        }

        private void BtnDisableProxy_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                btnDisableProxy.BackColor = Color.White;
                btnDisableProxy.ForeColor = Color.FromArgb(220, 53, 69);
            }
            catch { }
        }

        private void BtnSetProxy_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProxyManager.SetSystemProxy())
                {
                    MessageBox.Show(
                        "✓ 代理配置成功！\n\n" +
                        "代理地址: 10.88.202.78\n" +
                        "端口: 50000\n\n" +
                        "内网 IP 已自动直连，无需通过代理。",
                        "配置成功",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    UpdateProxyStatus();
                }
                else
                {
                    MessageBox.Show("❌ 代理配置失败\n\n请检查是否有足够的权限。", 
                        "配置失败", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 发生错误\n\n{ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void BtnDisableProxy_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要禁用代理吗？\n\n禁用后将直接连接网络。", 
                    "确认禁用", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (ProxyManager.DisableSystemProxy())
                    {
                        MessageBox.Show("✓ 代理已成功禁用。", 
                            "禁用成功", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
                        UpdateProxyStatus();
                    }
                    else
                    {
                        MessageBox.Show("❌ 禁用代理失败\n\n请检查是否有足够的权限。", 
                            "禁用失败", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 发生错误\n\n{ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void UpdateProxyStatus()
        {
            var (enabled, proxyServer, exceptions) = ProxyManager.GetCurrentProxySettings();

            if (enabled && !string.IsNullOrEmpty(proxyServer))
            {
                _lblStatus.Text = $"● 已连接  |  {proxyServer}";
                _lblStatus.ForeColor = Color.FromArgb(25, 135, 84); // 绿色

                // 已启用代理后，禁用“启用代理”按钮，启用“禁用代理”按钮
                try
                {
                    btnSetProxy.Enabled = false;
                    btnSetProxy.BackColor = Color.LightGray;
                    btnSetProxy.ForeColor = Color.White;

                    btnDisableProxy.Enabled = true;
                    btnDisableProxy.BackColor = Color.White;
                    btnDisableProxy.ForeColor = Color.FromArgb(220, 53, 69);
                }
                catch { }
            }
            else
            {
                _lblStatus.Text = "● 未连接";
                _lblStatus.ForeColor = Color.FromArgb(220, 53, 69);

                // 未连接时，启用“启用代理”按钮，禁用“禁用代理”按钮
                try
                {
                    btnSetProxy.Enabled = true;
                    btnSetProxy.BackColor = Color.FromArgb(13, 110, 253);
                    btnSetProxy.ForeColor = Color.White;

                    btnDisableProxy.Enabled = false;
                    btnDisableProxy.BackColor = Color.White;
                    btnDisableProxy.ForeColor = Color.FromArgb(220, 53, 69);
                }
                catch { }
                // 更改主图标为开锁状态
                try { lblIcon.Text = "🔓"; } catch { }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateProxyStatus();
            // 打开时Ping代理服务器
            _ = CheckProxyHostPingAsync("10.88.202.78");
        }

        private async Task CheckProxyHostPingAsync(string host)
        {
            bool ok = false;
            try
            {
                using var p = new Ping();
                var reply = await p.SendPingAsync(host, 1000);
                ok = reply.Status == IPStatus.Success;
            }
            catch { ok = false; }

            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke((Action)(() => UpdatePingLabel(ok)));
                }
                else
                {
                    UpdatePingLabel(ok);
                }
            }
            catch { }
        }

        private void UpdatePingLabel(bool ok)
        {
            try
            {
                if (ok)
                {
                    _lblPingStatus.Text = "● 有效";
                    _lblPingStatus.ForeColor = Color.FromArgb(25, 135, 84);
                }
                else
                {
                    _lblPingStatus.Text = "● 失败";
                    _lblPingStatus.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
            catch { }
        }

        private void SetupTray()
        {
            try
            {
                _trayMenu = new ContextMenuStrip();
                var exitItem = new ToolStripMenuItem("退出");
                exitItem.Click += (s, e) =>
                {
                    // 退出前禁用系统代理
                    try { ProxyManager.DisableSystemProxy(); } catch { }

                    _allowExit = true;
                    _trayIcon.Visible = false;
                    Application.Exit();
                };
                _trayMenu.Items.Add(exitItem);

                _trayIcon = (components != null)
                    ? new NotifyIcon(components)
                    : new NotifyIcon();

                _trayIcon.Icon = System.Drawing.SystemIcons.Application;
                _trayIcon.Text = "代理连接器";
                _trayIcon.ContextMenuStrip = _trayMenu;
                _trayIcon.Visible = true;

                _trayIcon.MouseClick += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        ShowMainWindow();
                    }
                };
            }
            catch
            {
            }
        }

        private void ShowMainWindow()
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke((Action)ShowMainWindow);
                    return;
                }

                Show();
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                Activate();
                BringToFront();
            }
            catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // 关闭窗口时，隐藏到托盘
            if (!_allowExit && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                ShowInTaskbar = false;
            }
            else
            {
                // 正常退出，清理托盘图标
                try { if (_trayIcon != null) _trayIcon.Visible = false; } catch { }
            }
        }
    }
}
