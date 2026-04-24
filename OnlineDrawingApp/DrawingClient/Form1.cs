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
namespace DrawingClient
{
    
    public partial class Form1 : Form
    {
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
        
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        { 
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        private void player1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Chỉ thêm điểm vào nét vẽ hiện tại nếu đang trong trạng thái vẽ (isDrawing = true), sau khi mousedown(nhấn chuột trái)
            // Nhấn và di chuột
            if (isDrawing && currentStroke != null)
            {
                currentStroke.Points.Add(e.Location);
                picCanvas.Invalidate();
            }
        }

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            // Bắt đầu vẽ khi nhấn chuột trái
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                currentStroke = new Stroke
                {
                    StrokeColor = currentColor,
                    StrokeSize = currentSize
                };
                currentStroke.Points.Add(e.Location);
            }
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            // Kết thúc vẽ khi thả chuột trái
            if (isDrawing && currentStroke != null)
            {
                isDrawing = false;
                allStrokes.Add(currentStroke);
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
        }

        private void btn_ClearCanvas_Click(object sender, EventArgs e)
        {
            // Xóa toàn bộ phần tử trong List
            allStrokes.Clear();
            picCanvas.Invalidate();
        }

        private void btn_Undo_Click(object sender, EventArgs e)
        {
            if (allStrokes.Count > 0)
            {
                // Lấy vị trí của nét vẽ cuối cùng
                int lastIndex = allStrokes.Count - 1;
                // Xóa nét đó khỏi bộ nhớ
                allStrokes.RemoveAt(lastIndex);
                // Yêu cầu vẽ lại (lúc này nét cuối đã biến mất)
                picCanvas.Invalidate();
            }
        }

        private void btn_Erase_Click(object sender, EventArgs e)
        {
            // Đổi màu thành màu nền của PictureBox (hoặc Color.White)
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

        private void btn_Undo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                // Thực hiện logic Undo y hệt như nút btnUndo
                if (allStrokes.Count > 0)
                {
                    // Lấy vị trí của nét vẽ cuối cùng
                    int lastIndex = allStrokes.Count - 1;

                    // Xóa nét đó khỏi danh sách
                    allStrokes.RemoveAt(lastIndex);

                    // Yêu cầu vẽ lại để cập nhật màn hình
                    picCanvas.Invalidate();
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

        }
    }

    public class Stroke
    {
        public Color StrokeColor { get; set; }
        public int StrokeSize { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
    }
}
