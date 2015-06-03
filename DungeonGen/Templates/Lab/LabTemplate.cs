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
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.Lab {
	public class LabTemplate : DungeonTemplate {
		internal static readonly TileType LabFloor = new TileType(0x00d3, "Lab Floor");
		internal static readonly TileType Space = new TileType(0x00fe, "Space");

		internal static readonly ObjectType LabWall = new ObjectType(0x188c, "Lab Wall");
		internal static readonly ObjectType DestructibleWall = new ObjectType(0x18c3, "Lab Destructible Wall");
		internal static readonly ObjectType Web = new ObjectType(0x0732, "Spider Web");

		internal static readonly ObjectType[] Big = {
			new ObjectType(0x0981, "Escaped Experiment"),
			new ObjectType(0x0982, "Enforcer Bot 3000"),
			new ObjectType(0x0983, "Crusher Abomination")
		};

		internal static readonly ObjectType[] Small = {
			new ObjectType(0x0979, "Mini Bot"),
			new ObjectType(0x0980, "Rampage Cyborg")
		};

		static readonly DungeonObject web = new DungeonObject {
			ObjectType = Web
		};

		internal static readonly DungeonTile[,] MapTemplate;

		static LabTemplate() {
			MapTemplate = ReadTemplate(typeof(LabTemplate));
		}

		public override int MaxDepth { get { return 20; } }

		NormDist targetDepth;
		public override NormDist TargetDepth { get { return targetDepth; } }

		public override NormDist SpecialRmCount { get { return null; } }

		public override NormDist SpecialRmDepthDist { get { return null; } }

		public override Range RoomSeparation { get { return new Range(6, 8); } }

		public override int CorridorWidth { get { return 4; } }

		public override Range NumRoomRate { get { return new Range(2, 3); } }

		bool generatedEvilRoom;

		public override void Initialize() {
			targetDepth = new NormDist(3, 10, 7, 15, Rand.Next());
			generatedEvilRoom = false;
		}

		public override Room CreateStart(int depth) {
			return new StartRoom();
		}

		public override Room CreateTarget(int depth, Room prev) {
			return new BossRoom();
		}

		public override Room CreateSpecial(int depth, Room prev) {
			throw new InvalidOperationException();
		}

		public override Room CreateNormal(int depth, Room prev) {
			var rm = new NormalRoom(prev as NormalRoom, Rand, generatedEvilRoom);
			if ((rm.Flags & NormalRoom.RoomFlags.Evil) != 0)
				generatedEvilRoom = true;
			return rm;
		}

		public override MapCorridor CreateCorridor() {
			return new Corridor();
		}

		public override MapRender CreateOverlay() {
			return new Overlay();
		}

		internal static void DrawSpiderWeb(BitmapRasterizer<DungeonTile> rasterizer, Rect bounds, Random rand) {
			int w = rasterizer.Width, h = rasterizer.Height;
			var buf = rasterizer.Bitmap;

			for (int x = bounds.X; x < bounds.MaxX; x++)
				for (int y = bounds.Y; y < bounds.MaxY; y++) {
					if (buf[x, y].TileType == Space || buf[x, y].Object != null)
						continue;

					if (rand.NextDouble() > 0.99)
						buf[x, y].Object = web;
				}
		}

		internal static void CreateEnemies(BitmapRasterizer<DungeonTile> rasterizer, Rect bounds, Random rand) {
			int numBig = new Range(0, 3).Random(rand);
			int numSmall = new Range(4, 10).Random(rand);

			var buf = rasterizer.Bitmap;
			while (numBig > 0 || numSmall > 0) {
				int x = rand.Next(bounds.X, bounds.MaxX);
				int y = rand.Next(bounds.Y, bounds.MaxY);
				if (buf[x, y].TileType == Space || buf[x, y].Object != null)
					continue;

				switch (rand.Next(2)) {
					case 0:
						if (numBig > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = Big[rand.Next(Big.Length)]
							};
							numBig--;
						}
						break;
					case 1:
						if (numSmall > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = Small[rand.Next(Small.Length)]
							};
							numSmall--;
						}
						break;
				}
			}
		}
	}
}