using System;
using Domination.EventsSystem;
using NUnit.Framework;
using UnityEngine.TestTools;


public class EventsAggregatorTests
{
    class TestMessage : IMessage { }


    private EventsAggregator aggregator;


    [SetUp]
    public void SetUp()
    {
        aggregator = new EventsAggregator();
    }

    #region SingleAggregator
    [Test]
    public void EventAggregatorSubscribing()
    {
        bool wasActionTriggered = false;

        Action<IMessage> action = m => wasActionTriggered = true;
        aggregator.Subscribe(typeof(TestMessage), action);
        aggregator.TriggerEvent(new TestMessage());

        Assert.IsTrue(wasActionTriggered);
    }

    [Test]
    public void EventAggregatorUnsubscribing()
    {
        bool wasActionTriggered = false;

        Action<IMessage> action = m => wasActionTriggered = true;
        aggregator.Subscribe(typeof(TestMessage), action);
        aggregator.Unsubscribe(typeof(TestMessage), action);
        aggregator.TriggerEvent(new TestMessage());

        Assert.IsFalse(wasActionTriggered);
    }

    [Test]
    public void EventAggregatorShutdown()
    {
        LogAssert.ignoreFailingMessages = true;
        bool wasActionTriggered = false;

        Action<IMessage> action = m => wasActionTriggered = true;
        aggregator.Subscribe(typeof(TestMessage), action);
        aggregator.ShutDown();
        aggregator.TriggerEvent(new TestMessage());

        Assert.IsFalse(wasActionTriggered);
    }
    #endregion

    #region MultipleAggregatorsEventSending
    [Test]
    public void SubscribeToChildAndSendEventUsingParentAggregator()
    {
        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        childAggregator.Subscribe(typeof(TestMessage), action);
        aggregator.TriggerEvent(new TestMessage());

        Assert.IsTrue(wasActionTriggered);
    }

    [Test]
    public void SubscribeToChildAndSendEventUsingChildAggregator()
    {
        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        childAggregator.Subscribe(typeof(TestMessage), action);
        childAggregator.TriggerEvent(new TestMessage());

        Assert.IsTrue(wasActionTriggered);
    }

    [Test]
    public void SubscribeToParentAndSendEventUsingChildAggregator()
    {
        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        aggregator.Subscribe(typeof(TestMessage), action);
        childAggregator.TriggerEvent(new TestMessage());

        Assert.IsTrue(wasActionTriggered);
    }
    #endregion

    #region MultipleAggregatorsShutdownTest
    [Test]
    public void SubscribeToParentShutDownChildTriggerParent()
    {
        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        aggregator.Subscribe(typeof(TestMessage), action);
        childAggregator.ShutDown();
        aggregator.TriggerEvent(new TestMessage());

        Assert.IsTrue(wasActionTriggered);
    }

    [Test]
    public void SubscribeToParentShutDownChildTriggerChild()
    {
        LogAssert.ignoreFailingMessages = true;

        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        aggregator.Subscribe(typeof(TestMessage), action);
        childAggregator.ShutDown();
        childAggregator.TriggerEvent(new TestMessage());

        Assert.IsFalse(wasActionTriggered);
    }

    [Test]
    public void SubscribeToChildShutDownChildTriggerParent()
    {
        LogAssert.ignoreFailingMessages = true;

        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        childAggregator.Subscribe(typeof(TestMessage), action);
        childAggregator.ShutDown();
        aggregator.TriggerEvent(new TestMessage());

        Assert.IsFalse(wasActionTriggered);
    }

    [Test]
    public void SubscribeToChildShutDownChildTriggerChild()
    {
        LogAssert.ignoreFailingMessages = true;

        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        childAggregator.Subscribe(typeof(TestMessage), action);
        childAggregator.ShutDown();
        childAggregator.TriggerEvent(new TestMessage());

        Assert.IsFalse(wasActionTriggered);
    }

    [Test]
    public void SubscribeToChildShutDownParentTriggerParent()
    {
        LogAssert.ignoreFailingMessages = true;

        bool wasActionTriggered = false;

        var childAggregator = new EventsAggregator(aggregator);

        Action<IMessage> action = m => wasActionTriggered = true;
        childAggregator.Subscribe(typeof(TestMessage), action);
        aggregator.ShutDown();
        aggregator.TriggerEvent(new TestMessage());

        Assert.IsFalse(wasActionTriggered);
    }
    #endregion
}
