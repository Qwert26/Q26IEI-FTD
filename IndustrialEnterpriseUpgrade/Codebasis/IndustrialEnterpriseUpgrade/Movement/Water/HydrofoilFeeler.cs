namespace IndustrialEnterpriseUpgrade.Movement.Water {
	/// <summary>
	/// Ein Fühler wird benutzt, um Blöcke einer Mehr-Block-Struktur zu finden und um Informationen des sendenen Blocks weiter zu geben.
	/// </summary>
	public class HydrofoilFeeler : GenericFeeler<HydrofoilNode> {
        public enumDirections LocalActuatorOutput { get; set; }
        public HydrofoilFeeler(HydrofoilNode sender) : base(sender) {
			LocalActuatorOutput = enumDirections.unknown;
		}
	}
}