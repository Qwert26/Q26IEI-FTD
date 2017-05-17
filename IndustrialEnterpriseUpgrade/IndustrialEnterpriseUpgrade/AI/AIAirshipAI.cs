namespace IndustrialEnterpriseUpgrade.AI {
	public class AIAirshipAI : AIConstructableControl {
		public ControllerWrapper pitch = new ControllerWrapper(new DefaultControl());
		public ControllerWrapper yaw = new ControllerWrapper(new DefaultControl());
		public ControllerWrapper roll = new ControllerWrapper(new DefaultControl());
		public ControllerWrapper altitude = new ControllerWrapper(new DefaultControl());
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
		public enum BaseState {
			ENGAGE,
			FLEETMOVE,
			PATROL,
			SEARCH
		}
		public BaseState baseState;
		/// <summary>
		/// Greift den am höchsten priorisierten Feind an.
		/// </summary>
		/// <seealso cref="AiNode.GetTargetPositionInfoForEngagementTarget()"/>
		public override void Engage() {
			baseState = BaseState.ENGAGE;
			TPI = Node.GetTargetPositionInfoForEngagementTarget();
		}
		/// <summary>
		/// Fliegt in Formation mit dem Flagschiff
		/// </summary>
		/// <seealso cref="Force.GetWaypointToMoveTo()"/>
		public override void FleetMove() {
			baseState = BaseState.FLEETMOVE;
			TPI = GetTargetPositionInfoForThisWayPoint(Node.MainConstruct.GetForce().GetWaypointToMoveTo());
		}
		/// <summary>
		/// Folgt einer vom Spieler gegebener Route.
		/// </summary>
		/// <seealso cref="AiNode.GetRouteParameters()"/>
		public override void Patrol() {
			baseState = BaseState.PATROL;
			PatrolRoutes routeParameters = Node.GetRouteParameters();
			if(routeParameters != null) {
				PatrolRoute route = routeParameters.GetSelectedRoute();
				if(route.GetNumberOfNodes() > 0) {
					PatrolWaypoint waypoint = route.GetNextNode();
					TPI = GetTargetPositionInfoForThisWayPoint(waypoint.GetPosition());
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
			baseState = BaseState.SEARCH;
			TPI = GetTargetPositionInfoForThisWayPoint(StaticPlayers.GetFriendlyPlayerPosition(Node.GetTeam()));
		}
	}
}