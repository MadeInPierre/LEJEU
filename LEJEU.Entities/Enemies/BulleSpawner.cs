using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Entities.Enemies
{
    public class BulleSpawner : Enemy
    {
        public BulleSpawner()
        {
            NeededInfos = SecondaryInfos.ActivationZone;
        }
    }
}
