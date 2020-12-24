using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace ViewWindow.Config
{
    [Serializable]
    public class Rectangle2
    {
        private double _row;
        private double _column;
        private double _phi;
        private double _length1;
        private double _length2;
        [XmlElement(ElementName = "Row")]
        public double Row
        {
            get { return this._row; }
            set { this._row = value; }
        }
        [XmlElement(ElementName = "Column")]
        public double Column
        {
            get { return this._column; }
            set { this._column = value; }
        }
        [XmlElement(ElementName = "Phi")]
        public double Phi
        {
            get { return this._phi; }
            set { this._phi = value; }
        }
        [XmlElement(ElementName = "Length1")]
        public double Length1
        {
            get { return this._length1; }
            set { this._length1 = value; }
        }
        [XmlElement(ElementName = "Length2")]
        public double Length2
        {
            get { return this._length2; }
            set { this._length2 = value; }
        }

        public Rectangle2()
        {

        }

        public Rectangle2(double row, double column, double phi, double length1, double length2)
        {
            this._row = row;
            this._column = column;
            this._phi = phi;
            this._length1 = length1;
            this._length2 = length2;
        }
    }
}
