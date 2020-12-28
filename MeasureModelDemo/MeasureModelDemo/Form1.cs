using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Data.OleDb;
using Microsoft.VisualBasic;
using ViewWindow;
using ChoiceTech;

namespace MeasureModelDemo
{
    public partial class Form1 : Form
    {
        //private static HWindow hwindow; //全局窗口变量
        public HObject m_OpenImage;//全局图像变量
        public HObject binObj, m_GrayImage, m_DealImage;

        public HTuple m_OpenImageWidth, m_OpenImageHeight;
        public HTuple m_HWindowWidth, m_HWindowHeight;
        public HTuple m_ImageScaleWidth, m_ImageScaleHeight;

        public HTuple ShapeModelID;//形状模板句柄
        public string ShapeModelName = "";

        public bool MeasureModelEnable = false;

        public string ImagePath = "";
        public double m_Factor = 1.0;
        int m_Threshold = 0;


        public Form1()
        {
            InitializeComponent();

            m_OpenImage = new HObject();
            HOperatorSet.GenEmptyObj(out m_OpenImage);
            m_GrayImage = new HObject();
            HOperatorSet.GenEmptyObj(out m_GrayImage);
            binObj = new HObject();
            HOperatorSet.GenEmptyObj(out binObj);
            m_DealImage = new HObject();
            HOperatorSet.GenEmptyObj(out m_DealImage);

            //hwindow = hWindowControl1.HalconWindow;//初始化窗口变量
            m_HWindowWidth = hWindowControl1.Size.Width;
            m_HWindowHeight = hWindowControl1.Size.Height;

            ShapeModelID = null;

            radioBtnBlackBack.Checked = true;
            radioBtnWhiteBack.Checked = false;
            //btn_CreatShapeModel.Enabled = false;
            //btn_MeasureModel.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;

            btn_AdaptShow.Enabled = false;
            btn_Scale1to1.Enabled = false;

            //添加测量工具形状类型
            comboBox_MeasureTool.Items.Add("矩形");
            comboBox_MeasureTool.Items.Add("圆形");
            comboBox_MeasureTool.Items.Add("直线");
            comboBox_MeasureTool.SelectedIndex = 0;
        }


        //ROI框架
        List<ViewWindow.Model.ROI> regions; //roi集合
        HObject ho_ModelImage;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.regions = new List<ViewWindow.Model.ROI>();

            ho_ModelImage = new HObject();
            hWindowControl1.hWindowControl.MouseUp += HWindow_MouseUp;
            //hWindowControl1.DrawModel = true;
        }

        private void HWindow_MouseUp(object sender, MouseEventArgs e)
        {
            int index;

            List<double> data;
            ViewWindow.Model.ROI roi = hWindowControl1.viewWindow.smallestActiveROI(out data, out index);

            if (index > -1)
            {
                string name = roi.GetType().Name;
                this.regions[index] = roi;
            }
            if(MeasureModelEnable)
            {
                applyMeasureTool();
            }
        }
        private void btn_OpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImagePath = openFileDialog1.FileName;
                m_OpenImageHeight = null;
                m_OpenImageWidth = null;
                HOperatorSet.GenEmptyObj(out m_OpenImage);
                //hwindow = hWindowControl1.HalconWindow;
                hWindowControl1.ClearWindow();
                HOperatorSet.ReadImage(out m_OpenImage, ImagePath);
                //ImagePath_state = 1;
                HOperatorSet.GetImageSize(m_OpenImage, out m_OpenImageWidth, out m_OpenImageHeight);
                //HOperatorSet.SetPart(hwindow, 0, 0, m_OpenImageHeight - 1, m_OpenImageWidth - 1);
                HOperatorSet.Rgb1ToGray(m_OpenImage, out m_GrayImage);
                hWindowControl1.HobjectToHimage(m_GrayImage);
                hWindowControl1.viewWindow.zoomWindowImage();
                hWindowControl1.viewWindow.displayImage(m_GrayImage);
                //hWindowControl1.DispObj(m_GrayImage);
                HOperatorSet.CopyImage(m_GrayImage, out m_DealImage);

                //btn_CreatShapeModel.Enabled = true;
                //btn_MeasureModel.Enabled = true;
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
        }


        #region 创建形状模板
        private void btn_CreatShapeModel_Click(object sender, EventArgs e)
        {
            if(binObj == null)
            {
                return;
            }
            else
            {
                HObject RegionClosing, RegionOpening, ConnectionRegions, SelectRegion, Image_Model;
                //HObject GrayImage, ImageRotate;
                HTuple R, C, P, L1, L2, rotateAngle;
                HOperatorSet.ClosingCircle(binObj, out RegionClosing, 13.5);
                HOperatorSet.OpeningCircle(RegionClosing, out RegionOpening, 40.5);
                HOperatorSet.Connection(RegionOpening, out ConnectionRegions);
                HOperatorSet.SelectShape(ConnectionRegions, out SelectRegion, "rectangularity", "and", 0.7, 1.0);
                HOperatorSet.SelectShape(SelectRegion, out SelectRegion, "area", "and", 0, m_OpenImageWidth * m_OpenImageWidth / 2);
                int num;
                num = SelectRegion.CountObj();
                if(num <= 0)
                {
                    MessageBox.Show("找不到矩形物料，请检查“黑底/白底”和“阈值”。", "错误：");
                    return;
                }
                //HOperatorSet.SmallestRectangle2(SelectRegion, out R, out C, out P, out L1, out L2);
                //HOperatorSet.TupleDeg(P, out rotateAngle);
                //HOperatorSet.Rgb1ToGray(m_OpenImage, out GrayImage);
                //HOperatorSet.RotateImage(GrayImage, out ImageRotate, -rotateAngle, "constant");

                HOperatorSet.GenContourRegionXld(SelectRegion, out Image_Model, "border");
                HOperatorSet.SmoothContoursXld(Image_Model, out Image_Model, 11);
                HOperatorSet.CreateShapeModelXld(Image_Model, "auto", 0, (new HTuple(360)).TupleRad(), "auto", "auto", "ignore_local_polarity", 5, out ShapeModelID);

                string materialName = Interaction.InputBox("请输入不包含中文的物料名称：", "形状模板名称", "");
                if(materialName == "")
                {
                    return;
                }
                ShapeModelName = materialName + "-ShapeModel";
                string ShapeModelPath = "..\\Debug\\" + ShapeModelName;
                //判断是否存在
                HOperatorSet.WriteShapeModel(ShapeModelID, ShapeModelPath);
                string WorkPath = System.IO.Directory.GetCurrentDirectory();
                MessageBox.Show("保存成功！保存路径：" + WorkPath + "\\" + ShapeModelName);
                hWindowControl1.HobjectToHimage(m_GrayImage);
                hWindowControl1.viewWindow.displayImage(m_GrayImage);
                //hWindowControl1.DispObj(m_GrayImage);
            }
            return;
        }

        private void trackBarThreshold_Scroll(object sender, EventArgs e)
        {
            {
                int val = 0;

                val = trackBarThreshold.Value;

                m_Threshold = val;
                textBoxThreshold.Text = m_Threshold.ToString();
                //m_IsBin = true;
                ThresholdImage(!radioBtnBlackBack.Checked, m_Threshold, m_Factor, true);

                return;
            }
        }

        private void textBoxThreshold_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int val = 0;

                val = int.Parse(textBoxThreshold.Text);

                if (val > 255)
                    val = 255;
                else if (val < 0)
                    val = 0;

                textBoxThreshold.Text = val.ToString();
                m_Threshold = val;
                trackBarThreshold.Value = m_Threshold;
                //m_IsBin = true;
                ThresholdImage(!radioBtnBlackBack.Checked, m_Threshold, m_Factor, true);
            }

            return;
        }
        #endregion

        #region 创建测量模板
        //打开测量工具
        private void pictureBox1_MearsureEnable_Click(object sender, EventArgs e)
        {
            switch (MeasureModelEnable)
            {
                case false:
                    pictureBox_MeasureToolEnable.Image = Properties.Resources.Enable;
                    if(m_OpenImage == null || m_OpenImage.CountObj() <= 0)
                    {
                        break; 
                    }
                    hWindowControl1.viewWindow.displayImage(m_GrayImage);
                    int MeasureToolType;
                    MeasureToolType = comboBox_MeasureTool.SelectedIndex;
                    switch (MeasureToolType)
                    {
                        case 0:
                            hWindowControl1.viewWindow.genRect2(200.0, 200.0, 0.0, 60.0, 30.0, ref this.regions);
                            this.regions.Last().Color = "blue";
                            break;
                        case 1:
                            hWindowControl1.viewWindow.genCircle(200.0, 200.0, 60.0, ref this.regions);
                            this.regions.Last().Color = "blue";
                            break;
                        case 2:
                            hWindowControl1.viewWindow.genLine(200.0, 150.0, 200.0, 300.0, ref this.regions);
                            this.regions.Last().Color = "blue";
                            break;
                    }
                    break;
                case true:
                    pictureBox_MeasureToolEnable.Image = Properties.Resources.Disable;
                    if (m_OpenImage == null || m_OpenImage.CountObj() <= 0)
                    {
                        break;
                    }
                    hWindowControl1.viewWindow.notDisplayRoi();
                    hWindowControl1.viewWindow.ClearWindow();
                    hWindowControl1.viewWindow.displayImage(m_GrayImage);
                    this.regions.Clear();
                    break;
            }
            MeasureModelEnable = !MeasureModelEnable;
        }

        //改变下拉列表
        private void comboBox_MeasureTool_Changed(object sender, EventArgs e)
        {
            if (MeasureModelEnable)
            {
                if (m_OpenImage == null || m_OpenImage.CountObj() <= 0)
                {
                    return;
                }
                hWindowControl1.viewWindow.notDisplayRoi();
                this.regions.Clear();
                int MeasureToolType;

                MeasureToolType = comboBox_MeasureTool.SelectedIndex;

                switch (MeasureToolType)
                {
                    case 0:
                        hWindowControl1.viewWindow.genRect2(200.0, 200.0, 0.0, 60.0, 30.0, ref this.regions);
                        this.regions.Last().Color = "blue";
                        break;
                    case 1:
                        hWindowControl1.viewWindow.genCircle(200.0, 200.0, 60.0, ref this.regions);
                        this.regions.Last().Color = "blue";
                        break;
                    case 2:
                        hWindowControl1.viewWindow.genLine(200.0, 150.0, 200.0, 300.0, ref this.regions);
                        this.regions.Last().Color = "blue";
                        break;
                }
            }
        }

        private void btn_MeasureModel_Click(object sender, EventArgs e)
        {
            applyMeasureTool();
        }

        //应用测量工具到ROI上
        #region 测量工具成员参数
        internal List<HTuple> newExpecCircleRow = new List<HTuple>(), newExpectCircleCol = new List<HTuple>(), newExpectCircleRadius = new List<HTuple>();
        /// <summary>
        /// 显示卡尺
        /// </summary>
        internal bool displayCaliper = true;
        /// <summary>
        /// 显示特征点
        /// </summary>
        internal bool displayFeature = true;
        /// <summary>
        /// 显示结果圆
        /// </summary>
        internal bool displayCircle = true;
        /// <summary>
        /// 显示结果圆圆心
        /// </summary>
        internal bool displayCircleCenter = true;
        /// <summary>
        /// 期望圆圆心行坐标
        /// </summary>
        internal HTuple expectCircleRow = 300;
        /// <summary>
        /// 期望圆圆心列坐标
        /// </summary>
        internal HTuple expectCircleCol = 300;
        /// <summary>
        /// 期望圆半径
        /// </summary>
        internal HTuple expectCircleRadius = 200;

        //internal List<double> ResultCircleRow = new List<double>();

        //internal List<double> ResultCircleCol = new List<double>();

        //internal List<double> ResultCircleRadius = new List<double>();

        /// <summary>
        /// 起始角度
        /// </summary>
        internal double startAngle = 10;
        /// <summary>
        /// 结束角度
        /// </summary>
        internal double endAngle = 360;
        /// <summary>
        /// 运行工具时是否刷新输入图像
        /// </summary>
        internal bool updateImage = false;
        /// <summary>
        /// 圆环径向长度
        /// </summary>
        internal int ringRadiusLength = 30;
        internal int caliperWidth = 5;
        /// <summary>
        /// 边阈值
        /// </summary>
        internal int threshold = 30;
        //internal List<XY> circleCenter = new List<XY>();
        internal int ignoreNum = 0;
        /// <summary>
        /// 卡尺
        /// </summary>
        internal HObject contours;
        /// <summary>
        /// 找边极性，从明到暗或从暗到明
        /// </summary>
        internal string polarity = "positive";
        internal string edgeSelect = "all";
        internal double minScore = 0.5;
        /// <summary>
        /// 卡尺数量
        /// </summary>
        internal int cliperNum = 30;
        #endregion
        private void applyMeasureTool()
        {
            if (this.regions.Count() <= 0 || m_OpenImage == null || m_OpenImage.CountObj() <= 0)
            {
                return;
            }

            //MessageBox.Show("ok");
            hWindowControl1.Focus();
            //////groupBox_tool.Enabled = false;

            HTuple hv_Button = null;
            HTuple hv_Row = null, hv_Column = null;
            HTuple areaBrush, rowBrush, columnBrush, homMat2D;


            HObject brush_region_affine = new HObject();
            HObject ho_Image = new HObject(m_GrayImage);
            try
            {
                HObject brush_region = null;
                brush_region = this.regions.Last().getRegion();
                HOperatorSet.AreaCenter(brush_region, out areaBrush, out rowBrush, out columnBrush);
                //显示
                //////hWindow_Final1.HobjectToHimage(shapeMatchTool.inputImage);

                HWindowControl temp = new HWindowControl();
                HTuple row2, col2, w, h;
                HTuple w1, h1;
                HOperatorSet.GetImageSize(m_GrayImage, out w1, out h1);
                hWindowControl1.viewWindow.ClearWindow();
                hWindowControl1.viewWindow.displayImage(m_GrayImage);

                HObject contours;
                HTuple row, col;

                newExpecCircleRow.Clear();
                newExpectCircleCol.Clear();
                newExpectCircleRadius.Clear();
                newExpecCircleRow.Add(this.regions[0].getModelData()[0]);
                newExpectCircleCol.Add(this.regions[0].getModelData()[1]);
                newExpectCircleRadius.Add(this.regions[0].getModelData()[2]);

                HTuple handleID;
                HOperatorSet.CreateMetrologyModel(out handleID);
                HTuple width, height;
                HOperatorSet.GetImageSize(m_GrayImage, out width, out height);
                HOperatorSet.SetMetrologyModelImageSize(handleID, width[0], height[0]);
                HTuple index;
                HOperatorSet.AddMetrologyObjectCircleMeasure(handleID, newExpecCircleRow[0], newExpectCircleCol[0], newExpectCircleRadius[0], new HTuple(ringRadiusLength), new HTuple(5), new HTuple(1), new HTuple(30), new HTuple(), new HTuple(), out index);
                //////HTuple hom;
                //////HTuple temp1 = (HTuple)L_regions[0].getModelData()[0];
                //////HOperatorSet.VectorAngleToRigid(LastExpectRow, LastExpectCol, 0, (HTuple)L_regions[0].getModelData()[0], (HTuple)L_regions[0].getModelData()[1],0,out hom);
                //////HObject contoursTransed;
                //////HOperatorSet.AffineTransContourXld(contours ,out contoursTransed,hom );
                //HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_transition"), new HTuple(polarity));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("num_measures"), new HTuple(cliperNum));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length1"), new HTuple(ringRadiusLength));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length2"), new HTuple(caliperWidth));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_threshold"), new HTuple(threshold));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_select"), new HTuple(edgeSelect));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("min_score"), new HTuple(minScore));
                HOperatorSet.ApplyMetrologyModel(m_GrayImage, handleID);
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row, out col);

                hWindowControl1.DispObj(contours, "red");

                //把点显示出来

                HTuple row1, col1;
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row1, out col1);


                HObject cross;
                HTuple row111, col111, row11, col11;
                HOperatorSet.GetPart(temp.HalconWindow, out row111, out col111, out row11, out col11);
                HOperatorSet.GenCrossContourXld(out cross, row, col, new HTuple((row11 - row111) / 90.0 + 1), new HTuple(0));        //小细节：我们要想使无论图像像素多大，显示的十字大小都是一样的，就需要得出y=kx+b中的k和b
                //hWindowControl1.DispObj(cross, "yellow");

                HOperatorSet.ClearMetrologyModel(handleID);
            }
            catch (HalconException HDevExpDefaultException)
            {
                throw HDevExpDefaultException;
            }
            finally
            {
                //////hWindow_Final1.HobjectToHimage(shapeMatchTool.inputImage);
                //hWindowControl1.DispObj(contours, "blue");
                hWindowControl1.DrawModel = false;
            }
        }


        #endregion
        private void btnModelFile_Click(object sender, EventArgs e)
        {
            string WorkPath = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start(WorkPath);
        }


        private void btn_Scale1to1_Click(object sender, EventArgs e)
        {
            if(m_GrayImage == null)
            {
                return;
            }

            HObject Scale1To1Image;
            HTuple w, h;
            HOperatorSet.GenEmptyObj(out Scale1To1Image);
            m_Factor = 1.0;
            HOperatorSet.ZoomImageFactor(m_GrayImage, out Scale1To1Image, m_Factor, m_Factor, "bicubic");
            HOperatorSet.GetImageSize(Scale1To1Image, out w, out h);

            bool tmpResize = false;
            //若输入图片的尺寸大于窗口尺寸
            //if (w > hWindowControl1.Width || h > hWindowControl1.Height)
            //{
            //    tmpResize = true;
            //    //m_IsSrcMagnify = true;
            //}
            //else
            //{
            //    tmpResize = false;
            //    //m_IsSrcMagnify = false;
            //}

            hWindowControl1.ClearWindow();

            int row, col;

            m_ImageScaleWidth = hWindowControl1.Size.Width;
            m_ImageScaleHeight = hWindowControl1.Size.Height;

            col = int.Parse(m_ImageScaleWidth.ToString()) - 0;
            row = int.Parse(m_ImageScaleHeight.ToString()) - 0;
            //hWindowControl1.SetPart(0, 0, row, col);
            //hWindowControl1.SetLineWidth(1.0);

            //DispZoomImage(m_Factor, Scale1To1Image);
            HOperatorSet.CopyImage(Scale1To1Image, out m_DealImage);

        }


        private void btn_AdaptShow_Click(object sender, EventArgs e)
        {
            if (m_GrayImage == null)
            {
                return;
            }

            bool Resize = false;

            //比较输入图像和窗口的尺寸大小，计算出比例。
            if (m_OpenImageWidth <= m_HWindowWidth && m_OpenImageHeight <= m_HWindowHeight)
            {
                Resize = false;
            }
            else
            {
                Resize = true;

                if (m_OpenImageWidth > m_HWindowWidth && m_OpenImageHeight > m_HWindowHeight)
                {
                    double ratioW = 0.0, ratioH = 0.0;
                    ratioW = double.Parse((m_HWindowWidth - 0).ToString()) / double.Parse(m_OpenImageWidth.ToString());
                    ratioH = double.Parse((m_HWindowHeight - 0).ToString()) / double.Parse(m_OpenImageHeight.ToString());
                    if (ratioW < ratioH)
                    {
                        m_Factor = ratioW;
                    }
                    else
                    {
                        m_Factor = ratioH;
                    }
                }
                else if (m_OpenImageWidth > m_HWindowWidth && m_OpenImageHeight <= m_HWindowHeight)
                {
                    m_Factor = double.Parse((m_HWindowWidth - 0).ToString()) / double.Parse(Width.ToString());
                }
                else if (m_OpenImageHeight > m_HWindowHeight && m_OpenImageWidth <= m_HWindowWidth)
                {
                    m_Factor = double.Parse((m_HWindowHeight - 0).ToString()) / double.Parse(Height.ToString());
                }
            }

            HOperatorSet.ZoomImageFactor(m_GrayImage, out m_DealImage, m_Factor, m_Factor, "bicubic");
            hWindowControl1.ClearWindow();
            //HOperatorSet.SetPart(hwindow, 0, 0, m_HWindowHeight, m_HWindowWidth);
            hWindowControl1.DispObj(m_DealImage);
        }

        void ThresholdImage(bool isDark, int thresholdVal, double factor, bool isResize)
        {
            if (m_OpenImage != null && m_OpenImage.CountObj() > 0 && (thresholdVal >= 0 && thresholdVal <= 255))
            {
                HObject grayObj;
                //HObject binObj;
                HObject binImage;
                HTuple w, h;

                HOperatorSet.Rgb1ToGray(m_OpenImage, out grayObj);
                if (!isDark)
                {
                    HOperatorSet.Threshold(grayObj, out binObj, thresholdVal, 255);
                }
                else
                {
                    HOperatorSet.Threshold(grayObj, out binObj, 0, thresholdVal);
                }

                HOperatorSet.GetImageSize(m_OpenImage, out w, out h);
                HOperatorSet.RegionToBin(binObj, out binImage, 255, 0, w, h);
                //lqj
                HOperatorSet.CopyImage(binImage, out m_DealImage);

                hWindowControl1.ClearWindow();
                hWindowControl1.HobjectToHimage(binImage);
                hWindowControl1.viewWindow.displayImage(binImage);
                //hWindowControl1.DispObj(binImage);
                //if (isResize)
                //{
                //    DispZoomImage(factor, binImage);
                //}
                //else
                //{
                //    DispImage(binImage);
                //}
            }

            return;
        }
    }
}
