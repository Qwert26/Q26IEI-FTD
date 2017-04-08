using System;
namespace IndustrialEnterpriseUpgrade.AI {
	public class AIAirshipAI : AIConstructableControl {
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
		/// <summary>
		/// Greift den am höchsten priorisierten Feind an.
		/// </summary>
		/// <see cref="AiNode.GetTargetPositionInfoForEngagementTarget()"/>
		public override void Engage() {
			throw new NotImplementedException();
		}
		/// <summary>
		/// Fliegt in Formation mit dem Flagschiff
		/// </summary>
		/// <see cref="Force.GetWaypointToMoveTo()"/>
		public override void FleetMove() {
			throw new NotImplementedException();
		}
		/// <summary>
		/// Folgt einer vom Spieler gegebener Route.
		/// </summary>
		/// <see cref="AiNode.GetRouteParameters()"/>
		public override void Patrol() {
			throw new NotImplementedException();
		}
		/// <summary>
		/// Fliegt zum Spieler, wenn keine Feinde im Spiel sind.
		/// </summary>
		/// <see cref="StaticPlayers.GetFriendlyPlayerPosition(ObjectId)"/>
		public override void Search() {
			throw new NotImplementedException();
		}
	}
}