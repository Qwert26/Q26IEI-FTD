using System;
namespace IndustrialEnterpriseUpgrade.AI {
	public class AIAirshipAI : AIConstructableControl {
		public ControllerWrapper pitch = new ControllerWrapper(new DefaultControl());
		public ControllerWrapper yaw = new ControllerWrapper(new DefaultControl());
		public ControllerWrapper roll = new ControllerWrapper(new DefaultControl());
		public ControllerWrapper altitude = new ControllerWrapper(new PidStandardForm());
		public override enumAIConstructableType Type {
			get {
				return enumAIConstructableType.aerial;
			}
		}
		public override bool AllowAerialWaypoints {
			get {
				return true;
			}
		}
		public AirshipAIParameters parameters;
		/// <summary>
		/// Greift den am höchsten priorisierten Feind an.
		/// </summary>
		/// <see cref="AiNode.GetTargetPositionInfoForEngagementTarget()"/>
		public override void Engage() {
			TPI = Node.GetTargetPositionInfoForEngagementTarget();
			MoveToCurrentTPI();
		}
		/// <summary>
		/// Fliegt in Formation mit dem Flagschiff
		/// </summary>
		/// <see cref="Force.GetWaypointToMoveTo()"/>
		public override void FleetMove() {
			TPI = GetTargetPositionInfoForThisWayPoint(Node.MainConstruct.GetForce().GetWaypointToMoveTo());
			MoveToCurrentTPI();
		}
		/// <summary>
		/// Folgt einer vom Spieler gegebener Route.
		/// </summary>
		/// <see cref="AiNode.GetRouteParameters()"/>
		public override void Patrol() {
			PatrolRoutes routeParameters = Node.GetRouteParameters();
			if(routeParameters != null) {
				PatrolRoute route = routeParameters.GetSelectedRoute();
				if(route.GetNumberOfNodes() > 0) {
					PatrolWaypoint waypoint = route.GetNextNode();
					TPI = new TargetPositionInfo(waypoint.GetPosition(), Node.GetOurConstructablePosition(), Node.GetOurForward(), Node.GetOurRight());
					MoveToCurrentTPI();
					route.AssessWaypointReached(Node.GetOurConstructablePosition());
					if(route.GetNumberOfNodes() == 0&&Node.GetTeam()==GAMESTATE.MyTeam) {
						AiSoundPlayer.PlayAiVoice(ForceTravelRestrictions.Air,"NewOrders");
					}
				}
			}
			throw new NotImplementedException();
		}
		/// <summary>
		/// Fliegt zum Spieler, wenn keine Feinde im Spiel sind.
		/// </summary>
		/// <see cref="StaticPlayers.GetFriendlyPlayerPosition(ObjectId)"/>
		public override void Search() {
			TPI = GetTargetPositionInfoForThisWayPoint(StaticPlayers.GetFriendlyPlayerPosition(Node.GetTeam()));
			MoveToCurrentTPI();
		}
		public void MoveToCurrentTPI() {

		}
	}
}