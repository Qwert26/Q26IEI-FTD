public class FlamethrowerFiringPiece : ConstructableWeapon, IGoverningBlock<FlamethrowerNode> {
	public FlamethrowerNode Node { get; set; }
	public override enumWeaponType GetWeaponType() {
		return enumWeaponType.cannon;
	}
	public override void StateChanged(IBlockStateChange change) {
		base.StateChanged(change);
		if (change.IsAvailableToConstruct) {
			MainConstruct.iNodeSets.DictionaryOfAllSets.GetOrConstruct(MainConstruct as global::MainConstruct, (mc) => { return new FlamethrowerNodeSet(mc); }).AddSender(this);
		}
		if (change.IsLostToConstructOrConstructLost) {
			MainConstruct.iNodeSets.DictionaryOfAllSets.GetOrConstruct(MainConstruct as global::MainConstruct, (mc) => { return new FlamethrowerNodeSet(mc); }).RemoveSender(this);
		}
	}
}