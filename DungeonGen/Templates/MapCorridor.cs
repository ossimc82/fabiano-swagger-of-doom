/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using DungeonGenerator.Dungeon;
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates {
	public class MapCorridor {
		protected BitmapRasterizer<DungeonTile> Rasterizer { get; private set; }
		protected DungeonGraph Graph { get; private set; }
		protected Random Rand { get; private set; }

		internal void Init(BitmapRasterizer<DungeonTile> rasterizer, DungeonGraph graph, Random rand) {
			Rasterizer = rasterizer;
			Graph = graph;
			Rand = rand;
		}

		public virtual void Rasterize(Room src, Room dst, Point srcPos, Point dstPos) {
		}

		protected void Default(Point srcPos, Point dstPos, DungeonTile tile) {
			if (srcPos.X == dstPos.X) {
				if (srcPos.Y > dstPos.Y)
					Utils.Swap(ref srcPos, ref dstPos);
				Rasterizer.FillRect(new Rect(srcPos.X, srcPos.Y, srcPos.X + Graph.Template.CorridorWidth, dstPos.Y), tile);
			}
			else if (srcPos.Y == dstPos.Y) {
				if (srcPos.X > dstPos.X)
					Utils.Swap(ref srcPos, ref dstPos);
				Rasterizer.FillRect(new Rect(srcPos.X, srcPos.Y, dstPos.X, srcPos.Y + Graph.Template.CorridorWidth), tile);
			}
		}
	}
}