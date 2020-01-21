using BrilliantSkies.Ftd.Avatar.Items;
using UnityEngine;
using BrilliantSkies.Core.Constants;
using System.Text;
using BrilliantSkies.Core.Types;
using System.IO;
using BrilliantSkies.Core;

namespace IndustrialEnterpriseUpgrade.MachineLearning
{
	public class RelativeFillingExtractor : CharacterItem
	{
		public override string PrimaryFunctionDescription => "Create relative filling map";
		public override bool AreYouTwoHanded()
		{
			return false;
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
			float[,,] fillings = new float[sx, sy, sz];
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
			string path = Get.PerminentPaths.GetSpecificModDir("Industrial Enterprise Upgrade") + $"RelativeFillings/{imcb.GetName()}.txt";
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
	}
}
