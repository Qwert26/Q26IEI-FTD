using System;
namespace IndustrialEnterpriseUpgrade.AI {
	public class AirshipAIParameters {
		/// <summary>
		/// Die maximale Höhe, die das Luftschiff jemals erreichen soll. Sollte niemals höher als die Atmosphärengrenze liegen.
		/// </summary>
		public float maxHeight;
		/// <summary>
		/// Die Höhe, die das Luftschiff halten soll, wenn es den Spieler sucht.
		/// </summary>
		public float idleHeight;
		/// <summary>
		/// Das Luftschiff fliegt IMMER auf der selben Höhe wie der Feind, um Kollsionen zu vermeiden, wird es links oder rechts ausweichen.
		/// </summary>
		public bool lockHeight;
	}
}