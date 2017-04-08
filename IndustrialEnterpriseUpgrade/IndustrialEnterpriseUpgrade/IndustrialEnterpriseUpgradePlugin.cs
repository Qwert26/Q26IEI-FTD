using System;
namespace IndustrialEnterpriseUpgrade {
	public class IndustrialEnterpriseUpgradePlugin : FTDPlugin {
		public string name {
			get {
				return "Industrial Enterprise Upgrade";
			}
		}
		public Version version {
			get {
				return new Version(1, 0, 0, 0);
			}
		}
		/// <summary>
		/// Das Plugin wird geladen und sollte notwendige Vorbereitungen treffen.
		/// </summary>
		public void OnLoad() {}
		/// <summary>
		/// Das Spiel wird beendet und das Plugin sollte nun Sachen abspeichern.
		/// </summary>
		public void OnSave() {}
	}
}