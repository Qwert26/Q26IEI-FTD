namespace IndustrialEnterpriseUpgrade.Movement.Water
{
    public class HydrofoilNodeSet : GovernedSetManager<HydrofoilNode, HydrofoilFeeler, HydrofoilConnectedTypeInfo, HydrofoilController> {
		public static int MAX_COMPONENT_COUNT=1000;
		public HydrofoilNodeSet(MainConstruct construct) : base(construct) {
			//Die GUID muss von der Definition der ItemGroup kommen! Ist der Aufruf jedoch wirklich notwendig?
			//HookUpRefreshFunctionToAChangeListener(new Guid("2909bf37-d528-4d64-951f-cc45c3b142b3"),1);
		}
		public override HydrofoilFeeler MakeFeeler(HydrofoilNode node) {
			return new HydrofoilFeeler(node);
		}
		public override HydrofoilNode MakeNode(AllConstruct C, HydrofoilController B) {
			return new HydrofoilNode(C,MAX_COMPONENT_COUNT, B);
		}
	}
}