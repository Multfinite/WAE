﻿namespace TSMapEditor.Models
{
    public class Infantry : Foot<InfantryType>
    {
        public Infantry(InfantryType objectType) : base(objectType)
        {
        }

        // [Infantry]
        // INDEX=OWNER,ID,HEALTH,X,Y,SUB_CELL,MISSION,FACING,TAG,VETERANCY,GROUP,HIGH,AUTOCREATE_NO_RECRUITABLE,AUTOCREATE_YES_RECRUITABLE

        public override RTTIType WhatAmI() => RTTIType.Infantry;

        public SubCell SubCell { get; set; }
    }
}
