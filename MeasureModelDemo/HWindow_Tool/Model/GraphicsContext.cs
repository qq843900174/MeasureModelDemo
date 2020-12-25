using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using HalconDotNet;

namespace ViewWindow.Model
{
    public delegate void GCDelegate(string val);
    public class GraphicsContext
    {

        public const string GC_COLOR = "Color";//dev_set_coloer
        public const string GC_COLORED = "Colored";//dev_set_colored
        public const string GC_LINEWIDTH = "LineWidth";//set_line_width
        public const string GC_DRAWMODE = "DrawMode";//set_draw
        public const string GC_SHAPE = "Shape";//set_shape
        public const string GC_LUT = "Lut";//set_lut
        public const string GC_PAINT = "Paint";//set_paint
        public const string GC_LINESTYLE = "LineStyle";//set_line_style

        /// <summary> 
        /// Hashlist包含全部图形模式条目（由GC_所定义），
        /// 然后链接到某个Halcon对象描述其图像内容。
        /// </summary>
        private Hashtable graphicalSettings;

        /// <summary> 
        /// 备份最近的图像内容应用于窗体。
        /// </summary>
        public Hashtable stateOfSettings;

        private IEnumerator iterator;

        /// <summary> 
        /// 将消息从图形内容委托给某个观察类。
        /// </summary>
        public GCDelegate gcNotification;
        /// <summary> 
        /// 创建一个没有图形模式的图像内容
        /// </summary> 
        public GraphicsContext()
        {
            graphicalSettings = new Hashtable(10, 0.2f);
            gcNotification = new GCDelegate(dummy);
            stateOfSettings = new Hashtable(10, 0.2f);
        }

        /// <summary> 
        /// 创建一个定义在Hashtable setting的图形模式的图像内容的实例
        /// </summary> 
        /// <param name="settings"> 
        /// 模式条目, 描述图形内容
        /// </param>
        public GraphicsContext(Hashtable settings)
        {
            graphicalSettings = settings;
            gcNotification = new GCDelegate(dummy);
            stateOfSettings = new Hashtable(10, 0.2f);
        }

        /// <summary>应用图形内容到HalconWindow</summary>
        /// <param name="window">活动的HalconWindow</param>
        /// <param name="cContext">
        /// 包含给窗体图像模式的条目
        /// </param>
        public void applyContext(HWindow window, Hashtable cContext)
        {
            string key = "";
            string valS = "";
            int valI = -1;
            HTuple valH = null;

            iterator = cContext.Keys.GetEnumerator();

            try
            {
                while (iterator.MoveNext())
                {

                    key = (string)iterator.Current;

                    if (stateOfSettings.Contains(key) &&
                        stateOfSettings[key] == cContext[key])
                        continue;

                    switch (key)
                    {
                        case GC_COLOR:
                            valS = (string)cContext[key];
                            window.SetColor(valS);
                            if (stateOfSettings.Contains(GC_COLORED))
                                stateOfSettings.Remove(GC_COLORED);

                            break;
                        case GC_COLORED:
                            valI = (int)cContext[key];
                            window.SetColored(valI);

                            if (stateOfSettings.Contains(GC_COLOR))
                                stateOfSettings.Remove(GC_COLOR);

                            break;
                        case GC_DRAWMODE:
                            valS = (string)cContext[key];
                            window.SetDraw(valS);
                            break;
                        case GC_LINEWIDTH:
                            valI = (int)cContext[key];
                            window.SetLineWidth(valI);
                            break;
                        case GC_LUT:
                            valS = (string)cContext[key];
                            window.SetLut(valS);
                            break;
                        case GC_PAINT:
                            valS = (string)cContext[key];
                            window.SetPaint(valS);
                            break;
                        case GC_SHAPE:
                            valS = (string)cContext[key];
                            window.SetShape(valS);
                            break;
                        case GC_LINESTYLE:
                            valH = (HTuple)cContext[key];
                            window.SetLineStyle(valH);
                            break;
                        default:
                            break;
                    }


                    if (valI != -1)
                    {
                        if (stateOfSettings.Contains(key))
                            stateOfSettings[key] = valI;
                        else
                            stateOfSettings.Add(key, valI);

                        valI = -1;
                    }
                    else if (valS != "")
                    {
                        if (stateOfSettings.Contains(key))
                            stateOfSettings[key] = valI;
                        else
                            stateOfSettings.Add(key, valI);

                        valS = "";
                    }
                    else if (valH != null)
                    {
                        if (stateOfSettings.Contains(key))
                            stateOfSettings[key] = valI;
                        else
                            stateOfSettings.Add(key, valI);

                        valH = null;
                    }
                }
            }
            catch (HOperatorException e)
            {
                gcNotification(e.Message);
                return;
            }
        }


        /// <summary>设置一个值给GC_COLOR</summary>
        /// <param name="val"> 
        /// 单色，"green","blue"...等等 
        /// </param>
        public void setColorAttribute(string val)
        {
            if (graphicalSettings.ContainsKey(GC_COLORED))
                graphicalSettings.Remove(GC_COLORED);

            addValue(GC_COLOR, val);
        }

        /// <summary>Sets a value for the graphical mode GC_COLORED</summary>
        /// <param name="val"> 
        /// The colored mode, which can be either "colored3" or "colored6"
        /// or "colored12" 
        /// </param>
        public void setColoredAttribute(int val)
        {
            if (graphicalSettings.ContainsKey(GC_COLOR))
                graphicalSettings.Remove(GC_COLOR);

            addValue(GC_COLORED, val);
        }

        /// <summary>Sets a value for the graphical mode GC_DRAWMODE</summary>
        /// <param name="val"> 
        /// One of the possible draw modes: "margin" or "fill" 
        /// </param>
        public void setDrawModeAttribute(string val)
        {
            addValue(GC_DRAWMODE, val);
        }

        /// <summary>Sets a value for the graphical mode GC_LINEWIDTH</summary>
        /// <param name="val"> 
        /// The line width, which can range from 1 to 50 
        /// </param>
        public void setLineWidthAttribute(int val)
        {
            addValue(GC_LINEWIDTH, val);
        }

        /// <summary>Sets a value for the graphical mode GC_LUT</summary>
        /// <param name="val"> 
        /// One of the possible modes of look up tables. For 
        /// further information on particular setups, please refer to the
        /// Reference Manual entry of the operator set_lut.
        /// </param>
        public void setLutAttribute(string val)
        {
            addValue(GC_LUT, val);
        }


        /// <summary>Sets a value for the graphical mode GC_PAINT</summary>
        /// <param name="val"> 
        /// One of the possible paint modes. For further 
        /// information on particular setups, please refer refer to the
        /// Reference Manual entry of the operator set_paint.
        /// </param>
        public void setPaintAttribute(string val)
        {
            addValue(GC_PAINT, val);
        }


        /// <summary>Sets a value for the graphical mode GC_SHAPE</summary>
        /// <param name="val">
        /// One of the possible shape modes. For further 
        /// information on particular setups, please refer refer to the
        /// Reference Manual entry of the operator set_shape.
        /// </param>
        public void setShapeAttribute(string val)
        {
            addValue(GC_SHAPE, val);
        }

        /// <summary>Sets a value for the graphical mode GC_LINESTYLE</summary>
        /// <param name="val"> 
        /// A line style mode, which works 
        /// identical to the input for the HDevelop operator 
        /// 'set_line_style'. For particular information on this 
        /// topic, please refer to the Reference Manual entry of the operator
        /// set_line_style.
        /// </param>
        public void setLineStyleAttribute(HTuple val)
        {
            addValue(GC_LINESTYLE, val);
        }

        /// <summary> 
        /// 为图像模式向hashlist‘graphicalSettings’添加一个值，通过参数“key”描述。
        /// </summary>
        /// <param name="key"> 
        /// 由常量GC_*定义的一个图像模式 
        /// </param>
        /// <param name="val"> 
        /// 定义图像模式'key'的整形值
        /// </param>
        private void addValue(string key, int val)
        {
            if (graphicalSettings.ContainsKey(key))
                graphicalSettings[key] = val;
            else
                graphicalSettings.Add(key, val);
        }

        /// <summary>
        /// 为图像模式向hashlist‘graphicalSettings’添加一个值，通过参数“key”描述。
        /// </summary>
        /// <param name="key"> 
        /// 由常量GC_*定义的一个图像模式 
        /// </param>
        /// <param name="val"> 
        /// 定义图像模式'key'的字符串值
        /// </param>
        private void addValue(string key, string val)
        {
            if (graphicalSettings.ContainsKey(key))
                graphicalSettings[key] = val;
            else
                graphicalSettings.Add(key, val);
        }


        /// <summary> 
        /// 为图像模式向hashlist‘graphicalSettings’添加一个值，通过参数“key”描述。
        /// </summary>
        /// <param name="key">
        /// 由常量GC_*定义的一个图像模式 
        /// </param>
        /// <param name="val"> 
        /// 定义图像模式'key'的HTuple值
        /// </param>
        private void addValue(string key, HTuple val)
        {
            if (graphicalSettings.ContainsKey(key))
                graphicalSettings[key] = val;
            else
                graphicalSettings.Add(key, val);
        }

        /// <summary> 
        /// 清楚图形设置的列表。
        /// 在绘制对象之前不会进行任何图形更改，因为没有要应用于窗口的图形条目。
        /// </summary>
        public void clear()
        {
            graphicalSettings.Clear();
        }


        /// <summary> 
        /// 返回graphicsContext实例的精确克隆
        /// </summary>
        public GraphicsContext copy()
        {
            return new GraphicsContext((Hashtable)this.graphicalSettings.Clone());
        }


        /// <summary> 
        /// 如果hashtable包含“key”，则返回相应值
        /// </summary>
        /// <param name="key"> 
        /// GC_*其中的一个key的值 
        /// </param>
        public object getGraphicsAttribute(string key)
        {
            if (graphicalSettings.ContainsKey(key))
                return graphicalSettings[key];

            return null;
        }

        /// <summary> 
        /// 返回包含当前图像内容的hashlist列表的副本
        /// </summary>
        /// <returns> 当前图形内容 </returns>
        public Hashtable copyContextList()
        {
            return (Hashtable)graphicalSettings.Clone();
        }


        /********************************************************************/
        public void dummy(string val) { }
    }
}
