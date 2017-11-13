namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class VoithSchneiderNode : GovernedNode<VoithSchneiderGearbox> {
		public VoithSchneiderNode(AllConstruct c, int maxComponentCount, VoithSchneiderGearbox B) : base(c, maxComponentCount, B) {}
		protected override void OnNodeDestroy() {

		}
	}
}
