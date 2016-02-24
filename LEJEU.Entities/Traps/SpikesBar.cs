using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Entities.Traps
{
    public class SpikesBar : Trap
    {
        public SpikesBar()
        {
            NeededInfos = SecondaryInfos.ActivationZone;
        }
    }
}
