using System;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit.Sdk;

namespace XDynamicStateMachine.Tests
{
    public abstract class XStatePositionTests<TState>
    {
        private Fixture fixture;

        protected XStatePositionTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Cannot_create_state_position_with_null_state()
        {
            XStatePosition<TState> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState>(default(TState), "REQUESTER", null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_actor()
        {
            XStatePosition<TState> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState>(fixture.Create<TState>(), null, fixture.Create<string>());
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_action()
        {
            XStatePosition<TState> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState>(fixture.Create<TState>(), fixture.Create<string>(), null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Can_campare_state_position_with_equal_value()
        {
            var state1 = fixture.Create<TState>();
            var actor = fixture.Create<string>();
            var action = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState>(state1, actor, action);
            var stateBuy2 = new XStatePosition<TState>(state1, actor, action);

            Assert.Equal(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_value()
        {
            var stateBuy1 = new XStatePosition<TState>(fixture.Create<TState>(), "buyer", "buynow");
            XStatePosition<TState> stateBuy2 = null;

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_actor()
        {
            var state1 = fixture.Create<TState>();
            var actor1 = fixture.Create<string>();
            var actor2 = fixture.Create<string>();
            var action = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState>(state1, actor1, action);
            var stateBuy2 = new XStatePosition<TState>(state1, actor2, action);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_action()
        {
            var state1 = fixture.Create<TState>();
            var actor = fixture.Create<string>();
            var action1 = fixture.Create<string>();
            var action2 = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState>(state1, actor, action1);
            var stateBuy2 = new XStatePosition<TState>(state1, actor, action2);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_state()
        {
            var state1 = fixture.Create<TState>();
            var state2 = fixture.Create<TState>();
            var actor = fixture.Create<string>();
            var action = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState>(state1,actor,action);
            var stateBuy2 = new XStatePosition<TState>(state2,actor,action);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }
    }

    public class StringXStatePositionTests : XStatePositionTests<string> { }
    public class IntegerXStatePositionTests : XStatePositionTests<int> { }
    public class DecimalXStatePositionTests : XStatePositionTests<decimal> { }
}
