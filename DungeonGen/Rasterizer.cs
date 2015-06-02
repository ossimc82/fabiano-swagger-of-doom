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
using System.Linq;
using DungeonGenerator.Dungeon;
using DungeonGenerator.Templates;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator {
	public enum RasterizationStep {
		Initialize = 5,

		Background = 6,
		Corridor = 7,
		Room = 8,
		Overlay = 9,

		Finish = 10
	}

	public class Rasterizer {
		readonly Random rand;
		readonly DungeonGraph graph;
		readonly BitmapRasterizer<DungeonTile> rasterizer;

		static readonly TileType Space = new TileType(0x00fe, "Space");

		public RasterizationStep Step { get; set; }

		public Rasterizer(int seed, DungeonGraph graph) {
			rand = new Random(seed);
			this.graph = graph;
			rasterizer = new BitmapRasterizer<DungeonTile>(graph.Width, graph.Height);
			Step = RasterizationStep.Initialize;
		}

		public void Rasterize(RasterizationStep? targetStep = null) {
			while (Step != targetStep && Step != RasterizationStep.Finish) {
				RunStep();
			}
		}

		void RunStep() {
			switch (Step) {
				case RasterizationStep.Initialize:
					rasterizer.Clear(new DungeonTile {
						TileType = Space
					});
					graph.Template.InitializeRasterization(graph);
					break;

				case RasterizationStep.Background:
					var bg = graph.Template.CreateBackground();
					bg.Init(rasterizer, graph, rand);
					bg.Rasterize();
					break;

				case RasterizationStep.Corridor:
					RasterizeCorridors();
					break;

				case RasterizationStep.Room:
					RasterizeRooms();
					break;

				case RasterizationStep.Overlay:
					var overlay = graph.Template.CreateOverlay();
					overlay.Init(rasterizer, graph, rand);
					overlay.Rasterize();
					break;
			}
			Step++;
		}

		void RasterizeCorridors() {
			var corridor = graph.Template.CreateCorridor();
			corridor.Init(rasterizer, graph, rand);

			foreach (var room in graph.Rooms)
				foreach (var edge in room.Edges) {
					if (edge.RoomA != room)
						continue;
					RasterizeCorridor(corridor, edge);
				}
		}

		void CreateCorridor(Room src, Room dst, out Point srcPos, out Point dstPos) {
			var edge = src.Edges.Single(ed => ed.RoomB == dst);
			var link = edge.Linkage;

			if (link.Direction == Direction.South || link.Direction == Direction.North) {
				srcPos = new Point(link.Offset, src.Pos.Y + src.Height / 2);
				dstPos = new Point(link.Offset, dst.Pos.Y + dst.Height / 2);
			}
			else if (link.Direction == Direction.East || link.Direction == Direction.West) {
				srcPos = new Point(src.Pos.X + src.Width / 2, link.Offset);
				dstPos = new Point(dst.Pos.X + dst.Width / 2, link.Offset);
			}
			else
				throw new ArgumentException();
		}

		void RasterizeCorridor(MapCorridor corridor, Edge edge) {
			Point srcPos, dstPos;
			CreateCorridor(edge.RoomA, edge.RoomB, out srcPos, out dstPos);
			corridor.Rasterize(edge.RoomA, edge.RoomB, srcPos, dstPos);
		}

		void RasterizeRooms() {
			foreach (var room in graph.Rooms)
				room.Rasterize(rasterizer, rand);
		}

		public DungeonTile[,] ExportMap() {
			return rasterizer.Bitmap;
		}
	}
}