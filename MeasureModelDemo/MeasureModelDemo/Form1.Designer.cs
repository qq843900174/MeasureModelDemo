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
            this.hWindowControl1 = new ChoiceTech.Halcon.Control.HWindow_Final();
            this.btn_OpenImage = new System.Windows.Forms.Button();
            this.btn_CreatShapeModel = new System.Windows.Forms.Button();
            this.btn_MeasureModel = new System.Windows.Forms.Button();
            this.groupBoxShapeModelParams = new System.Windows.Forms.GroupBox();
            this.checkBoxSetROI = new System.Windows.Forms.CheckBox();
            this.radioBtnWhiteBack = new System.Windows.Forms.RadioButton();
            this.radioBtnBlackBack = new System.Windows.Forms.RadioButton();
            this.trackBarThreshold = new System.Windows.Forms.TrackBar();
            this.textBoxThreshold = new System.Windows.Forms.TextBox();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.btnModelFile = new System.Windows.Forms.Button();
            this.groupBoxMeasrueModelParams = new System.Windows.Forms.GroupBox();
            this.checkBoxDisplayMeasureTool = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayResultCont = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayFeature = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayCaliper = new System.Windows.Forms.CheckBox();
            this.comboBox_MeasureTool = new System.Windows.Forms.ComboBox();
            this.pictureBox_MeasureToolEnable = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonTest = new System.Windows.Forms.Button();
            this.groupBoxReadModel = new System.Windows.Forms.GroupBox();
            this.buttonReadModel = new System.Windows.Forms.Button();
            this.textBoxMaterialName = new System.Windows.Forms.TextBox();
            this.labelMaterialName = new System.Windows.Forms.Label();
            this.groupBoxTestResult = new System.Windows.Forms.GroupBox();
            this.textBoxAngle = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.labelResultAngle = new System.Windows.Forms.Label();
            this.labelResultY = new System.Windows.Forms.Label();
            this.labelResultX = new System.Windows.Forms.Label();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.btnDispOriginImage = new System.Windows.Forms.Button();
            this.groupBoxShapeModelParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreshold)).BeginInit();
            this.groupBoxMeasrueModelParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MeasureToolEnable)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBoxReadModel.SuspendLayout();
            this.groupBoxTestResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hWindowControl1.DrawModel = false;
            this.hWindowControl1.Image = null;
            this.hWindowControl1.Location = new System.Drawing.Point(12, 57);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(640, 480);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.Load += new System.EventHandler(this.Form1_Load);
            // 
            // btn_OpenImage
            // 
            this.btn_OpenImage.Location = new System.Drawing.Point(13, 544);
            this.btn_OpenImage.Name = "btn_OpenImage";
            this.btn_OpenImage.Size = new System.Drawing.Size(117, 43);
            this.btn_OpenImage.TabIndex = 1;
            this.btn_OpenImage.Text = "打开图片";
            this.btn_OpenImage.UseVisualStyleBackColor = true;
            this.btn_OpenImage.Click += new System.EventHandler(this.btn_OpenImage_Click);
            // 
            // btn_CreatShapeModel
            // 
            this.btn_CreatShapeModel.Location = new System.Drawing.Point(137, 145);
            this.btn_CreatShapeModel.Name = "btn_CreatShapeModel";
            this.btn_CreatShapeModel.Size = new System.Drawing.Size(117, 35);
            this.btn_CreatShapeModel.TabIndex = 2;
            this.btn_CreatShapeModel.Text = "创建形状模板";
            this.btn_CreatShapeModel.UseVisualStyleBackColor = true;
            this.btn_CreatShapeModel.Click += new System.EventHandler(this.btn_CreatShapeModel_Click);
            // 
            // btn_MeasureModel
            // 
            this.btn_MeasureModel.Location = new System.Drawing.Point(78, 283);
            this.btn_MeasureModel.Name = "btn_MeasureModel";
            this.btn_MeasureModel.Size = new System.Drawing.Size(117, 35);
            this.btn_MeasureModel.TabIndex = 3;
            this.btn_MeasureModel.Text = "创建测量模板";
            this.btn_MeasureModel.UseVisualStyleBackColor = true;
            this.btn_MeasureModel.Click += new System.EventHandler(this.btn_MeasureModel_Click);
            // 
            // groupBoxShapeModelParams
            // 
            this.groupBoxShapeModelParams.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxShapeModelParams.Controls.Add(this.checkBoxSetROI);
            this.groupBoxShapeModelParams.Controls.Add(this.radioBtnWhiteBack);
            this.groupBoxShapeModelParams.Controls.Add(this.btn_CreatShapeModel);
            this.groupBoxShapeModelParams.Controls.Add(this.radioBtnBlackBack);
            this.groupBoxShapeModelParams.Controls.Add(this.trackBarThreshold);
            this.groupBoxShapeModelParams.Controls.Add(this.textBoxThreshold);
            this.groupBoxShapeModelParams.Controls.Add(this.labelThreshold);
            this.groupBoxShapeModelParams.Location = new System.Drawing.Point(6, 6);
            this.groupBoxShapeModelParams.Name = "groupBoxShapeModelParams";
            this.groupBoxShapeModelParams.Size = new System.Drawing.Size(268, 203);
            this.groupBoxShapeModelParams.TabIndex = 6;
            this.groupBoxShapeModelParams.TabStop = false;
            this.groupBoxShapeModelParams.Text = "形状模板参数";
            // 
            // checkBoxSetROI
            // 
            this.checkBoxSetROI.AutoSize = true;
            this.checkBoxSetROI.Location = new System.Drawing.Point(13, 154);
            this.checkBoxSetROI.Name = "checkBoxSetROI";
            this.checkBoxSetROI.Size = new System.Drawing.Size(83, 19);
            this.checkBoxSetROI.TabIndex = 22;
            this.checkBoxSetROI.Text = "设置ROI";
            this.checkBoxSetROI.UseVisualStyleBackColor = true;
            this.checkBoxSetROI.CheckedChanged += new System.EventHandler(this.checkBoxSetROI_CheckedChanged);
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
            this.btnModelFile.Location = new System.Drawing.Point(506, 544);
            this.btnModelFile.Name = "btnModelFile";
            this.btnModelFile.Size = new System.Drawing.Size(147, 43);
            this.btnModelFile.TabIndex = 7;
            this.btnModelFile.Text = "打开模板文件夹";
            this.btnModelFile.UseVisualStyleBackColor = true;
            this.btnModelFile.Click += new System.EventHandler(this.btnModelFile_Click);
            // 
            // groupBoxMeasrueModelParams
            // 
            this.groupBoxMeasrueModelParams.Controls.Add(this.checkBoxDisplayMeasureTool);
            this.groupBoxMeasrueModelParams.Controls.Add(this.checkBoxDisplayResultCont);
            this.groupBoxMeasrueModelParams.Controls.Add(this.checkBoxDisplayFeature);
            this.groupBoxMeasrueModelParams.Controls.Add(this.checkBoxDisplayCaliper);
            this.groupBoxMeasrueModelParams.Controls.Add(this.comboBox_MeasureTool);
            this.groupBoxMeasrueModelParams.Controls.Add(this.pictureBox_MeasureToolEnable);
            this.groupBoxMeasrueModelParams.Controls.Add(this.btn_MeasureModel);
            this.groupBoxMeasrueModelParams.Location = new System.Drawing.Point(6, 226);
            this.groupBoxMeasrueModelParams.Name = "groupBoxMeasrueModelParams";
            this.groupBoxMeasrueModelParams.Size = new System.Drawing.Size(268, 322);
            this.groupBoxMeasrueModelParams.TabIndex = 8;
            this.groupBoxMeasrueModelParams.TabStop = false;
            this.groupBoxMeasrueModelParams.Text = "测量模板参数";
            // 
            // checkBoxDisplayMeasureTool
            // 
            this.checkBoxDisplayMeasureTool.AutoSize = true;
            this.checkBoxDisplayMeasureTool.Location = new System.Drawing.Point(78, 245);
            this.checkBoxDisplayMeasureTool.Name = "checkBoxDisplayMeasureTool";
            this.checkBoxDisplayMeasureTool.Size = new System.Drawing.Size(119, 19);
            this.checkBoxDisplayMeasureTool.TabIndex = 9;
            this.checkBoxDisplayMeasureTool.Text = "显示测量形状";
            this.checkBoxDisplayMeasureTool.UseVisualStyleBackColor = true;
            this.checkBoxDisplayMeasureTool.CheckedChanged += new System.EventHandler(this.checkBoxDisplayMeasureToolCheckedChanged);
            // 
            // checkBoxDisplayResultCont
            // 
            this.checkBoxDisplayResultCont.AutoSize = true;
            this.checkBoxDisplayResultCont.Location = new System.Drawing.Point(78, 210);
            this.checkBoxDisplayResultCont.Name = "checkBoxDisplayResultCont";
            this.checkBoxDisplayResultCont.Size = new System.Drawing.Size(119, 19);
            this.checkBoxDisplayResultCont.TabIndex = 8;
            this.checkBoxDisplayResultCont.Text = "显示结果轮廓";
            this.checkBoxDisplayResultCont.UseVisualStyleBackColor = true;
            this.checkBoxDisplayResultCont.CheckedChanged += new System.EventHandler(this.checkBoxDisplayResultContCheckedChanged);
            // 
            // checkBoxDisplayFeature
            // 
            this.checkBoxDisplayFeature.AutoSize = true;
            this.checkBoxDisplayFeature.Location = new System.Drawing.Point(78, 171);
            this.checkBoxDisplayFeature.Name = "checkBoxDisplayFeature";
            this.checkBoxDisplayFeature.Size = new System.Drawing.Size(104, 19);
            this.checkBoxDisplayFeature.TabIndex = 7;
            this.checkBoxDisplayFeature.Text = "显示特征点";
            this.checkBoxDisplayFeature.UseVisualStyleBackColor = true;
            this.checkBoxDisplayFeature.CheckedChanged += new System.EventHandler(this.checkBoxDisplayFeatureCheckedChanged);
            // 
            // checkBoxDisplayCaliper
            // 
            this.checkBoxDisplayCaliper.AutoSize = true;
            this.checkBoxDisplayCaliper.Location = new System.Drawing.Point(78, 132);
            this.checkBoxDisplayCaliper.Name = "checkBoxDisplayCaliper";
            this.checkBoxDisplayCaliper.Size = new System.Drawing.Size(89, 19);
            this.checkBoxDisplayCaliper.TabIndex = 6;
            this.checkBoxDisplayCaliper.Text = "显示卡尺";
            this.checkBoxDisplayCaliper.UseVisualStyleBackColor = true;
            this.checkBoxDisplayCaliper.CheckedChanged += new System.EventHandler(this.checkBoxDisplayCaliperCheckedChanged);
            // 
            // comboBox_MeasureTool
            // 
            this.comboBox_MeasureTool.FormattingEnabled = true;
            this.comboBox_MeasureTool.Location = new System.Drawing.Point(50, 88);
            this.comboBox_MeasureTool.Name = "comboBox_MeasureTool";
            this.comboBox_MeasureTool.Size = new System.Drawing.Size(157, 23);
            this.comboBox_MeasureTool.TabIndex = 5;
            this.comboBox_MeasureTool.SelectedIndexChanged += new System.EventHandler(this.comboBox_MeasureTool_Changed);
            // 
            // pictureBox_MeasureToolEnable
            // 
            this.pictureBox_MeasureToolEnable.Image = global::MeasureModelDemo.Properties.Resources.Disable;
            this.pictureBox_MeasureToolEnable.Location = new System.Drawing.Point(13, 25);
            this.pictureBox_MeasureToolEnable.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_MeasureToolEnable.Name = "pictureBox_MeasureToolEnable";
            this.pictureBox_MeasureToolEnable.Size = new System.Drawing.Size(78, 37);
            this.pictureBox_MeasureToolEnable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_MeasureToolEnable.TabIndex = 4;
            this.pictureBox_MeasureToolEnable.TabStop = false;
            this.pictureBox_MeasureToolEnable.Click += new System.EventHandler(this.pictureBox1_MearsureEnable_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(659, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(289, 583);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.groupBoxMeasrueModelParams);
            this.tabPage1.Controls.Add(this.groupBoxShapeModelParams);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(281, 554);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "模板参数";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.buttonTest);
            this.tabPage2.Controls.Add(this.groupBoxReadModel);
            this.tabPage2.Controls.Add(this.groupBoxTestResult);
            this.tabPage2.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(281, 554);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "运行测试";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(92, 490);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(104, 43);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "测试";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // groupBoxReadModel
            // 
            this.groupBoxReadModel.Controls.Add(this.buttonReadModel);
            this.groupBoxReadModel.Controls.Add(this.textBoxMaterialName);
            this.groupBoxReadModel.Controls.Add(this.labelMaterialName);
            this.groupBoxReadModel.Location = new System.Drawing.Point(6, 6);
            this.groupBoxReadModel.Name = "groupBoxReadModel";
            this.groupBoxReadModel.Size = new System.Drawing.Size(269, 172);
            this.groupBoxReadModel.TabIndex = 3;
            this.groupBoxReadModel.TabStop = false;
            this.groupBoxReadModel.Text = "模板读取";
            // 
            // buttonReadModel
            // 
            this.buttonReadModel.Location = new System.Drawing.Point(86, 98);
            this.buttonReadModel.Name = "buttonReadModel";
            this.buttonReadModel.Size = new System.Drawing.Size(104, 43);
            this.buttonReadModel.TabIndex = 5;
            this.buttonReadModel.Text = "读取模板";
            this.buttonReadModel.UseVisualStyleBackColor = true;
            this.buttonReadModel.Click += new System.EventHandler(this.buttonReadModel_Click);
            // 
            // textBoxMaterialName
            // 
            this.textBoxMaterialName.Location = new System.Drawing.Point(92, 36);
            this.textBoxMaterialName.Name = "textBoxMaterialName";
            this.textBoxMaterialName.Size = new System.Drawing.Size(108, 25);
            this.textBoxMaterialName.TabIndex = 3;
            this.textBoxMaterialName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelMaterialName
            // 
            this.labelMaterialName.AutoSize = true;
            this.labelMaterialName.Location = new System.Drawing.Point(11, 39);
            this.labelMaterialName.Name = "labelMaterialName";
            this.labelMaterialName.Size = new System.Drawing.Size(75, 15);
            this.labelMaterialName.TabIndex = 2;
            this.labelMaterialName.Text = "物料名称:";
            // 
            // groupBoxTestResult
            // 
            this.groupBoxTestResult.Controls.Add(this.textBoxResult);
            this.groupBoxTestResult.Controls.Add(this.labelResult);
            this.groupBoxTestResult.Controls.Add(this.textBoxAngle);
            this.groupBoxTestResult.Controls.Add(this.textBoxY);
            this.groupBoxTestResult.Controls.Add(this.textBoxX);
            this.groupBoxTestResult.Controls.Add(this.labelResultAngle);
            this.groupBoxTestResult.Controls.Add(this.labelResultY);
            this.groupBoxTestResult.Controls.Add(this.labelResultX);
            this.groupBoxTestResult.Location = new System.Drawing.Point(6, 184);
            this.groupBoxTestResult.Name = "groupBoxTestResult";
            this.groupBoxTestResult.Size = new System.Drawing.Size(269, 216);
            this.groupBoxTestResult.TabIndex = 1;
            this.groupBoxTestResult.TabStop = false;
            this.groupBoxTestResult.Text = "测试结果";
            // 
            // textBoxAngle
            // 
            this.textBoxAngle.Location = new System.Drawing.Point(86, 173);
            this.textBoxAngle.Name = "textBoxAngle";
            this.textBoxAngle.ReadOnly = true;
            this.textBoxAngle.Size = new System.Drawing.Size(98, 25);
            this.textBoxAngle.TabIndex = 6;
            this.textBoxAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(86, 130);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.ReadOnly = true;
            this.textBoxY.Size = new System.Drawing.Size(98, 25);
            this.textBoxY.TabIndex = 5;
            this.textBoxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(86, 87);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.ReadOnly = true;
            this.textBoxX.Size = new System.Drawing.Size(98, 25);
            this.textBoxX.TabIndex = 4;
            this.textBoxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelResultAngle
            // 
            this.labelResultAngle.AutoSize = true;
            this.labelResultAngle.Location = new System.Drawing.Point(35, 176);
            this.labelResultAngle.Name = "labelResultAngle";
            this.labelResultAngle.Size = new System.Drawing.Size(45, 15);
            this.labelResultAngle.TabIndex = 2;
            this.labelResultAngle.Text = "角度:";
            // 
            // labelResultY
            // 
            this.labelResultY.AutoSize = true;
            this.labelResultY.Location = new System.Drawing.Point(57, 133);
            this.labelResultY.Name = "labelResultY";
            this.labelResultY.Size = new System.Drawing.Size(23, 15);
            this.labelResultY.TabIndex = 1;
            this.labelResultY.Text = "Y:";
            // 
            // labelResultX
            // 
            this.labelResultX.AutoSize = true;
            this.labelResultX.Location = new System.Drawing.Point(57, 90);
            this.labelResultX.Name = "labelResultX";
            this.labelResultX.Size = new System.Drawing.Size(23, 15);
            this.labelResultX.TabIndex = 0;
            this.labelResultX.Text = "X:";
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(86, 44);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(98, 25);
            this.textBoxResult.TabIndex = 8;
            this.textBoxResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(35, 47);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(45, 15);
            this.labelResult.TabIndex = 7;
            this.labelResult.Text = "结果:";
            // 
            // btnDispOriginImage
            // 
            this.btnDispOriginImage.Location = new System.Drawing.Point(13, 12);
            this.btnDispOriginImage.Name = "btnDispOriginImage";
            this.btnDispOriginImage.Size = new System.Drawing.Size(116, 38);
            this.btnDispOriginImage.TabIndex = 10;
            this.btnDispOriginImage.Text = "显示原图";
            this.btnDispOriginImage.UseVisualStyleBackColor = true;
            this.btnDispOriginImage.Click += new System.EventHandler(this.btnDispOriginImage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 599);
            this.Controls.Add(this.btnDispOriginImage);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnModelFile);
            this.Controls.Add(this.btn_OpenImage);
            this.Controls.Add(this.hWindowControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MeasureModel";
            this.groupBoxShapeModelParams.ResumeLayout(false);
            this.groupBoxShapeModelParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreshold)).EndInit();
            this.groupBoxMeasrueModelParams.ResumeLayout(false);
            this.groupBoxMeasrueModelParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MeasureToolEnable)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBoxReadModel.ResumeLayout(false);
            this.groupBoxReadModel.PerformLayout();
            this.groupBoxTestResult.ResumeLayout(false);
            this.groupBoxTestResult.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ChoiceTech.Halcon.Control.HWindow_Final hWindowControl1;
        private System.Windows.Forms.Button btn_OpenImage;
        private System.Windows.Forms.Button btn_CreatShapeModel;
        private System.Windows.Forms.Button btn_MeasureModel;
        private System.Windows.Forms.GroupBox groupBoxShapeModelParams;
        private System.Windows.Forms.TrackBar trackBarThreshold;
        private System.Windows.Forms.TextBox textBoxThreshold;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.RadioButton radioBtnBlackBack;
        private System.Windows.Forms.RadioButton radioBtnWhiteBack;
        private System.Windows.Forms.Button btnModelFile;
        private System.Windows.Forms.GroupBox groupBoxMeasrueModelParams;
        public System.Windows.Forms.PictureBox pictureBox_MeasureToolEnable;
        private System.Windows.Forms.ComboBox comboBox_MeasureTool;
        private System.Windows.Forms.CheckBox checkBoxDisplayCaliper;
        private System.Windows.Forms.CheckBox checkBoxDisplayResultCont;
        private System.Windows.Forms.CheckBox checkBoxDisplayFeature;
        private System.Windows.Forms.CheckBox checkBoxDisplayMeasureTool;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBoxTestResult;
        private System.Windows.Forms.Label labelResultX;
        private System.Windows.Forms.Label labelResultAngle;
        private System.Windows.Forms.Label labelResultY;
        private System.Windows.Forms.GroupBox groupBoxReadModel;
        private System.Windows.Forms.Label labelMaterialName;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.TextBox textBoxMaterialName;
        private System.Windows.Forms.TextBox textBoxAngle;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.Button buttonReadModel;
        private System.Windows.Forms.CheckBox checkBoxSetROI;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button btnDispOriginImage;
    }
}

