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
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.PirateCave {
	internal class StartRoom : Room {
		readonly int radius;

		public StartRoom(int radius) {
			this.radius = radius;
		}

		public override RoomType Type { get { return RoomType.Start; } }

		public override int Width { get { return radius * 2 + 1; } }

		public override int Height { get { return radius * 2 + 1; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			var tile = new DungeonTile {
				TileType = PirateCaveTemplate.LightSand
			};

			var cX = Pos.X + radius + 0.5;
			var cY = Pos.Y + radius + 0.5;
			var bounds = Bounds;
			var r2 = radius * radius;
			var buf = rasterizer.Bitmap;

			double pR = rand.NextDouble() * (radius - 2), pA = rand.NextDouble() * 2 * Math.PI;
			int pX = (int)(cX + Math.Cos(pR) * pR);
			int pY = (int)(cY + Math.Sin(pR) * pR);

			for (int x = bounds.X; x < bounds.MaxX; x++)
				for (int y = bounds.Y; y < bounds.MaxY; y++) {
					if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2) {
						buf[x, y] = tile;
						if (rand.NextDouble() > 0.95) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = PirateCaveTemplate.PalmTree
							};
						}
					}
					if (x == pX && y == pY) {
						buf[x, y].Region = "Spawn";
						buf[x, y].Object = new DungeonObject {
							ObjectType = PirateCaveTemplate.CowardicePortal
						};
					}
				}
		}
	}
}