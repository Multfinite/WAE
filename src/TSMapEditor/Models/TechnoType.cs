﻿using System;

namespace TSMapEditor.Models
{
    public abstract class TechnoType : GameObjectType
    {
        public TechnoType(string iniName) : base(iniName)
        {
        }

        public string Image { get; set; }
        public string Owner { get; set; }

        public Weapon Primary { get; set; }
        public Weapon Secondary { get; set; }

        public double GuardRange { get; set; }

        public double GetWeaponRange()
        {
            double primaryRange = Primary != null ? Primary.Range : 0.0;
            double secondaryRange = Secondary != null ? Secondary.Range : 0.0;

            return Math.Max(primaryRange, secondaryRange);
        }
    }
}
