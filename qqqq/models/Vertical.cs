using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qqqq.models {
    public class Vertical {
        public double mileage { get; set; }//里程    
        public string pType { get; set; }//主点类型
        public double H { get; set; }//高程
        public double i { get; set; }//坡度
        public double R { get; set; }//半径
        public double T { get; set; }//切线长
        public double vCurve { get; set; }//竖曲线长
    }
}
