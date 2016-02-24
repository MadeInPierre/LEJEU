using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Entities.Enemies
{
    public class Schrumbli : Enemy
    {
        public Schrumbli()
        {
            NeededInfos = SecondaryInfos.DualWaypoints;
        }
    }
}
