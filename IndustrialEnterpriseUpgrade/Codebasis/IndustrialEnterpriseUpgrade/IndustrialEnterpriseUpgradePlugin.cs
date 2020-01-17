using System;
using BrilliantSkies.Core;
using BrilliantSkies.Modding;
namespace IndustrialEnterpriseUpgrade
{
	public class IndustrialEnterpriseUpgradePlugin : GamePlugin, GamePlugin_PostLoad {
		public string name => "Industrial Enterprise Upgrade";
		public Version version => new Version(2, 5, 2, 19);
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
			SafeLogging.Log(name+" V"+version+" has been loaded.");
		}
		/// <summary>
		/// Das Spiel wird beendet und das Plugin sollte nun Sachen abspeichern.
		/// </summary>
		public void OnSave() {
			SafeLogging.Log(name+" didn´t saved anything.");
		}
	}
}