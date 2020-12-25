using System;
using BrilliantSkies.Common.Circuits;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Modding;
using IndustrialEnterpriseUpgrade.BreadBoards;
namespace IndustrialEnterpriseUpgrade
{
	public class IndustrialEnterpriseUpgradePlugin : GamePlugin, GamePlugin_PostLoad {
		public string name => "Industrial Enterprise Upgrade";
		public Version version => new Version(2, 8, 2, 5);
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
			BoardTypes.FtdBreadboard.Add(new BoardTypes.ComponentType(typeof(CustomTagDriveModule)));
			//BoardTypes.FtdBreadboard.Add(new BoardTypes.ComponentType(typeof(CustomTagInput)));
			AdvLogger.LogInfo(name+" V"+version+" has been loaded.");
		}
		/// <summary>
		/// Das Spiel wird beendet und das Plugin sollte nun Sachen abspeichern.
		/// </summary>
		public void OnSave() {
			AdvLogger.LogInfo(name+" didn´t saved anything.");
		}
	}
}