using System;
namespace IndustrialEnterpriseUpgrade.Movment.Water {
	public class HydrofoilNodeSet : GovernedSetManager<HydrofoilNode, HydrofoilFeeler, HydrofoilConnectedTypeInfo, HydrofoilController> {
		public HydrofoilNodeSet(MainConstruct C) : base(C) {
			//Die GUID muss von der Definition der ItemGroup kommen! Ist der Aufruf jedoch wirklich notwendig?
			HookUpRefreshFunctionToAChangeListener(Guid.Empty, 1);
		}
		public override HydrofoilFeeler MakeFeeler(HydrofoilNode node) {
			return new HydrofoilFeeler(node);
		}
		public override HydrofoilNode MakeNode(AllConstruct C, HydrofoilController B) {
			return new HydrofoilNode(C,1000,B);
		}
	}
}