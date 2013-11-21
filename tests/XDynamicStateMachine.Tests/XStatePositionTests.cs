using System;
using Xunit;

namespace XDynamicStateMachine.Tests
{
    public class XStatePositionTests
    {
        [Fact]
        public void Cannot_create_state_position_with_null_state()
        {
            XStatePosition state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition(null, "REQUESTER", null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_empty_state()
        {
            XStatePosition state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition("", "REQUESTER", null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_actor()
        {
            XStatePosition state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition("APPROVE", null, null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_empty_actor()
        {
            XStatePosition state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition("APPROVE", null, null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_null_action()
        {
            XStatePosition state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition("UNKNOWN", "REQUESTER", null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Cannot_create_state_position_with_empty_action()
        {
            XStatePosition state = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                state = new XStatePosition("UNKNOWN", "REQUESTER", null);
            });

            Assert.Null(state);
        }

        [Fact]
        public void Can_campare_state_position_with_equal_value()
        {
            var stateBuy1 = new XStatePosition("unknown", "buyer", "buynow");
            var stateBuy2 = new XStatePosition("unknown", "buyer", "buynow");

            Assert.Equal(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_value()
        {
            var stateBuy1 = new XStatePosition("unknown", "buyer", "buynow");
            XStatePosition stateBuy2 = null;

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_actor()
        {
            var stateBuy1 = new XStatePosition("unknown", "buyer1", "buynow");
            var stateBuy2 = new XStatePosition("unknown", "buyer2", "buynow");

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_action()
        {
            var stateBuy1 = new XStatePosition("unknown", "buyer", "buynow10");
            var stateBuy2 = new XStatePosition("unknown", "buyer", "buynow100");

            Assert.NotEqual(stateBuy1, stateBuy2);
        }

        [Fact]
        public void Can_campare_state_position_with_inequal_state()
        {
            var stateBuy1 = new XStatePosition("unknown", "buyer", "buynow");
            var stateBuy2 = new XStatePosition("new", "buyer", "buynow");

            Assert.NotEqual(stateBuy1, stateBuy2);
        }
    }
}
