using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace ViewWindow.Model
{
    /// <summary>
    /// 这个类是一个基类，包含用于处理ROI的虚拟方法。因此，继承类需要 定义/重写 这些方法，
    /// 以向ROIController提供有关其（ROI）形状和位置的必要信息。
    /// 示例项目为矩形、直线、圆、圆弧提供派生的ROI形状。
    /// 要使用其他形状的话，必须从基类ROI派生一个新类并实现其方法。
    /// </summary>
    [Serializable]
    public class ROI
    {
        private string color = "yellow";

        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        private string _type;
        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        //继承ROI类的类成员
        protected int NumHandles;
        protected int activeHandleIdx;

        /// <summary>
        /// 定义ROI“正”/“负”的标志
        /// </summary>
        protected int OperatorFlag;

        /// <summary>定义ROI的线风格的参数</summary>
        public HTuple flagLineStyle;

        /// <summary>正ROI标志的常数</summary>
        public const int POSITIVE_FLAG = ROIController.MODE_ROI_POS;

        /// <summary>负ROI标志的常数</summary>
        public const int NEGATIVE_FLAG = ROIController.MODE_ROI_NEG;

        public const int ROI_TYPE_LINE = 10;
        public const int ROI_TYPE_CIRCLE = 11;
        public const int ROI_TYPE_CIRCLEARC = 12;
        public const int ROI_TYPE_RECTANCLE1 = 13;
        public const int ROI_TYPE_RECTANGLE2 = 14;


        protected HTuple posOperation = new HTuple();
        protected HTuple negOperation = new HTuple(new int[] { 2, 2 });

        /// <summary>构造ROI类的抽象类</summary>
        public ROI() { }

        public virtual void createRectangle1(double row1, double col1, double row2, double col2) { }
        public virtual void createRectangle2(double row, double col, double phi, double length1, double length2) { }
        public virtual void createCircle(double row, double col, double radius) { }
        public virtual void createLine(double beginRow, double beginCol, double endRow, double endCol) { }

        /// <summary>在鼠标位置创建一个新的ROI实例</summary>
        /// <param name="midX">
        /// ROI的x轴
        /// x (=column) coordinate for ROI
        /// </param>
        /// <param name="midY">
        /// ROI的y轴
        /// y (=row) coordinate for ROI
        /// </param>
        public virtual void createROI(double midX, double midY) { }

        /// <summary>
        /// Paints the ROI into the supplied window.
        /// 画ROI进提供的窗体
        /// </summary>
        /// <param name="window">HALCON window</param>
        public virtual void draw(HalconDotNet.HWindow window) { }

        /// <summary> 
        /// 返回ROI句柄最接近图像点（x,y）的距离
        /// Returns the distance of the ROI handle being
        /// closest to the image point(x,y)
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        /// <returns> 
        /// 最近ROI句柄距离。
        /// Distance of the closest ROI handle.
        /// </returns>
        public virtual double distToClosestHandle(double x, double y)
        {
            return 0.0;
        }

        /// <summary> 
        /// 画ROI的活动句柄到提供的窗体
        /// Paints the active handle of the ROI object into the supplied window. 
        /// </summary>
        /// <param name="window">HALCON window</param>
        public virtual void displayActive(HalconDotNet.HWindow window) { }

        /// <summary> 
        /// 重新计算ROI的形状，在ROI对象的活动句柄处执行图像坐标（x,y）的平移。
        /// Recalculates the shape of the ROI. Translation is 
        /// performed at the active handle of the ROI object 
        /// for the image coordinate (x,y).
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        public virtual void moveByHandle(double x, double y) { }

        /// <summary>
        /// 获得由ROI描述的halcon region
        /// Gets the HALCON region described by the ROI.
        /// </summary>
        public virtual HRegion getRegion()
        {
            return null;
        }

        public virtual double getDistanceFromStartPoint(double row, double col)
        {
            return 0.0;
        }
        /// <summary>
        /// 获得由ROI描述的模型信息
        /// Gets the model information described by the ROI.
        /// </summary> 
        public virtual HTuple getModelData()
        {
            return null;
        }

        /// <summary>
        /// 由ROI定义的句柄数目
        /// Number of handles defined for the ROI.
        /// </summary>
        /// <returns>
        /// 句柄的数目
        /// Number of handles
        /// </returns>
        public int getNumHandles()
        {
            return NumHandles;
        }

        /// <summary>
        /// 获得ROI的活动句柄
        /// Gets the active handle of the ROI.
        /// </summary>
        /// <returns>
        /// 活动句柄的下标（来自句柄的列表）
        /// Index of the active handle (from the handle list)
        /// </returns>
        public int getActHandleIdx()
        {
            return activeHandleIdx;
        }

        /// <summary>
        /// 获得ROI对象的符号，正/负。
        /// 这个符号用于当创建一个模型区域来匹配来自ROI列表的应用程序。
        /// Gets the sign of the ROI object, being either 
        /// 'positive' or 'negative'. This sign is used when creating a model
        /// region for matching applications from a list of ROIs.
        /// </summary>
        public int getOperatorFlag()
        {
            return OperatorFlag;
        }

        /// <summary>
        /// 设置ROI对象的符号。
        /// 在创建一个匹配应用程序的模型区域时，将使用该符号。
        /// 方法是将迄今为止创建的所有正ROI和负ROI模型的相加。
        /// Sets the sign of a ROI object to be positive or negative. 
        /// The sign is used when creating a model region for matching
        /// applications by summing up all positive and negative ROI models
        /// created so far.
        /// </summary>
        /// <param name="flag">Sign of ROI object</param>
        public void setOperatorFlag(int flag)
        {
            OperatorFlag = flag;

            switch (OperatorFlag)
            {
                case ROI.POSITIVE_FLAG:
                    flagLineStyle = posOperation;
                    break;
                case ROI.NEGATIVE_FLAG:
                    flagLineStyle = negOperation;
                    break;
                default:
                    flagLineStyle = posOperation;
                    break;
            }
        }
    }
}
