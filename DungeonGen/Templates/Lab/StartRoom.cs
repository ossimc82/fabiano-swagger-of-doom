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

namespace DungeonGenerator.Templates.Lab {
	internal class StartRoom : FixedRoom {
		static readonly Rect template = new Rect(0, 96, 26, 128);

		public override RoomType Type { get { return RoomType.Start; } }

		public override int Width { get { return template.MaxX - template.X; } }

		public override int Height { get { return template.MaxY - template.Y; } }

		static readonly Tuple<Direction, int>[] connections = {
			Tuple.Create(Direction.North, 11)
		};

		public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			rasterizer.Copy(LabTemplate.MapTemplate, template, Pos);
			LabTemplate.DrawSpiderWeb(rasterizer, Bounds, rand);
		}
	}
}