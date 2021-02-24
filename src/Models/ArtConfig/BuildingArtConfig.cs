﻿using Rampastring.Tools;
using System;

namespace TSMapEditor.Models.ArtConfig
{
    public class BuildingArtConfig : IArtConfig
    {
        public BuildingArtConfig() { }

        public int FoundationX { get; set; }
        public int FoundationY { get; set; }
        public int Height { get; set; }
        public bool Remapable { get; set; }
        public bool NewTheater { get; set; }
        public bool TerrainPalette { get; set; }
        public bool Theater { get; set; }

        public void ReadFromIniSection(IniSection iniSection)
        {
            if (iniSection == null)
                return;

            string foundationString = iniSection.GetStringValue("Foundation", string.Empty);
            if (!string.IsNullOrWhiteSpace(foundationString))
            {
                string[] foundationParts = foundationString.Split('x');
                if (foundationParts.Length != 2)
                {
                    throw new InvalidOperationException("Invalid Foundation= specified in Art.ini section " + iniSection.SectionName);
                }

                FoundationX = Conversions.IntFromString(foundationParts[0], 0);
                FoundationY = Conversions.IntFromString(foundationParts[1], 0);
            }

            Height = iniSection.GetIntValue(nameof(Height), Height);
            Remapable = iniSection.GetBooleanValue(nameof(Remapable), Remapable);
            NewTheater = iniSection.GetBooleanValue(nameof(NewTheater), NewTheater);
            TerrainPalette = iniSection.GetBooleanValue(nameof(TerrainPalette), TerrainPalette);
            Theater = iniSection.GetBooleanValue(nameof(Theater), Theater);
        }
    }
}
