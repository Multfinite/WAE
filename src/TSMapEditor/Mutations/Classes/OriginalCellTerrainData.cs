﻿using TSMapEditor.GameMath;

namespace TSMapEditor.Mutations.Classes
{
    struct OriginalCellTerrainData
    {
        public Point2D CellCoords;
        public int TileIndex;
        public byte SubTileIndex;

        public OriginalCellTerrainData(Point2D cellCoords, int tileIndex, byte subTileIndex)
        {
            CellCoords = cellCoords;
            TileIndex = tileIndex;
            SubTileIndex = subTileIndex;
        }
    }
}
