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

namespace DungeonGenerator.Templates {
	public class MapRender {
		protected BitmapRasterizer<DungeonTile> Rasterizer { get; private set; }
		protected DungeonGraph Graph { get; private set; }
		protected Random Rand { get; private set; }

		internal void Init(BitmapRasterizer<DungeonTile> rasterizer, DungeonGraph graph, Random rand) {
			Rasterizer = rasterizer;
			Graph = graph;
			Rand = rand;
		}

		public virtual void Rasterize() {
		}
	}
}