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
        public string materialName = "";
        //public string ShapeModelName = "";

        public bool MeasureModelEnable = false;
        public bool MeasureModelSave = false;
        int MeasureToolType;

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
            groupBoxShapeModelParams.Enabled = false;
            groupBoxMeasrueModelParams.Enabled = false;

            //添加测量工具形状类型
            comboBox_MeasureTool.Items.Add("矩形");
            comboBox_MeasureTool.Items.Add("圆形");
            comboBox_MeasureTool.Items.Add("直线");
            comboBox_MeasureTool.SelectedIndex = 0;
            MeasureToolType = comboBox_MeasureTool.SelectedIndex;
        }


        //ROI框架
        List<ViewWindow.Model.ROI> regions; //roi集合
        HObject ho_ModelImage;
        //加载窗体界面
        private void Form1_Load(object sender, EventArgs e)
        {
            this.regions = new List<ViewWindow.Model.ROI>();

            ho_ModelImage = new HObject();
            hWindowControl1.hWindowControl.MouseUp += HWindow_MouseUp;
            //hWindowControl1.DrawModel = true;

            checkBoxDisplayCaliper.Checked = true;
            checkBoxDisplayFeature.Checked = true;
            checkBoxDisplayResultCont.Checked = true;
            checkBoxDisplayMeasureTool.Checked = true;
        }
        //鼠标事件注册
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
                //MeasureToolType = comboBox_MeasureTool.SelectedIndex;
                switch (MeasureToolType)
                {
                    case 0:
                        applyRectangle2MeasureTool();
                        break;
                    case 1:
                        applyCircleMeasureTool();
                        break;
                    case 2:
                        applyLineMeasureTool();
                        break;
                }
            }
        }
        //打开图片
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
                groupBoxShapeModelParams.Enabled = true;
                groupBoxMeasrueModelParams.Enabled = true;

                materialName = "";
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

                if (materialName == "")
                {
                    materialName = Interaction.InputBox("请输入不包含中文的物料名称：", "测量模板名称", "");

                }
                else //测量模板已创建名字
                {

                }

                if(materialName == "")
                {
                    return;
                }

                string WorkPath = System.IO.Directory.GetCurrentDirectory();
                string ModelPath = WorkPath + "\\Model";
                if(System.IO.Directory.Exists(ModelPath) == false)
                {
                    System.IO.Directory.CreateDirectory(ModelPath);
                }
                //后缀.shm
                string ShapeModelName = materialName + "-ShapeModel.shm";
                string ShapeModelPath = "..\\Debug\\Model\\" + ShapeModelName;
                //判断是否存在
                if (System.IO.File.Exists(ModelPath + "\\" + ShapeModelName))
                {
                    DialogResult dr = MessageBox.Show("物料模板文件已存在，是否要覆盖？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        HOperatorSet.WriteShapeModel(ShapeModelID, ShapeModelPath);
                        MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + ShapeModelName);
                        hWindowControl1.HobjectToHimage(m_GrayImage);
                        hWindowControl1.viewWindow.displayImage(m_GrayImage);
                    }
                    else
                    {
                    }
                }
                else
                {
                    HOperatorSet.WriteShapeModel(ShapeModelID, ShapeModelPath);
                    MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + ShapeModelName);
                    hWindowControl1.HobjectToHimage(m_GrayImage);
                    hWindowControl1.viewWindow.displayImage(m_GrayImage);
                }
                textBoxMaterialName.Text = materialName;
                //hWindowControl1.DispObj(m_GrayImage);
                HOperatorSet.ClearShapeModel(ShapeModelID);
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
                    //int MeasureToolType;
                    //MeasureToolType = comboBox_MeasureTool.SelectedIndex;
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
            MeasureToolType = comboBox_MeasureTool.SelectedIndex;
            if (MeasureModelEnable)
            {
                if (m_OpenImage == null || m_OpenImage.CountObj() <= 0)
                {
                    return;
                }
                hWindowControl1.viewWindow.notDisplayRoi();
                this.regions.Clear();

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

        //创建测量模板并保存
        private void btn_MeasureModel_Click(object sender, EventArgs e)
        {
            MeasureModelSave = true;
            MeasureToolType = comboBox_MeasureTool.SelectedIndex;
            switch(MeasureToolType)
            {
                case 0:
                    applyRectangle2MeasureTool();
                    break;
                case 1:
                    applyCircleMeasureTool();
                    break;
                case 2:
                    applyLineMeasureTool();
                    break;
            }
            MeasureModelSave = false;
        }

        //应用测量工具到ROI上
        #region 测量工具成员参数
        internal List<HTuple> newExpectCircleRow = new List<HTuple>(), newExpectCircleCol = new List<HTuple>(), newExpectCircleRadius = new List<HTuple>();
        internal List<HTuple> newExpectRectRow = new List<HTuple>(), newExpectRectCol = new List<HTuple>(), newExpectRectPhi = new List<HTuple>(), 
                              newExpectRectLength1 = new List<HTuple>(), newExpectRectLength2 = new List<HTuple>();
        internal List<HTuple> newExpectLineRow1 = new List<HTuple>(), newExpectLineCol1 = new List<HTuple>(), newExpectLineRow2 = new List<HTuple>(), newExpectLineCol2 = new List<HTuple>();
        /// <summary>
        /// 显示卡尺
        /// </summary>
        internal bool displayCaliper = true;
        /// <summary>
        /// 显示特征点
        /// </summary>
        internal bool displayFeature = true;
        /// <summary>
        /// 显示结果轮廓
        /// </summary>
        internal bool displayResultCont = true;
        /// <summary>
        /// 显示测量形状
        /// </summary>
        internal bool displayMeasureTool = true;
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

        /// <summary>
        /// 起始角度
        /// </summary>
        internal double startAngle = 10;
        /// <summary>
        /// 结束角度
        /// </summary>
        internal double endAngle = 360;
        /// <summary>
        /// 卡尺长、宽、距离
        /// </summary>
        internal int measureLength1 = 20;
        internal int measureLength2 = 5;
        internal int measureDistance = 10;
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

        internal HObject brush_region;
        #endregion
        private void applyRectangle2MeasureTool()
        {
            if (this.regions.Count() <= 0 || m_OpenImage == null || m_OpenImage.CountObj() <= 0)
            {
                return;
            }

            hWindowControl1.Focus();
            HObject brush_region_affine = new HObject();
            HObject ho_Image = new HObject(m_GrayImage);
            try
            {
                brush_region = null;
                brush_region = this.regions.Last().getRegion();

                HObject contours;
                HTuple row, col;

                newExpectRectRow.Clear();
                newExpectRectCol.Clear();
                newExpectRectPhi.Clear();
                newExpectRectLength1.Clear();
                newExpectRectLength2.Clear();

                newExpectRectRow.Add(this.regions[0].getModelData()[0]);
                newExpectRectCol.Add(this.regions[0].getModelData()[1]);
                newExpectRectPhi.Add(this.regions[0].getModelData()[2]);
                newExpectRectLength1.Add(this.regions[0].getModelData()[3]);
                newExpectRectLength2.Add(this.regions[0].getModelData()[4]);

                HTuple handleID;
                HOperatorSet.CreateMetrologyModel(out handleID);
                HTuple width, height;
                HOperatorSet.GetImageSize(m_GrayImage, out width, out height);
                HOperatorSet.SetMetrologyModelImageSize(handleID, width[0], height[0]);
                HTuple index;
                HOperatorSet.AddMetrologyObjectRectangle2Measure(handleID, newExpectRectRow[0], newExpectRectCol[0], newExpectRectPhi[0], newExpectRectLength1[0], newExpectRectLength2[0], new HTuple(measureLength1), new HTuple(5), new HTuple(1), new HTuple(30), new HTuple(), new HTuple(), out index);
                //HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_transition"), new HTuple(polarity));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("num_measures"), new HTuple(cliperNum));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length1"), new HTuple(measureLength1));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length2"), new HTuple(measureLength2));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_threshold"), new HTuple(threshold));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_distance"), new HTuple(measureDistance));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_select"), new HTuple(edgeSelect));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("min_score"), new HTuple(minScore));
                HOperatorSet.ApplyMetrologyModel(m_GrayImage, handleID);
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row, out col);
                hWindowControl1.viewWindow.ClearWindow();
                hWindowControl1.viewWindow.displayImage(m_GrayImage);
                hWindowControl1.viewWindow.notDisplayRoi();
                this.regions.Clear();
                hWindowControl1.viewWindow.genRect2(newExpectRectRow[0], newExpectRectCol[0], newExpectRectPhi[0], newExpectRectLength1[0], newExpectRectLength2[0], ref this.regions);
                hWindowControl1.viewWindow.notDisplayRoi();
                //显示各种测量元素
                if (displayMeasureTool)
                {
                    //hWindowControl1.viewWindow.notDisplayRoi();
                    this.regions.Clear();
                    hWindowControl1.viewWindow.genRect2(newExpectRectRow[0], newExpectRectCol[0], newExpectRectPhi[0], newExpectRectLength1[0], newExpectRectLength2[0], ref this.regions);
                    hWindowControl1.DispObj(brush_region, "yellow");
                }
                //hWindowControl1.DispObj(contours, "red");

                HTuple row1, col1;
                HOperatorSet.ApplyMetrologyModel(m_GrayImage, handleID);
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row1, out col1);
                if (displayCaliper)
                {
                    hWindowControl1.DispObj(contours, "red");
                }

                if (displayFeature)
                {
                    //HTuple row1, col1;
                    HObject cross;
                    //HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row1, out col1);
                    HOperatorSet.GenCrossContourXld(out cross, row1, col1, 5.0, new HTuple(0));
                    hWindowControl1.DispObj(cross, "blue");
                }

                HTuple ParamResult = new HTuple();
                HOperatorSet.GetMetrologyObjectResult(handleID, new HTuple("all"), "all", "result_type", "all_param", out ParamResult);
                if (displayResultCont)
                {
                    HObject resultCont;
                    if (ParamResult.Length <= 0)
                    {

                    }
                    else
                    {
                        HOperatorSet.GenRectangle2ContourXld(out resultCont, ParamResult[0], ParamResult[1], ParamResult[2], ParamResult[3], ParamResult[4]);
                        hWindowControl1.DispObj(resultCont, "blue");
                    }
                }

                //保存模板
                if (MeasureModelSave)
                {
                    if (ParamResult.Length <= 0)
                    {
                        MessageBox.Show("特征点不足，拟合矩形失败，创建测量模板失败！");
                    }
                    else
                    {
                        if (materialName == "")
                        {
                            materialName = Interaction.InputBox("请输入不包含中文的物料名称：", "测量模板名称", "");

                        }
                        else //形状模板已创建名字
                        {

                        }

                        if (materialName == "")
                        {
                            //return;
                        }
                        else
                        {
                            string WorkPath = System.IO.Directory.GetCurrentDirectory();
                            string ModelPath = WorkPath + "\\Model";
                            if (System.IO.Directory.Exists(ModelPath) == false)
                            {
                                System.IO.Directory.CreateDirectory(ModelPath);
                            }
                            //后缀.mtr
                            string MeasureModelName = materialName + "-MeasureModel.mtr";
                            string MeasureModelPath = "..\\Debug\\Model\\" + MeasureModelName;
                            //判断是否存在
                            if(System.IO.File.Exists(ModelPath + "\\" + MeasureModelName))
                            {
                                DialogResult dr = MessageBox.Show("物料模板文件已存在，是否要覆盖？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                if (dr == DialogResult.OK)
                                {
                                    HOperatorSet.WriteMetrologyModel(handleID, MeasureModelPath);
                                    MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + MeasureModelName);
                                }
                                else
                                {        
                                }
                            }
                            else
                            {
                                HOperatorSet.WriteMetrologyModel(handleID, MeasureModelPath);
                                MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + MeasureModelName);
                            }

                        }
                        textBoxMaterialName.Text = materialName;

                    }
                }
                //HObject cross;
                //HTuple row111, col111, row11, col11;
                //HOperatorSet.GetPart(hWindowControl1.HalconWindow, out row111, out col111, out row11, out col11);
                //HOperatorSet.GenCrossContourXld(out cross, row, col, new HTuple((row11 - row111) / 90.0 + 1), new HTuple(0));        //小细节：我们要想使无论图像像素多大，显示的十字大小都是一样的，就需要得出y=kx+b中的k和b
                //hWindowControl1.DispObj(cross, "yellow");

                HOperatorSet.ClearMetrologyModel(handleID);
            }
            catch (HalconException HDevExpDefaultException)
            {
                throw HDevExpDefaultException;
            }
            finally
            {
                hWindowControl1.DrawModel = false;
                GC.Collect();
            }
        }
        private void applyCircleMeasureTool()
        {
            if (this.regions.Count() <= 0 || m_OpenImage == null || m_OpenImage.CountObj() <= 0)
            {
                return;
            }

            hWindowControl1.Focus();
            HObject brush_region_affine = new HObject();
            HObject ho_Image = new HObject(m_GrayImage);
            try
            {
                brush_region = null;
                brush_region = this.regions.Last().getRegion();

                HObject contours;
                HTuple row, col;

                newExpectCircleRow.Clear();
                newExpectCircleCol.Clear();
                newExpectCircleRadius.Clear();
                newExpectCircleRow.Add(this.regions[0].getModelData()[0]);
                newExpectCircleCol.Add(this.regions[0].getModelData()[1]);
                newExpectCircleRadius.Add(this.regions[0].getModelData()[2]);

                HTuple handleID;
                HOperatorSet.CreateMetrologyModel(out handleID);
                HTuple width, height;
                HOperatorSet.GetImageSize(m_GrayImage, out width, out height);
                HOperatorSet.SetMetrologyModelImageSize(handleID, width[0], height[0]);
                HTuple index;
                HOperatorSet.AddMetrologyObjectCircleMeasure(handleID, newExpectCircleRow[0], newExpectCircleCol[0], newExpectCircleRadius[0], new HTuple(measureLength1), new HTuple(5), new HTuple(1), new HTuple(30), new HTuple(), new HTuple(), out index);
                //HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_transition"), new HTuple(polarity));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("num_measures"), new HTuple(cliperNum));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length1"), new HTuple(measureLength1));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length2"), new HTuple(measureLength2));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_threshold"), new HTuple(threshold));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_distance"), new HTuple(measureDistance));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_select"), new HTuple(edgeSelect));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("min_score"), new HTuple(minScore));
                HOperatorSet.ApplyMetrologyModel(m_GrayImage, handleID);
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row, out col);
                hWindowControl1.viewWindow.ClearWindow();
                hWindowControl1.viewWindow.displayImage(m_GrayImage);
                hWindowControl1.viewWindow.notDisplayRoi();
                this.regions.Clear();
                hWindowControl1.viewWindow.genCircle(newExpectCircleRow[0], newExpectCircleCol[0], newExpectCircleRadius[0], ref this.regions);
                hWindowControl1.viewWindow.notDisplayRoi();
                //显示各种测量元素
                if (displayMeasureTool)
                {
                    //hWindowControl1.viewWindow.notDisplayRoi();
                    this.regions.Clear();
                    hWindowControl1.viewWindow.genCircle(newExpectCircleRow[0], newExpectCircleCol[0], newExpectCircleRadius[0], ref this.regions);
                    hWindowControl1.DispObj(brush_region, "yellow");
                }

                HTuple row1, col1;
                HOperatorSet.ApplyMetrologyModel(m_GrayImage, handleID);
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row1, out col1);
                if (displayCaliper)
                {
                    hWindowControl1.DispObj(contours, "red");
                }

                if (displayFeature)
                {
                    HObject cross;
                    HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row1, out col1);
                    HOperatorSet.GenCrossContourXld(out cross, row1, col1, 5.0, new HTuple(0));
                    hWindowControl1.DispObj(cross, "blue");
                }

                HTuple ParamResult = new HTuple();
                HOperatorSet.GetMetrologyObjectResult(handleID, new HTuple("all"), "all", "result_type", "all_param", out ParamResult);
                if (displayResultCont)
                {
                    HObject resultCont;
                    if (ParamResult.Length <= 0)
                    {

                    }
                    else
                    {
                        HOperatorSet.GenCircleContourXld(out resultCont, ParamResult[0], ParamResult[1], ParamResult[2], 
                                                         new HTuple(0).TupleRad(), new HTuple(360).TupleRad(), "positive", 1.0);
                        hWindowControl1.DispObj(resultCont, "blue");
                    }
                }
                //保存模板
                if (MeasureModelSave)
                {
                    if (ParamResult.Length <= 0)
                    {
                        MessageBox.Show("特征点不足，拟合圆形失败，创建测量模板失败！");
                    }
                    else
                    {
                        if (materialName == "")
                        {
                            materialName = Interaction.InputBox("请输入不包含中文的物料名称：", "测量模板名称", "");

                        }
                        else //形状模板已创建名字
                        {

                        }

                        if (materialName == "")
                        {
                            //return;
                        }
                        else
                        {
                            string WorkPath = System.IO.Directory.GetCurrentDirectory();
                            string ModelPath = WorkPath + "\\Model";
                            if (System.IO.Directory.Exists(ModelPath) == false)
                            {
                                System.IO.Directory.CreateDirectory(ModelPath);
                            }
                            //后缀.mtr
                            string MeasureModelName = materialName + "-MeasureModel.mtr";
                            string MeasureModelPath = "..\\Debug\\Model\\" + MeasureModelName;
                            //判断是否存在
                            if (System.IO.File.Exists(ModelPath + "\\" + MeasureModelName))
                            {
                                DialogResult dr = MessageBox.Show("物料模板文件已存在，是否要覆盖？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                if (dr == DialogResult.OK)
                                {
                                    HOperatorSet.WriteMetrologyModel(handleID, MeasureModelPath);
                                    MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + MeasureModelName);
                                }
                                else
                                {
                                }
                            }
                            {
                                HOperatorSet.WriteMetrologyModel(handleID, MeasureModelPath);
                                MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + MeasureModelName);
                            }
                        }
                        textBoxMaterialName.Text = materialName;
                    }
                }
                HOperatorSet.ClearMetrologyModel(handleID);
            }
            catch (HalconException HDevExpDefaultException)
            {
                throw HDevExpDefaultException;
            }
            finally
            {
                hWindowControl1.DrawModel = false;
                GC.Collect();
            }
        }
        private void applyLineMeasureTool()
        {
            if (this.regions.Count() <= 0 || m_OpenImage == null || m_OpenImage.CountObj() <= 0)
            {
                return;
            }

            hWindowControl1.Focus();
            HObject brush_region_affine = new HObject();
            HObject ho_Image = new HObject(m_GrayImage);
            try
            {
                brush_region = null;
                brush_region = this.regions.Last().getRegion();

                HObject contours;
                HTuple row, col;

                newExpectLineRow1.Clear();
                newExpectLineCol1.Clear();
                newExpectLineRow2.Clear();
                newExpectLineCol2.Clear();
                newExpectLineRow1.Add(this.regions[0].getModelData()[0]);
                newExpectLineCol1.Add(this.regions[0].getModelData()[1]);
                newExpectLineRow2.Add(this.regions[0].getModelData()[2]);
                newExpectLineCol2.Add(this.regions[0].getModelData()[3]);

                HTuple handleID;
                HOperatorSet.CreateMetrologyModel(out handleID);
                HTuple width, height;
                HOperatorSet.GetImageSize(m_GrayImage, out width, out height);
                HOperatorSet.SetMetrologyModelImageSize(handleID, width[0], height[0]);
                HTuple index;
                HOperatorSet.AddMetrologyObjectLineMeasure(handleID, newExpectLineRow1[0], newExpectLineCol1[0], newExpectLineRow2[0], newExpectLineCol2[0], new HTuple(measureLength1), new HTuple(5), new HTuple(1), new HTuple(30), new HTuple(), new HTuple(), out index);
                //HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_transition"), new HTuple(polarity));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("num_measures"), new HTuple(cliperNum));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length1"), new HTuple(measureLength1));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_length2"), new HTuple(measureLength2));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_threshold"), new HTuple(threshold));
                HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_distance"), new HTuple(measureDistance));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("measure_select"), new HTuple(edgeSelect));
                //////HOperatorSet.SetMetrologyObjectParam(handleID, new HTuple("all"), new HTuple("min_score"), new HTuple(minScore));
                HOperatorSet.ApplyMetrologyModel(m_GrayImage, handleID);
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row, out col);
                hWindowControl1.viewWindow.ClearWindow();
                hWindowControl1.viewWindow.displayImage(m_GrayImage);
                hWindowControl1.viewWindow.notDisplayRoi();
                this.regions.Clear();
                hWindowControl1.viewWindow.genLine(newExpectLineRow1[0], newExpectLineCol1[0], newExpectLineRow2[0], newExpectLineCol2[0], ref this.regions);
                hWindowControl1.viewWindow.notDisplayRoi();
                //显示各种测量元素
                if (displayMeasureTool)
                {
                    //hWindowControl1.viewWindow.notDisplayRoi();
                    this.regions.Clear();
                    hWindowControl1.viewWindow.genLine(newExpectLineRow1[0], newExpectLineCol1[0], newExpectLineRow2[0], newExpectLineCol2[0], ref this.regions);
                    hWindowControl1.DispObj(brush_region, "yellow");
                }

                HTuple row1, col1;
                HOperatorSet.ApplyMetrologyModel(m_GrayImage, handleID);
                HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row1, out col1);
                if (displayCaliper)
                {
                    hWindowControl1.DispObj(contours, "red");
                }


                if (displayFeature)
                {
                    HObject cross;
                    HOperatorSet.GetMetrologyObjectMeasures(out contours, handleID, new HTuple("all"), new HTuple("all"), out row1, out col1);
                    HOperatorSet.GenCrossContourXld(out cross, row1, col1, 5.0, new HTuple(0));
                    hWindowControl1.DispObj(cross, "blue");
                }

                HTuple ParamResult = new HTuple();
                HOperatorSet.GetMetrologyObjectResult(handleID, new HTuple("all"), "all", "result_type", "all_param", out ParamResult);
                if (displayResultCont)
                {
                    HObject resultCont;
                    if (ParamResult.Length <= 0)
                    {

                    }
                    else
                    {
                        HOperatorSet.GenContourPolygonXld(out resultCont, (ParamResult.TupleSelect(0)).TupleConcat(ParamResult[2]),
                                                                          (ParamResult.TupleSelect(1)).TupleConcat(ParamResult[3]));
                        hWindowControl1.DispObj(resultCont, "blue");
                    }
                }
                //保存模板
                if (MeasureModelSave)
                {
                    if (ParamResult.Length <= 0)
                    {
                        MessageBox.Show("特征点不足，拟合直线失败，创建测量模板失败！");
                    }
                    else
                    {
                        if (materialName == "")
                        {
                            materialName = Interaction.InputBox("请输入不包含中文的物料名称：", "测量模板名称", "");

                        }
                        else //形状模板已创建名字
                        {

                        }

                        if (materialName == "")
                        {
                            //return;
                        }
                        else
                        {
                            string WorkPath = System.IO.Directory.GetCurrentDirectory();
                            string ModelPath = WorkPath + "\\Model";
                            if (System.IO.Directory.Exists(ModelPath) == false)
                            {
                                System.IO.Directory.CreateDirectory(ModelPath);
                            }
                            //后缀.mtr
                            string MeasureModelName = materialName + "-MeasureModel.mtr";
                            string MeasureModelPath = "..\\Debug\\Model\\" + MeasureModelName;
                            //判断是否存在
                            if (System.IO.File.Exists(ModelPath + "\\" + MeasureModelName))
                            {
                                DialogResult dr = MessageBox.Show("物料模板文件已存在，是否要覆盖？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                if (dr == DialogResult.OK)
                                {
                                    HOperatorSet.WriteMetrologyModel(handleID, MeasureModelPath);
                                    MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + MeasureModelName);
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                                HOperatorSet.WriteMetrologyModel(handleID, MeasureModelPath);
                                MessageBox.Show("保存成功！保存路径：" + ModelPath + "\\" + MeasureModelName);
                            }
                        }
                        textBoxMaterialName.Text = materialName;
                    }
                }
                //HObject cross;
                //HTuple row111, col111, row11, col11;
                //HOperatorSet.GetPart(hWindowControl1.HalconWindow, out row111, out col111, out row11, out col11);
                //HOperatorSet.GenCrossContourXld(out cross, row, col, new HTuple((row11 - row111) / 90.0 + 1), new HTuple(0));        //小细节：我们要想使无论图像像素多大，显示的十字大小都是一样的，就需要得出y=kx+b中的k和b
                //hWindowControl1.DispObj(cross, "yellow");

                HOperatorSet.ClearMetrologyModel(handleID);
            }
            catch (HalconException HDevExpDefaultException)
            {
                throw HDevExpDefaultException;
            }
            finally
            {
                hWindowControl1.DrawModel = false;
                GC.Collect();
            }
        }

        //改变显示参数
        private void checkBoxDisplayCaliperCheckedChanged(object sender, EventArgs e)
        {
            displayCaliper = checkBoxDisplayCaliper.Checked;
            if (this.regions.Count() <= 0 || m_OpenImage == null || m_OpenImage.CountObj() <= 0)
            {
                return;
            }
            switch (MeasureToolType)
            {
                case 0:
                    applyRectangle2MeasureTool();
                    break;
                case 1:
                    applyCircleMeasureTool();
                    break;
                case 2:
                    applyLineMeasureTool();
                    break;
            }
        }
        private void checkBoxDisplayFeatureCheckedChanged(object sender, EventArgs e)
        {
            displayFeature = checkBoxDisplayFeature.Checked;
            if (this.regions.Count() <= 0 || m_OpenImage == null || m_OpenImage.CountObj() <= 0)
            {
                return;
            }
            switch (MeasureToolType)
            {
                case 0:
                    applyRectangle2MeasureTool();
                    break;
                case 1:
                    applyCircleMeasureTool();
                    break;
                case 2:
                    applyLineMeasureTool();
                    break;
            }
        }
        private void checkBoxDisplayResultContCheckedChanged(object sender, EventArgs e)
        {
            displayResultCont = checkBoxDisplayResultCont.Checked;
            switch (MeasureToolType)
            {
                case 0:
                    applyRectangle2MeasureTool();
                    break;
                case 1:
                    applyCircleMeasureTool();
                    break;
                case 2:
                    applyLineMeasureTool();
                    break;
            }
        }
        private void checkBoxDisplayMeasureToolCheckedChanged(object sender, EventArgs e)
        {
            displayMeasureTool = checkBoxDisplayMeasureTool.Checked;
            if (!displayMeasureTool)
            {
                switch (MeasureToolType)
                {
                    case 0:
                        applyRectangle2MeasureTool();
                        hWindowControl1.viewWindow.notDisplayRoi();
                        break;
                    case 1:
                        applyCircleMeasureTool();
                        hWindowControl1.viewWindow.notDisplayRoi();
                        break;
                    case 2:
                        applyLineMeasureTool();
                        hWindowControl1.viewWindow.notDisplayRoi();
                        break;
                }
            }
            else
            {
                switch (MeasureToolType)
                {
                    case 0:
                        applyRectangle2MeasureTool();
                        break;
                    case 1:
                        applyCircleMeasureTool();
                        break;
                    case 2:
                        applyLineMeasureTool();
                        break;
                }
            }
        }

        #endregion

        //打开模板文件夹
        private void btnModelFile_Click(object sender, EventArgs e)
        {
            string WorkPath = System.IO.Directory.GetCurrentDirectory();
            string ModelPath = WorkPath + "\\Model";
            if (System.IO.Directory.Exists(ModelPath) == false)
            {
                System.IO.Directory.CreateDirectory(ModelPath);
            }
            System.Diagnostics.Process.Start(ModelPath);
        }
        //二值化
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
