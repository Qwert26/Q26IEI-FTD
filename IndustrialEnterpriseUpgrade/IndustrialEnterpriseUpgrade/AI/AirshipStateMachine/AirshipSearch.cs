using IndustrialEnterpriseUpgrade.Framework.Statemachine;
namespace IndustrialEnterpriseUpgrade.AI.AirshipStateMachine {
	public class AirshipSearch : AbstractAirshipAIState {
		public AirshipSearch(AIAirshipAI Fokus) : base(Fokus) {}
		public override AbstractState<AIAirshipAI> NextState() {
			switch(Fokus.baseState) {
				case AIAirshipAI.BaseState.ENGAGE:
					break;
				case AIAirshipAI.BaseState.FLEETMOVE:
					break;
				case AIAirshipAI.BaseState.PATROL:
					break;
				case AIAirshipAI.BaseState.SEARCH:
					return this;
				default:
					break;
			}
			return null;
		}
		public override void PullCurrentState() {
			base.PullCurrentState();
			targetHeight = Fokus.parameters.idleHeight;
		}
		public override void WorkWithCurrentState() {

		}
	}
}