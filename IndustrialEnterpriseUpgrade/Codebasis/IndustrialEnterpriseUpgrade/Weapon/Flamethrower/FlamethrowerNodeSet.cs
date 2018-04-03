using System;
public class FlamethrowerNodeSet : GovernedSetManager<FlamethrowerNode, FlamethrowerFeeler, FlamethrowerConnectedTypeInfo, FlamethrowerFiringPiece> {
	public FlamethrowerNodeSet(MainConstruct C) : base(C) {
		HookUpRefreshFunctionToAChangeListener(new Guid("00000000-0000-0000-0000-000000000000"),0.1f);
	}

	public override FlamethrowerFeeler MakeFeeler(FlamethrowerNode node) {
		return new FlamethrowerFeeler(node);
	}

	public override FlamethrowerNode MakeNode(AllConstruct C, FlamethrowerFiringPiece B) {
		return new FlamethrowerNode(C, 1000, B);
	}
}