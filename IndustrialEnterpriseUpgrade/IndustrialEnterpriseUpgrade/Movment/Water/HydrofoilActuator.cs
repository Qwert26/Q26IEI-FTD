namespace IndustrialEnterpriseUpgrade.Movment.Water {
	public class HydrofoilActuator : HydrofoilComponent {
		protected override int ConnectionType {
			get {
				return (int)HydrofoilConnectionTypes.Actuators;
			}
		}
	}
}