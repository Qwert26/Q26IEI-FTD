using IndustrialEnterpriseUpgrade.Framework.Statemachine;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade.AI.AirshipStateMachine {
	public abstract class AbstractAirshipAIState : AbstractAIConstructableControlState<AIAirshipAI> {
		/// <summary>
		/// Dies ist die angestrebte Höhe des Luftschiffes.
		/// </summary>
		protected float targetHeight;
		public AbstractAirshipAIState(AIAirshipAI Fokus) : base(Fokus) {}
		public override void CommonWork() {

		}
	}
}