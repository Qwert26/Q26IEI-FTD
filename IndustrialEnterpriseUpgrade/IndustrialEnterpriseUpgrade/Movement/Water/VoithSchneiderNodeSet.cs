using System;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class VoithSchneiderNodeSet : GovernedSetManager<VoithSchneiderNode, VoithSchneiderFeeler, VoithSchneiderConnectedTypeInfo, VoithSchneiderEngine> {
		public VoithSchneiderNodeSet(MainConstruct Construct) : base(Construct) {
			//HookUpRefreshFunctionToAChangeListener();
		}
		public override VoithSchneiderFeeler MakeFeeler(VoithSchneiderNode node) {
			return new VoithSchneiderFeeler(node);
		}
		public override VoithSchneiderNode MakeNode(AllConstruct C, VoithSchneiderEngine B) {
			return new VoithSchneiderNode(C, 1000, B);
		}
	}
}
