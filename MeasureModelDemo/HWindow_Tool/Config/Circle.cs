using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ViewWindow.Config
{
    //序列化，有助于远程调用
    [Serializable]
    public class Circle
    {
        private double _row;
        private double _column;
        private double _radius;

        [XmlElement(ElementName = "Row")]
        public double Row
        {
            get { return this._row; }
            //
        }
    }
}
