using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class ChatCollectionTests {
		private ChatCollection chat;

		public ChatCollectionTests() {
			chat = new ChatCollection();
		}

		private Chat A() => new() { timestamp = 1, message = "a" };
		private Chat B() => new() { timestamp = 2, message = "b" };
		private Chat C() => new() { timestamp = 3, message = "c" };

		[Fact]
		public void Construct_Empty_NoMessages() {
			chat = new ChatCollection(new());
			Assert.Empty(chat.Chat);
			Assert.Empty(chat.NewMessages);
			Assert.Null(chat.LastChat);
		}

		[Fact]
		public void Construct_TwoMessages_TwoMessages() {
			chat = new ChatCollection(new() { A(), B() });
			AssertHelpers.Equivalent(new() { A(), B() }, chat.Chat);
			Assert.Empty(chat.NewMessages);
			Assert.Equivalent(B(), chat.LastChat);
		}

		[Fact]
		public void Update_Null_NoMessages() {
			chat.Update(null);
			Assert.Empty(chat.Chat);
			Assert.Empty(chat.NewMessages);
			Assert.Null(chat.LastChat);
		}

		[Fact]
		public void Update_Empty_NoMessages() {
			chat.Update(new());
			Assert.Empty(chat.Chat);
			Assert.Empty(chat.NewMessages);
			Assert.Null(chat.LastChat);
		}

		[Fact]
		public void Update_TwoMessages_TwoMessages() {
			chat.Update(new() { A(), B() });
			AssertHelpers.Equivalent(new() { A(), B() }, chat.Chat);
			AssertHelpers.Equivalent(new() { A(), B() }, chat.NewMessages);
			Assert.Equivalent(B(), chat.LastChat);
		}

		[Fact]
		public void Update_TwoMessagesThenNull_TwoMessages() {
			chat.Update(new() { A(), B() });
			chat.Update(null);
			AssertHelpers.Equivalent(new() { A(), B() }, chat.Chat);
			Assert.Empty(chat.NewMessages);
			Assert.Equivalent(B(), chat.LastChat);
		}

		[Fact]
		public void Update_TwoMessagesThenNone_TwoMessages() {
			chat.Update(new() { A(), B() });
			chat.Update(new());
			AssertHelpers.Equivalent(new() { A(), B() }, chat.Chat);
			Assert.Empty(chat.NewMessages);
			Assert.Equivalent(B(), chat.LastChat);
		}

		[Fact]
		public void Update_TwoMessagesThenOne_ThreeMessages() {
			chat.Update(new() { A(), B() });
			chat.Update(new() { C() });
			AssertHelpers.Equivalent(new() { A(), B(), C() }, chat.Chat);
			AssertHelpers.Equivalent(new() { C() }, chat.NewMessages);
			Assert.Equivalent(C(), chat.LastChat);
		}

		[Fact]
		public void Update_TwoMessagesThenSame_TwoMessages() {
			chat.Update(new() { A(), B() });
			chat.Update(new() { A(), B() });
			AssertHelpers.Equivalent(new() { A(), B() }, chat.Chat);
			Assert.Empty(chat.NewMessages);
			Assert.Equivalent(B(), chat.LastChat);
		}
	}
}
