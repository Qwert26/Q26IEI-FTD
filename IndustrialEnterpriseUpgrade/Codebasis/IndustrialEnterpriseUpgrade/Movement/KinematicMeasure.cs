using UnityEngine;
using System;
namespace IndustrialEnterpriseUpgrade.Movement {
	public class KinematicMeasure : Block {
		private static string[] NAMES = new string[] {"velocity","acceleration","jerk","jounce"};
		private const string INFINITY = "Unlimited";
		private Vector3[] lastMeasurements=new Vector3[NAMES.Length];
		private float[] limits = new float[NAMES.Length];
		private Vector2 scrollPosition;
		public override void BlockStart() {
			base.BlockStart();
			for (int i = 0; i < limits.Length; i++) {
				limits[i] = item.Code.Variables.GetFloat(NAMES[i]+"limit",float.PositiveInfinity);
				if (limits[i] < 0) {
					limits[i] = float.PositiveInfinity;
				}
			}
		}
		public override void StateChanged(IBlockStateChange change) {
			base.StateChanged(change);
			if (change.IsAvailableToConstruct) {
				GetConstructableOrSubConstructable().iScheduler.RegisterForFixedUpdate(new Action<float>(Measure));
			} else if (change.IsLostToConstructOrConstructLost) {
				GetConstructableOrSubConstructable().iScheduler.UnregisterForFixedUpdate(new Action<float>(Measure));
			}
		}
		private void Measure(float deltaTime) {
			Vector3[] currentMeasurements = new Vector3[NAMES.Length];
			currentMeasurements[0] = MainConstruct.iPhysics.iVelocities.VelocityVector;
			for (int i=1;i<NAMES.Length;i++) {
				currentMeasurements[i] = (currentMeasurements[i - 1] - lastMeasurements[i - 1]) / deltaTime;
			}
			lastMeasurements = currentMeasurements;
		}
		#region Nutzerinteraktion
		public override InteractionReturn Secondary() {
			InteractionReturn ret = new InteractionReturn() {
				SpecialNameField = "Kinematic Measurement Block",
				SpecialBasicDescriptionField="Measures kinematic vectors."
			};
			ret.AddExtraLine("Press <<Q>> to open the configuration GUI.");
			for (int i = 0; i < NAMES.Length; i++) {
				if (limits[i] * limits[i] <= lastMeasurements[i].sqrMagnitude) {
					ret.AddExtraLine("<!current " + NAMES[i] + " is " + lastMeasurements[i] + "!>");
				} else {
					ret.AddExtraLine("current "+NAMES[i]+" is "+lastMeasurements[i]);
				}
			}
			return ret;
		}
		public override BlockTechInfo GetTechInfo() {
			return new BlockTechInfo().AddStatement("Measures current velocity, acceleration, jerk and jounce vectors.");
		}
		public override void Secondary(Transform T){
			new GenericBlockGUI().ActivateGui(this);
		}
		public override bool ExtraGUI() {
			GUILayout.BeginArea(new Rect(0,0,320,800),"Limit settings",GUI.skin.window);
			scrollPosition=GUILayout.BeginScrollView(scrollPosition,false,true);
			for (int i = 0; i < NAMES.Length; i++) {
				GUILayout.Box("Limit for "+NAMES[i]+":");
				string input;
				if (float.IsInfinity(limits[i])) {
					input = GUILayout.TextField(INFINITY);
				} else {
					input = GUILayout.TextField(limits[i].ToString());
				}
				if (!float.TryParse(input, out limits[i])) {
					limits[i] = float.PositiveInfinity;
				}
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