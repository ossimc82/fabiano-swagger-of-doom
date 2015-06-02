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
	internal class BossRoom : Room {
		public override RoomType Type { get { return RoomType.Target; } }

		public override int Width { get { return 42; } }

		public override int Height { get { return 42; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			var buf = rasterizer.Bitmap;
			var bounds = Bounds;

			rasterizer.Copy(AbyssTemplate.MapTemplate, new Rect(10, 10, 52, 52), Pos, tile => tile.TileType.Name == "Space");

			int numCorrupt = new Range(2, 10).Random(rand);
			while (numCorrupt > 0) {
				int x = rand.Next(bounds.X, bounds.MaxX);
				int y = rand.Next(bounds.Y, bounds.MaxY);

				if (buf[x, y].Object == null)
					continue;
				if (buf[x, y].Object.ObjectType != AbyssTemplate.PartialRedFloor)
					continue;

				buf[x, y].Object = null;
				numCorrupt--;
			}

			int numImp = new Range(1, 2).Random(rand);
			int numDemon = new Range(1, 3).Random(rand);
			int numBrute = new Range(1, 3).Random(rand);

			while (numImp > 0 || numDemon > 0 || numBrute > 0) {
				int x = rand.Next(bounds.X, bounds.MaxX);
				int y = rand.Next(bounds.Y, bounds.MaxY);

				if (buf[x, y].Object != null || buf[x, y].TileType == AbyssTemplate.Space)
					continue;

				switch (rand.Next(3)) {
					case 0:
						if (numImp > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = AbyssTemplate.AbyssImp
							};
							numImp--;
						}
						break;
					case 1:
						if (numDemon > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = AbyssTemplate.AbyssDemon[rand.Next(AbyssTemplate.AbyssDemon.Length)]
							};
							numDemon--;
						}
						break;
					case 2:
						if (numBrute > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = AbyssTemplate.AbyssBrute[rand.Next(AbyssTemplate.AbyssBrute.Length)]
							};
							numBrute--;
						}
						break;
				}
			}
		}
	}
}