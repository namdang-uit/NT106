using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
namespace DrawingClient
{
    
    public partial class Form1 : Form
    {
        // Các biến kết nối mạng
        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;
        private bool isConnected = false;
        private readonly string LB_IP = "127.0.0.1"; // Thay bằng IP thật của Load Balancer
        private readonly int LB_PORT = 8080;         // Thay bằng Port thật

        // --- BIẾN QUẢN LÝ TRẠNG THÁI GÓI TIN ---
        private int sequenceNumber = 1;
        private string userToken = "token_tam_thoi";
        private System.Windows.Forms.Timer pingTimer;

        // --- BIẾN TỐI ƯU VẼ (BATCHING) VÀ VẼ QUA MẠNG ---
        private string currentStrokeId = "";
        private List<Point> pointBuffer = new List<Point>();
        private Dictionary<string, Stroke> networkActiveStrokes = new Dictionary<string, Stroke>();
        // Các biến vẽ
        // Khai báo một List để lưu trữ tất cả các nét vẽ đã hoàn thành
        private List<Stroke> allStrokes = new List<Stroke>();
        private Stroke currentStroke = null;
        private bool isDrawing = false;
        // Biến lưu trữ màu sắc và kích thước bút hiện tại, cũng như lần sử dụng cuối cùng
        private Color currentColor = Color.Black;
        private Color lastUsedColor = Color.Black;
        private int currentSize = 3;
        private int lastUsedSize = 3;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1280, 720);
            //có thể sử dụng phím tắt
            this.KeyPreview = true;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectToLoadBalancer();

            // Setup Ping Timer (30s 1 lần)
            pingTimer = new System.Windows.Forms.Timer();
            pingTimer.Interval = 30000;
            pingTimer.Tick += (s, ev) => SendPacket("PING", new { });
            pingTimer.Start();
        }

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            // Bắt đầu vẽ khi nhấn chuột trái
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                currentStrokeId = "stroke_" + Guid.NewGuid().ToString("N").Substring(0, 8);
                pointBuffer.Clear();
                currentStroke = new Stroke
                {
                    StrokeId = currentStrokeId,
                    StrokeColor = currentColor,
                    StrokeSize = currentSize
                };
                currentStroke.Points.Add(e.Location);
                pointBuffer.Add(e.Location);
                FlushDrawBuffer("start");
            }
        }
        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Chỉ thêm điểm vào nét vẽ hiện tại nếu đang trong trạng thái vẽ (isDrawing = true), sau khi mousedown(nhấn chuột trái)
            // Nhấn và di chuột
            if (isDrawing && currentStroke != null)
            {
                currentStroke.Points.Add(e.Location);
                pointBuffer.Add(e.Location);
                picCanvas.Invalidate();
                if (pointBuffer.Count >= 5) // Đạt 5 điểm thì gửi
                {
                    FlushDrawBuffer("move");
                }
            }
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            // Kết thúc vẽ khi thả chuột trái
            if (isDrawing && currentStroke != null)
            {
                isDrawing = false;
                allStrokes.Add(currentStroke);
                if (pointBuffer.Count > 0)
                {
                    FlushDrawBuffer("end");
                }
                else
                {
                    // Nếu buffer rỗng nhưng vẫn nhả chuột, gửi mảng rỗng kèm tín hiệu end
                    SendPacket("DRAW", new { StrokeId = currentStrokeId, Phase = "end", Color = ColorTranslator.ToHtml(currentColor), Size = currentSize, Points = new List<object>() });
                }
                currentStroke = null;
            }
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            //set chất lượng vẽ mượt mà hơn
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            // Vẽ tất cả các nét đã hoàn thành
            foreach (var stroke in allStrokes)
            {
                if (stroke.Points.Count > 1)
                {
                    using (Pen pen = new Pen(stroke.StrokeColor, stroke.StrokeSize))
                    {
                        pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                        e.Graphics.DrawLines(pen, stroke.Points.ToArray());
                    }
                }
            }
            //vẽ mượt hơn với DrawCurve, nhưng cần ít nhất 3 điểm để vẽ
            if (currentStroke != null && currentStroke.Points.Count >= 3)
            {
                using (Pen pen = new Pen(currentStroke.StrokeColor, currentStroke.StrokeSize))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                    e.Graphics.DrawCurve(pen, currentStroke.Points.ToArray());
                }
            }
            foreach (var activeStroke in networkActiveStrokes.Values)
            {
                if (activeStroke.Points.Count > 1)
                {
                    using (Pen pen = new Pen(activeStroke.StrokeColor, activeStroke.StrokeSize))
                    {
                        pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        e.Graphics.DrawLines(pen, activeStroke.Points.ToArray());
                    }
                }
            }
        }

        private void btn_ClearCanvas_Click(object sender, EventArgs e)
        {
            // Xóa toàn bộ phần tử trong List
            allStrokes.Clear();
            picCanvas.Invalidate();
            SendPacket("CLEAR_CANVAS", new { });
        }

        private void btn_Undo_Click(object sender, EventArgs e)
        {
            if (allStrokes.Count > 0)
            {
                // Lấy vị trí của nét vẽ cuối cùng
                string lastStrokeId = allStrokes.Last().StrokeId;
                allStrokes.RemoveAt(allStrokes.Count - 1);
                picCanvas.Invalidate();

                SendPacket("UNDO", new { StrokeId = lastStrokeId });
            }
        }

        private void btn_Erase_Click(object sender, EventArgs e)
        {
            // Đổi màu thành màu nền của PictureBox
            currentColor = picCanvas.BackColor;
            // Tăng size bút 
            currentSize = 20;
        }

        private void btn_Color_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    currentColor = colorDialog.Color;
                    currentSize = 3;

                    // LƯU LẠI VÀO BỘ NHỚ TẠM
                    lastUsedColor = currentColor;
                    lastUsedSize = currentSize;
                }
            }
        }

        private void btn_Pen_Click(object sender, EventArgs e)
        {
            //chọn lại màu và size trước khi chuyển sang chế độ tẩy
            currentColor = lastUsedColor;
            currentSize = lastUsedSize;
        }

        public async Task ReplayDrawingAsync(List<Stroke> strokesToReplay)
        {
            // Tạm thời vô hiệu hóa việc người dùng vẽ bậy vào màn hình lúc đang chiếu lại
            picCanvas.Enabled = false;

            // Xóa sạch bảng vẽ hiện tại
            allStrokes.Clear();
            picCanvas.Invalidate();

            // Vòng lặp tua lại
            foreach (var originalStroke in strokesToReplay)
            {
                Stroke replayStroke = new Stroke
                {
                    StrokeColor = originalStroke.StrokeColor,
                    StrokeSize = originalStroke.StrokeSize
                };
                allStrokes.Add(replayStroke);

                foreach (var point in originalStroke.Points)
                {
                    replayStroke.Points.Add(point);
                    picCanvas.Invalidate();

                    
                    await Task.Delay(1);
                }
            }

            // Mở khóa lại bảng vẽ sau khi chiếu xong
            picCanvas.Enabled = true;
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (allStrokes.Count == 0)
            {
                MessageBox.Show("Bạn phải vẽ gì đó lên bảng trước đã!", "Thông báo");
                return;
            }
            string jsonClone = JsonConvert.SerializeObject(allStrokes);
            List<Stroke> clonedStrokes = JsonConvert.DeserializeObject<List<Stroke>>(jsonClone);

            // Bắt đầu gọi hàm chiếu lại với bản sao vừa tạo
            await ReplayDrawingAsync(clonedStrokes);
            allStrokes.Clear();
            picCanvas.Invalidate();
            // Thông báo khi chiếu xong (Đoạn này sau này sẽ dùng để chuyển lượt chơi)
            MessageBox.Show("Đã diễn hoạ xong quá trình vẽ của bạn!", "Hoàn tất");
        }
        // Hàm Replay độc lập (để dùng chung cho mọi trường hợp sau này)
        // các hàm kết nối mạng, gửi gói tin
        private async void ConnectToLoadBalancer()
        {
            try
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(LB_IP, LB_PORT);

                NetworkStream stream = tcpClient.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                isConnected = true;

                // Gửi lệnh xin vào phòng ngay khi kết nối
                SendPacket("JOIN_ROOM", new
                {
                    RoomId = "12345",
                    Username = "Player_" + new Random().Next(100, 999),
                    AvatarBase64 = ""
                });

                // Lắng nghe dữ liệu
                _ = Task.Run(() => ListenForServerData());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối Load Balancer: " + ex.Message);
            }
        }
        // --- HÀM BỌC VÀ GỬI GÓI TIN ---
        private void SendPacket(string type, object payload)
        {
            if (!isConnected) return;
            try
            {
                var packet = new BasePacket
                {
                    Type = type,
                    Seq = sequenceNumber++,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Token = userToken,
                    Payload = payload
                };
                writer.WriteLine(JsonConvert.SerializeObject(packet));
            }
            catch { /* Bỏ qua lỗi rớt mạng cục bộ */ }
        }
        private async Task ListenForServerData()
        {
            try
            {
                while (isConnected && tcpClient != null && tcpClient.Connected)
                {
                    string jsonResponse = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        this.Invoke(new Action(() => ProcessReceivedPacket(jsonResponse)));
                    }
                }
            }
            catch { isConnected = false; }
        }
        private async void ProcessReceivedPacket(string json)
        {
            try
            {
                JObject packet = JObject.Parse(json);
                string type = packet["Type"].ToString();
                var payload = packet["Payload"];

                switch (type)
                {
                    case "SYNC_CANVAS":
                        List<Stroke> currentBoard = payload["Strokes"].ToObject<List<Stroke>>();
                        await ReplayDrawingAsync(currentBoard);
                        break;
                    case "DRAW":
                        HandleIncomingDraw(payload);
                        break;
                    case "UNDO":
                        string strokeIdToRemove = payload["StrokeId"].ToString();
                        allStrokes.RemoveAll(s => s.StrokeId == strokeIdToRemove);
                        picCanvas.Invalidate();
                        break;
                    case "CLEAR_CANVAS":
                        allStrokes.Clear();
                        picCanvas.Invalidate();
                        break;
                    case "ERROR":
                        MessageBox.Show(payload["Message"].ToString(), "Lỗi từ Server");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Thay vì bỏ qua, hãy in ra để debug
                Console.WriteLine("Lỗi đọc gói tin: " + ex.Message);
            }
        }
        private void HandleIncomingDraw(JToken payload)
        {
            string strokeId = payload["StrokeId"].ToString();
            string phase = payload["Phase"].ToString();

            // SỬA LỖI 1: Tự map dữ liệu X, Y sang kiểu Point của C# thay vì dùng ToObject tự động
            List<Point> incomingPoints = payload["Points"]
                .Select(p => new Point((int)p["X"], (int)p["Y"]))
                .ToList();

            if (phase == "start")
            {
                Stroke newStroke = new Stroke
                {
                    StrokeId = strokeId,
                    StrokeColor = ColorTranslator.FromHtml(payload["Color"].ToString()),
                    // SỬA LỖI 2: Lấy giá trị int an toàn từ JToken
                    StrokeSize = payload["Size"].Value<int>()
                };
                newStroke.Points.AddRange(incomingPoints);
                networkActiveStrokes[strokeId] = newStroke;
            }
            else if (phase == "move" && networkActiveStrokes.ContainsKey(strokeId))
            {
                networkActiveStrokes[strokeId].Points.AddRange(incomingPoints);
            }
            else if (phase == "end" && networkActiveStrokes.ContainsKey(strokeId))
            {
                Stroke finishedStroke = networkActiveStrokes[strokeId];
                finishedStroke.Points.AddRange(incomingPoints);
                allStrokes.Add(finishedStroke);
                networkActiveStrokes.Remove(strokeId);
            }

            picCanvas.Invalidate();
        }
        private void FlushDrawBuffer(string phase)
        {
            var payload = new
            {
                StrokeId = currentStrokeId,
                Phase = phase,
                Color = ColorTranslator.ToHtml(currentColor),
                Size = currentSize,
                Points = pointBuffer.Select(p => new { X = p.X, Y = p.Y }).ToList()
            };

            SendPacket("DRAW", payload);
            pointBuffer.Clear();
        }
    }
    // Lớp Stroke để lưu trữ thông tin về mỗi nét vẽ, bao gồm màu sắc, kích thước và danh sách các điểm tạo nên nét đó
    public class BasePacket
    {
        public string Type { get; set; }
        public int Seq { get; set; }
        public long Timestamp { get; set; }
        public string Token { get; set; }
        public object Payload { get; set; }
    }

    public class Stroke
    {
        public string StrokeId { get; set; } // THÊM MỚI: ID để phân biệt nét vẽ
        public Color StrokeColor { get; set; }
        public int StrokeSize { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
    }
}
