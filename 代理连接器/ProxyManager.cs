using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace 代理连接器
{
    public class ProxyManager
    {
        private const string PROXY_ADDRESS = "10.88.202.78";
        private const int PROXY_PORT = 50000;
        private const string REGISTRY_PATH = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";

        private static readonly List<string> IntranetRanges = new()
        {
            "10.0.0.0/8",
            "172.16.0.0/12",
            "192.168.0.0/16",
            "127.0.0.1",
            "localhost",
            "*.local",
            "*.intranet"
        };

        public static bool SetSystemProxy()
        {
            try
            {
                string proxyServer = $"{PROXY_ADDRESS}:{PROXY_PORT}";
                string exceptions = GetProxyExceptions();

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
                {
                    if (key == null)
                        return false;

                    // 启用代理
                    key.SetValue("ProxyEnable", 1, RegistryValueKind.DWord);

                    // 设置代理地址
                    key.SetValue("ProxyServer", proxyServer, RegistryValueKind.String);

                    // 设置代理例外
                    key.SetValue("ProxyOverride", exceptions, RegistryValueKind.String);
                }

                // 刷新 Internet 设置
                InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
                InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"配置代理失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 禁用系统代理
        /// </summary>
        public static bool DisableSystemProxy()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
                {
                    if (key == null)
                        return false;

                    key.SetValue("ProxyEnable", 0, RegistryValueKind.DWord);
                }

                // 刷新 Internet 设置
                InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
                InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"禁用代理失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取当前代理设置
        /// </summary>
        public static (bool enabled, string proxyServer, string exceptions) GetCurrentProxySettings()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                {
                    if (key == null)
                        return (false, "", "");

                    int proxyEnable = (int?)key.GetValue("ProxyEnable") ?? 0;
                    string proxyServer = (string?)key.GetValue("ProxyServer") ?? "";
                    string proxyOverride = (string?)key.GetValue("ProxyOverride") ?? "";

                    return (proxyEnable == 1, proxyServer, proxyOverride);
                }
            }
            catch
            {
                return (false, "", "");
            }
        }

        /// <summary>
        /// 生成代理例外列表
        /// </summary>
        private static string GetProxyExceptions()
        {
            var exceptions = new List<string>
            {
                "127.0.0.1",
                "localhost",
                "*.local",
                "10.*",
                "192.168.*",
                "<local>"
            };

            for (int i = 16; i <= 31; i++)
            {
                exceptions.Add($"172.{i}.*");
            }

            return string.Join("; ", exceptions);
        }

        /// <summary>
        /// 验证 IP 地址是否是内网 IP
        /// </summary>
        public static bool IsIntranetIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return false;

            ipAddress = ipAddress.ToLower().Trim();

            // 检查特殊情况
            if (ipAddress == "127.0.0.1" || ipAddress == "localhost" || ipAddress.EndsWith(".local"))
                return true;

            // 尝试解析 IP 地址
            if (IPAddress.TryParse(ipAddress, out var address))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    byte[] bytes = address.GetAddressBytes();

                    // 10.0.0.0/8
                    if (bytes[0] == 10)
                        return true;

                    // 172.16.0.0/12
                    if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                        return true;

                    // 192.168.0.0/16
                    if (bytes[0] == 192 && bytes[1] == 168)
                        return true;

                    // 127.0.0.0/8 (本地环回)
                    if (bytes[0] == 127)
                        return true;
                }

                if (address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    if (IPAddress.IsLoopback(address))
                        return true;
                }
            }

            return false;
        }

        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;

        [System.Runtime.InteropServices.DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
    }
}
