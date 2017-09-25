using UnityEngine;
using System;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class HydrofoilController : Block, IGoverningBlock<HydrofoilNode> {
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
		#endregion
		private static HydrofoilNodeSet Construct(MainConstruct mc) {
			return new HydrofoilNodeSet(mc);
		}
		public HydrofoilNode Node {
			get;
			set;
		}
		public override void StateChanged(IBlockStateChange change) {
			if(change == null) {
				return;
			}
			base.StateChanged(change);
			if(change.IsAvailableToConstruct) {
				//GetOrConstruct fügt das NodeSet automatisch hinzu!
				MainConstruct.iNodeSets.DictionaryOfAllSets.GetOrConstruct(MainConstruct as global::MainConstruct,Construct).AddSender(this);
				//MainConstruct.iControls.AileronStore.Add(this); Leider aktuell nicht möglich und das erstellen einer Mock-Up-Klasse, die Aileron erweitert, ist auch nicht möglich, da die nötigen Methoden nicht überschrieben werden können.
				//MainConstruct.iControls.AirElevatorStore.Add(this);
				//MainConstruct.iControls.AirRudderStore.Add(this);

				//Steuereingaben finden im FixedUpdate statt, wir müssen für unser Update dahinter sein.
				MainConstruct.iScheduler.RegisterForFixedUpdateTwo(new Action<float>(FixedUpdate));
			}
			if(change.IsLostToConstructOrConstructLost) {
				//GetOrConstruct fügt das NodeSet automatisch hinzu!
				MainConstruct.iNodeSets.DictionaryOfAllSets.GetOrConstruct(MainConstruct as global::MainConstruct,Construct).RemoveSender(this);
				//MainConstruct.iControls.AileronStore.Remove(this); Leider aktuell nicht möglich und das erstellen einer Mock-Up-Klasse, die Aileron erweitert, ist auch nicht möglich, da die nötigen Methoden nicht überschrieben werden können.
				//MainConstruct.iControls.AirElevatorStore.Remove(this);
				//MainConstruct.iControls.AirRudderStore.Remove(this);

				//Steuereingaben finden im FixedUpdate statt, wir müssen für unser Update dahinter sein.
				MainConstruct.iScheduler.UnregisterForFixedUpdateTwo(new Action<float>(FixedUpdate));
			}
		}
		public void FixedUpdate(float deltaTime) {
			{//Wir verwenden dieselbe Funktionsweise wie die Klasse "ControlBlock", auch bekannt als ACB.
				yaw = MainConstruct.iControls.Last.Max(ControlType.Right);
				if(yaw == 0) {
					yaw = -MainConstruct.iControls.Last.Max(ControlType.Left);
				}
				pitch = MainConstruct.iControls.Last.Max(ControlType.Up);
				if(pitch == 0) {
					pitch = -MainConstruct.iControls.Last.Max(ControlType.Down);
				}
				roll = MainConstruct.iControls.Last.Max(ControlType.RollRight);
				if(roll == 0) {
					roll = -MainConstruct.iControls.Last.Max(ControlType.RollLeft);
				}
				//HUD.InfoStore.Add(new InfoSnippet("Yaw,Pitch,Roll ist aktuell "+yaw+","+pitch+","+roll+".",1));
			}
			{//Berechne alle nötigen Winkel und setze die Aktuatoren.
				float angle = 0;
				angle = topPitch * pitch + topRoll * roll + topYaw * yaw;
				//HUD.InfoStore.Add(new InfoSnippet("Winkel für oben ist "+angle+"."));
				foreach(HydrofoilActuator actuator in Node.TopActuators) {
					actuator.SetAngle(angle);
				}
				angle = bottomPitch * pitch + bottomRoll * roll + bottomYaw * yaw;
				//HUD.InfoStore.Add(new InfoSnippet("Winkel für unten ist "+angle+"."));
				foreach(HydrofoilActuator actuator in Node.BottomActuators) {
					actuator.SetAngle(angle);
				}
				angle = leftPitch * pitch + leftRoll * roll + leftYaw * yaw;
				//HUD.InfoStore.Add(new InfoSnippet("Winkel für links ist "+angle+"."));
				foreach(HydrofoilActuator actuator in Node.LeftActuators) {
					actuator.SetAngle(angle);
				}
				angle = rightPitch * pitch + rightRoll * roll + rightYaw * yaw;
				//HUD.InfoStore.Add(new InfoSnippet("Winkel für rechts ist " + angle + "."));
				foreach(HydrofoilActuator actuator in Node.RightActuators) {
					actuator.SetAngle(angle);
				}
			}
		}
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
		/// <returns>Informationen über den Block, damit der Nutzer entscheiden kann, ob er ihn jetzt braucht oder nicht.</returns>
		public override BlockTechInfo GetTechInfo() {
			return new BlockTechInfo().
				AddSpec("Maximum total Angle", 45).
				AddStatement("Allows AIs to take control of special Hydrofoils, which have also limited angles. This provides greater control than ACBs.");
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
			float allow;
			#region Einstellung für oben
			GUILayout.BeginHorizontal();
			allow = Allowance(topYaw, topRoll);
			topPitch=GUISliders.LayoutDisplaySlider("T-P",topPitch,-allow,allow,enumMinMax.none,new ToolTip("The maximum absolute angle for Hydrofoils on the top when only a pitch up or down is issued."));
			allow = Allowance(topPitch, topRoll);
			topYaw = GUISliders.LayoutDisplaySlider("T-Y", topYaw, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the top when only a yaw left or right is issued."));
			allow = Allowance(topPitch, topYaw);
			topRoll = GUISliders.LayoutDisplaySlider("T-R", topRoll, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the top when only a roll left or right is issued."));
			GUILayout.EndHorizontal();
			#endregion
			#region Einstellung für unten
			GUILayout.BeginHorizontal();
			allow = Allowance(bottomYaw, bottomRoll);
			bottomPitch = GUISliders.LayoutDisplaySlider("B-P", bottomPitch, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the bottom when only a pitch up or down is issued."));
			allow = Allowance(bottomPitch, bottomRoll);
			bottomYaw = GUISliders.LayoutDisplaySlider("B-Y", bottomYaw, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the bottom when only a yaw left or right is issued."));
			allow = Allowance(bottomPitch, bottomYaw);
			bottomRoll = GUISliders.LayoutDisplaySlider("B-R", bottomRoll, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the bottom when only a roll left or right is issued."));
			GUILayout.EndHorizontal();
			#endregion
			#region Einstellung für links
			GUILayout.BeginHorizontal();
			allow = Allowance(leftYaw, leftRoll);
			leftPitch = GUISliders.LayoutDisplaySlider("L-P", leftPitch, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the left when only a pitch up or down is issued."));
			allow = Allowance(leftPitch, leftRoll);
			leftYaw = GUISliders.LayoutDisplaySlider("L-Y", leftYaw, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the left when only a yaw left or right is issued."));
			allow = Allowance(leftPitch, leftYaw);
			leftRoll = GUISliders.LayoutDisplaySlider("L-R", leftRoll, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the left when only a roll left or right is issued."));
			GUILayout.EndHorizontal();
			#endregion
			#region Einstellung für rechts
			GUILayout.BeginHorizontal();
			allow = Allowance(rightYaw, rightRoll);
			rightPitch = GUISliders.LayoutDisplaySlider("R-P", rightPitch, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the right when only a pitch up or down is issued."));
			allow = Allowance(rightPitch, rightRoll);
			rightYaw = GUISliders.LayoutDisplaySlider("R-Y", rightYaw, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the right when only a yaw left or right is issued."));
			allow = Allowance(rightPitch, rightYaw);
			rightRoll = GUISliders.LayoutDisplaySlider("R-R", rightRoll, -allow, allow, enumMinMax.none, new ToolTip("The maximum absolute angle for Hydrofoils on the right when only a roll left or right is issued."));
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
			if(GUILayout.Button("Roll control")) {
				topRoll = leftRoll = 45f;
				bottomRoll = rightRoll = -45f;
				topPitch = topYaw = bottomPitch = bottomYaw = leftPitch = leftYaw = rightPitch = rightYaw = 0f;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			#endregion
			bool ret = false;
			if(GuiCommon.DisplayCloseButton(1280)) {
				ret = true;
				GUISoundManager.GetSingleton().PlayBeep();
			}
			GUILayout.EndArea();
			return ret;
		}
		public static float Allowance(float angle1, float angle2) {
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
		public override void SyncroniseUpdate(float tp,float ty,float tr,float bp,float by,float br,float lp,float ly,float lr,float rp,float ry,float rr,float ignored) {
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
			if(v == null) {
				return;
			}
			base.SetExtraInfo(v);
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
			base.GetExtraInfo(v);
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
		public override void LoadWithoutState() {
			base.LoadWithoutState();
			topPitch = topRoll = topYaw = 0;
			bottomPitch = bottomRoll = bottomYaw = 0;
			leftPitch = leftRoll = leftYaw = 0;
			rightPitch = rightRoll = rightYaw = 0;
		}
		#endregion
	}
}