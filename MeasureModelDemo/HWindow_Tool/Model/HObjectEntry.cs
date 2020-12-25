using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Collections;

namespace ViewWindow.Model
{
    /// <summary>
    /// 这个类是个辅助类，用于连接图像内容给HObject，图像内容用哈希列表描述，包含图像模式和对应值的条目。
    /// 这些图像状态在显示对象前应用于窗体。
    /// </summary>
    public class HObjectEntry
    {
        public Hashtable gContext;

        public HObject HObj;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="obj">HObject连接到图像内容gc</param>
        /// <param name="gc">图像状态的哈希列表，在显示对象前应用于窗体。</param>
        public HObjectEntry(HObject obj, Hashtable gc)
        {
            gContext = gc;
            HObj = obj;
        }
        /// <summary>
        /// 清除类成员HObj和gContext的条目
        /// </summary>
        public void clear()
        {
            gContext.Clear();
            HObj.Dispose();
        }
    }
}
