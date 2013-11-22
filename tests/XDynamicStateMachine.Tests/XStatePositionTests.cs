using System;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit.Sdk;

namespace XDynamicStateMachine.Tests
{
    public abstract class XStatePositionTests<TState, TActor>
    {
        private Fixture fixture;

        protected XStatePositionTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Cannot_create_state_position_with_null_state()
        {
            XStatePosition<TState, TActor> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState, TActor>(default(TState), fixture.Create<TActor>(), null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_actor()
        {
            XStatePosition<TState, TActor> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState, TActor>(fixture.Create<TState>(), default(TActor), fixture.Create<string>());
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_action()
        {
            XStatePosition<TState, TActor> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState, TActor>(fixture.Create<TState>(), fixture.Create<TActor>(), null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Can_campare_state_position_with_equal_value()
        {
            var state1 = fixture.Create<TState>();
            var actor = fixture.Create<TActor>();
            var action = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState, TActor>(state1, actor, action);
            var stateBuy2 = new XStatePosition<TState, TActor>(state1, actor, action);

            Assert.Equal(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_value()
        {
            var stateBuy1 = new XStatePosition<TState, TActor>(fixture.Create<TState>(),fixture.Create<TActor>(), "buynow");
            XStatePosition<TState, TActor> stateBuy2 = null;

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_actor()
        {
            var state1 = fixture.Create<TState>();
            var actor1 = fixture.Create<TActor>();
            var actor2 = fixture.Create<TActor>();
            var action = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState, TActor>(state1, actor1, action);
            var stateBuy2 = new XStatePosition<TState, TActor>(state1, actor2, action);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_action()
        {
            var state1 = fixture.Create<TState>();
            var actor = fixture.Create<TActor>();
            var action1 = fixture.Create<string>();
            var action2 = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState, TActor>(state1, actor, action1);
            var stateBuy2 = new XStatePosition<TState, TActor>(state1, actor, action2);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_state()
        {
            var state1 = fixture.Create<TState>();
            var state2 = fixture.Create<TState>();
            var actor = fixture.Create<TActor>();
            var action = fixture.Create<string>();

            var stateBuy1 = new XStatePosition<TState, TActor>(state1,actor,action);
            var stateBuy2 = new XStatePosition<TState, TActor>(state2,actor,action);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }
    }

    public class StringXStatePositionTests : XStatePositionTests<string, int> { }
    public class IntegerXStatePositionTests : XStatePositionTests<int, decimal> { }
    public class DecimalXStatePositionTests : XStatePositionTests<decimal, string> { }
}
