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
using System.Linq;
using RotMG.Common;
using RotMG.Common.Rasterizer;
using wServer.generator.dungeon;
using wServer.generator.templates;

namespace wServer.generator {
	public enum GenerationStep {
		Initialize = 0,

		TargetGeneration = 1,
		SpecialGeneration = 2,
		BranchGeneration = 3,

		Finish = 4
	}

	public class Generator {
		readonly Random rand;
		readonly DungeonTemplate template;

		RoomCollision collision;
		Room rootRoom;
		List<Room> rooms;
		int maxDepth;
		int maxRoomNum;

		public GenerationStep Step { get; set; }

		public Generator(uint seed, DungeonTemplate template) {
			rand = new Random((int)seed);
			this.template = template;
			Step = GenerationStep.Initialize;
		}

		public void Generate(GenerationStep? targetStep = null) {
			while (Step != targetStep && Step != GenerationStep.Finish) {
				RunStep();
			}
		}

		public IEnumerable<Room> GetRooms() {
			return rooms;
		}

		void RunStep() {
			switch (Step) {
				case GenerationStep.Initialize:
					template.SetRandom(rand);
					template.Initialize();
					collision = new RoomCollision();
					rootRoom = null;
					rooms = new List<Room>();
					break;

				case GenerationStep.TargetGeneration:
					if (!GenerateTarget()) {
						Step = GenerationStep.Initialize;
						return;
					}
					break;

				case GenerationStep.BranchGeneration:
					GenerateBranches();
					break;
			}
			Step++;
		}

		bool PlaceRoom(Room src, Room target, int connPt) {
			var sep = template.RoomSeparation.Random(rand);
			int x, y;
			switch (connPt) {
				case 0:
				case 2:
					// North & South
					int minX = src.Pos.X + template.CorridorWidth - target.Width;
					int maxX = src.Pos.X + src.Width - template.CorridorWidth;
					x = rand.Next(minX, maxX + 1);

					if (connPt == 0)
						y = src.Pos.Y + src.Height + sep;
					else
						y = src.Pos.Y - sep - target.Height;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return false;
					break;

				case 1:
				case 3:
					// East & West
					int minY = src.Pos.Y + template.CorridorWidth - target.Height;
					int maxY = src.Pos.Y + src.Height - template.CorridorWidth;
					y = rand.Next(minY, maxY + 1);

					if (connPt == 1)
						x = src.Pos.X + src.Width + sep;
					else
						x = src.Pos.X - sep - target.Width;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return false;
					break;
			}

			collision.Add(target);
			return true;
		}

		int GetMaxConnectionPoints(Room rm) {
			return 4;
		}

		bool GenerateTarget() {
			var targetDepth = (int)template.TargetDepth.NextValue();

			rootRoom = template.CreateStart(0);
			rootRoom.Pos = new Point(0, 0);
			collision.Add(rootRoom);
			rooms.Add(rootRoom);

			if (GenerateTargetInternal(rootRoom, 1, targetDepth)) {
				maxRoomNum = rooms.Count * 3;
				maxDepth = rooms.Count;
				return true;
			}
			return false;
		}

		bool GenerateTargetInternal(Room prev, int depth, int targetDepth) {
			var connPtNum = GetMaxConnectionPoints(prev);
			var seq = Enumerable.Range(0, connPtNum).ToList();
			rand.Shuffle(seq);

			bool targetPlaced;
			do {
				Room rm;
				if (targetDepth == depth)
					rm = template.CreateTarget(depth, prev);
				else
					rm = template.CreateNormal(depth, prev);

				bool connected = false;
				foreach (var connPt in seq)
					if (PlaceRoom(prev, rm, connPt)) {
						seq.Remove(connPt);
						connected = true;
						break;
					}

				if (!connected)
					return false;

				rm.Depth = depth;
				Edge.Link(prev, rm);
				rooms.Add(rm);

				if (targetDepth == depth)
					targetPlaced = true;
				else
					targetPlaced = GenerateTargetInternal(rm, depth + 1, targetDepth);
			} while (!targetPlaced);

			return true;
		}

		void GenerateBranches() {
			var originalRooms = rooms.ToList();
			rand.Shuffle(originalRooms);

			foreach (var room in originalRooms) {
				GenerateBranchInternal(room, room.Depth + 1, room.Type == RoomType.Target ? template.MaxDepth : maxDepth);
				if (rooms.Count >= maxRoomNum)
					break;
			}
		}

		void GenerateBranchInternal(Room prev, int depth, int maxDepth) {
			if (depth >= maxDepth)
				return;

			if (rooms.Count >= maxRoomNum)
				return;

			var connPtNum = GetMaxConnectionPoints(prev);
			var seq = Enumerable.Range(0, connPtNum).ToList();
			rand.Shuffle(seq);

			int numBranch = rand.Next(8);
			switch (numBranch) {
				case 0:
				case 1:
					numBranch = 1;
					break;
				case 2:
				case 3:
				case 4:
					numBranch = 2;
					break;
				case 5:
				case 6:
					numBranch = 3;
					break;
				case 7:
					numBranch = 4;
					break;
			}
			numBranch -= prev.Edges.Count;
			for (int i = 0; i < numBranch; i++) {
				var rm = template.CreateNormal(depth, prev);

				bool connected = false;
				foreach (var connPt in seq)
					if (PlaceRoom(prev, rm, connPt)) {
						seq.Remove(connPt);
						connected = true;
						break;
					}

				if (!connected)
					return;

				rm.Depth = depth;
				Edge.Link(prev, rm);
				rooms.Add(rm);

				GenerateBranchInternal(rm, depth + 1, maxDepth);
			}
		}

		public DungeonGraph ExportGraph() {
			if (Step != GenerationStep.Finish)
				throw new InvalidOperationException();
			return new DungeonGraph(template, rooms.ToArray());
		}
	}
}