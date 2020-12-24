namespace MeasureModelDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.btn_OpenImage = new System.Windows.Forms.Button();
            this.btn_CreatShapeModel = new System.Windows.Forms.Button();
            this.btn_MeasureModel = new System.Windows.Forms.Button();
            this.btn_AdaptShow = new System.Windows.Forms.Button();
            this.btn_Scale1to1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBtnWhiteBack = new System.Windows.Forms.RadioButton();
            this.radioBtnBlackBack = new System.Windows.Forms.RadioButton();
            this.trackBarThreshold = new System.Windows.Forms.TrackBar();
            this.textBoxThreshold = new System.Windows.Forms.TextBox();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.btnModelFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreshold)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(12, 43);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(640, 480);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(640, 480);
            this.hWindowControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.HWindow_MouseWheel);
            // 
            // btn_OpenImage
            // 
            this.btn_OpenImage.Location = new System.Drawing.Point(12, 538);
            this.btn_OpenImage.Name = "btn_OpenImage";
            this.btn_OpenImage.Size = new System.Drawing.Size(117, 43);
            this.btn_OpenImage.TabIndex = 1;
            this.btn_OpenImage.Text = "打开图片";
            this.btn_OpenImage.UseVisualStyleBackColor = true;
            this.btn_OpenImage.Click += new System.EventHandler(this.btn_OpenImage_Click);
            // 
            // btn_CreatShapeModel
            // 
            this.btn_CreatShapeModel.Location = new System.Drawing.Point(78, 145);
            this.btn_CreatShapeModel.Name = "btn_CreatShapeModel";
            this.btn_CreatShapeModel.Size = new System.Drawing.Size(117, 35);
            this.btn_CreatShapeModel.TabIndex = 2;
            this.btn_CreatShapeModel.Text = "创建形状模板";
            this.btn_CreatShapeModel.UseVisualStyleBackColor = true;
            this.btn_CreatShapeModel.Click += new System.EventHandler(this.btn_CreatShapeModel_Click);
            // 
            // btn_MeasureModel
            // 
            this.btn_MeasureModel.Location = new System.Drawing.Point(145, 319);
            this.btn_MeasureModel.Name = "btn_MeasureModel";
            this.btn_MeasureModel.Size = new System.Drawing.Size(117, 35);
            this.btn_MeasureModel.TabIndex = 3;
            this.btn_MeasureModel.Text = "创建测量模板";
            this.btn_MeasureModel.UseVisualStyleBackColor = true;
            // 
            // btn_AdaptShow
            // 
            this.btn_AdaptShow.Location = new System.Drawing.Point(12, 12);
            this.btn_AdaptShow.Name = "btn_AdaptShow";
            this.btn_AdaptShow.Size = new System.Drawing.Size(97, 25);
            this.btn_AdaptShow.TabIndex = 4;
            this.btn_AdaptShow.Text = "适应窗口";
            this.btn_AdaptShow.UseVisualStyleBackColor = true;
            this.btn_AdaptShow.Click += new System.EventHandler(this.btn_AdaptShow_Click);
            // 
            // btn_Scale1to1
            // 
            this.btn_Scale1to1.Location = new System.Drawing.Point(555, 12);
            this.btn_Scale1to1.Name = "btn_Scale1to1";
            this.btn_Scale1to1.Size = new System.Drawing.Size(97, 25);
            this.btn_Scale1to1.TabIndex = 5;
            this.btn_Scale1to1.Text = "1:1";
            this.btn_Scale1to1.UseVisualStyleBackColor = true;
            this.btn_Scale1to1.Click += new System.EventHandler(this.btn_Scale1to1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBtnWhiteBack);
            this.groupBox1.Controls.Add(this.btn_CreatShapeModel);
            this.groupBox1.Controls.Add(this.radioBtnBlackBack);
            this.groupBox1.Controls.Add(this.trackBarThreshold);
            this.groupBox1.Controls.Add(this.textBoxThreshold);
            this.groupBox1.Controls.Add(this.labelThreshold);
            this.groupBox1.Location = new System.Drawing.Point(679, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 203);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "形状模板参数";
            // 
            // radioBtnWhiteBack
            // 
            this.radioBtnWhiteBack.AutoSize = true;
            this.radioBtnWhiteBack.Location = new System.Drawing.Point(165, 39);
            this.radioBtnWhiteBack.Name = "radioBtnWhiteBack";
            this.radioBtnWhiteBack.Size = new System.Drawing.Size(58, 19);
            this.radioBtnWhiteBack.TabIndex = 21;
            this.radioBtnWhiteBack.Text = "白底";
            this.radioBtnWhiteBack.UseVisualStyleBackColor = true;
            // 
            // radioBtnBlackBack
            // 
            this.radioBtnBlackBack.AutoSize = true;
            this.radioBtnBlackBack.Checked = true;
            this.radioBtnBlackBack.Location = new System.Drawing.Point(50, 39);
            this.radioBtnBlackBack.Name = "radioBtnBlackBack";
            this.radioBtnBlackBack.Size = new System.Drawing.Size(58, 19);
            this.radioBtnBlackBack.TabIndex = 20;
            this.radioBtnBlackBack.TabStop = true;
            this.radioBtnBlackBack.Text = "黑底";
            this.radioBtnBlackBack.UseVisualStyleBackColor = true;
            // 
            // trackBarThreshold
            // 
            this.trackBarThreshold.Location = new System.Drawing.Point(63, 82);
            this.trackBarThreshold.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarThreshold.Maximum = 255;
            this.trackBarThreshold.Name = "trackBarThreshold";
            this.trackBarThreshold.Size = new System.Drawing.Size(144, 56);
            this.trackBarThreshold.TabIndex = 17;
            this.trackBarThreshold.Scroll += new System.EventHandler(this.trackBarThreshold_Scroll);
            // 
            // textBoxThreshold
            // 
            this.textBoxThreshold.Location = new System.Drawing.Point(215, 82);
            this.textBoxThreshold.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxThreshold.Name = "textBoxThreshold";
            this.textBoxThreshold.Size = new System.Drawing.Size(39, 25);
            this.textBoxThreshold.TabIndex = 18;
            this.textBoxThreshold.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxThreshold_KeyUp);
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(10, 85);
            this.labelThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(45, 15);
            this.labelThreshold.TabIndex = 19;
            this.labelThreshold.Text = "阈值:";
            // 
            // btnModelFile
            // 
            this.btnModelFile.Location = new System.Drawing.Point(505, 538);
            this.btnModelFile.Name = "btnModelFile";
            this.btnModelFile.Size = new System.Drawing.Size(147, 43);
            this.btnModelFile.TabIndex = 7;
            this.btnModelFile.Text = "打开模板文件夹";
            this.btnModelFile.UseVisualStyleBackColor = true;
            this.btnModelFile.Click += new System.EventHandler(this.btnModelFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.btn_MeasureModel);
            this.groupBox2.Location = new System.Drawing.Point(679, 221);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 360);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测量模板参数";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MeasureModelDemo.Properties.Resources.Disable;
            this.pictureBox1.Location = new System.Drawing.Point(13, 25);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(78, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 593);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnModelFile);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_Scale1to1);
            this.Controls.Add(this.btn_AdaptShow);
            this.Controls.Add(this.btn_OpenImage);
            this.Controls.Add(this.hWindowControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MeasureModel";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreshold)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Button btn_OpenImage;
        private System.Windows.Forms.Button btn_CreatShapeModel;
        private System.Windows.Forms.Button btn_MeasureModel;
        private System.Windows.Forms.Button btn_AdaptShow;
        private System.Windows.Forms.Button btn_Scale1to1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBarThreshold;
        private System.Windows.Forms.TextBox textBoxThreshold;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.RadioButton radioBtnBlackBack;
        private System.Windows.Forms.RadioButton radioBtnWhiteBack;
        private System.Windows.Forms.Button btnModelFile;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.PictureBox pictureBox1;
    }
}

