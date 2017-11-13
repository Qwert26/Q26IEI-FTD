using System;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade.AI {
	public class AIThrustercraftAI : AIConstructableControl {
		public override enumAIConstructableType Type {
			get {
				switch (Math.Sign(MainConstruct.Thrusters.WaterThrusters - MainConstruct.Thrusters.AirAndSpaceThrusters)) {
					case -1:
						return enumAIConstructableType.aerial;
					case 1:
						return enumAIConstructableType.naval;
					default:
						return enumAIConstructableType.land;//?
				}
			}
		}
		public override void Engage() {}
		public override void FleetMove() {}
		public override void Patrol() {}
		public override void Search() {}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="requestFractions">Die errechneten Antriebsfraktionen für die gewollte Bewegung.
		/// x steht für Rechts/Links, y für Oben/Unten und z für Vorwärts/Rückwärts.</param>
		private void Translation(Vector3 requestFractions) {
			if (requestFractions == Vector3.zero) {
				return;
			}
			requestFractions = requestFractions.normalized;
			if (requestFractions.x > 0) {
				MainConstruct.iThrusters.BalancedSideStepRight.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.x);
			} else {
				MainConstruct.iThrusters.BalancedSideStepLeft.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.x);
			}
			if(requestFractions.y>0) {
				MainConstruct.iThrusters.BalancedUpwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.y);
			} else {
				MainConstruct.iThrusters.BalancedDownwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.y);
			}
			if (requestFractions.z > 0) {
				MainConstruct.iThrusters.BalancedForwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.z);
			} else {
				MainConstruct.iThrusters.BalancedBackwards.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.z);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="requestFractions">Die errechneten Rotationsbewegungen für die gewollte Drehung.
		/// x steht für hochziehen/runterdrücken, y steht für rechts/links drehen und z steht für links/rechts rollen.</param>
		private void Rotation(Vector3 requestFractions) {
			if (requestFractions == Vector3.zero) {
				return;
			}
			requestFractions = requestFractions.normalized;
			if (requestFractions.x > 0) {
				MainConstruct.iThrusters.BalancedNoseDown.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.x);
			} else {
				MainConstruct.iThrusters.BalancedNoseUp.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.x);
			}
			if (requestFractions.y > 0) {
				MainConstruct.iThrusters.BalancedYawRight.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.y);
			} else {
				MainConstruct.iThrusters.BalancedYawLeft.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.y);
			}
			if (requestFractions.z > 0) {
				MainConstruct.iThrusters.BalancedRollLeft.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, requestFractions.z);
			} else {
				MainConstruct.iThrusters.BalancedRollRight.RecalculateIfNecessaryAndRun(MainConstruct.iThrusters.FullThrusterSet, -requestFractions.z);
			}
		}
	}
}