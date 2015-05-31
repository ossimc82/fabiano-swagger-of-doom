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
using RotMG.Common.Rasterizer;
using wServer.generator.dungeon;

namespace wServer.generator.templates.pirateCave {
	internal class Corridor : MapCorridor {
		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Room src, Room dst, Point srcPos,
			Point dstPos, Random rand) {
			var tile = new DungeonTile {
				TileType = PirateCaveTemplate.BrownLines
			};

			if (srcPos.X == dstPos.X) {
				if (srcPos.Y > dstPos.Y)
					Utils.Swap(ref srcPos, ref dstPos);
				rasterizer.FillRect(new Rect(srcPos.X, srcPos.Y, srcPos.X + 2, dstPos.Y), tile);
			}
			else if (srcPos.Y == dstPos.Y) {
				if (srcPos.X > dstPos.X)
					Utils.Swap(ref srcPos, ref dstPos);
				rasterizer.FillRect(new Rect(srcPos.X, srcPos.Y, dstPos.X, srcPos.Y + 2), tile);
			}
		}
	}
}