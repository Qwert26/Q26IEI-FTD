using System;
using UnityEngine;
namespace IndustrialEnterpriseUpgrade.Movement.Water {
	public class VoithSchneiderComponent : BlockComponent<VoithSchneiderNode, VoithSchneiderFeeler, VoithSchneiderConnectedTypeInfo> {
		protected override int ConnectionType {
			get {
				throw new NotImplementedException();
			}
		}
	}
}
