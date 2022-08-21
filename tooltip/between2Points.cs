using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tooltip
{
    public class between2Points
    {
        public int index1, index2, distance;
        public between2Points(int index1, int index2, int distance)
        {
            this.index1 = index1;
            this.index2 = index2;
            this.distance = distance;
        }
        public between2Points(int index1, int index2)
        {
            this.index1 = index1;
            this.index2 = index2;
            this.distance = 0;
        }
    }
}
