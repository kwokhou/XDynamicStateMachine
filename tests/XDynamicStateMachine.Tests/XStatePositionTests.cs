using System;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit.Sdk;

namespace XDynamicStateMachine.Tests
{
    public abstract class XStatePositionTests<TState, TActor, TAction>
    {
        private Fixture fixture;

        protected XStatePositionTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Cannot_create_state_position_with_null_state()
        {
            XStatePosition<TState, TActor, TAction> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState, TActor, TAction>(default(TState), fixture.Create<TActor>(), default(TAction));
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_actor()
        {
            XStatePosition<TState, TActor, TAction> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState, TActor, TAction>(fixture.Create<TState>(), default(TActor), fixture.Create<TAction>());
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_action()
        {
            XStatePosition<TState, TActor, TAction> state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition<TState, TActor, TAction>(fixture.Create<TState>(), fixture.Create<TActor>(), default(TAction));
            });

            Assert.Null(state);
        }

        [Fact]
        public void Can_campare_state_position_with_equal_value()
        {
            var state1 = fixture.Create<TState>();
            var actor = fixture.Create<TActor>();
            var action = fixture.Create<TAction>();

            var stateBuy1 = new XStatePosition<TState, TActor, TAction>(state1, actor, action);
            var stateBuy2 = new XStatePosition<TState, TActor, TAction>(state1, actor, action);

            Assert.Equal(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_value()
        {
            var stateBuy1 = new XStatePosition<TState, TActor, TAction>(fixture.Create<TState>(),fixture.Create<TActor>(), fixture.Create<TAction>());
            XStatePosition<TState, TActor, TAction> stateBuy2 = null;

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_actor()
        {
            var state1 = fixture.Create<TState>();
            var actor1 = fixture.Create<TActor>();
            var actor2 = fixture.Create<TActor>();
            var action = fixture.Create<TAction>();

            var stateBuy1 = new XStatePosition<TState, TActor, TAction>(state1, actor1, action);
            var stateBuy2 = new XStatePosition<TState, TActor, TAction>(state1, actor2, action);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_action()
        {
            var state1 = fixture.Create<TState>();
            var actor = fixture.Create<TActor>();
            var action1 = fixture.Create<TAction>();
            var action2 = fixture.Create<TAction>();

            var stateBuy1 = new XStatePosition<TState, TActor, TAction>(state1, actor, action1);
            var stateBuy2 = new XStatePosition<TState, TActor, TAction>(state1, actor, action2);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_state()
        {
            var state1 = fixture.Create<TState>();
            var state2 = fixture.Create<TState>();
            var actor = fixture.Create<TActor>();
            var action = fixture.Create<TAction>();

            var stateBuy1 = new XStatePosition<TState, TActor, TAction>(state1,actor,action);
            var stateBuy2 = new XStatePosition<TState, TActor, TAction>(state2,actor,action);

            Assert.NotEqual(stateBuy1, stateBuy2);
        }
    }

    public class StringXStatePositionTests : XStatePositionTests<string, int, char> { }
    public class IntegerXStatePositionTests : XStatePositionTests<int, decimal, double> { }
    public class DecimalXStatePositionTests : XStatePositionTests<decimal, string, int> { }
}
