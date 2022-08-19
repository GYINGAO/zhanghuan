using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qqqq.models {
    [Serializable]
    public class BPD {
        public double mileage { get; set; }//里程
        public double H { get; set; }//高程
        public double R { get; set; }//半径
        public double i { get; set; }//坡度
    }
}
