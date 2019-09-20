using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebukam.Slate
{
    public static class SlotNeighbor
    {
        
        // LEFT/RIGHT is over X
        public static IntTrio LEFT = new IntTrio(1, 0, 0);
        public static IntTrio RIGHT = new IntTrio(-1, 0, 0);
        
        // UP/DOWN is over Y
        public static IntTrio UP = new IntTrio(0, 1, 0);
        public static IntTrio DOWN = new IntTrio(0, -1, 0);

        // FRONT/BACK is over Z
        public static IntTrio FRONT = new IntTrio(0, 0, 1);
        public static IntTrio BACK = new IntTrio(0, 0, -1);

    }
}
