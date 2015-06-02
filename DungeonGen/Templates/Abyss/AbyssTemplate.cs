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

namespace DungeonGenerator.Templates.Abyss {
	public class AbyssTemplate : DungeonTemplate {
		internal static readonly TileType RedSmallChecks = new TileType(0x003c, "Red Small Checks");
		internal static readonly TileType Lava = new TileType(0x0070, "Lava");
		internal static readonly TileType Space = new TileType(0x00fe, "Space");

		internal static readonly ObjectType RedWall = new ObjectType(0x0150, "Red Wall");
		internal static readonly ObjectType RedTorchWall = new ObjectType(0x0151, "Red Torch Wall");
		internal static readonly ObjectType PartialRedFloor = new ObjectType(0x0153, "Partial Red Floor");
		internal static readonly ObjectType RedPillar = new ObjectType(0x017e, "Red Pillar");
		internal static readonly ObjectType BrokenRedPillar = new ObjectType(0x0183, "Broken Red Pillar");
		internal static readonly ObjectType CowardicePortal = new ObjectType(0x0703, "Portal of Cowardice");

		internal static readonly ObjectType AbyssImp = new ObjectType(0x66d, "Imp of the Abyss");

		internal static readonly ObjectType[] AbyssDemon = {
			new ObjectType(0x66e, "Demon of the Abyss"),
			new ObjectType(0x66f, "Demon Warrior of the Abyss"),
			new ObjectType(0x670, "Demon Mage of the Abyss")
		};

		internal static readonly ObjectType[] AbyssBrute = {
			new ObjectType(0x671, "Brute of the Abyss"),
			new ObjectType(0x672, "Brute Warrior of the Abyss")
		};

		internal static readonly ObjectType AbyssBones = new ObjectType(0x01fa, "Abyss Bones");

		internal static readonly DungeonTile[,] MapTemplate;

		static AbyssTemplate() {
			MapTemplate = ReadTemplate(typeof(AbyssTemplate));
		}

		public override int MaxDepth { get { return 50; } }

		NormDist targetDepth;
		public override NormDist TargetDepth { get { return targetDepth; } }

		NormDist specialRmCount;
		public override NormDist SpecialRmCount { get { return specialRmCount; } }

		NormDist specialRmDepthDist;
		public override NormDist SpecialRmDepthDist { get { return specialRmDepthDist; } }

		public override Range RoomSeparation { get { return new Range(0, 1); } }

		public override int CorridorWidth { get { return 3; } }

		public override void Initialize() {
			targetDepth = new NormDist(3, 20, 15, 35, Rand.Next());
			specialRmCount = new NormDist(1.5f, 0.5f, 0, 5, Rand.Next());
			specialRmDepthDist = new NormDist(5, 20, 10, 35, Rand.Next());
		}

		public override Room CreateStart(int depth) {
			return new StartRoom(16);
		}

		public override Room CreateTarget(int depth, Room prev) {
			return new BossRoom();
		}

		public override Room CreateSpecial(int depth, Room prev) {
			return new TreasureRoom();
		}

		public override Room CreateNormal(int depth, Room prev) {
			return new NormalRoom(8, 8);
		}

		public override MapCorridor CreateCorridor() {
			return new Corridor();
		}

		public override MapRender CreateOverlay() {
			return new Overlay();
		}
	}
}