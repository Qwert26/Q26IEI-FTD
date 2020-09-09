using BrilliantSkies.Ftd.Avatar.Items;
using UnityEngine;
using UnityEngine.UI;
using BrilliantSkies.Core.Constants;
using BrilliantSkies.Core.Types;
using System.IO;
using System;
namespace IndustrialEnterpriseUpgrade.MachineLearning
{
	public class RelativeFillingExtractor : CharacterItem
	{
		public override string PrimaryFunctionDescription => "Create relative filling map";
		public override string SecondaryFunctionDescription => "Save relative filling map as Pictures";
		public override bool AreYouTwoHanded()
		{
			return true;
		}
		public override void LeftClick()
		{
			Block pointedBlock = GetPointedBlock();
			if (pointedBlock == null)
				return;
			IMainConstructBlock imcb = pointedBlock.MainConstruct;
			IAllBasicsRestricted cb = imcb.AllBasicsRestricted;
			int sx = cb.sx;
			int sy = cb.sy;
			int sz = cb.sz;
			float[,,] fillings = GetFillings(pointedBlock);
			string path = Get.PermanentPaths.GetSpecificModDir("Industrial Enterprise Upgrade") + $"RelativeFillings/{imcb.GetForce().Name}.txt";
			using (StreamWriter sw = new StreamWriter(path, false))
			{
				sw.WriteLine("XYZ");//Reihenfolge
				sw.WriteLine($"{sx} {sy} {sz} {cb.minx_} {cb.miny_} {cb.minz_} {cb.maxx_} {cb.maxy_} {cb.maxz_}");//Größe, Minimum, Maximum
				for (int x = 0; x < sx; x++)
				{
					for (int y = 0; y < sy; y++)
					{
						for (int z = 0; z < sz; z++)
						{
							sw.Write($"{fillings[x, y, z]} ");//Daten
						}
					}
				}
			}
		}
		public override void RightClick()
		{
			Block pointedBlock = GetPointedBlock();
			if (pointedBlock == null)
				return;
			IMainConstructBlock imcb = pointedBlock.MainConstruct;
			IAllBasicsRestricted cb = imcb.AllBasicsRestricted;
			int sx = cb.sx;
			int sy = cb.sy;
			int sz = cb.sz;
			float[,,] fillings = GetFillings(pointedBlock);
			//Texture3D texture3D = new Texture3D(sx, sy, sz, TextureFormat.RFloat, false);
			Texture2D texture2D = new Texture2D(sy, sz, TextureFormat.RGB24, false)
			{
				filterMode = FilterMode.Point
			};
			for (int ix = 0; ix < sx; ix++)
			{
				for (int iy = 0; iy < sy; iy++)
				{
					for (int iz = 0; iz < sz; iz++)
					{
						texture2D.SetPixel(iy, iz, Color.white * fillings[ix, iy, iz]);
					}
				}
				texture2D.Apply(false, false);
				string path = Get.PermanentPaths.GetSpecificModDir("Industrial Enterprise Upgrade") + $"RelativeFillings/{imcb.GetForce().Name}_ScanX_{ix}.png";
				File.WriteAllBytes(path, texture2D.EncodeToPNG());
			}
			Destroy(texture2D);
		}
		private float[,,] GetFillings(Block pointedBlock) {
			IMainConstructBlock imcb = pointedBlock.MainConstruct;
			IAllBasicsRestricted cb = imcb.AllBasicsRestricted;
			float[,,] fillings = new float[cb.sx, cb.sy, cb.sz];
			foreach (Block b in cb.AliveAndDead.Blocks)
			{
				if (b == null)
					continue;
				Quaternion inverseLocalRotation = Quaternion.Inverse(b.LocalRotation);
				if (b.LocalPositions != null) //Block ist größer als 1X1X1.
				{
					foreach (Vector3i lps in b.LocalPositions)
					{
						Vector3i deltaPos = lps - b.LocalPosition;
						Vector3i inverseLocal = inverseLocalRotation * deltaPos;
						RelativeFilling? rf = b.RelativeFillingByName();
						fillings[lps.x - cb.minx_, lps.y - cb.miny_, lps.z - cb.minz_] = rf.HasValue ? rf.Value.RelativeFillingAtLocalCoordinates(inverseLocal.x, inverseLocal.y, inverseLocal.z) : 0;
					}
				}
				else
				{
					RelativeFilling? rf = b.RelativeFillingByName();
					fillings[b.LocalPosition.x - cb.minx_, b.LocalPosition.y - cb.miny_, b.LocalPosition.z - cb.minz_] = rf.HasValue ? rf.Value.RelativeFillingAtLocalCoordinates(0, 0, 0) : 0;
				}
			}
			return fillings;
		}
	}
}
