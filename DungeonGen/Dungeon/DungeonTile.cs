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

namespace DungeonGenerator.Dungeon {
	public struct TileType {
		public readonly uint Id;
		public readonly string Name;

		public TileType(uint id, string name) {
			Id = id;
			Name = name;
		}

		public static bool operator ==(TileType a, TileType b) {
			return a.Id == b.Id || a.Name == b.Name;
		}

		public static bool operator !=(TileType a, TileType b) {
			return a.Id != b.Id && a.Name != b.Name;
		}

		public override int GetHashCode() {
			return Name.GetHashCode();
		}

		public override bool Equals(object obj) {
			return obj is TileType && (TileType)obj == this;
		}

		public override string ToString() {
			return Name;
		}
	}

	public struct DungeonTile {
		public TileType TileType;
		public string Region;
		public DungeonObject Object;
	}
}