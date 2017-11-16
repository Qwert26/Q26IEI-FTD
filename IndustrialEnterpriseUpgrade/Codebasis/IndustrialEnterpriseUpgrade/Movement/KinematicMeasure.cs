using UnityEngine;
using System;
namespace IndustrialEnterpriseUpgrade.Movement {
	public class KinematicMeasure : Block {
		private static string[] NAMES = new string[] {"velocity","acceleration","jerk","jounce"};
		private const string INFINITY = "Unlimited";
		private Vector3[] lastLinearMeasurements=new Vector3[NAMES.Length];
		private Vector3[] lastAngularMeasurements = new Vector3[NAMES.Length];
		private float[] linearLimits = new float[NAMES.Length];
		private float[] angularLimits = new float[NAMES.Length];
		private Vector2 scrollPosition;
		public override void StateChanged(IBlockStateChange change) {
			base.StateChanged(change);
			if (change.IsAvailableToConstruct) {
				GetConstructableOrSubConstructable().iScheduler.RegisterForFixedUpdate(new Action<float>(Measure));
			} else if (change.IsLostToConstructOrConstructLost) {
				GetConstructableOrSubConstructable().iScheduler.UnregisterForFixedUpdate(new Action<float>(Measure));
			}
		}
		private void Measure(float deltaTime) {
			Vector3[] currentLinearMeasurements = new Vector3[NAMES.Length];
			currentLinearMeasurements[0] = MainConstruct.iPhysics.iVelocities.VelocityVector;
			Vector3[] currentAngularMeasurements = new Vector3[NAMES.Length];
			currentAngularMeasurements[0] = MainConstruct.iPhysics.iVelocities.AngularVelocity;
			for (int i=1;i<NAMES.Length;i++) {
				currentLinearMeasurements[i] = (currentLinearMeasurements[i - 1] - lastLinearMeasurements[i - 1]) / deltaTime;
				currentAngularMeasurements[i] = (currentAngularMeasurements[i - 1] - lastAngularMeasurements[i - 1]) / deltaTime;
			}
			lastLinearMeasurements = currentLinearMeasurements;
			lastAngularMeasurements = currentAngularMeasurements;
		}
		#region Nutzerinteraktion
		public override InteractionReturn Secondary() {
			InteractionReturn ret = new InteractionReturn() {
				SpecialNameField = "Kinematic Measurement Block",
				SpecialBasicDescriptionField="Measures kinematic vectors."
			};
			ret.AddExtraLine("Press <<Q>> to open the configuration GUI.");
			for (int i = 0; i < NAMES.Length; i++) {
				string line;
				if (linearLimits[i] * linearLimits[i] <= lastLinearMeasurements[i].sqrMagnitude) {
					line = "Current <!linear " + NAMES[i] + " is " + lastLinearMeasurements[i] + "!>";
				} else {
					line = "Current linear " + NAMES[i] + " is " + lastLinearMeasurements[i];
				}
				line += ", while ";
				if (angularLimits[i] * angularLimits[i] <= lastAngularMeasurements[i].sqrMagnitude) {
					line += "<!angular " + NAMES[i] + " is " + lastAngularMeasurements[i] + "!>.";
				} else {
					line += "angular " + NAMES[i] + " is " + lastAngularMeasurements[i] + ".";
				}
				ret.AddExtraLine(line);
			}
			return ret;
		}
		public override BlockTechInfo GetTechInfo() {
			return new BlockTechInfo().AddStatement("Measures current linear velocity, acceleration, jerk and jounce vectors.").
				AddStatement("Also measures the angular variations of those.");
		}
		public override void Secondary(Transform T){
			new GenericBlockGUI().ActivateGui(this);
		}
		public override bool ExtraGUI() {
			GUILayout.BeginArea(new Rect(0,0,320,800),"Limit settings",GUI.skin.window);
			scrollPosition=GUILayout.BeginScrollView(scrollPosition,true,true);
			for (int i = 0; i < NAMES.Length; i++) {
				GUILayout.BeginHorizontal();
				GUILayout.Box("Limit for linear " + NAMES[i] + ":");
				GUILayout.Box("Limit for angular " + NAMES[i] + ":");
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				string input;
				if (float.IsInfinity(linearLimits[i])) {
					input = GUILayout.TextField(INFINITY);
				} else {
					input = GUILayout.TextField(linearLimits[i].ToString());
				}
				if (!float.TryParse(input, out linearLimits[i])) {
					linearLimits[i] = float.PositiveInfinity;
				}

				if (float.IsInfinity(angularLimits[i])) {
					input = GUILayout.TextField(INFINITY);
				} else {
					input = GUILayout.TextField(angularLimits[i].ToString());
				}
				if (!float.TryParse(input, out angularLimits[i])) {
					angularLimits[i] = float.PositiveInfinity;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
			bool ret = false;
			if (GuiCommon.DisplayCloseButton(320)) {
				ret = true;
				GUISoundManager.GetSingleton().PlayBeep();
			}
			GUILayout.EndArea();
			return ret;
		}
		public override void ExtraGUIClosed() {
			base.ExtraGUIClosed();
		}
		#endregion
	}
}