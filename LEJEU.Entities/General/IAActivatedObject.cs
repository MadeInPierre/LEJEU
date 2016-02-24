using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Entities
{
    public class IAActivatedObject : GameObject
    { //covers all objects that potentially needs an IA (enemies and traps)
        public enum SecondaryInfos
        {
            None,               // E T
            DualWaypoints,      // E 
            Radius,             // E T
            Path,               // E 
            ActivationZone,     // E T
            SpawnPoint          // (E T) 
        }
        public SecondaryInfos NeededInfos;
    }
}
