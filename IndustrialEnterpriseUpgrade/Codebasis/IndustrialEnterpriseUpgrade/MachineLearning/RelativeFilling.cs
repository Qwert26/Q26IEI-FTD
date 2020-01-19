using UnityEngine;
namespace IndustrialEnterpriseUpgrade.MachineLearning {
	public enum RelativeFilling {
		Block, Beam2m, Beam3m, Beam4m,
		Ramp1m, Ramp2m, Ramp3m, Ramp4m,
		Wedge1m, Wedge2m, Wedge3m, Wedge4m,
		TriangleCorner1m, TriangleCorner2m, TriangleCorner3m, TriangleCorner4m,
		FrontWedge1m, FrontWedge2m, FrontWedge3m, FrontWedge4m,
		InvertedTriangleCorner1m, InvertedTriangleCorner2m, InvertedTriangleCorner3m, InvertedTriangleCorner4m,
		SquareCorner1m, SquareCorner2m, SquareCorner3m, SquareCorner4m,
		BackWedge1m, BackWedge2m, BackWedge3m, BackWedge4m,
		Pole1m, Pole2m, Pole3m, Pole4m
	}
	public static class RelativeFillingExtensions {
		public static RelativeFilling? RelativeFillingByName(this Block block) {
			string lowerCaseName = block.Name.ToLowerInvariant();
			if (!block.item.ExtraSettings.StructuralComponent)
				return null;
			if (lowerCaseName.Contains("block") || lowerCaseName.Equals("heavy armour") || lowerCaseName.Contains("reinforced"))
				return RelativeFilling.Block;
			if (lowerCaseName.Contains("(1m)"))
			{
				if (lowerCaseName.Contains("slope"))
					return RelativeFilling.Ramp1m;
				if (lowerCaseName.Contains("inverted"))
					return RelativeFilling.InvertedTriangleCorner1m;
				if (lowerCaseName.Contains("pole"))
					return RelativeFilling.Pole1m;
				if (lowerCaseName.Contains("square corner"))
					return RelativeFilling.SquareCorner1m;
				if (lowerCaseName.Contains("triangle corner"))
					return RelativeFilling.TriangleCorner1m;
				if (lowerCaseName.Contains("wedge"))
				{
					if (lowerCaseName.Contains("front"))
						return RelativeFilling.FrontWedge1m;
					else if (lowerCaseName.Contains("back"))
						return RelativeFilling.BackWedge1m;
					else
						return RelativeFilling.Wedge1m;
				}
			}
			else if (lowerCaseName.Contains("(2m)"))
			{
				if (lowerCaseName.Contains("beam"))
					return RelativeFilling.Beam2m;
				if (lowerCaseName.Contains("slope"))
					return RelativeFilling.Ramp2m;
				if (lowerCaseName.Contains("inverted"))
					return RelativeFilling.InvertedTriangleCorner2m;
				if (lowerCaseName.Contains("pole"))
					return RelativeFilling.Pole2m;
				if (lowerCaseName.Contains("square corner"))
					return RelativeFilling.SquareCorner2m;
				if (lowerCaseName.Contains("triangle corner"))
					return RelativeFilling.TriangleCorner2m;
				if (lowerCaseName.Contains("wedge"))
				{
					if (lowerCaseName.Contains("front"))
						return RelativeFilling.FrontWedge2m;
					else if (lowerCaseName.Contains("back"))
						return RelativeFilling.BackWedge2m;
					else
						return RelativeFilling.Wedge2m;
				}
			}
			else if (lowerCaseName.Contains("(3m)"))
			{
				if (lowerCaseName.Contains("beam"))
					return RelativeFilling.Beam3m;
				if (lowerCaseName.Contains("slope"))
					return RelativeFilling.Ramp3m;
				if (lowerCaseName.Contains("inverted"))
					return RelativeFilling.InvertedTriangleCorner3m;
				if (lowerCaseName.Contains("pole"))
					return RelativeFilling.Pole3m;
				if (lowerCaseName.Contains("square corner"))
					return RelativeFilling.SquareCorner3m;
				if (lowerCaseName.Contains("triangle corner"))
					return RelativeFilling.TriangleCorner3m;
				if (lowerCaseName.Contains("wedge"))
				{
					if (lowerCaseName.Contains("front"))
						return RelativeFilling.FrontWedge3m;
					else if (lowerCaseName.Contains("back"))
						return RelativeFilling.BackWedge3m;
					else
						return RelativeFilling.Wedge3m;
				}
			}
			else if (lowerCaseName.Contains("(4m)"))
			{
				if (lowerCaseName.Contains("beam"))
					return RelativeFilling.Beam4m;
				if (lowerCaseName.Contains("slope"))
				{
					if (lowerCaseName.Contains("diagonal"))
						return RelativeFilling.Ramp1m;
					else
						return RelativeFilling.Ramp4m;
				}
				if (lowerCaseName.Contains("inverted"))
					return RelativeFilling.InvertedTriangleCorner4m;
				if (lowerCaseName.Contains("pole"))
					return RelativeFilling.Pole4m;
				if (lowerCaseName.Contains("square corner"))
					return RelativeFilling.SquareCorner4m;
				if (lowerCaseName.Contains("triangle corner"))
					return RelativeFilling.TriangleCorner4m;
				if (lowerCaseName.Contains("wedge"))
				{
					if (lowerCaseName.Contains("front"))
						return RelativeFilling.FrontWedge4m;
					else if (lowerCaseName.Contains("back"))
						return RelativeFilling.BackWedge4m;
					else
						return RelativeFilling.Wedge4m;
				}
			}
			return null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeFilling"></param>
		/// <param name="localX"></param>
		/// <param name="localY"></param>
		/// <param name="localZ"></param>
		/// <returns></returns>
		public static float RelativeFillingAtLocalCoordinates(this RelativeFilling? relativeFilling, int localX, int localY, int localZ)
		{
			if (relativeFilling.HasValue)
			{
				switch (relativeFilling.Value)
				{
					case RelativeFilling.Block:
					case RelativeFilling.Beam2m:
					case RelativeFilling.Beam3m:
					case RelativeFilling.Beam4m:
						return 1;
					case RelativeFilling.Ramp1m:
					case RelativeFilling.Wedge1m:
						return 0.5f;
					case RelativeFilling.Ramp2m:
					case RelativeFilling.Wedge2m:
						switch (localZ) {
							case 0:
								return 0.25f;
							case 1:
								return 0.75f;
							default:
								return 0;
						}
					case RelativeFilling.Ramp3m:
					case RelativeFilling.Wedge3m:
						switch (localZ)
						{
							case 0:
								return 5f / 6f;
							case 1:
								return 0.5f;
							case 2:
								return 1f / 6f;
							default:
								return 0;
						}
					case RelativeFilling.Ramp4m:
					case RelativeFilling.Wedge4m:
						switch (localZ)
						{
							case 0:
								return 0.875f;
							case 1:
								return 0.625f;
							case 2:
								return 0.375f;
							case 3:
								return 0.125f;
							default:
								return 0;
						}
					case RelativeFilling.TriangleCorner1m:
					case RelativeFilling.FrontWedge1m:
						return 1f / 6f;
					case RelativeFilling.TriangleCorner2m:
					case RelativeFilling.FrontWedge2m:
						switch (localZ)
						{
							case 0:
								return 7f / 24f;
							case 1:
								return 1f / 24f;
							default:
								return 0;
						}
					case RelativeFilling.TriangleCorner3m:
					case RelativeFilling.FrontWedge3m:
						switch (localZ)
						{
							case 0:
								return 19f / 54f;
							case 1:
								return 7f / 54f;
							case 2:
								return 1f / 54f;
							default:
								return 0;
						}
					case RelativeFilling.TriangleCorner4m:
					case RelativeFilling.FrontWedge4m:
						switch (localZ)
						{
							case 0:
								return 37f / 96f;
							case 1:
								return 19f / 96f;
							case 2:
								return 7f / 96f;
							case 3:
								return 1f / 96f;
							default:
								return 0;
						}
					case RelativeFilling.InvertedTriangleCorner1m:
					case RelativeFilling.BackWedge1m:
						return 5f / 6f;
					case RelativeFilling.InvertedTriangleCorner2m:
					case RelativeFilling.BackWedge2m:
						switch (localZ)
						{
							case 0:
								return 23f / 24f;
							case 1:
								return 17f / 24f;
							default:
								return 0;
						}
					case RelativeFilling.InvertedTriangleCorner3m:
					case RelativeFilling.BackWedge3m:
						switch (localZ)
						{
							case 0:
								return 53f / 54f;
							case 1:
								return 47f / 54f;
							case 2:
								return 35f / 54f;
							default:
								return 0;
						}
					case RelativeFilling.InvertedTriangleCorner4m:
					case RelativeFilling.BackWedge4m:
						switch (localZ)
						{
							case 0:
								return 95f / 96f;
							case 1:
								return 89f / 96f;
							case 2:
								return 77f / 96f;
							case 3:
								return 59f / 96f;
							default:
								return 0;
						}
					case RelativeFilling.SquareCorner1m:
						return 1f / 3f;
					case RelativeFilling.SquareCorner2m:
						switch (localZ)
						{
							case 0:
								return 7f / 12f;
							case 1:
								return 1f / 12f;
							default:
								return 0;
						}
					case RelativeFilling.SquareCorner3m:
						switch (localZ)
						{
							case 0:
								return 19f / 27f;
							case 1:
								return 7f / 27f;
							case 2:
								return 1f / 27f;
							default:
								return 0;
						}
					case RelativeFilling.SquareCorner4m:
						switch (localZ)
						{
							case 0:
								return 37f / 48f;
							case 1:
								return 19f / 48f;
							case 2:
								return 7f / 48f;
							case 3:
								return 1f / 48f;
							default:
								return 0;
						}
					case RelativeFilling.Pole1m:
					case RelativeFilling.Pole2m:
					case RelativeFilling.Pole3m:
					case RelativeFilling.Pole4m:
						return Mathf.PI / 4;
					default:
						return 0;
				}
			}
			else
			{
				return 0;
			}
		}
	}
}