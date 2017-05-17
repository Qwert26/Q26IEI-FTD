using System;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade.AI {
	public class AIAirshipMovementCard : AICard {
		public AIAirshipAI airshipAi;
		public AirshipAIParameters airshipParameters;
		public override void ComponentStart() {
			base.ComponentStart();
			airshipAi = new AIAirshipAI();
			airshipParameters = new AirshipAIParameters();
			airshipAi.parameters = airshipParameters;
		}
		/// <summary>
		/// Diese Karte ist nun mit einem Mainframe verbunden.
		/// </summary>
		/// <param name="feeler">Der Fühler</param>
		public override void CardFlowDown(AiFeeler feeler) {
			Node.SetController(airshipAi);
		}
		/// <summary>
		/// Erzeugt ein <c>InteractionReturn</c>, welches dann auf dem HUD dargestellt wird, wenn der Cursor des Spielers auf dem Block zeigt.
		/// </summary>
		/// <returns></returns>
		public override InteractionReturn Secondary() {
			InteractionReturn ret=base.Secondary();
			ret.SpecialNameField = "Airship movement algorithm Card";
			ret.SpecialBasicDescriptionField = "Provides customisable airship AI for blimps. Allows user selectable modes such as COMBAT and PATROL";
			ret.AddExtraLine("Press <<Q>> to adjust this algorithm");
			return ret;
		}
		/// <summary>
		/// Wird aufgerufen, wenn auf einem drauf gezeigten Block q gedrückt wird.
		/// </summary>
		/// <param name="T">Ein Transform von Unity, wird nicht benutzt.</param>
		public override void Secondary(Transform T) {
			new GenericBlockGUI().ActivateGui(this);
		}
		/// <summary>
		/// Erstellt eine GUI, die die vorherige GUI ausblendet.
		/// </summary>
		/// <returns>Wahr, wenn die GUI geschlossen werden soll, ansonsten Falsch.</returns>
		public override bool ExtraGUI() {
			return true;
		}
		/// <summary>
		/// Der Block wird in einer .blueprint-Datei gespeichert und soll nun alle wichtigen Werte im gegebenen Packet abspeichern, damit diese später wiederhergestellt werden können.
		/// </summary>
		/// <param name="v">Das Packet, in dem die Informationen geschrieben werden sollen.</param>
		public override void GetExtraInfo(ExtraInfoArrayWritePackage v) {
			base.GetExtraInfo(v);
			v.AddDelimiterOpen(DelimiterType.Card);
			v.WriteNextFloat(airshipParameters.maxHeight);
			v.WriteNextFloat(airshipParameters.minHeight);
			v.WriteNextFloat(airshipParameters.idleHeight);
			v.WriteNextBool(airshipParameters.lockHeight);
			v.AddDelimiterClose(DelimiterType.Card);
		}
		/// <summary>
		/// Der Block wurde aus einer .blueprint-Datei gelesen und soll nun alle wichtigen Werte aus dem gegeben Packet einlesen, damit dieser seinen "vorherigen" Zustand erhält.
		/// </summary>
		/// <param name="v">Das Packet, aus dem die Informationen gelesen werden sollen.</param>
		public override void SetExtraInfo(ExtraInfoArrayReadPackage v) {
			base.SetExtraInfo(v);
			if(v.FindDelimiterAndSpoolToIt(DelimiterType.Card)) {
				int size = v.ElementsToDelimiterIfThereIsOneOrEndOfArrayIfNot(DelimiterType.Card);
			}
		}
	}
}