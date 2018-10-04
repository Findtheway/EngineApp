using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class MICSetting
    {
        public MICSetting()
        {
            DOA_MIC_Interval = "0.06";
            BF_Select_Angle = "-45";
        }
        public string AEC_Length { get; set; }

        public string DOA_MIC_Interval { get; set; }

        public string BF_Select_Angle { get; set; }

        public bool AES_Status { get; set; }

        public string AES_Level { get; set; }

        public string NR_Level { get; set; }

        public bool DRC_Status { get; set; }

        public bool AGC_Status { get; set; }

        public string DRC_Gain { get; set; }

        public string MIC_Type { get; set; }
    }
}
