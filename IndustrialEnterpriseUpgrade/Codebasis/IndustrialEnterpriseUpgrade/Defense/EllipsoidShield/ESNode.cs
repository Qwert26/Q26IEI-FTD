using System;
namespace IndustrialEnterpriseUpgrade.Defense.EllipsoidShield
{
	public class ESNode : GovernedNode<ESGenerator>
	{
		public ESNode(AllConstruct c, int maxComponentCount, ESGenerator B) : base(c, maxComponentCount, B) { }
		protected override void OnNodeDestroy()
		{
			throw new NotImplementedException();
		}
	}
}