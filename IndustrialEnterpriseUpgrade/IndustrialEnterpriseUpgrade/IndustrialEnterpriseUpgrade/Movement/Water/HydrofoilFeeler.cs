namespace IndustrialEnterpriseUpgrade.Movement.Water {
	/// <summary>
	/// Ein Fühler wird benutzt, um Blöcke einer Mehr-Block-Struktur zu finden und um Informationen des sendenen Blocks weiter zu geben.
	/// </summary>
	public class HydrofoilFeeler : GenericFeeler<HydrofoilNode> {
		private enumDirections localActuatorOutput;
		public enumDirections LocalActuatorOutput {
			get => localActuatorOutput;
			set => localActuatorOutput = value;
		}
		public HydrofoilFeeler(HydrofoilNode sender) : base(sender) {
			localActuatorOutput = enumDirections.unknown;
		}
	}
}