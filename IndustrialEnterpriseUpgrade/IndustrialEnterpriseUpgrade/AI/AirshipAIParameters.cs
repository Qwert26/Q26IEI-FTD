namespace IndustrialEnterpriseUpgrade.AI {
	public class AirshipAIParameters {
		#region Höheneinstellungen
		/// <summary>
		/// Die maximale Höhe, die das Luftschiff jemals erreichen soll. Sollte niemals höher als die Atmosphärengrenze liegen.
		/// </summary>
		public float maxHeight;
		/// <summary>
		/// Die minimale Höhe, die das Luftschiff jemals erreichen wird. Sollte immer größer als 0 sein.
		/// </summary>
		public float minHeight;
		/// <summary>
		/// Die Höhe, die das Luftschiff halten soll, wenn es den Spieler sucht. Sollte zwischen <see cref="minHeight"/> und <see cref="maxHeight"/> liegen.
		/// </summary>
		public float idleHeight;
		/// <summary>
		/// Das Luftschiff fliegt IMMER auf der selben Höhe wie der Feind, um Kollsionen mit anderen Fahrzeugen oder dem Gelände zu vermeiden, wird es links oder rechts ausweichen.
		/// </summary>
		public bool lockHeight;
		#endregion
		#region Distanzeinstellungen
		/// <summary>
		/// Die maximale Distanz, über der das Luftschiff wieder anfängt, genau auf den Gegner zu fliegen.
		/// </summary>
		public float maxDistance;
		/// <summary>
		/// Diese Distanz muss unterschritten werden, während eines "Turn-by", bevor das Luftschiff mit seiner Breitseite anfängt.
		/// </summary>
		public float broadsideBeginMax;
		/// <summary>
		/// Diese Distanz muss überschritten werden, während eines "Turn-by", bevor das Luftschiff mit seiner Breitseite anfängt.
		/// </summary>
		public float broadsideBeginMin;
		/// <summary>
		/// Die minimale Distanz, unter der das Luftschiff anfängt, vor seinem Gegner zu flüchten.
		/// </summary>
		public float minDistance;
		/// <summary>
		/// Die maximale Distanz zum Spieler, unter das Luftschiff sämtliche Bewegung einstellt.
		/// </summary>
		public float idleDistance;
		#endregion
		#region Winkeleinstellungen
		/// <summary>
		/// Das Luftschiff beginnt den "Turn-by" aus großer Entfernung. Dies ist der Startwinkel den es unmittelbar einnehmen wird.
		/// </summary>
		public float upperStartAngle = 0f;
		/// <summary>
		/// Das Luftschiff wird diesen Winkel so lange bei behalten, bis ein "Turn-by" wieder notwendig ist.
		/// </summary>
		public float nominalAngle = 90f;
		/// <summary>
		/// Das Luftschiff beginnt den "Turn-by" aus geringer Entfernung. Dies ist der Startwinkel den es unmittelbar einnehmen wird.
		/// </summary>
		public float lowerStartAngle = 180f;
		#endregion
	}
}