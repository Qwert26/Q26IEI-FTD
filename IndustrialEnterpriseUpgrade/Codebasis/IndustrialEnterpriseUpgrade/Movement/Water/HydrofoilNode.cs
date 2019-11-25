using System.Collections.Generic;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class HydrofoilNode : GovernedNode<HydrofoilController> {
        public ICollection<HydrofoilActuator> TopActuators { get; }
        public ICollection<HydrofoilActuator> BottomActuators { get; }
        public ICollection<HydrofoilActuator> LeftActuators { get; }
        public ICollection<HydrofoilActuator> RightActuators { get; }
        public HydrofoilNode(AllConstruct construct, int maxComponentCount, HydrofoilController block) : base(construct, maxComponentCount, block) {
			TopActuators = new List<HydrofoilActuator>();
			BottomActuators = new List<HydrofoilActuator>();
			LeftActuators = new List<HydrofoilActuator>();
			RightActuators = new List<HydrofoilActuator>();
		}
		protected override void OnNodeDestroy() {}
		/// <summary>
		/// Führt einen Reset des Nodes durch, bevor der Fühler ausgesendet wird.
		/// </summary>
		public override void PriorToSendingOutFeelers() {
			base.PriorToSendingOutFeelers();
			TopActuators.Clear();
			BottomActuators.Clear();
			LeftActuators.Clear();
			RightActuators.Clear();
		}
	}
}