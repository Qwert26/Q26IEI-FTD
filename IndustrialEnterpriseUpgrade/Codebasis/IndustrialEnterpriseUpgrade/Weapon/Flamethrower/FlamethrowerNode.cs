public class FlamethrowerNode : GovernedNode<FlamethrowerFiringPiece> {
	public const float DAMAGE_PER_FUEL = 10f;
	public ConsumablesModule fuel = new ConsumablesModule();
	public int Regulators { get; set; }
	public int Nozzles { get; set; }
	public int Superheaters { get; set; }
	public bool Ignitor = false;
	//Abgeleitete Eigenschaften
	public float FiringArc { get; private set; }
	public float FlameSpeed { get; private set; }
	public float Damage { get; private set; }
	public float FuelUsage { get; private set; }
	public FlamethrowerNode(AllConstruct c, int maxComponentCount, FlamethrowerFiringPiece B) : base(c, maxComponentCount, B) {
		fuel.LP = B.LocalPosition;
		fuel.GrowthPeriod = 1f;
		fuel.GrowthPerPeriodPerContainer = 1f;
		c.iFloatMultiplayerSyncroniser.Add(fuel);
	}
	protected override void OnNodeDestroy() {
		C.iFloatMultiplayerSyncroniser.Remove(fuel);
	}
	public override void PriorToSendingOutFeelers() {
		base.PriorToSendingOutFeelers();
		fuel.ClearContainerList();
		Regulators = 0;
		Nozzles = 0;
		Superheaters = 0;
		Ignitor = false;
	}
	public override void AfterSendingOutFeelers() {
		base.AfterSendingOutFeelers();
		FiringArc = 45f / (1 + Nozzles);
		FlameSpeed = StaticMaths.BiAM(0f, Regulators, 25f, 0.9f) + Nozzles * 5f;
		FuelUsage = StaticMaths.BiAM(0f, Regulators, 100f, 0.9f) * (1 + 0.15f * Superheaters);
		Damage = FuelUsage / FiringArc * DAMAGE_PER_FUEL * (1 + 0.1f * Superheaters);
	}
}