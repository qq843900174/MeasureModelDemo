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

namespace MeasureModelDemo
{
    public partial class Form1 : Form
    {
        private static HWindow hwindow; //全局窗口变量
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

        //ROI框架
        //List<>

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

            hwindow = hWindowControl1.HalconWindow;//初始化窗口变量
            m_HWindowWidth = hWindowControl1.Size.Width;
            m_HWindowHeight = hWindowControl1.Size.Height;

            ShapeModelID = null;

            radioBtnBlackBack.Checked = true;
            radioBtnWhiteBack.Checked = false;
            //btn_CreatShapeModel.Enabled = false;
            //btn_MeasureModel.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
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
                hwindow = hWindowControl1.HalconWindow;
                hwindow.ClearWindow();
                HOperatorSet.ReadImage(out m_OpenImage, ImagePath);
                //ImagePath_state = 1;
                HOperatorSet.GetImageSize(m_OpenImage, out m_OpenImageWidth, out m_OpenImageHeight);
                //HOperatorSet.SetPart(hwindow, 0, 0, m_OpenImageHeight - 1, m_OpenImageWidth - 1);
                HOperatorSet.Rgb1ToGray(m_OpenImage, out m_GrayImage);
                HOperatorSet.DispObj(m_GrayImage, hwindow);
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
                ShapeModelName = materialName + "-ShapeModel";
                string ShapeModelPath = "..\\Debug\\" + ShapeModelName;
                //判断是否存在
                HOperatorSet.WriteShapeModel(ShapeModelID, ShapeModelPath);
                string WorkPath = System.IO.Directory.GetCurrentDirectory();
                MessageBox.Show("保存成功！保存路径：" + WorkPath + "\\" + ShapeModelName);
                DispZoomImage(m_Factor, m_GrayImage);
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

        private void btnModelFile_Click(object sender, EventArgs e)
        {
            string WorkPath = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start(WorkPath);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!MeasureModelEnable)
                pictureBox1.Image = Properties.Resources.Enable;
            else
                pictureBox1.Image = Properties.Resources.Disable;
            MeasureModelEnable = !MeasureModelEnable;
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

            hWindowControl1.HalconWindow.ClearWindow();

            int row, col;

            m_ImageScaleWidth = hWindowControl1.Size.Width;
            m_ImageScaleHeight = hWindowControl1.Size.Height;

            col = int.Parse(m_ImageScaleWidth.ToString()) - 0;
            row = int.Parse(m_ImageScaleHeight.ToString()) - 0;
            hWindowControl1.HalconWindow.SetPart(0, 0, row, col);
            hWindowControl1.HalconWindow.SetLineWidth(1.0);

            DispZoomImage(m_Factor, Scale1To1Image);
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
            hWindowControl1.HalconWindow.ClearWindow();
            HOperatorSet.SetPart(hwindow, 0, 0, m_HWindowHeight, m_HWindowWidth);
            HOperatorSet.DispObj(m_DealImage, hwindow);
        }



        public void DispImage(HObject DrawObj)
        {
            if (DrawObj.CountObj() <= 0)
            {
                return;
            }
            hWindowControl1.HalconWindow.DispObj(DrawObj);
            return;
        }
        public void DispZoomImage(double Factor, HObject SrcObj)
        {
            HObject DrawObj;

            if (Factor <= 0.0 || Factor >= 4.0)
            {
                return;
            }

            if (SrcObj.CountObj() <= 0)
            {
                return;
            }

            HOperatorSet.ZoomImageFactor(SrcObj, out DrawObj, Factor, Factor, "bicubic");
            hWindowControl1.HalconWindow.DispObj(DrawObj);

            return;
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

                hWindowControl1.HalconWindow.ClearWindow();
                if (isResize)
                {
                    DispZoomImage(factor, binImage);
                }
                else
                {
                    DispImage(binImage);
                }
            }

            return;
        }

        private void HWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            if (MeasureModelEnable)
            {
                return;
            }

            if (!hWindowControl1.ClientRectangle.Contains(e.Location))
            {
                return;
            }

            if (m_OpenImage == null || m_OpenImage.CountObj() <= 0)
            {
                return;
            }

            //Min--0.1 Max--4.0
            if (e.Delta > 0)//Up
            {
                m_Factor += 0.1;
            }
            else//Down
            {
                m_Factor -= 0.1;
            }

            if (m_Factor < 0.1)
            {
                m_Factor = 0.1;
            }
            if (m_Factor > 4.0)
            {
                m_Factor = 4.0;
            }

            HTuple ImgW = 0, ImgH = 0, ScaledW = 0, ScaledH = 0;
            HOperatorSet.GetImageSize(m_DealImage, out ImgW, out ImgH);
            ScaledW = ImgW * m_Factor;
            ScaledH = ImgH * m_Factor;
            //if (ScaledW > m_HWindowWidth || ScaledH > m_HWindowHeight)
            //{
            //    m_IsSrcMagnify = true;
            //}
            //else
            //{
            //    m_IsSrcMagnify = false;
            //}

            hWindowControl1.HalconWindow.ClearWindow();
            //             if (!m_IsSrcMagnify)
            //             {
            //                 SetPart(true);
            //             }
            // 
            DispZoomImage(m_Factor, m_DealImage);

            //CleanDrawRect();
            //DrawRectCont(m_DealImage, m_Factor, m_SrcDrawX1, m_SrcDrawY1, true);

            return;
        }
        /*
        private void SrcImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (MeasureModelEnable)
            {
                return;
            }
            if (m_DealImage == null || m_DealImage.CountObj() <= 0)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                m_EightPt = 0;

                //已画框
                if ((m_SrcDrawX0 >= 2 && m_SrcDrawX0 < m_SrcDrawX1) &&
                    (m_SrcDrawY0 >= 2 && m_SrcDrawY0 < m_SrcDrawY1) &&
                    (m_SrcDrawX1 >= (m_SrcDrawX0 + 4) && m_SrcDrawX1 <= (hWindowControl_SrcImage.Width - 2)) &&
                    (m_SrcDrawY1 >= (m_SrcDrawY0 + 4) && m_SrcDrawY1 <= (hWindowControl_SrcImage.Height - 2)))
                {
                    int pty = 0, ptx = 0;
                    pty = e.Location.Y;
                    ptx = e.Location.X;

                    //判断鼠标是否落在8个小方点内
                    int w = 15, h = 15;
                    int lty = 0, ltx = 0, rty = 0, rtx = 0, lby = 0, lbx = 0, rby = 0, rbx = 0,
                        lmy = 0, lmx = 0, rmy = 0, rmx = 0, tmy = 0, tmx = 0, bmy = 0, bmx = 0;
                    int x0 = m_SrcDrawX0, y0 = m_SrcDrawY0, x1 = m_SrcDrawX1, y1 = m_SrcDrawY1;
                    double tw = m_SrcDrawX1 - m_SrcDrawX0, th = m_SrcDrawY1 - m_SrcDrawY0;

                    lty = y0;
                    ltx = x0;
                    rty = y0;
                    rtx = x1;
                    lby = y1;
                    lbx = x0;
                    rby = y1;
                    rbx = x1;

                    lmy = y0 + Convert.ToInt32(th / 2.0);
                    lmx = x0;
                    rmy = y0 + Convert.ToInt32(th / 2.0);
                    rmx = x1;
                    tmy = y0;
                    tmx = x0 + Convert.ToInt32(tw / 2.0);
                    bmy = y1;
                    bmx = x0 + Convert.ToInt32(tw / 2.0);

                    //                         Rectangle ltRt = new Rectangle(ltx, lty, w, h);
                    //                         Rectangle rtRt = new Rectangle(rtx, rty, w, h);
                    //                         Rectangle lbRt = new Rectangle(lbx, lby, w, h);
                    //                         Rectangle rbRt = new Rectangle(rbx, rby, w, h);
                    //                         Rectangle lmRt = new Rectangle(lmx, lmy, w, h);
                    //                         Rectangle rmRt = new Rectangle(rmx, rmy, w, h);
                    //                         Rectangle tmRt = new Rectangle(tmx, tmy, w, h);
                    //                         Rectangle bmRt = new Rectangle(bmx, bmy, w, h);
                    //                         if (ltRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 1;
                    //                         }
                    //                         else if (rtRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 2;
                    //                         }
                    //                         else if (lbRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 3;
                    //                         }
                    //                         else if (rbRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 4;
                    //                         }
                    //                         else if (lmRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 5;
                    //                         }
                    //                         else if (rmRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 6;
                    //                         }
                    //                         else if (tmRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 7;
                    //                         }
                    //                         else if (bmRt.Contains(ptx, pty))
                    //                         {
                    //                             m_EightPt = 8;
                    //                         }
                    //                         else
                    //                         {
                    //                             m_EightPt = 0;
                    //                         }

                    HObject ltRtObj, rtRtObj, lbRtObj, rbRtObj, lmRtObj, rmRtObj, tmRtObj, bmRtObj;
                    int row1, col1, row2, col2;
                    hWindowControl_SrcImage.HalconWindow.GetPart(out row1, out col1, out row2, out col2);
                    HOperatorSet.GenRectangle2(out ltRtObj, lty + row1, ltx + col1, 0.0, w, h);
                    HOperatorSet.GenRectangle2(out rtRtObj, rty + row1, rtx + col1, 0.0, w, h);
                    HOperatorSet.GenRectangle2(out lbRtObj, lby + row1, lbx + col1, 0.0, w, h);
                    HOperatorSet.GenRectangle2(out rbRtObj, rby + row1, rbx + col1, 0.0, w, h);
                    HOperatorSet.GenRectangle2(out lmRtObj, lmy + row1, lmx + col1, 0.0, w, h);
                    HOperatorSet.GenRectangle2(out rmRtObj, rmy + row1, rmx + col1, 0.0, w, h);
                    HOperatorSet.GenRectangle2(out tmRtObj, tmy + row1, tmx + col1, 0.0, w, h);
                    HOperatorSet.GenRectangle2(out bmRtObj, bmy + row1, bmx + col1, 0.0, w, h);

                    HTuple rowPt = 0.0, colPt = 0.0;
                    HObject ptObj;
                    rowPt = pty + row1;
                    colPt = ptx + col1;
                    HOperatorSet.GenRectangle2(out ptObj, rowPt, colPt, 0.0, 1.0, 1.0);

                    HObject crossObj;
                    HTuple area, row, col;

                    m_EightPt = 0;
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(ltRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 1;
                        }
                    }
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(rtRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 2;
                        }
                    }
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(lbRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 3;
                        }
                    }
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(rbRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 4;
                        }
                    }
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(lmRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 5;
                        }
                    }
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(rmRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 6;
                        }
                    }
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(tmRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 7;
                        }
                    }
                    if (m_EightPt == 0)
                    {
                        HOperatorSet.Intersection(bmRtObj, ptObj, out crossObj);
                        HOperatorSet.AreaCenter(crossObj, out area, out row, out col);
                        if (area.Length <= 0 || area < 4.0)
                        {
                            m_EightPt = 0;
                        }
                        else
                        {
                            m_EightPt = 8;
                        }
                    }

                    //                         hWindowControl_SrcImage.HalconWindow.SetColor("green");
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(ptObj);
                    //                         hWindowControl_SrcImage.HalconWindow.SetColor("blue");
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(ltRtObj);
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(rtRtObj);
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(lbRtObj);
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(rbRtObj);
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(lmRtObj);
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(rmRtObj);
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(tmRtObj);
                    //                         hWindowControl_SrcImage.HalconWindow.DispObj(bmRtObj);
                }

                if (m_EightPt < 1 || m_EightPt > 8)
                {
                    m_SrcDrawX0 = e.Location.X;
                    m_SrcDrawY0 = e.Location.Y;
                }

                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                m_SrcMouseX0 = e.Location.Y;
                m_SrcMouseY0 = e.Location.X;
            }

            return;
        }
        */
        /*
        private void DrawRectCont(HObject drawImage, double factor, double ptx, double pty, bool isMouseUp)
        {
            if (drawImage == null || drawImage.CountObj() <= 0)
            {
                return;
            }

            int tx = 0, ty = 0;
            double zoomLeft = 0.0, zoomTop = 0.0, zoomSchW = 0.0, zoomSchH = 0.0,
                srcLeft = 0.0, srcTop = 0.0, srcSchW = 0.0, srcSchH = 0.0;
            int row1 = 0, col1 = 0, row2 = 0, col2 = 0;

            bool tmpResize = false;
            HTuple w, h;
            HOperatorSet.GetImageSize(drawImage, out w, out h);
            if (w > hWindowControl_SrcImage.Width || h > hWindowControl_SrcImage.Height)
            {
                tmpResize = true;
            }
            else
            {
                tmpResize = false;
            }

            if (!m_DrawRect)
            {
                hWindowControl_SrcImage.HalconWindow.ClearWindow();

                if (tmpResize)
                {
                    DispZoomImage(true, factor, drawImage);
                }
                else
                {
                    DispImage(true, drawImage);
                }

                return;
            }

            tx = Convert.ToInt32(ptx);
            ty = Convert.ToInt32(pty);
            if ((m_SrcDrawX0 < 2.0 || m_SrcDrawY0 < 2.0) ||
                (tx <= (m_SrcDrawX0 + 4.0) || tx > (hWindowControl_SrcImage.Width - 2.0)) ||
                (ty <= (m_SrcDrawY0 + 4.0) || ty > (hWindowControl_SrcImage.Height - 2.0)))
            {
                hWindowControl_SrcImage.HalconWindow.ClearWindow();

                if (tmpResize)
                {
                    DispZoomImage(true, factor, drawImage);
                }
                else
                {
                    DispImage(true, drawImage);
                }

                m_DrawLeft = 0.0;
                m_DrawTop = 0.0;
                m_DrawWidth = 0.0;
                m_DrawHeight = 0.0;
                textBoxLeft.Text = m_DrawLeft.ToString();
                textBoxTop.Text = m_DrawTop.ToString();
                textBoxWidth.Text = m_DrawWidth.ToString();
                textBoxHeight.Text = m_DrawHeight.ToString();

                return;
            }

            hWindowControl_SrcImage.HalconWindow.ClearWindow();

            zoomSchW = tx - m_SrcDrawX0;
            zoomSchH = ty - m_SrcDrawY0;
            hWindowControl_SrcImage.HalconWindow.GetPart(out row1, out col1, out row2, out col2);
            zoomLeft = col1 + m_SrcDrawX0;
            zoomTop = row1 + m_SrcDrawY0;

            srcLeft = zoomLeft / factor;
            srcTop = zoomTop / factor;
            srcSchW = zoomSchW / factor;
            srcSchH = zoomSchH / factor;

            srcLeft = Convert.ToInt32(srcLeft);
            srcTop = Convert.ToInt32(srcTop);
            srcSchW = Convert.ToInt32(srcSchW);
            srcSchH = Convert.ToInt32(srcSchH);

            textBoxLeft.Text = srcLeft.ToString();
            textBoxTop.Text = srcTop.ToString();
            textBoxWidth.Text = srcSchW.ToString();
            textBoxHeight.Text = srcSchH.ToString();

            HObject tmpRtObj, tmpObj;
            HObject ptObj, drawObj;
            HOperatorSet.GenEmptyObj(out ptObj);
            HOperatorSet.GenEmptyObj(out drawObj);
            HOperatorSet.GenRectangle1(out tmpRtObj, srcTop, srcLeft, srcTop + srcSchH, srcLeft + srcSchW);
            if (!isMouseUp)
            {
                HOperatorSet.GenContourRegionXld(tmpRtObj, out tmpObj, "border");

                HTuple lsty = 0.0;
                lsty[0] = 20;
                lsty[1] = 7;
                lsty[2] = 3;
                lsty[3] = 7;
                hWindowControl_SrcImage.HalconWindow.SetLineStyle(lsty);
                hWindowControl_SrcImage.HalconWindow.SetColor("pink");

                HOperatorSet.ConcatObj(drawObj, tmpObj, out drawObj);
            }
            else
            {
                HOperatorSet.GenContourRegionXld(tmpRtObj, out tmpObj, "border");

                HTuple lsty = 0;
                hWindowControl_SrcImage.HalconWindow.SetLineStyle(lsty);
                hWindowControl_SrcImage.HalconWindow.SetColor("green");

                HOperatorSet.ConcatObj(drawObj, tmpObj, out drawObj);

                //生成拖动小点(8个)
                HTuple l1 = 2.0, l2 = 2.0;
                HTuple ltRow = 0.0, ltCol = 0.0, rtRow = 0.0, rtCol = 0.0, lbRow = 0.0, lbCol = 0.0, rbRow = 0.0, rbCol = 0.0,
                    lmRow = 0.0, lmCol = 0.0, rmRow = 0.0, rmCol = 0.0, tmRow = 0.0, tmCol = 0.0, bmRow = 0.0, bmCol = 0.0;
                ltRow = srcTop;
                ltCol = srcLeft;
                rtRow = srcTop;
                rtCol = srcLeft + srcSchW;
                lbRow = srcTop + srcSchH;
                lbCol = srcLeft;
                rbRow = srcTop + srcSchH;
                rbCol = srcLeft + srcSchW;

                lmRow = srcTop + srcSchH / 2.0;
                lmCol = srcLeft;
                rmRow = srcTop + srcSchH / 2.0;
                rmCol = srcLeft + srcSchW;
                tmRow = srcTop;
                tmCol = srcLeft + srcSchW / 2.0;
                bmRow = srcTop + srcSchH;
                bmCol = srcLeft + srcSchW / 2.0;

                HObject ltObj, rtObj, lbObj, rbObj, lmObj, rmObj, tmObj, bmObj;
                HOperatorSet.GenRectangle2(out ltObj, ltRow, ltCol, 0.0, l1, l2);
                HOperatorSet.GenRectangle2(out rtObj, rtRow, rtCol, 0.0, l1, l2);
                HOperatorSet.GenRectangle2(out lbObj, lbRow, lbCol, 0.0, l1, l2);
                HOperatorSet.GenRectangle2(out rbObj, rbRow, rbCol, 0.0, l1, l2);
                HOperatorSet.GenRectangle2(out lmObj, lmRow, lmCol, 0.0, l1, l2);
                HOperatorSet.GenRectangle2(out rmObj, rmRow, rmCol, 0.0, l1, l2);
                HOperatorSet.GenRectangle2(out tmObj, tmRow, tmCol, 0.0, l1, l2);
                HOperatorSet.GenRectangle2(out bmObj, bmRow, bmCol, 0.0, l1, l2);

                HOperatorSet.ConcatObj(ptObj, ltObj, out ptObj);
                HOperatorSet.ConcatObj(ptObj, rtObj, out ptObj);
                HOperatorSet.ConcatObj(ptObj, lbObj, out ptObj);
                HOperatorSet.ConcatObj(ptObj, rbObj, out ptObj);
                HOperatorSet.ConcatObj(ptObj, lmObj, out ptObj);
                HOperatorSet.ConcatObj(ptObj, rmObj, out ptObj);
                HOperatorSet.ConcatObj(ptObj, tmObj, out ptObj);
                HOperatorSet.ConcatObj(ptObj, bmObj, out ptObj);

                m_DrawLeft = srcLeft;
                m_DrawTop = srcTop;
                m_DrawWidth = srcSchW;
                m_DrawHeight = srcSchH;
            }

            if (checkBoxIsMaterialSize.Checked)
            {
                textBoxMaterialW.Text = srcSchW.ToString();
                textBoxMaterialH.Text = srcSchH.ToString();
            }
            else
            {
                textBoxSearchLeft.Text = srcLeft.ToString();
                textBoxSearchTop.Text = srcTop.ToString();
                textBoxSearchWidth.Text = srcSchW.ToString();
                textBoxSearchHeight.Text = srcSchH.ToString();
            }

            if (tmpResize)
            {
                DispZoomImage(true, factor, drawImage);
                DispZoomEdges(factor, drawObj, true);

                if (isMouseUp)
                {
                    DispZoomRegions(factor, ptObj, true, "blue");
                }
            }
            else
            {
                DispImage(true, drawImage);
                DispEdges(drawObj, true);

                if (isMouseUp)
                {
                    DispRegions(ptObj, true, "blue");
                }
            }

            return;
        }

        private void SrcImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_LoopTest)
            {
                return;
            }

            if (!hWindowControl_SrcImage.ClientRectangle.Contains(e.Location))
            {
                return;
            }

            if (m_DrawRect)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (m_EightPt >= 1 && m_EightPt <= 8)
                    {
                        int ptx = 0, pty = 0, srcX0 = 0, srcY0 = 0, srcX1 = 0, srcY1 = 0;

                        ptx = e.Location.X;
                        pty = e.Location.Y;

                        switch (m_EightPt)
                        {
                            case 1:
                                srcX0 = ptx;
                                srcY0 = pty;
                                srcX1 = m_SrcDrawX1;
                                srcY1 = m_SrcDrawY1;

                                break;

                            case 2:
                                srcX0 = m_SrcDrawX0;
                                srcY0 = pty;
                                srcX1 = ptx;
                                srcY1 = m_SrcDrawY1;

                                break;

                            case 3:
                                srcX0 = ptx;
                                srcY0 = m_SrcDrawY0;
                                srcX1 = m_SrcDrawX1;
                                srcY1 = pty;

                                break;

                            case 4:
                                srcX0 = m_SrcDrawX0;
                                srcY0 = m_SrcDrawY0;
                                srcX1 = ptx;
                                srcY1 = pty;

                                break;

                            case 5:
                                srcX0 = ptx;
                                srcY0 = m_SrcDrawY0;
                                srcX1 = m_SrcDrawX1;
                                srcY1 = m_SrcDrawY1;

                                break;

                            case 6:
                                srcX0 = m_SrcDrawX0;
                                srcY0 = m_SrcDrawY0;
                                srcX1 = ptx;
                                srcY1 = m_SrcDrawY1;

                                break;

                            case 7:
                                srcX0 = m_SrcDrawX0;
                                srcY0 = pty;
                                srcX1 = m_SrcDrawX1;
                                srcY1 = m_SrcDrawY1;

                                break;

                            case 8:
                                srcX0 = m_SrcDrawX0;
                                srcY0 = m_SrcDrawY0;
                                srcX1 = m_SrcDrawX1;
                                srcY1 = pty;

                                break;

                            default:

                                break;
                        }

                        m_SrcDrawX0 = srcX0;
                        m_SrcDrawY0 = srcY0;
                        DrawRectCont(m_RotateSrcImage, m_SrcFactor, srcX1, srcY1, false);
                    }
                    else
                    {
                        DrawRectCont(m_RotateSrcImage, m_SrcFactor, e.Location.X, e.Location.Y, false);
                    }
                }
            }

            return;
        }

        private void SrcImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_LoopTest)
            {
                return;
            }

            if (!hWindowControl_SrcImage.ClientRectangle.Contains(e.Location))
            {
                return;
            }

            //             if (e.Button == MouseButtons.Right)
            //             {
            //                 m_DrawRect = !m_DrawRect;
            //                 checkBoxIsDrawRect.Checked = m_DrawRect;
            //                 DrawRectCont(m_RotateSrcImage, m_SrcFactor, m_SrcDrawX1, m_SrcDrawY1, true);
            // 
            //                 return;
            //             }

            if (m_DrawRect && e.Button == MouseButtons.Left)
            {
                if (m_EightPt >= 1 && m_EightPt <= 8)
                {
                    int ptx = 0, pty = 0, srcX0 = 0, srcY0 = 0, srcX1 = 0, srcY1 = 0;

                    ptx = e.Location.X;
                    pty = e.Location.Y;

                    switch (m_EightPt)
                    {
                        case 1:
                            srcX0 = ptx;
                            srcY0 = pty;
                            srcX1 = m_SrcDrawX1;
                            srcY1 = m_SrcDrawY1;

                            break;

                        case 2:
                            srcX0 = m_SrcDrawX0;
                            srcY0 = pty;
                            srcX1 = ptx;
                            srcY1 = m_SrcDrawY1;

                            break;

                        case 3:
                            srcX0 = ptx;
                            srcY0 = m_SrcDrawY0;
                            srcX1 = m_SrcDrawX1;
                            srcY1 = pty;

                            break;

                        case 4:
                            srcX0 = m_SrcDrawX0;
                            srcY0 = m_SrcDrawY0;
                            srcX1 = ptx;
                            srcY1 = pty;

                            break;

                        case 5:
                            srcX0 = ptx;
                            srcY0 = m_SrcDrawY0;
                            srcX1 = m_SrcDrawX1;
                            srcY1 = m_SrcDrawY1;

                            break;

                        case 6:
                            srcX0 = m_SrcDrawX0;
                            srcY0 = m_SrcDrawY0;
                            srcX1 = ptx;
                            srcY1 = m_SrcDrawY1;

                            break;

                        case 7:
                            srcX0 = m_SrcDrawX0;
                            srcY0 = pty;
                            srcX1 = m_SrcDrawX1;
                            srcY1 = m_SrcDrawY1;

                            break;

                        case 8:
                            srcX0 = m_SrcDrawX0;
                            srcY0 = m_SrcDrawY0;
                            srcX1 = m_SrcDrawX1;
                            srcY1 = pty;

                            break;

                        default:

                            break;
                    }

                    m_SrcDrawX0 = srcX0;
                    m_SrcDrawY0 = srcY0;
                    m_SrcDrawX1 = srcX1;
                    m_SrcDrawY1 = srcY1;
                }
                else
                {
                    m_SrcDrawX1 = e.Location.X;
                    m_SrcDrawY1 = e.Location.Y;
                }

                DrawRectCont(m_RotateSrcImage, m_SrcFactor, m_SrcDrawX1, m_SrcDrawY1, true);

                return;
            }

            if (!m_IsSrcMagnify)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                int row1 = 0, col1 = 0, row2 = 0, col2 = 0;
                double dbRowMove = 0.0, dbColMove = 0.0;

                if (m_SrcMouseX0 == 0 || m_SrcMouseY0 == 0)
                {
                    return;
                }

                m_SrcMouseX1 = e.Location.Y;
                m_SrcMouseY1 = e.Location.X;
                dbRowMove = m_SrcMouseX0 - m_SrcMouseX1;//计算光标在X轴拖动的距离
                dbColMove = m_SrcMouseY0 - m_SrcMouseY1;//计算光标在Y轴拖动的距离

                hWindowControl_SrcImage.HalconWindow.GetPart(out row1, out col1, out row2, out col2);//计算HWindow控件在当前状态下显示图像的位置
                hWindowControl_SrcImage.HalconWindow.SetPart((int)(row1 + dbRowMove), (int)(col1 + dbColMove),
                    (int)(row2 + dbRowMove), (int)(col2 + dbColMove));//根据拖动距离调整HWindows控件显示图像的位置

                hWindowControl_SrcImage.HalconWindow.ClearWindow();
                DispZoomImage(true, m_SrcFactor, m_RotateSrcImage);

                CleanDrawRect();
            }

            return;
        }
        */
    }
}
