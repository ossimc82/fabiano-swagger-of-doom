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

namespace DungeonGenerator {
	public struct Range {
		public static readonly Range Zero = new Range(0, 0);

		public readonly int Begin;
		public readonly int End;

		public Range(int begin, int end) {
			if (end < begin)
				end = begin;
			Begin = begin;
			End = end;
		}

		public int Random(Random rand) {
			return rand.Next(Begin, End + 1);
		}

		public bool IsEmpty { get { return Begin == End; } }

		public Range Intersection(Range range) {
			return new Range(Math.Max(Begin, range.Begin), Math.Min(End, range.End));
		}

		public override string ToString() {
			return string.Format("[{0}, {1}]", Begin, End);
		}
	}
}