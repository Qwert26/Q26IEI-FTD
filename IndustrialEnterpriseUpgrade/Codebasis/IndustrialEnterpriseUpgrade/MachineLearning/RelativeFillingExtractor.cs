using BrilliantSkies.Ftd.Avatar.Items;
using UnityEngine;
using BrilliantSkies.Core.Constants;
using System.Text;
using BrilliantSkies.Core.Types;
using System.IO;
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
			IMainConstructBlock imcb = GetPointedBlock().MainConstruct;
			ConstructBasics cb = imcb.AllBasicsRestricted as ConstructBasics;
			int sx = cb.sx;
			int sy = cb.sy;
			int sz = cb.sz;
			float[,,] fillings = new float[sx, sy, sz];
			foreach (Block b in cb.AliveAndDead.Blocks)
			{
				Quaternion inverseLocalRotation = Quaternion.Inverse(b.LocalRotation);
				foreach (Vector3i lps in b.LocalPositions) {
					Vector3i deltaPos = lps - b.LocalPosition;
					Vector3i inverseLocal = (inverseLocalRotation * deltaPos);
					fillings[lps.x - cb.minx_, lps.y - cb.miny_, lps.z - cb.minz_] = b.RelativeFillingByName().RelativeFillingAtLocalCoordinates(inverseLocal.x, inverseLocal.y, inverseLocal.z);
				}
			}
			StringBuilder path = Get.ProfilePaths.ProfileRootDir().Append($"RelativeFillings/{imcb.GetName()}.txt");
			using (StreamWriter sw = new StreamWriter(path.ToString(), false))
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
