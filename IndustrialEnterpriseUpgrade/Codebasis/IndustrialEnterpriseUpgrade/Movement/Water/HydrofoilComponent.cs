using UnityEngine;
using BrilliantSkies.Core;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public abstract class HydrofoilComponent : BlockComponent<HydrofoilNode, HydrofoilFeeler, HydrofoilConnectedTypeInfo> {
		public override void Secondary(Transform T) {
			if(LinkedUp) {
				Node.GoverningBlock.Secondary(T);
			}
		}
	}
}