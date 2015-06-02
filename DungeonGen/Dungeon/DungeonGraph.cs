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
using DungeonGenerator.Templates;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Dungeon {
	public class DungeonGraph {
		public DungeonTemplate Template { get; private set; }

		public int Width { get; private set; }
		public int Height { get; private set; }

		public Room[] Rooms { get; private set; }

		internal DungeonGraph(DungeonTemplate template, Room[] rooms) {
			Template = template;

			int dx = int.MaxValue, dy = int.MaxValue;
			int mx = int.MinValue, my = int.MinValue;

			for (int i = 0; i < rooms.Length; i++) {
				var bounds = rooms[i].Bounds;

				if (bounds.X < dx)
					dx = bounds.X;
				if (bounds.Y < dy)
					dy = bounds.Y;

				if (bounds.MaxX > mx)
					mx = bounds.MaxX;
				if (bounds.MaxY > my)
					my = bounds.MaxY;
			}

			const int Pad = 4;

			Width = mx - dx + Pad * 2;
			Height = my - dy + Pad * 2;

			for (int i = 0; i < rooms.Length; i++) {
				var room = rooms[i];
				var pos = room.Pos;
				room.Pos = new Point(pos.X - dx + Pad, pos.Y - dy + Pad);

				foreach (var edge in room.Edges) {
					if (edge.RoomA != room)
						continue;
					if (edge.Linkage.Direction == Direction.South || edge.Linkage.Direction == Direction.North)
						edge.Linkage = new Link(edge.Linkage.Direction, edge.Linkage.Offset - dx + Pad);
					else if (edge.Linkage.Direction == Direction.East || edge.Linkage.Direction == Direction.West)
						edge.Linkage = new Link(edge.Linkage.Direction, edge.Linkage.Offset - dy + Pad);
				}
			}
			Rooms = rooms;
		}
	}
}