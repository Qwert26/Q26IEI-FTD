using System;
namespace IndustrialEnterpriseUpgrade.Movment.Water {
	public class HydrofoilConnector : HydrofoilComponent {
		protected override int ConnectionType {
			get {
				return (int)HydrofoilConnectionTypes.Connectors;
			}
		}
		public override void ComponentStart() {
			base.ComponentStart();
		}
	}
}