public abstract class FlamethrowerComponent : BlockComponent<FlamethrowerNode, FlamethrowerFeeler, FlamethrowerConnectedTypeInfo> {
	public override InteractionReturn Secondary() {
		if (Node == null) {
			return new InteractionReturn("<!Not Connected.!> Connect to Firing Piece!");
		} else {
			return Node.GoverningBlock.Secondary().AddExtraLineAtTop("<<Connected to a Firing Piece with the following Stats:>>");
		}
	}
}