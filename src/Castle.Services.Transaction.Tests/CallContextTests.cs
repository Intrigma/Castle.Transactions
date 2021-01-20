﻿using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Castle.Services.Transaction.Tests
{
	/// <summary>
	/// <see href="https://www.cazzulino.com/callcontext-netstandard-netcore.html"/>
	/// </summary>
	[TestFixture]
	public sealed class CallContextTests
	{
		[Test]
		public void WhenFlowingData_ThenCanUseContext()
		{
			var d1 = new object();
			var t1 = default(object);
			var t10 = default(object);
			var t11 = default(object);
			var t12 = default(object);
			var t13 = default(object);
			var d2 = new object();
			var t2 = default(object);
			var t20 = default(object);
			var t21 = default(object);
			var t22 = default(object);
			var t23 = default(object);

			Task.WaitAll(
				Task.Run(() =>
				{
					CallContext.SetData("d1", d1);
					new Thread(() => t10 = CallContext.GetData("d1")).Start();
					Task.WaitAll(
						Task.Run(() => t1 = CallContext.GetData("d1"))
							.ContinueWith(t => Task.Run(() => t11 = CallContext.GetData("d1"))),
						Task.Run(() => t12 = CallContext.GetData("d1")),
						Task.Run(() => t13 = CallContext.GetData("d1"))
					);
				}),
				Task.Run(() =>
				{
					CallContext.SetData("d2", d2);
					new Thread(() => t20 = CallContext.GetData("d2")).Start();
					Task.WaitAll(
						Task.Run(() => t2 = CallContext.GetData("d2"))
							.ContinueWith(t => Task.Run(() => t21 = CallContext.GetData("d2"))),
						Task.Run(() => t22 = CallContext.GetData("d2")),
						Task.Run(() => t23 = CallContext.GetData("d2"))
					);
				})
			);

			Assert.AreSame(d1, t1);
			Assert.AreSame(d1, t10);
			Assert.AreSame(d1, t11);
			Assert.AreSame(d1, t12);
			Assert.AreSame(d1, t13);

			Assert.AreSame(d2, t2);
			Assert.AreSame(d2, t20);
			Assert.AreSame(d2, t21);
			Assert.AreSame(d2, t22);
			Assert.AreSame(d2, t23);

			Assert.Null(CallContext.GetData("d1"));
			Assert.Null(CallContext.GetData("d2"));
		}
	}
}