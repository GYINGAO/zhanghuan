using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qqqq.models {
    public class Deviation {
        public string ID { get; set; }
        public double designMile { get; set; } //反算的设计里程（可能需要改一下）
        public double fym { get; set; } //中线横偏
        public double fhm { get; set; } //中线竖偏
        public double fyl { get; set; } //左轨横偏
        public double fhl { get; set; } //左轨竖偏
        public double fyr { get; set; } //右轨横偏
        public double fhr { get; set; } //右轨竖偏
    }
}
