using System;
namespace IndustrialEnterpriseUpgrade.Movement.Water{
	public class VoithSchneiderNodeSet : GovernedSetManager<VoithSchneiderNode, VoithSchneiderFeeler, VoithSchneiderConnectedTypeInfo, VoithSchneiderGearbox> {
		public VoithSchneiderNodeSet(MainConstruct C) : base(C) {
			HookUpRefreshFunctionToAChangeListener(Guid.Empty);
		}
		public override VoithSchneiderFeeler MakeFeeler(VoithSchneiderNode node) {
			return new VoithSchneiderFeeler(node);
		}
		public override VoithSchneiderNode MakeNode(AllConstruct C, VoithSchneiderGearbox B) {
			return new VoithSchneiderNode(C, 1000, B);
		}
	}
}