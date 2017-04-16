using UnityEngine;
namespace IndustrialEnterpriseUpgrade.Movment.Water {
	public class HydrofoilController : Block, IGoverningBlock, IGoverningBlock<HydrofoilNode>, IAileron, IAirElevator, IAirRudder {
		#region Steuervariablen
		public float topPitch, topYaw, topRoll;
		public float bottomPitch, bottomYaw, bottomRoll;
		public float leftPitch, leftYaw, leftRoll;
		public float rightPitch, rightYaw, rightRoll;
		#endregion
		private static HydrofoilNodeSet construct(MainConstruct mc) {
			return new HydrofoilNodeSet(mc);
		}
		public HydrofoilNode Node {
			get;
			set;
		}
		public override void StateChanged(IBlockStateChange change) {
			base.StateChanged(change);
			if(change.IsAvailableToConstruct) {
				//GetOrConstruct fügt das NodeSet automatisch hinzu!
				MainConstruct.iNodeSets.DictionaryOfAllSets.GetOrConstruct(MainConstruct as global::MainConstruct,construct).AddSender(this);
				MainConstruct.iControls.AileronStore.Add(this);
				MainConstruct.iControls.AirElevatorStore.Add(this);
				MainConstruct.iControls.AirRudderStore.Add(this);
			}
			if(change.IsLostToConstructOrConstructLost) {
				//GetOrConstruct fügt das NodeSet automatisch hinzu!
				MainConstruct.iNodeSets.DictionaryOfAllSets.GetOrConstruct(MainConstruct as global::MainConstruct,construct).RemoveSender(this);
				MainConstruct.iControls.AileronStore.Remove(this);
				MainConstruct.iControls.AirElevatorStore.Remove(this);
				MainConstruct.iControls.AirRudderStore.Remove(this);
			}
		}
		#region Implementierung von IAileron
		public void RollLeft(float factor = 1) {}
		public void RollRight(float factor = 1) {}
		#endregion
		#region Implementierung von IAirElevator
		public void NoseUp(float f) {}
		public void NoseDown(float f) {}
		#endregion
		#region Implementierung von IAirRudder
		public void YawLeft(float f) {}
		public void YawRight(float f) {}
		#endregion
		public override InteractionReturn Secondary() {
			InteractionReturn ret=new InteractionReturn();
			ret.SpecialNameField = "Hydrofoil controller";
			ret.SpecialBasicDescriptionField = "Central piece for AI-controllable Hydrofoils";
			ret.AddExtraLine("Press <<Q>> to open the configuration GUI.");
			return ret;
		}
		public override void Secondary(Transform T) {
			new GenericBlockGUI().ActivateGui(this);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Wahr, wenn die GUI geschlossen werden soll, falsch wenn nicht.</returns>
		public override bool ExtraGUI() {
			GUILayout.BeginArea(new Rect(0,0,1280,800),"",GUI.skin.window);
			GUISliders.DecimalPlaces = 1;
			GUISliders.TotalWidthOfWindow = 1280;
			float allowance;
			#region Einstellung für oben
			GUILayout.BeginHorizontal();
			allowance = this.allowance(topYaw, topRoll);
			topPitch=GUISliders.LayoutDisplaySlider("T-P",topPitch,-allowance,allowance,enumMinMax.none,new ToolTip("The maximum absolute angle for Hydrofoils on the top when only a pitch up or down is issued."));
			allowance = this.allowance(topPitch, topRoll);
			topYaw = GUISliders.LayoutDisplaySlider("T-Y", topYaw, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the top when only a yaw left or right is issued."));
			allowance = this.allowance(topPitch, topYaw);
			topRoll = GUISliders.LayoutDisplaySlider("T-R", topRoll, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the top when only a roll left or right is issued."));
			GUILayout.EndHorizontal();
			#endregion
			#region Einstellung für unten
			GUILayout.BeginHorizontal();
			allowance = this.allowance(bottomYaw, bottomRoll);
			bottomPitch = GUISliders.LayoutDisplaySlider("B-P", bottomPitch, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the bottom when only a pitch up or down is issued."));
			allowance = this.allowance(bottomPitch, bottomRoll);
			bottomYaw = GUISliders.LayoutDisplaySlider("B-Y", bottomYaw, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the bottom when only a yaw left or right is issued."));
			allowance = this.allowance(bottomPitch, bottomYaw);
			bottomRoll = GUISliders.LayoutDisplaySlider("B-R", bottomRoll, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the bottom when only a roll left or right is issued."));
			GUILayout.EndHorizontal();
			#endregion
			#region Einstellung für links
			GUILayout.BeginHorizontal();
			allowance = this.allowance(leftYaw, leftRoll);
			leftPitch = GUISliders.LayoutDisplaySlider("L-P", leftPitch, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the left when only a pitch up or down is issued."));
			allowance = this.allowance(leftPitch, leftRoll);
			leftYaw = GUISliders.LayoutDisplaySlider("L-Y", leftYaw, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the left when only a yaw left or right is issued."));
			allowance = this.allowance(leftPitch, leftYaw);
			leftRoll = GUISliders.LayoutDisplaySlider("L-R", leftRoll, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the left when only a roll left or right is issued."));
			GUILayout.EndHorizontal();
			#endregion
			#region Einstellung für rechts
			GUILayout.BeginHorizontal();
			allowance = this.allowance(rightYaw, rightRoll);
			rightPitch = GUISliders.LayoutDisplaySlider("R-P", rightPitch, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the right when only a pitch up or down is issued."));
			allowance = this.allowance(rightPitch, rightRoll);
			rightYaw = GUISliders.LayoutDisplaySlider("R-Y", rightYaw, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the right when only a yaw left or right is issued."));
			allowance = this.allowance(rightPitch, rightYaw);
			rightRoll = GUISliders.LayoutDisplaySlider("R-R", rightRoll, -allowance, allowance, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the right when only a roll left or right is issued."));
			GUILayout.EndHorizontal();
			bool ret=GuiCommon.DisplayCloseButton(1280);
			#endregion
			GUILayout.EndArea();
			return ret;
		}
		private float allowance(float angle1, float angle2) {
			return 45f - (Mathf.Abs(angle1) + Mathf.Abs(angle2));
		}
		/// <summary>
		/// Die Methode <see cref="ExtraGUI"/> hat den Wert "wahr" zurückgegeben und die GUI wurde gschlossen. Diese Methode wird nun aufgerufen
		/// </summary>
		public override void ExtraGUIClosed() {
			base.ExtraGUIClosed();
			StuffChangedSyncIt();
		}
		public override void StuffChangedSyncIt() {
			GetConstructableOrSubConstructable().iMultiplayerSyncroniser.RPCRequest_SyncroniseBlock(this,topPitch,topYaw,topRoll,bottomPitch,bottomYaw,bottomRoll,leftPitch,leftYaw,leftRoll,rightPitch,rightYaw,rightRoll,0);
		}
		public override void SyncroniseUpdate(float tp,float ty,float tr,float bp,float by,float br,float lp,float ly,float lr,float rp,float ry,float rr,float ignored=0) {
			topPitch = tp;
			topYaw = ty;
			topRoll = tr;
			bottomPitch = bp;
			bottomYaw = by;
			bottomRoll = br;
			leftPitch = lp;
			leftYaw = ly;
			leftRoll = lr;
			rightPitch = rp;
			rightYaw = ry;
			rightRoll = rr;
		}
	}
}