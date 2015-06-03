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
using DungeonGenerator.Dungeon;
using DungeonGenerator.Templates;
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator {
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
		int minRoomNum;
		int maxRoomNum;

		public GenerationStep Step { get; set; }

		public Generator(int seed, DungeonTemplate template) {
			rand = new Random(seed);
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

				case GenerationStep.SpecialGeneration:
					GenerateSpecials();
					break;

				case GenerationStep.BranchGeneration:
					GenerateBranches();
					break;
			}
			Step++;
		}

		Link? PlaceRoom(Room src, Room target, int connPt) {
			var sep = template.RoomSeparation.Random(rand);
			if (src is FixedRoom && target is FixedRoom)
				return PlaceRoomFixed((FixedRoom)src, (FixedRoom)target, connPt, sep);
			if (src is FixedRoom)
				return PlaceRoomSourceFixed((FixedRoom)src, target, connPt, sep);
			if (target is FixedRoom)
				return PlaceRoomTargetFixed(src, (FixedRoom)target, connPt, sep);

			return PlaceRoomFree(src, target, (Direction)connPt, sep);
		}

		Link? PlaceRoomFree(Room src, Room target, Direction connPt, int sep) {
			int x, y;
			Link? link = null;

			switch (connPt) {
				case Direction.North:
				case Direction.South:
					// North & South
					int minX = src.Pos.X + template.CorridorWidth - target.Width;
					int maxX = src.Pos.X + src.Width - template.CorridorWidth;
					x = rand.Next(minX, maxX + 1);

					if (connPt == Direction.South)
						y = src.Pos.Y + src.Height + sep;
					else
						y = src.Pos.Y - sep - target.Height;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					var linkX = new Range(src.Pos.X, src.Pos.X + src.Width).Intersection(
						new Range(target.Pos.X, target.Pos.X + target.Width));
					link = new Link(connPt, new Range(linkX.Begin, linkX.End - template.CorridorWidth).Random(rand));
					break;

				case Direction.East:
				case Direction.West:
					// East & West
					int minY = src.Pos.Y + template.CorridorWidth - target.Height;
					int maxY = src.Pos.Y + src.Height - template.CorridorWidth;
					y = rand.Next(minY, maxY + 1);

					if (connPt == Direction.East)
						x = src.Pos.X + src.Width + sep;
					else
						x = src.Pos.X - sep - target.Width;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					var linkY = new Range(src.Pos.Y, src.Pos.Y + src.Height).Intersection(
						new Range(target.Pos.Y, target.Pos.Y + target.Height));
					link = new Link(connPt, new Range(linkY.Begin, linkY.End - template.CorridorWidth).Random(rand));
					break;
			}

			collision.Add(target);
			return link;
		}

		Link? PlaceRoomSourceFixed(FixedRoom src, Room target, int connPt, int sep) {
			var conn = src.ConnectionPoints[connPt];
			int x, y;
			Link? link = null;

			switch (conn.Item1) {
				case Direction.North:
				case Direction.South:
					// North & South
					int minX = src.Pos.X + conn.Item2 + template.CorridorWidth - target.Width;
					int maxX = src.Pos.X + conn.Item2;
					x = rand.Next(minX, maxX + 1);

					if (conn.Item1 == Direction.South)
						y = src.Pos.Y + src.Height + sep;
					else
						y = src.Pos.Y - sep - target.Height;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					link = new Link(conn.Item1, src.Pos.X + conn.Item2);
					break;

				case Direction.East:
				case Direction.West:
					// East & West
					int minY = src.Pos.Y + conn.Item2 + template.CorridorWidth - target.Height;
					int maxY = src.Pos.Y + conn.Item2;
					y = rand.Next(minY, maxY + 1);

					if (conn.Item1 == Direction.East)
						x = src.Pos.X + src.Width + sep;
					else
						x = src.Pos.X - sep - target.Width;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					var linkY = new Range(src.Pos.Y, src.Pos.Y + src.Height).Intersection(
						new Range(target.Pos.Y, target.Pos.Y + target.Height));
					link = new Link(conn.Item1, src.Pos.Y + conn.Item2);
					break;
			}

			collision.Add(target);
			return link;
		}

		Link? PlaceRoomTargetFixed(Room src, FixedRoom target, int connPt, int sep) {
			var targetDir = ((Direction)connPt).Reverse();

			var connPts = (Tuple<Direction, int>[])target.ConnectionPoints.Clone();
			rand.Shuffle(connPts);
			Tuple<Direction, int> conn = null;
			foreach (var pt in connPts) {
				if (pt.Item1 == targetDir) {
					conn = pt;
					break;
				}
			}

			if (conn == null)
				return null;

			int x, y;
			Link? link = null;
			switch (conn.Item1) {
				case Direction.North:
				case Direction.South:
					// North & South
					int minX = src.Pos.X - conn.Item2;
					int maxX = src.Pos.X + src.Width - template.CorridorWidth - conn.Item2;
					x = rand.Next(minX, maxX + 1);

					if (conn.Item1 == Direction.North)
						y = src.Pos.Y + src.Height + sep;
					else
						y = src.Pos.Y - sep - target.Height;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					link = new Link((Direction)connPt, target.Pos.X + conn.Item2);
					break;

				case Direction.East:
				case Direction.West:
					// East & West
					int minY = src.Pos.Y - conn.Item2;
					int maxY = src.Pos.Y + src.Height - template.CorridorWidth - conn.Item2;
					y = rand.Next(minY, maxY + 1);

					if (conn.Item1 == Direction.West)
						x = src.Pos.X + src.Width + sep;
					else
						x = src.Pos.X - sep - target.Width;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					link = new Link((Direction)connPt, target.Pos.Y + conn.Item2);
					break;
			}

			collision.Add(target);
			return link;
		}

		Link? PlaceRoomFixed(FixedRoom src, FixedRoom target, int connPt, int sep) {
			var conn = src.ConnectionPoints[connPt];

			var targetDirection = conn.Item1.Reverse();
			var targetConns = (Tuple<Direction, int>[])target.ConnectionPoints.Clone();
			rand.Shuffle(targetConns);
			Tuple<Direction, int> targetConnPt = null;
			foreach (var targetConn in targetConns)
				if (targetConn.Item1 == targetDirection) {
					targetConnPt = targetConn;
					break;
				}

			if (targetConnPt == null)
				return null;

			int x, y;
			Link? link = null;
			switch (conn.Item1) {
				case Direction.North:
				case Direction.South:
					// North & South
					x = src.Pos.X + conn.Item2 - targetConnPt.Item2;

					if (conn.Item1 == Direction.South)
						y = src.Pos.Y + src.Height + sep;
					else
						y = src.Pos.Y - sep - target.Height;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					link = new Link(conn.Item1, src.Pos.X + conn.Item2);
					break;

				case Direction.East:
				case Direction.West:
					// East & West
					y = src.Pos.Y + conn.Item2 - targetConnPt.Item2;

					if (conn.Item1 == Direction.East)
						x = src.Pos.X + src.Width + sep;
					else
						x = src.Pos.X - sep - target.Width;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return null;

					link = new Link(conn.Item1, src.Pos.Y + conn.Item2);
					break;
			}

			collision.Add(target);
			return link;
		}

		int GetMaxConnectionPoints(Room rm) {
			if (rm is FixedRoom)
				return ((FixedRoom)rm).ConnectionPoints.Length;
			return 4;
		}

		bool GenerateTarget() {
			var targetDepth = (int)template.TargetDepth.NextValue();

			rootRoom = template.CreateStart(0);
			rootRoom.Pos = new Point(0, 0);
			collision.Add(rootRoom);
			rooms.Add(rootRoom);

			if (GenerateTargetInternal(rootRoom, 1, targetDepth)) {
				minRoomNum = targetDepth * template.NumRoomRate.Begin;
				maxRoomNum = targetDepth * template.NumRoomRate.End;
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

				Link? link = null;
				foreach (var connPt in seq)
					if ((link = PlaceRoom(prev, rm, connPt)) != null) {
						seq.Remove(connPt);
						break;
					}

				if (link == null)
					return false;

				if (targetDepth == depth)
					targetPlaced = true;
				else
					targetPlaced = GenerateTargetInternal(rm, depth + 1, targetDepth);

				if (targetPlaced) {
					rm.Depth = depth;
					Edge.Link(prev, rm, link.Value);
					rooms.Add(rm);
				}
				else
					collision.Remove(rm);
			} while (!targetPlaced);
			return true;
		}

		void GenerateSpecials() {
			if (template.SpecialRmCount == null)
				return;

			int numRooms = (int)template.SpecialRmCount.NextValue();
			for (int i = 0; i < numRooms; i++) {
				int targetDepth;
				do {
					targetDepth = (int)template.SpecialRmDepthDist.NextValue();
				} while (targetDepth > maxDepth * 3 / 2);

				bool generated = false;
				do {
					var room = rooms[rand.Next(rooms.Count)];
					if (room.Depth >= targetDepth)
						continue;
					generated = GenerateSpecialInternal(room, room.Depth + 1, targetDepth);
				} while (!generated);
			}
		}

		bool GenerateSpecialInternal(Room prev, int depth, int targetDepth) {
			var connPtNum = GetMaxConnectionPoints(prev);
			var seq = Enumerable.Range(0, connPtNum).ToList();
			rand.Shuffle(seq);

			bool specialPlaced;
			do {
				Room rm;
				if (targetDepth == depth)
					rm = template.CreateSpecial(depth, prev);
				else
					rm = template.CreateNormal(depth, prev);

				Link? link = null;
				foreach (var connPt in seq)
					if ((link = PlaceRoom(prev, rm, connPt)) != null) {
						seq.Remove(connPt);
						break;
					}

				if (link == null)
					return false;

				if (targetDepth == depth)
					specialPlaced = true;
				else
					specialPlaced = GenerateSpecialInternal(rm, depth + 1, targetDepth);

				if (specialPlaced) {
					rm.Depth = depth;
					Edge.Link(prev, rm, link.Value);
					rooms.Add(rm);
				}
				else
					collision.Remove(rm);
			} while (!specialPlaced);
			return true;
		}

		void GenerateBranches() {
			int numRooms = new Range(minRoomNum, maxRoomNum).Random(rand);

			List<Room> copy;
			while (rooms.Count < numRooms) {
				copy = new List<Room>(rooms);
				rand.Shuffle(copy);

				bool worked = false;
				foreach (var room in copy) {
					if (rooms.Count > numRooms)
						break;
					if (rand.Next() % 2 == 0)
						continue;
					worked |= GenerateBranchInternal(room, room.Depth + 1, template.MaxDepth, true);
				}
				if (!worked)
					break;
			}
		}

		bool GenerateBranchInternal(Room prev, int depth, int maxDepth, bool doBranch) {
			if (depth >= maxDepth)
				return false;

			var connPtNum = GetMaxConnectionPoints(prev);
			var seq = Enumerable.Range(0, connPtNum).ToList();
			rand.Shuffle(seq);

			if (doBranch) {
				var numBranch = prev.NumBranches.Random(rand);
				numBranch -= prev.Edges.Count;
				for (int i = 0; i < numBranch; i++) {
					var rm = template.CreateNormal(depth, prev);

					Link? link = null;
					foreach (var connPt in seq)
						if ((link = PlaceRoom(prev, rm, connPt)) != null) {
							seq.Remove(connPt);
							break;
						}

					if (link == null)
						return false;

					Edge.Link(prev, rm, link.Value);
					if (!GenerateBranchInternal(rm, depth + 1, maxDepth, false)) {
						collision.Remove(rm);
						Edge.UnLink(prev, rm);
						return false;
					}
					rm.Depth = depth;
					rooms.Add(rm);
				}
			}
			else {
				while (prev.Edges.Count < prev.NumBranches.Begin) {
					var rm = template.CreateNormal(depth, prev);

					Link? link = null;
					foreach (var connPt in seq)
						if ((link = PlaceRoom(prev, rm, connPt)) != null) {
							seq.Remove(connPt);
							break;
						}

					if (link == null)
						return false;

					Edge.Link(prev, rm, link.Value);
					if (!GenerateBranchInternal(rm, depth + 1, maxDepth, false)) {
						collision.Remove(rm);
						Edge.UnLink(prev, rm);
						return false;
					}
					rm.Depth = depth;
					rooms.Add(rm);
				}
			}
			return true;
		}

		public DungeonGraph ExportGraph() {
			if (Step != GenerationStep.Finish)
				throw new InvalidOperationException();
			return new DungeonGraph(template, rooms.ToArray());
		}
	}
}