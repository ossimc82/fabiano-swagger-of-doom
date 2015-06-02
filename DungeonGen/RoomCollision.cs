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
using System.Collections.Generic;
using DungeonGenerator.Dungeon;
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator {
	public class RoomCollision {
		const int GridScale = 3;
		const int GridSize = 1 << GridScale;

		struct RoomKey {
			public readonly int XKey;
			public readonly int YKey;

			public RoomKey(int x, int y) {
				XKey = x >> GridScale;
				YKey = y >> GridScale;
			}

			public override int GetHashCode() {
				return XKey * 7 + YKey;
			}
		}

		readonly Dictionary<RoomKey, HashSet<Room>> rooms = new Dictionary<RoomKey, HashSet<Room>>();

		void Add(int x, int y, Room rm) {
			var key = new RoomKey(x, y);
			var roomList = rooms.GetValueOrCreate(key, k => new HashSet<Room>());
			roomList.Add(rm);
		}

		public void Add(Room rm) {
			var bounds = rm.Bounds;
			int x = bounds.X, y = bounds.Y;
			for (; y <= bounds.MaxY + GridSize; y += GridSize) {
				for (x = bounds.X; x <= bounds.MaxX + 20; x += GridSize)
					Add(x, y, rm);
			}
		}

		void Remove(int x, int y, Room rm) {
			var key = new RoomKey(x, y);
			HashSet<Room> roomList;
			if (rooms.TryGetValue(key, out roomList))
				roomList.Remove(rm);
		}

		public void Remove(Room rm) {
			var bounds = rm.Bounds;
			int x = bounds.X, y = bounds.Y;
			for (; y <= bounds.MaxY + GridSize; y += GridSize) {
				for (x = bounds.X; x <= bounds.MaxX + 20; x += GridSize)
					Remove(x, y, rm);
			}
		}

		bool HitTest(int x, int y, Rect bounds) {
			var key = new RoomKey(x, y);
			var roomList = rooms.GetValueOrDefault(key, (HashSet<Room>)null);
			if (roomList != null) {
				foreach (var room in roomList)
					if (!room.Bounds.Intersection(bounds).IsEmpty)
						return true;
			}
			return false;
		}

		public bool HitTest(Room rm) {
			var bounds = new Rect(rm.Bounds.X - 1, rm.Bounds.Y - 1, rm.Bounds.MaxX + 1, rm.Bounds.MaxY + 1);

			int x = bounds.X, y = bounds.Y;
			for (; y <= bounds.MaxY + GridSize; y += GridSize) {
				for (x = bounds.X; x <= bounds.MaxX + GridSize; x += GridSize) {
					if (HitTest(x, y, bounds))
						return true;
				}
			}
			return false;
		}
	}
}