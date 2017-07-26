using System.Collections.Generic;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class HydrofoilNode : GovernedNode<HydrofoilController> {
		private ICollection<HydrofoilActuator> topActuators,bottomActuators,leftActuators,rightActuators;
		public ICollection<HydrofoilActuator> TopActuators {
			get {
				return topActuators;
			}
		}
		public ICollection<HydrofoilActuator> BottomActuators {
			get {
				return bottomActuators;
			}
		}
		public ICollection<HydrofoilActuator> LeftActuators {
			get {
				return leftActuators;
			}
		}
		public ICollection<HydrofoilActuator> RightActuators {
			get {
				return rightActuators;
			}
		}
		public HydrofoilNode(AllConstruct construct, int maxComponentCount, HydrofoilController block) : base(construct, maxComponentCount, block) {
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