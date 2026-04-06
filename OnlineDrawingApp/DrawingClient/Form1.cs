using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingClient
{
    public partial class Form1 : Form
    {
        // 1. Khai báo biến lưu kích thước gốc của Form
        private Rectangle originalFormSize;

        // 2. Khai báo các biến lưu kích thước gốc của các Control bạn muốn co giãn
        // Ví dụ: Avatar, Khung Vẽ, Khung Chat...
        private Rectangle originalAvatarRect;
        private Rectangle originalCanvasRect;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1280, 720);
        }

        private void resizeControl(Rectangle originalControlRect, Control control)
        {
            // Nếu form chưa kịp load mà đã chạy hàm này thì bỏ qua để tránh lỗi
            if (originalFormSize.Width == 0 || originalFormSize.Height == 0) return;

            // Tính tỉ lệ phóng to/thu nhỏ
            float xRatio = (float)this.Width / (float)originalFormSize.Width;
            float yRatio = (float)this.Height / (float)originalFormSize.Height;

            // Tính toán tọa độ và kích thước mới
            int newX = (int)(originalControlRect.Location.X * xRatio);
            int newY = (int)(originalControlRect.Location.Y * yRatio);
            int newWidth = (int)(originalControlRect.Width * xRatio);
            int newHeight = (int)(originalControlRect.Height * yRatio);

            // Áp dụng thông số mới cho Control
            control.Location = new Point(newX, newY);
            control.Size = new Size(newWidth, newHeight);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);

            // Lưu lại kích thước ban đầu của các Control
            originalAvatarRect = new Rectangle(roundedPanel1.Location.X, roundedPanel1.Location.Y, roundedPanel1.Width, roundedPanel1.Height);
            //originalCanvasRect = new Rectangle(pnlCanvas.Location.X, pnlCanvas.Location.Y, pnlCanvas.Width, pnlCanvas.Height);
            // Làm tương tự với các nút bấm hay panel khác...
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //resizeControl(originalAvatarRect, roundedPanel1);
        }

        private void player1_Load(object sender, EventArgs e)
        {

        }
    }
}
