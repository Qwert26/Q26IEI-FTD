using System.Collections.Generic;
namespace IndustrialEnterpriseUpgrade.Movment.Water {
	public class HydrofoilNode : GovernedNode<HydrofoilController> {
		public List<HydrofoilActuator> topActuators,bottomActuators,leftActuators,rightActuators;
		public HydrofoilNode(AllConstruct c, int maxComponentCount, HydrofoilController B) : base(c, maxComponentCount, B) {
			topActuators = new List<HydrofoilActuator>();
			bottomActuators = new List<HydrofoilActuator>();
			leftActuators = new List<HydrofoilActuator>();
			rightActuators = new List<HydrofoilActuator>();
		}
		protected override void OnNodeDestroy() {}
		/// <summary>
		/// Führt einen Reset des Nodes durch, bevor der Fühler ausgesendet wird.
		/// </summary>
		public override void PriorToSendingOutFeelers() {
			base.PriorToSendingOutFeelers();
			topActuators.Clear();
			bottomActuators.Clear();
			leftActuators.Clear();
			rightActuators.Clear();
		}
	}
}