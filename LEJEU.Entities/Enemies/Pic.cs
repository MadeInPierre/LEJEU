using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Entities.Enemies
{
    public enum Ages
    {
        Baby,
        Adult,
        Old
    }

    public class Pic : Enemy
    {
        public Pic()
        { //Only made for the LevelEditor
            NeededInfos = SecondaryInfos.Path;
        }

        public Pic(Ages age)
        {
            //create the ennemy depending on the age
            // 3 sub-classes :
            //      - PicBaby
            //      - PicAdult
            //      - PicOld

            //if(age == Ages.Baby) subInstance = new PicBaby();
        }
    }
}
