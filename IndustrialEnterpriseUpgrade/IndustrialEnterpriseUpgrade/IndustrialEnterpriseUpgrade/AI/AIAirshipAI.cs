using BrilliantSkies.Core.CollisionAvoid;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade.AI {
	public class AIAirshipAI : AIConstructableControl {
		#region PYRA
		private ControllerWrapper pitch = new ControllerWrapper(new DefaultControl());
		private ControllerWrapper yaw = new ControllerWrapper(new DefaultControl());
		private ControllerWrapper roll = new ControllerWrapper(new DefaultControl());
		private ControllerWrapper altitude = new ControllerWrapper(new DefaultControl());
		#endregion
		#region
		
		#endregion
		public AIAirshipAI():base(){}
		public override enumAIConstructableType Type => enumAIConstructableType.aerial;
		public override bool AllowAerialWaypoints => true;
		private AirshipAIParameters parameters;
		public AirshipAIParameters Parameters {
			get => parameters;
			set => parameters = value;
		}
		/// <summary>
		/// Greift den am höchsten priorisierten Feind an.
		/// </summary>
		/// <seealso cref="AiNode.GetTargetPositionInfoForEngagementTarget()"/>
		public override void Engage() {
			TPI = Node.GetTargetPositionInfoForEngagementTarget();
			AdjustAngles();
			MoveToHeight(TPI.AltitudeAboveSeaLevel);
		}
		/// <summary>
		/// Fliegt in Formation mit dem Flagschiff
		/// </summary>
		/// <seealso cref="Force.GetWaypointToMoveTo()"/>
		public override void FleetMove() {
			TPI = GetTargetPositionInfoForThisWayPoint(Node.MainConstruct.GetForce().GetWaypointToMoveTo());
			AdjustAngles();
			MoveToHeight(TPI.AltitudeAboveSeaLevel);
		}
		/// <summary>
		/// Folgt einer vom Spieler gegebener Route.
		/// </summary>
		/// <seealso cref="AiNode.GetRouteParameters()"/>
		public override void Patrol() {
			PatrolRoutes routeParameters = Node.GetRouteParameters();
			if(routeParameters != null) {
				PatrolRoute route = routeParameters.GetSelectedRoute();
				if(route.GetNumberOfNodes() > 0) {
					PatrolWaypoint waypoint = route.GetNextNode();
					TPI = GetTargetPositionInfoForThisWayPoint(waypoint.GetPosition());
					AdjustAngles();
					MoveToHeight(TPI.AltitudeAboveSeaLevel);
					route.AssessWaypointReached(Node.GetOurConstructablePosition());
					if(route.GetNumberOfNodes() == 0 && Node.GetTeam() == GAMESTATE.MyTeam) {
						AiSoundPlayer.PlayAiVoice(ForceTravelRestrictions.Air, "NewOrders");
					}
				}
			}
		}
		/// <summary>
		/// Fliegt zum Spieler, wenn keine Feinde im Spiel sind.
		/// </summary>
		/// <seealso cref="StaticPlayers.GetFriendlyPlayerPosition(ObjectId)"/>
		public override void Search() {
			TPI = GetTargetPositionInfoForThisWayPoint(StaticPlayers.GetFriendlyPlayerPosition(Node.GetTeam()));
			AdjustAngles();
			MoveToHeight(parameters.idleHeight);
		}
		private void AdjustAngles() {
			//TODO
		}
		private void MoveToHeight(float height) {
			CollisionWarning warningForObject = CollisionCheckManager.Instance.WarningManager.GetWarningForObject(Node.MainConstruct);
			float altiDrive;
			if(warningForObject == null) {
				altiDrive = altitude.NewMeasurement(height, MainConstruct.AltitudeOfComAboveMeanSeaOrGroundLevel, Time.time);
			} else {
				if(parameters.lockHeight) {

				} else {
					altiDrive = altitude.NewMeasurement(warningForObject.AltitudeToMoveTo, MainConstruct.AltitudeOfComAboveMeanSeaOrGroundLevel, Time.time);
				}
			}
		}
	}
}