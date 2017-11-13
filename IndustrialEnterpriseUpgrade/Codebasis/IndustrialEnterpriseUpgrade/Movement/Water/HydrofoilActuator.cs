using UnityEngine;
using System;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class HydrofoilActuator : HydrofoilComponent {
		private const float lift = 10;
		private float angle;
		private float forceMultiplier;
		private CarriedObjectReference foilModel;
		private enumDirections register;
		public override void ComponentStart() {
			base.ComponentStart();
			foilModel = CarryThisWithUs(0);
		}
		protected override int ConnectionType => (int)HydrofoilConnectionTypes.Actuators;
		public override void FeelerFlowDown(HydrofoilFeeler feeler) {
			if(feeler == null) {
				return;
			}
			register = feeler.LocalActuatorOutput;
			switch(register) {
				case enumDirections.down:
					Node.BottomActuators.Add(this);
					break;
				case enumDirections.up:
					Node.TopActuators.Add(this);
					break;
				case enumDirections.left:
					Node.LeftActuators.Add(this);
					break;
				case enumDirections.right:
					Node.RightActuators.Add(this);
					break;
				default:
					break;
			}
		}
		public void SetAngle(float parameterAngle) {
			parameterAngle = Mathf.Clamp(parameterAngle, -45, 45);
			if(parameterAngle != angle) {
				angle = parameterAngle;
				if(foilModel != null) {
					foilModel.SetLocalRotation(Quaternion.Euler(-parameterAngle,0f,0f));
				}
				forceMultiplier = Mathf.Sin(Mathf.Deg2Rad*2f*parameterAngle);
			}
		}
		public void FixedUpdatePhysics(float deltaTime) {
			float waterLevel = MainConstruct.iMainPhysics.WaterLevelArray[ArrayPosition.x, ArrayPosition.z];
			float forwardSpeed = MainConstruct.iMainPhysics.iVelocities.VelocityInParticularDirection(GameWorldForwards);
			float relativeSubmersion = Mathf.Min(1, waterLevel + 0.5f - AltitudeAboveMeanSeaLevel);
			if(relativeSubmersion > 0) {
				MainConstruct.iMainPhysics.RequestForce(GameWorldUp * lift * forwardSpeed * forceMultiplier * relativeSubmersion, GameWorldPosition, enumForceType.LiftSurface);
			}
		}
		public override void StateChanged(IBlockStateChange change) {
			if(change == null) {
				return;
			}
			base.StateChanged(change);
			if(change.IsAvailableToConstruct) {
				//Das Aktualisieren des Winkels findet in FixedUpdateTwo statt, wir wollen unsere Kraft erst danach zum gelten bringen.
				GetConstructableOrSubConstructable().iScheduler.RegisterForFixedUpdateThree(new Action<float>(FixedUpdatePhysics));
			} else if(change.IsLostToConstructOrConstructLost) {
				//Das Aktualisieren des Winkels findet in FixedUpdateTwo statt, wir wollen unsere Kraft erst danach zum gelten bringen.
				GetConstructableOrSubConstructable().iScheduler.UnregisterForFixedUpdateThree(new Action<float>(FixedUpdatePhysics));
			}
		}
		#region Nutzerinteraktion
		public override InteractionReturn Secondary() {
			InteractionReturn ret = new InteractionReturn {
				SpecialNameField = "Hydrofoil Actuator",
				SpecialBasicDescriptionField = "The Actuator is the one applying the force. Current angle is " + angle + "."
			};
			if (LinkedUp) {
				ret.AddExtraLine("Currently linked up to a controller. Registered as "+register.ToString());
			} else {
				ret.AddExtraLine("<!Not linked up and thus stuck in its current angle!>");
			}
			return ret;
		}
		public override BlockTechInfo GetTechInfo() {
			return new BlockTechInfo().AddSpec("Max force per Velocity",lift).
				AddStatement("Its angle can only be changed when it is hooked up to a controller.").
				AddStatement("All Hydrofoils of the same group of the same controller must have the same orientation!").
				AddStatement("Only works when it is underwater!");
		}
		#endregion
	}
}