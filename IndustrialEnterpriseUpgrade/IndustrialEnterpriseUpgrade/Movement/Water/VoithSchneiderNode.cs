using System;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class VoithSchneiderNode : GovernedNode<VoithSchneiderEngine> {
		public VoithSchneiderNode(AllConstruct c, int maxComponentCount, VoithSchneiderEngine B) : base(c, maxComponentCount, B) {}
		protected override void OnNodeDestroy() {}
	}
}
