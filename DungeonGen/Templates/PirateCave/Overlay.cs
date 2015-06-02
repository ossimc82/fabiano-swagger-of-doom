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

namespace DungeonGenerator.Templates.PirateCave {
	internal class Overlay : MapRender {
		public override void Rasterize() {
			var wall = new DungeonTile {
				TileType = PirateCaveTemplate.Composite,
				Object = new DungeonObject {
					ObjectType = PirateCaveTemplate.CaveWall
				}
			};
			var water = new DungeonTile {
				TileType = PirateCaveTemplate.ShallowWater
			};
			var space = new DungeonTile {
				TileType = PirateCaveTemplate.Space
			};

			int w = Rasterizer.Width, h = Rasterizer.Height;
			var buf = Rasterizer.Bitmap;
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != PirateCaveTemplate.ShallowWater)
						continue;

					bool notWall = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						notWall = false;
					else if (buf[x + 1, y].TileType == PirateCaveTemplate.BrownLines ||
					         buf[x - 1, y].TileType == PirateCaveTemplate.BrownLines ||
					         buf[x, y + 1].TileType == PirateCaveTemplate.BrownLines ||
					         buf[x, y - 1].TileType == PirateCaveTemplate.BrownLines) {
						notWall = true;
					}
					if (!notWall)
						buf[x, y] = wall;
				}

			var tmp = (DungeonTile[,])buf.Clone();
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != PirateCaveTemplate.Composite)
						continue;

					bool nearWater = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						nearWater = false;
					else if (tmp[x + 1, y].TileType == PirateCaveTemplate.ShallowWater ||
					         tmp[x - 1, y].TileType == PirateCaveTemplate.ShallowWater ||
					         tmp[x, y + 1].TileType == PirateCaveTemplate.ShallowWater ||
					         tmp[x, y - 1].TileType == PirateCaveTemplate.ShallowWater) {
						nearWater = true;
					}
					if (nearWater && Rand.NextDouble() > 0.4)
						buf[x, y] = water;
				}

			tmp = (DungeonTile[,])buf.Clone();
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != PirateCaveTemplate.Composite)
						continue;

					bool allWall = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						allWall = true;
					else {
						allWall = true;
						for (int dx = -1; dx <= 1 && allWall; dx++)
							for (int dy = -1; dy <= 1 && allWall; dy++) {
								if (tmp[x + dx, y + dy].TileType != PirateCaveTemplate.Composite) {
									allWall = false;
									break;
								}
							}
					}
					if (allWall)
						buf[x, y] = space;
				}
		}
	}
}