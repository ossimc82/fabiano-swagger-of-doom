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
using RotMG.Common.Rasterizer;
using wServer.generator.dungeon;
using wServer.generator.templates;

namespace wServer.generator {
	public enum RasterizationStep {
		Initialize = 5,

		Background = 6,
		Corridor = 7,
		Room = 8,
		Overlay = 9,

		Finish = 10
	}

	internal class Rasterizer {
		readonly Random rand;
		readonly DungeonGraph graph;
		readonly BitmapRasterizer<DungeonTile> rasterizer;

		static readonly TileType Space = new TileType(0x00fe, "Space");

		public RasterizationStep Step { get; set; }

		public Rasterizer(uint seed, DungeonGraph graph) {
			rand = new Random((int)seed);
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
					graph.Template.CreateBackground().Rasterize(rasterizer, rand);
					break;

				case RasterizationStep.Corridor:
					RasterizeCorridors();
					break;

				case RasterizationStep.Room:
					RasterizeRooms();
					break;

				case RasterizationStep.Overlay:
					graph.Template.CreateOverlay().Rasterize(rasterizer, rand);
					break;
			}
			Step++;
		}

		void RasterizeCorridors() {
			var corridor = graph.Template.CreateCorridor();

			foreach (var room in graph.Rooms)
				foreach (var edge in room.Edges) {
					if (edge.RoomA != room)
						continue;
					RasterizeCorridor(corridor, edge);
				}
		}

		void CreateCorridor(Room src, Room dst, out Point srcPos, out Point dstPos) {
			var srcX = new Range(src.Bounds.X, src.Bounds.MaxX);
			var srcY = new Range(src.Bounds.Y, src.Bounds.MaxY);
			var dstX = new Range(dst.Bounds.X, dst.Bounds.MaxX);
			var dstY = new Range(dst.Bounds.Y, dst.Bounds.MaxY);

			Range isect;
			if (!(isect = srcX.Intersection(dstX)).IsEmpty && srcY.Intersection(dstY).IsEmpty) {
				// South / North
				int x = rand.Next(isect.Begin, isect.End - graph.Template.CorridorWidth + 1);
				srcPos = new Point(x, src.Pos.Y + src.Height / 2);
				dstPos = new Point(x, dst.Pos.Y + dst.Height / 2);
			}
			else if (srcX.Intersection(dstX).IsEmpty && !(isect = srcY.Intersection(dstY)).IsEmpty) {
				// East / West
				int y = rand.Next(isect.Begin, isect.End - graph.Template.CorridorWidth + 1);
				srcPos = new Point(src.Pos.X + src.Width / 2, y);
				dstPos = new Point(dst.Pos.X + dst.Width / 2, y);
			}
			else
				throw new InvalidOperationException();
		}

		void RasterizeCorridor(MapCorridor corridor, Edge edge) {
			Point srcPos, dstPos;
			CreateCorridor(edge.RoomA, edge.RoomB, out srcPos, out dstPos);
			corridor.Rasterize(rasterizer, edge.RoomA, edge.RoomB, srcPos, dstPos, rand);
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