using System;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade {
	public class IndustrialEnterpriseUpgradePlugin : FTDPlugin, FTDPlugin_PostLoad {
		public string name => "Industrial Enterprise Upgrade";
		public Version version => new Version(2,16,0,0);
		/// <summary>
		/// Lädt weitere Daten nach, sobald das Spiel sämtliche Plug-Ins geladen hat.
		/// </summary>
		/// <returns>
		/// true
		/// </returns>
		public bool AfterAllPluginsLoaded() {
			return true;
		}
		/// <summary>
		/// Das Plugin wird geladen und sollte notwendige Vorbereitungen treffen.
		/// </summary>
		public void OnLoad() {
			Debug.Log(name+" V"+version+" has been loaded.");
		}
		/// <summary>
		/// Das Spiel wird beendet und das Plugin sollte nun Sachen abspeichern. Die Methode wird jedoch im Spiel nie aufgerufen...
		/// </summary>
		public void OnSave() {
			Debug.Log(name+" didn´t saved anything.");
		}
	}
}