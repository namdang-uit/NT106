using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DrawingServer
{
    public partial class Form1 : Form
    {
        private TcpListener _listener;
        private bool _isRunning = false;
        private int _port = 0;

        // Quản lý danh sách Client: Key là ID, Value là StreamWriter để gửi tin
        private ConcurrentDictionary<string, StreamWriter> _connectedClients = new ConcurrentDictionary<string, StreamWriter>();

        public Form1()
        {
            InitializeComponent();
        }
        private async void StartServer()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                _isRunning = true;
                LogAction($"[HỆ THỐNG] Server bắt đầu chạy tại Port {_port}...");

                while (_isRunning)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    string clientId = Guid.NewGuid().ToString().Substring(0, 8);
                    // Xử lý kết nối của client trong một Task riêng để không block server
                    _ = Task.Run(() => HandleClientAsync(client, clientId));
                }
            }
            catch (Exception ex)
            {
                LogAction($"[LỖI HỆ THỐNG] {ex.Message}");
            }
        }
        // Xử lý kết nối của từng client
        private async Task HandleClientAsync(TcpClient client, string clientId)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            _connectedClients.TryAdd(clientId, writer);

            // Cập nhật UI: Thêm vào danh sách Online
            UpdateClientList(clientId, true);
            LogAction($"[KẾT NỐI] Người chơi mới: {clientId}");

            try
            {
                while (client.Connected)
                {
                    string json = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(json)) break;

                    // Ghi log hoạt động của người dùng
                    ProcessAndLogUserActivity(json, clientId);

                    // Chuyển tiếp gói tin cho các máy khác (Broadcast)
                    BroadcastToOthers(json, clientId);
                }
            }
            catch (Exception ex)
            { 
                LogAction($"[LỖI] {ex.Message}");
            }
            finally
            {
                _connectedClients.TryRemove(clientId, out _);
                UpdateClientList(clientId, false);
                LogAction($"[NGẮT KẾT NỐI] Người chơi thoát: {clientId}");
                client.Close();
            }
        }

        // Hàm ghi Log lên giao diện (Thread-safe)
        private void LogAction(string message)
        {
            if (rtbLogs.InvokeRequired)
            {
                rtbLogs.Invoke(new Action(() => LogAction(message)));
                return;
            }
            string time = DateTime.Now.ToString("HH:mm:ss");
            rtbLogs.AppendText($"[{time}] {message}\r\n");
            rtbLogs.ScrollToCaret(); // Luôn cuộn xuống dòng mới nhất
        }

        // Cập nhật danh sách ListBox người chơi
        private void UpdateClientList(string clientId, bool isAdding)
        {
            if (lbClients.InvokeRequired)
            {
                lbClients.Invoke(new Action(() => UpdateClientList(clientId, isAdding)));
                return;
            }

            if (isAdding) lbClients.Items.Add(clientId);
            else lbClients.Items.Remove(clientId);
            lblOnlineCount.Text = $"Đang online: {_connectedClients.Count}";
        }
        // Phân tích gói tin JSON và ghi log hoạt động của người dùng
        private void ProcessAndLogUserActivity(string json, string clientId)
        {
            try
            {
                dynamic packet = JsonConvert.DeserializeObject(json);
                string type = packet.Type;
                switch (type)
                {
                    case "DRAW":
                        // Chỉ log khi bắt đầu vẽ để tránh tràn màn hình log
                        if (packet.Payload.Phase == "start")
                            LogAction($"[VẼ] {clientId} bắt đầu nét vẽ mới.");
                        break;
                    case "CHAT_MESSAGE":
                        LogAction($"[CHAT] {clientId}: {packet.Payload.Message}");
                        break;
                    case "CLEAR_CANVAS":
                        LogAction($"[XÓA] {clientId} đã xóa toàn bộ bảng.");
                        break;
                }
            }
            catch (Exception ex)
            {
                LogAction($"[LỖI] {ex.Message}");
            }
        }
        // duyệt tất cả client và gửi gói tin đến tất cả trừ người gửi
        private void BroadcastToOthers(string json, string senderId)
        {
            foreach (var kvp in _connectedClients)
            {
                if (kvp.Key != senderId)
                {
                    try { kvp.Value.WriteLine(json); }
                    catch (Exception ex)
                    {
                        LogAction($"[LỖI] {ex.Message}");
                    }
                }
            }
        }
        // lấy Port từ TextBox và bắt đầu server khi nhấn nút Start
        private void btnStart_Click(object sender, EventArgs e)
        {
            _port = int.Parse(txtPort.Text);
            StartServer();
            btnStart.Enabled = false;
            txtPort.Enabled = false;
        }
    }
}