using System;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class VoithSchneiderEngine : BlockWithPropulsion, IGoverningBlock<VoithSchneiderNode> {
		#region Implementierung von BlockWithPropulsion
		public override float MaxThrust {
			get {
				return 300f;
			}
		}
		public override float PowerUsePerFixedUpdate {
			get {
				return 5f;
			}
		}
		public override float SpaceRequiredBehindPropulsion {
			get {
				return 0f;
			}
		}
		public override PropulsionBlockType Type {
			get {
				return PropulsionBlockType.Water;
			}
		}
		protected override bool CanRunInReverse {
			get {
				return true;
			}
		}
		protected override PowerRequestType PowerRequestType {
			get {
				return PowerRequestType.Propulsion;
			}
		}
		protected override float TopSpeed {
			get {
				return 50f;
			}
		}
		public override string Name {
			get {
				return "Cyclorotor, Water";
			}
		}
		public override void EmissionUpdate(float dt) {

		}
		public override void RunPropulsion() {

		}
		#endregion
		#region Implementierung von IGoverningBlock
		public VoithSchneiderNode Node {
			get;
			set;
		}
		#endregion
	}
}