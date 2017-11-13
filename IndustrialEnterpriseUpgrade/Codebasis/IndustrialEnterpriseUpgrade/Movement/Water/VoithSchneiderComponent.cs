using UnityEngine;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public abstract class VoithSchneiderComponent : BlockComponent<VoithSchneiderNode, VoithSchneiderFeeler, VoithSchneiderConnectedTypeInfo> {
		public override void Secondary(Transform T) {
			if (LinkedUp) {
				Node.GoverningBlock.Secondary(T);
			}
		}
	}
}