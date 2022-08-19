using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qqqq.models {
    public class Coordinate {
        public Coordinate(double mileage, double spaceMile,
                          double X, double Y, double H,
                          double Xl, double Yl, double Hl,
                          double Xr, double Yr, double Hr,
                          double A, double w, string pType) {
            this.mileage = mileage;
            this.spaceMile = spaceMile;
            this.X = X;
            this.Y = Y;
            this.H = H;
            this.Xl = Xl;
            this.Yl = Yl;
            this.Hl = Hl;
            this.Xr = Xr;
            this.Yr = Yr;
            this.Hr = Hr;
            this.A = A;
            this.w = w;
            this.pType = pType;
        }
        public double mileage { get; set; }//里程    
        public double spaceMile { get; set; }//空间里程
        public double X { get; set; }//中线北坐标
        public double Y { get; set; }//中线东坐标
        public double H { get; set; }//中线高程
        public double Xl { get; set; }//左轨北坐标
        public double Yl { get; set; }//左轨东坐标
        public double Hl { get; set; }//左轨高程
        public double Xr { get; set; }//右轨北坐标
        public double Yr { get; set; }//右轨东坐标
        public double Hr { get; set; }//右轨高程
        public double A { get; set; }//切线方位角
        public double w { get; set; }//竖象限角
        public string pType { get; set; }//所处区段线路类型
    }
}
