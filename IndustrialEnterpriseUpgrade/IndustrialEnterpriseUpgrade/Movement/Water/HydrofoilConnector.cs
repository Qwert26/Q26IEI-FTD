using UnityEngine;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class HydrofoilConnector : HydrofoilComponent {
		private enumDirections localActuatorOutput;
		protected override int ConnectionType {
			get {
				return (int)HydrofoilConnectionTypes.Connectors;
			}
		}
		public override void ComponentStart() {
			base.ComponentStart();
			int actuatorOutput = (int)item.Code.Variables.GetInt("ActuatorOutput", 0);
			switch(actuatorOutput) {
				case 2:
					localActuatorOutput = enumDirections.left;
					break;
				case 3:
					localActuatorOutput = enumDirections.right;
					break;
				case 5:
					localActuatorOutput = enumDirections.up;
					break;
				case 6:
					localActuatorOutput = enumDirections.down;
					break;
				default:
					localActuatorOutput = enumDirections.unknown;
					break;
			}
		}
		public override void ItemSet() {
			base.ItemSet();
		}
		public override void TagFeelerConnectRules(IConnectionTypes feeler) {
			base.TagFeelerConnectRules(feeler);
			if(localActuatorOutput == enumDirections.left && feeler.LocalOutDirection.normalized == Vector3i.left) {
				feeler.SetNoConnection(ConnectionType);
				feeler.SetConnection((int)HydrofoilConnectionTypes.Actuators);
			} else if(localActuatorOutput == enumDirections.right && feeler.LocalOutDirection.normalized == Vector3i.right) {
				feeler.SetNoConnection(ConnectionType);
				feeler.SetConnection((int)HydrofoilConnectionTypes.Actuators);
			} else if(localActuatorOutput == enumDirections.down && feeler.LocalOutDirection.normalized == Vector3i.down) {
				feeler.SetNoConnection(ConnectionType);
				feeler.SetConnection((int)HydrofoilConnectionTypes.Actuators);
			} else if(localActuatorOutput == enumDirections.up && feeler.LocalOutDirection.normalized == Vector3i.up) {
				feeler.SetNoConnection(ConnectionType);
				feeler.SetConnection((int)HydrofoilConnectionTypes.Actuators);
			}
		}
		public override void InspectFeelerBeforeDirection(HydrofoilFeeler feeler, Vector3i localDirection, int index) {
			feeler.localActuatorOutput = localActuatorOutput;
		}
		public override InteractionReturn Secondary() {
			InteractionReturn ret;
			if(LinkedUp) {
				ret=Node.GoverningBlock.Secondary();
				ret.SpecialNameField = "Hydrofoil Connector " + ((localActuatorOutput != enumDirections.unknown) ? "Extern" : "Intern");
				ret.SpecialBasicDescriptionField = "Connects the controller on the inside with the actuators on the outside.";
				ret.AddExtraLineAtTop("The local Actuator output is " + localActuatorOutput.ToString());
			} else {
				ret=new InteractionReturn();
				ret.SpecialNameField = "Hydrofoil Connector " + ((localActuatorOutput != enumDirections.unknown) ? "Extern" : "Intern");
				ret.SpecialBasicDescriptionField = "Connects the controller on the inside with the actuators on the outside.";
				ret.AddExtraLine("<!NOT CONNECTED.!>Connect with a controller!");
			}
			return ret;
		}
		public override void Secondary(Transform T) {
			if(LinkedUp) {
				Node.GoverningBlock.Secondary(T);
			}
		}
	}
}