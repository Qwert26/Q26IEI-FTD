namespace IndustrialEnterpriseUpgrade.Defense.EllipsoidShield
{
	public class ESGenerator : Block, IGoverningBlock<ESNode>
	{
		public ESNode Node { get; set; }
		public INode NodeInterface => Node;
	}
}