namespace DrawingClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_ClearCanvas = new System.Windows.Forms.Button();
            this.btn_Erase = new System.Windows.Forms.Button();
            this.btn_Color = new System.Windows.Forms.Button();
            this.btn_Undo = new System.Windows.Forms.Button();
            this.btn_Pen = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.roundedPanel2 = new RoundedPanel();
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.roundedPanel1 = new RoundedPanel();
            this.player2 = new DrawingClient.Player();
            this.player1 = new DrawingClient.Player();
            this.roundedPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.roundedPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::DrawingClient.Properties.Resources.cd86ed96_c744_4e99_b8f8_3cb857b395bc;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(430, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 70);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::DrawingClient.Properties.Resources.bd4ff88a_f5e3_4d9b_8406_988238df7564;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(521, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 70);
            this.button2.TabIndex = 2;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::DrawingClient.Properties.Resources._39855636_9061_4073_8e25_d84a87561568;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(1624, 116);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(70, 70);
            this.button3.TabIndex = 3;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::DrawingClient.Properties.Resources.ba5b0b88_e6cd_47ce_9499_67eb5fc47a6c;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(1725, 116);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(70, 70);
            this.button4.TabIndex = 4;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btn_ClearCanvas
            // 
            this.btn_ClearCanvas.Location = new System.Drawing.Point(635, 116);
            this.btn_ClearCanvas.Name = "btn_ClearCanvas";
            this.btn_ClearCanvas.Size = new System.Drawing.Size(167, 45);
            this.btn_ClearCanvas.TabIndex = 6;
            this.btn_ClearCanvas.Text = "Xoá bảng";
            this.btn_ClearCanvas.UseVisualStyleBackColor = true;
            this.btn_ClearCanvas.Click += new System.EventHandler(this.btn_ClearCanvas_Click);
            // 
            // btn_Erase
            // 
            this.btn_Erase.Location = new System.Drawing.Point(851, 123);
            this.btn_Erase.Name = "btn_Erase";
            this.btn_Erase.Size = new System.Drawing.Size(159, 37);
            this.btn_Erase.TabIndex = 7;
            this.btn_Erase.Text = "Tẩy";
            this.btn_Erase.UseVisualStyleBackColor = true;
            this.btn_Erase.Click += new System.EventHandler(this.btn_Erase_Click);
            // 
            // btn_Color
            // 
            this.btn_Color.Location = new System.Drawing.Point(1113, 100);
            this.btn_Color.Name = "btn_Color";
            this.btn_Color.Size = new System.Drawing.Size(107, 60);
            this.btn_Color.TabIndex = 8;
            this.btn_Color.Text = "Chọn màu";
            this.btn_Color.UseVisualStyleBackColor = true;
            this.btn_Color.Click += new System.EventHandler(this.btn_Color_Click);
            // 
            // btn_Undo
            // 
            this.btn_Undo.Location = new System.Drawing.Point(1344, 100);
            this.btn_Undo.Name = "btn_Undo";
            this.btn_Undo.Size = new System.Drawing.Size(111, 52);
            this.btn_Undo.TabIndex = 9;
            this.btn_Undo.Text = "Undo";
            this.btn_Undo.UseVisualStyleBackColor = true;
            this.btn_Undo.Click += new System.EventHandler(this.btn_Undo_Click);
            this.btn_Undo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_Undo_KeyDown);
            // 
            // btn_Pen
            // 
            this.btn_Pen.Location = new System.Drawing.Point(834, 52);
            this.btn_Pen.Name = "btn_Pen";
            this.btn_Pen.Size = new System.Drawing.Size(105, 35);
            this.btn_Pen.TabIndex = 10;
            this.btn_Pen.Text = "Pen";
            this.btn_Pen.UseVisualStyleBackColor = true;
            this.btn_Pen.Click += new System.EventHandler(this.btn_Pen_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(1504, 111);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(64, 74);
            this.btn_Save.TabIndex = 11;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // roundedPanel2
            // 
            this.roundedPanel2.BorderRadius = 40;
            this.roundedPanel2.Controls.Add(this.picCanvas);
            this.roundedPanel2.Location = new System.Drawing.Point(430, 197);
            this.roundedPanel2.Name = "roundedPanel2";
            this.roundedPanel2.Size = new System.Drawing.Size(1364, 503);
            this.roundedPanel2.TabIndex = 5;
            // 
            // picCanvas
            // 
            this.picCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCanvas.Location = new System.Drawing.Point(0, 0);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(1364, 503);
            this.picCanvas.TabIndex = 0;
            this.picCanvas.TabStop = false;
            this.picCanvas.Click += new System.EventHandler(this.pictureBox1_Click);
            this.picCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.picCanvas_Paint);
            this.picCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseDown);
            this.picCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseMove);
            this.picCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseUp);
            // 
            // roundedPanel1
            // 
            this.roundedPanel1.BackColor = System.Drawing.Color.Transparent;
            this.roundedPanel1.BorderRadius = 26;
            this.roundedPanel1.Controls.Add(this.player2);
            this.roundedPanel1.Controls.Add(this.player1);
            this.roundedPanel1.Location = new System.Drawing.Point(105, 198);
            this.roundedPanel1.Name = "roundedPanel1";
            this.roundedPanel1.Size = new System.Drawing.Size(293, 795);
            this.roundedPanel1.TabIndex = 0;
            // 
            // player2
            // 
            this.player2.BackColor = System.Drawing.Color.Transparent;
            this.player2.Location = new System.Drawing.Point(18, 99);
            this.player2.Name = "player2";
            this.player2.Size = new System.Drawing.Size(260, 65);
            this.player2.TabIndex = 1;
            // 
            // player1
            // 
            this.player1.BackColor = System.Drawing.Color.Transparent;
            this.player1.Location = new System.Drawing.Point(18, 14);
            this.player1.Name = "player1";
            this.player1.Size = new System.Drawing.Size(260, 65);
            this.player1.TabIndex = 0;
            this.player1.Load += new System.EventHandler(this.player1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DrawingClient.Properties.Resources.bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1898, 1024);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Pen);
            this.Controls.Add(this.btn_Undo);
            this.Controls.Add(this.btn_Color);
            this.Controls.Add(this.btn_Erase);
            this.Controls.Add(this.btn_ClearCanvas);
            this.Controls.Add(this.roundedPanel2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.roundedPanel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.roundedPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.roundedPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RoundedPanel roundedPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private Player player1;
        private Player player2;
        private RoundedPanel roundedPanel2;
        private System.Windows.Forms.PictureBox picCanvas;
        private System.Windows.Forms.Button btn_ClearCanvas;
        private System.Windows.Forms.Button btn_Erase;
        private System.Windows.Forms.Button btn_Color;
        private System.Windows.Forms.Button btn_Undo;
        private System.Windows.Forms.Button btn_Pen;
        private System.Windows.Forms.Button btn_Save;
    }
}

