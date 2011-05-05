using NUnit.Framework;

namespace Castle.Services.Transaction.Tests
{
	public class TransactionManager_IgnoreDependents
	{
		[Test, Ignore("TODO this test")]
		public void If_Parent_Completes_BeforeChild_ButChildThrows_NoFinalizerShouldThrowException_FromTask()
		{
			// see the multi-threaded tests on AutoTx project
		}
	}
}