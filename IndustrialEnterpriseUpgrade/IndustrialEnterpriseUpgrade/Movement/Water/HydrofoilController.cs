using UnityEngine;
using System;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class HydrofoilController : Block, IGoverningBlock, IGoverningBlock<HydrofoilNode>, IAileron, IAirElevator, IAirRudder {
		#region Steuervariablen
		private float topPitch, topYaw, topRoll;
		private float bottomPitch, bottomYaw, bottomRoll;
		private float leftPitch, leftYaw, leftRoll;
		private float rightPitch, rightYaw, rightRoll;
		#endregion
		#region Eingabe
		/// <summary>
		/// Positive Werte meinen eine Rolle nach rechts.
		/// </summary>
		private float roll;
		/// <summary>
		/// Positive Werte meinen ein Hochziehen der Nase.
		/// </summary>
		private float pitch;
		/// <summary>
		/// Positive Werte meinen eine Kurve nach rechts.
		/// </summary>
		private float yaw;
		/// <summary>
		/// Die Zeitpunkte, wann jeweils <see cref="roll"/>, <see cref="pitch"/> oder <see cref=" yaw"/> zuletzt gesetzt worden sind.
		/// </summary>
		private float lastRollSet = -1f, lastPitchSet = -1f, lastYawSet = -1f;
		#endregion
		#region Tuning
		private float deltaTimeMultipliaktor;
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
				//MainConstruct.iControls.AileronStore.Add(this); Leider aktuell nicht möglich und das erstellen einer Mock-Up-Klasse, die Aileron erweitert, ist auch nicht möglich, da die nötigen Methoden nicht überschrieben werden können.
				MainConstruct.iControls.AirElevatorStore.Add(this);
				MainConstruct.iControls.AirRudderStore.Add(this);
				//Steuereingaben finden im FixedUpdate statt, wir müssen für unser Update dahinter sein.
				MainConstruct.iScheduler.RegisterForFixedUpdateTwo(new Action<float>(FixedUpdate));
			}
			if(change.IsLostToConstructOrConstructLost) {
				//GetOrConstruct fügt das NodeSet automatisch hinzu!
				MainConstruct.iNodeSets.DictionaryOfAllSets.GetOrConstruct(MainConstruct as global::MainConstruct,construct).RemoveSender(this);
				//MainConstruct.iControls.AileronStore.Remove(this); Leider aktuell nicht möglich und das erstellen einer Mock-Up-Klasse, die Aileron erweitert, ist auch nicht möglich, da die nötigen Methoden nicht überschrieben werden können.
				MainConstruct.iControls.AirElevatorStore.Remove(this);
				MainConstruct.iControls.AirRudderStore.Remove(this);
				//Steuereingaben finden im FixedUpdate statt, wir müssen für unser Update dahinter sein.
				MainConstruct.iScheduler.UnregisterForFixedUpdateTwo(new Action<float>(FixedUpdate));
			}
		}
		public override void BlockStart() {
			//Dieser künstliche Lag ist notwendig für die Funktion! Er muss mindestens 1 betragen!
			deltaTimeMultipliaktor=item.Code.Variables.GetFloat("dtMultiplikator", 1);
		}
		public void FixedUpdate(float deltaTime) {
			{//Sind wir schon im nächsten Zeitschritt? Wurden die Steuereingaben verändert?
				float fixedTime = Time.fixedTime-deltaTimeMultipliaktor*Time.fixedDeltaTime;
				if(fixedTime > lastPitchSet) {
					pitch = 0;
				}
				if(fixedTime > lastRollSet) {
					roll = 0;
				}
				if(fixedTime > lastYawSet) {
					yaw = 0;
				}
			}
			{//Berechne alle nötigen Winkel und setze die Aktuatoren.
				float angle = 0;
				angle = topPitch * pitch + topRoll * roll + topYaw * yaw;
				foreach(HydrofoilActuator actuator in Node.topActuators) {
					actuator.SetAngle(angle);
				}
				angle = bottomPitch * pitch + bottomRoll * roll + bottomYaw * yaw;
				foreach(HydrofoilActuator actuator in Node.bottomActuators) {
					actuator.SetAngle(angle);
				}
				angle = leftPitch * pitch + leftRoll * roll + leftYaw * yaw;
				foreach(HydrofoilActuator actuator in Node.leftActuators) {
					actuator.SetAngle(angle);
				}
				angle = rightPitch * pitch + rightRoll * roll + rightYaw * yaw;
				foreach(HydrofoilActuator actuator in Node.rightActuators) {
					actuator.SetAngle(angle);
				}
			}
		}
		#region Implementierung von IAileron
		public void RollLeft(float factor = 1) {
			RollRight(-factor);
		}
		public void RollRight(float factor = 1) {
			roll = factor;
			lastRollSet = Time.fixedTime;
		}
		#endregion
		#region Implementierung von IAirElevator
		public void NoseUp(float f=1) {
			pitch = f;
			lastPitchSet = Time.fixedTime;
		}
		public void NoseDown(float f=1) {
			NoseUp(-f);
		}
		#endregion
		#region Implementierung von IAirRudder
		public void YawLeft(float f=1) {
			YawRight(-f);
		}
		public void YawRight(float f=1) {
			yaw = f;
			lastYawSet = Time.fixedTime;
		}
		#endregion
		#region Nutzerinteraktion
		/// <summary>
		/// Der Block ist nun im Zentrum des Fokus und sollte nun in kurzer Form alle nötigen Informationen darstellen.
		/// </summary>
		/// <returns>Den aktuellen Status, so kurz wie möglich.</returns>
		public override InteractionReturn Secondary() {
			InteractionReturn ret=new InteractionReturn();
			ret.SpecialNameField = "Hydrofoil controller";
			ret.SpecialBasicDescriptionField = "Central piece for AI-controllable Hydrofoils";
			ret.AddExtraLine("Press <<Q>> to open the configuration GUI.");
			ret.AddExtraLine("<!Currently unable to hook itself into the roll-controls.!> Because it is not an Aileron!");
			return ret;
		}
		/// <summary>
		/// Es wurde auf diesem Block 'Q' gedrückt. Dies wird für das Öffnen der GUI benutzt.
		/// </summary>
		/// <param name="T">ignoriert</param>
		public override void Secondary(Transform T) {
			new GenericBlockGUI().ActivateGui(this);
		}
		/// <summary>
		/// Der Mauszeiger ist nun über die Schaltfläche für das Auswählen des Blocks.
		/// </summary>
		/// <returns>Informationen über den Block, damit der Nutzer entscheiden, ob er ihn jetzt braucht oder nicht.</returns>
		public override BlockTechInfo GetTechInfo() {
			return new BlockTechInfo().
				AddSpec("Maximum total Angle", 45).
				AddStatement("Allows Aerial AIs to take control of special Hydrofoils, which have also limited angles. This provides greater control than ACBs.").
				AddStatement("Currently is unable to hook itself into the roll-controls. Because is not an Aileron!");
		}
		/// <summary>
		/// Erstellt eine GUI, mit dem der Nutzer interagieren kann.
		/// </summary>
		/// <returns>Wahr, wenn die GUI geschlossen werden soll, falsch wenn nicht.</returns>
		public override bool ExtraGUI() {
			GUILayout.BeginArea(new Rect(0,0,1280,800),"Angle Settings",GUI.skin.window);
			GUISliders.DecimalPlaces = 1;
			GUISliders.TotalWidthOfWindow = 1280;
			GUISliders.TextWidth = 40;
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
			#endregion
			#region Vorlagen
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Reset")) {
				topPitch = topRoll = topYaw = 0f;
				bottomPitch = bottomRoll = bottomYaw = 0f;
				leftPitch = leftRoll = leftYaw = 0f;
				rightPitch = rightRoll = rightYaw = 0f;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Front Pitch control")) {
				leftPitch = rightPitch = 45f;
				leftRoll = rightRoll = leftYaw = rightYaw = 0f;
			}
			if(GUILayout.Button("Rear Pitch control")) {
				leftPitch = rightPitch = -45f;
				leftRoll = rightRoll = leftYaw = rightYaw = 0f;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Front Yaw control")) {
				topYaw = bottomYaw = 45f;
				topPitch = topRoll = bottomPitch = bottomRoll = 0f;
			}
			if(GUILayout.Button("Rear Yaw control")) {
				topYaw = bottomYaw = -45f;
				topPitch = topRoll = bottomPitch = bottomRoll = 0f;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Roll control(currently not functioning...)")) {
				topRoll = leftRoll = 45f;
				bottomRoll = rightRoll = -45f;
				topPitch = topYaw = bottomPitch = bottomYaw = leftPitch = leftYaw = rightPitch = rightYaw = 0f;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			#endregion
			bool ret = false;
			if(ret = GuiCommon.DisplayCloseButton(1280)) {
				GUISoundManager.GetSingleton().PlayBeep();
			}
			GUILayout.EndArea();
			return ret;
		}
		private float allowance(float angle1, float angle2) {
			return 45f - (Mathf.Abs(angle1) + Mathf.Abs(angle2));
		}
		/// <summary>
		/// Die Methode <see cref="ExtraGUI"/> hat den Wert "wahr" zurückgegeben und die GUI wurde gschlossen. Diese Methode wird nun aufgerufen.
		/// </summary>
		public override void ExtraGUIClosed() {
			base.ExtraGUIClosed();
			StuffChangedSyncIt();
		}
		#endregion
		#region Netzwerksynchronisation
		public override void StuffChangedSyncIt() {
			base.StuffChangedSyncIt();
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
		#endregion
		#region Serialisierung und Deserialisierung
		/// <summary>
		/// Der HydrofoilController wurde aus einer .blueprint-Datei eingelesen. Alle wichtigen Parameter sind im <see cref="ExtraInfoArrayReadPackage"/> enthalten.
		/// </summary>
		/// <param name="v"></param>
		public override void SetExtraInfo(ExtraInfoArrayReadPackage v) {
			if(v.FindDelimiterAndSpoolToIt(DelimiterType.FirstTier)) {
				int elements = v.ElementsToDelimiterIfThereIsOneOrEndOfArrayIfNot(DelimiterType.FirstTier);
				if(elements >= 12) {
					topPitch = v.GetNextFloat();
					topYaw = v.GetNextFloat();
					topRoll = v.GetNextFloat();
					bottomPitch = v.GetNextFloat();
					bottomYaw = v.GetNextFloat();
					bottomRoll = v.GetNextFloat();
					leftPitch = v.GetNextFloat();
					leftYaw = v.GetNextFloat();
					leftRoll = v.GetNextFloat();
					rightPitch = v.GetNextFloat();
					rightYaw = v.GetNextFloat();
					rightRoll = v.GetNextFloat();
				}
			}
		}
		/// <summary>
		/// Der HydrofoilController wird nun in einer .blueprint-Datei gespeichert. Alle wichtigen Parameter sollten im <see cref="ExtraInfoArrayWritePackage"/> geschrieben werden.
		/// </summary>
		/// <param name="v"></param>
		public override void GetExtraInfo(ExtraInfoArrayWritePackage v) {
			v.AddDelimiterOpen(DelimiterType.FirstTier);
			v.WriteNextFloat(topPitch);
			v.WriteNextFloat(topYaw);
			v.WriteNextFloat(topRoll);
			v.WriteNextFloat(bottomPitch);
			v.WriteNextFloat(bottomYaw);
			v.WriteNextFloat(bottomRoll);
			v.WriteNextFloat(leftPitch);
			v.WriteNextFloat(leftYaw);
			v.WriteNextFloat(leftRoll);
			v.WriteNextFloat(rightPitch);
			v.WriteNextFloat(rightYaw);
			v.WriteNextFloat(rightRoll);
			v.AddDelimiterClose(DelimiterType.FirstTier);
		}
		#endregion
	}
}