using UnityEngine;
namespace IndustrialEnterpriseUpgrade.AI {
	public class AIThrustercraftAI : AIConstructableControl {
		public override enumAIConstructableType Type => enumAIConstructableType.none;
		public override void Engage() {}
		public override void FleetMove() {}
		public override void Patrol() {}
		public override void Search() {}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="requestFractions">Die errechneten Antriebsfraktionen für die gewollte Bewegung.
		/// x steht für Vorwärts/Rückwärts, y für Links/Rechts und z für Oben/Unten.</param>
		private void Translation(Vector3 requestFractions) {
			if (requestFractions == Vector3.zero) {
				return;
			}
			float absMax = Mathf.Max(Mathf.Abs(requestFractions.x),Mathf.Abs(requestFractions.y),Mathf.Abs(requestFractions.z));
			requestFractions /= absMax;
			if (requestFractions.x > 0) {
				MainConstruct.iThrusters.BalancedForwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.x);
			} else {
				MainConstruct.iThrusters.BalancedBackwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.x);
			}
			if(requestFractions.y>0) {
				MainConstruct.iThrusters.BalancedSideStepRight.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.y);
			} else {
				MainConstruct.iThrusters.BalancedSideStepLeft.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.y);
			}
			if (requestFractions.z > 0) {
				MainConstruct.iThrusters.BalancedUpwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.z);
			} else {
				MainConstruct.iThrusters.BalancedDownwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.z);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="requestFractions">Die errechneten Rotationsbewegungen für die gewollte Drehung.
		/// x steht für Nicken, y steht für Gieren und z steht für Rollen.</param>
		private void Rotation(Vector3 requestFractions) {
			float absMax = Mathf.Max(Mathf.Abs(requestFractions.x), Mathf.Abs(requestFractions.y), Mathf.Abs(requestFractions.z));
			requestFractions /= absMax;
			if (requestFractions.x > 0) {
				MainConstruct.iThrusters.BalancedNoseUp.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.x);
			} else {
				MainConstruct.iThrusters.BalancedNoseDown.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.x);
			}
			if (requestFractions.y > 0) {
				MainConstruct.iThrusters.BalancedYawRight.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.y);
			} else {
				MainConstruct.iThrusters.BalancedYawLeft.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.y);
			}
			if (requestFractions.z > 0) {
				MainConstruct.iThrusters.BalancedRollRight.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.z);
			} else {
				MainConstruct.iThrusters.BalancedRollLeft.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.z);
			}
		}
	}
}