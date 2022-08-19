using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qqqq.models {
    public class Horizontal {
        public double mileage { get; set; }//里程    
        public string pType { get; set; }//主点类型
        public double X { get; set; }//北坐标
        public double Y { get; set; }//东坐标
        public double R { get; set; }//半径
        public double l { get; set; }//缓长
        public double A { get; set; }//切线方位角
        public double h { get; set; }//超高
        public string hType { get; set; }//超高设置方式
        public double sp { get; set; }//顺坡距离
        public double widen { get; set; }//加宽值
        public double jk { get; set; }//加宽距离
    }
}
