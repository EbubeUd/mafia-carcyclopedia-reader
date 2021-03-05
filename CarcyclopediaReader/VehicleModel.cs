using System;
using System.Collections.Generic;
using System.Text;

namespace CarcyclopediaReader
{
    public class VehicleModel
    {
        public VehicleModel()
        {
            association = new Association();
            categorization = new Categorization();
            colors = new Colors();
            other = new Other();
        }
        //Vehicle Overview
        public Association association;
        public Categorization categorization;
        public Colors colors;
        public Other other;


    }

    [Serializable]
    public class Association
    {
        public string vehModelName;
        public string vehWreckFileName;
        public string vehShadFileName;
        public string vehIngameName;
        public int unknown;
    }

    [Serializable]
    public class Categorization
    {
        public int vehRaceCategory;
        public int vehClassID;
        public int unknown2;
        public int vehStealMask;
        public int vehStealTime;
        public int unknown3;
        public int unknown4;
        public int vehGarageMask;
        public int vehGroupID;
        public int vehColorNum;
        public int unknown5;
        public int vehEncyMask;
    }

    [Serializable]
    public class Colors
    {
        public int vehCol1;
        public int vehCol2;
        public int vehCol3;
        public int vehCol4;
        public int vehCol5;
 
    }

    [Serializable]
    public class Other
    {
        public int  vehDescription;

    }



}
