using System;
using System.Collections.Generic;
using FluentAssertions;
using JetBrains.Annotations;
using TmpmsChecker.Algorithm;
using Xunit;

namespace TmpmsScheduler.UnitTest.Algorithm;

[TestSubject(typeof(CostBasedQueue<>))]
public class CostBasedQueueTest
{
    public static IEnumerable<Object[]> TestDataList = [
        [2f, 1f],
        [10f, 1f],
        [100f, 1f],
        [1.0000002f, 1.0000001f],
    ];
    
    [Theory]
    [MemberData(nameof(TestDataList))]
    public void GivesHighestPriorityFirst_WhenAddedHighestFirst(float comesSecond, float comesFirst)
    {
        var list = new CostBasedQueue<float>();
        list.Enqueue(comesFirst,comesFirst);
        list.Enqueue(comesSecond,comesSecond);
        list.Dequeue().Should().Be(comesFirst);
    }
    
    [Theory]
    [MemberData(nameof(TestDataList))]
    public void GivesHighestPriorityFirst_WhenAddedHighestLast(float comesSecond, float comesFirst)
    {
        var list = new CostBasedQueue<float>();
        list.Enqueue(comesSecond,comesSecond);
        list.Enqueue(comesFirst,comesFirst);
        list.Dequeue().Should().Be(comesFirst);
    }

    [Fact]
    public void WhenSameValueIsAdded_DoesNotThrow()
    {
        CostBasedQueue<int> sut = new CostBasedQueue<int>();
        sut.Enqueue(1,1);
        sut.Enqueue(1,1);
    }
}