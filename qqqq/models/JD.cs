using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qqqq.models {
    [Serializable]
    public class JD {
        public string pName { get; set; }//点名
        public double X { get; set; }//北坐标X
        public double Y { get; set; }//东坐标Y
        public double a { get; set; }//转向角
        public double R { get; set; }//半径
        public double l1 { get; set; }//第一缓长
        public double l2 { get; set; } //第二缓长
        public double h { get; set; }//超高
        public string hType { get; set; } //超高设置方式
        public int curveType { get; set; } //曲线类型{1:回头曲线，2:反向曲线，3:直线段顺坡的圆曲线，0:无特殊类型}
        public double sp1 { get; set; } //第一顺坡距离
        public double sp2 { get; set; } //第二顺坡距离
        public double sp3 { get; set; } //第三顺坡距离
        public double sp4 { get; set; } //第四顺坡距离
        public double widening { get; set; } //加宽值
        public double jk1 { get; set; } //第一加宽距离
        public double jk2 { get; set; } //第二加宽距离
        public double jk3 { get; set; } //第三加宽距离
        public double jk4 { get; set; } //第四加宽距离
    }
}
