using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator {
	public static class Extensions {
		public static void Copy<TPixel>(this BitmapRasterizer<TPixel> self, TPixel[,] src, Rect srcRect, Point dst,
			Func<TPixel, bool> transprent = null)
			where TPixel : struct {
			int w = srcRect.MaxX - srcRect.X;
			int h = srcRect.MaxY - srcRect.Y;
			var buf = self.Bitmap;

			if (transprent == null)
				transprent = pix => false;

			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					var pix = src[x + srcRect.X, y + srcRect.Y];
					if (transprent(pix))
						continue;
					buf[x + dst.X, y + dst.Y] = src[x + srcRect.X, y + srcRect.Y];
				}
		}

		public static Direction Reverse(this Direction direction) {
			switch (direction) {
				case Direction.North:
					return Direction.South;
				case Direction.South:
					return Direction.North;
				case Direction.East:
					return Direction.West;
				case Direction.West:
					return Direction.East;
			}
			throw new ArgumentException();
		}
	}
}