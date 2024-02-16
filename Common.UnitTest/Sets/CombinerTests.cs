using System.Text;
using FluentAssertions;

namespace Common.UnitTest.Sets;

public class CombinerTests
{
    
    [Fact]
    void When_Given_4Elements_PartitionedInto3_ShouldHaveSize4()
    {

        var arr = new int[]
        {
            1, 2, 3, 4
        };
        Combiner.CombinationsOfSize(arr,3).Should().HaveCount(4);
    }
    
    [Fact]
    void When_Given_5Elements_PartitionedInto3_ShouldHaveSize10()
    {
        /// the following subsets should be contained
        var res = Combiner.CombinationsOfSize([1, 2, 3, 4, 5], 3);
        
        
        res.Should().HaveCount(10);
    }

    
    [Fact]
    void When_Given_MultipleOfSameElements_DoesNotRepeat()
    {
        var res = Combiner.CombinationsOfSize([1, 1, 1, 2], 2);

        res.Should().HaveCount(1);
    }
    static string PrettyPrint(List<List<int>> listOfLists)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var list in listOfLists)
        {
            sb.Append('[');
            sb.Append(string.Join(",", list.Select(e => e.ToString())));
            sb.Append(']');
        }

        return sb.ToString();
    }
    
}