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

namespace DungeonGenerator.Templates.Abyss {
	internal class StartRoom : Room {
		readonly int len;
		internal Point portalPos;

		public StartRoom(int len) {
			this.len = len;
		}

		public override RoomType Type { get { return RoomType.Start; } }

		public override int Width { get { return len; } }

		public override int Height { get { return len; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			rasterizer.FillRect(Bounds, new DungeonTile {
				TileType = AbyssTemplate.RedSmallChecks
			});

			var buf = rasterizer.Bitmap;
			var bounds = Bounds;

			bool portalPlaced = false;
			while (!portalPlaced) {
				int x = rand.Next(bounds.X + 2, bounds.MaxX - 4);
				int y = rand.Next(bounds.Y + 2, bounds.MaxY - 4);
				if (buf[x, y].Object != null)
					continue;

				buf[x, y].Region = "Spawn";
				buf[x, y].Object = new DungeonObject {
					ObjectType = AbyssTemplate.CowardicePortal
				};
				portalPos = new Point(x, y);
				portalPlaced = true;
			}
		}
	}
}