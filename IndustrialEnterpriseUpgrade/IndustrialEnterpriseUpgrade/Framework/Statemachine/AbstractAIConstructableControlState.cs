using System;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade.Framework.Statemachine {
	public abstract class AbstractAIConstructableControlState<TFokus> : AbstractState<TFokus> where TFokus:AIConstructableControl {
		protected TargetPositionInfo TPI;
		/// <summary>
		/// Enthält die Werte von der Höhe über "Nur Seelevel" gefolgt von "See- oder Bodenlevel".
		/// </summary>
		protected Vector2 altitudes;
		/// <summary>
		/// Enthält die Werte von Yaw, Pitch und Roll in dieser Reihenfolge.
		/// </summary>
		protected Vector3 eulerAngels;
		public AbstractAIConstructableControlState(TFokus Fokus) : base(Fokus) {}
		/// <summary>
		/// Holt sich vom <see cref="AIConstructableControl"/> den aktuellen Status, wie das aktuelle <see cref="TargetPositionInfo"/> die Höhen, sowie die aktuellen Winkel.
		/// </summary>
		public override void PullCurrentState() {
			TPI = Fokus.TPI;
			altitudes = new Vector2(Fokus.MainConstruct.AltitudeOfComAboveMeanSeaLevel,Fokus.MainConstruct.AltitudeOfComAboveMeanSeaOrGroundLevel);
			eulerAngels = Fokus.MainConstruct.SafeEulerAngles;
			eulerAngels.y = StaticMaths.FixRot180to180(eulerAngels.y);
			eulerAngels.y = -eulerAngels.y;
		}
	}
}