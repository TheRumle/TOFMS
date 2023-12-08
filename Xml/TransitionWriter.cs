using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using Tofms.Common.Move;

namespace Xml;

public class TransitionWriter
{
    public StringBuilder StringBuilder { get; }
    public MoveAction MoveAction { get; }

    public TransitionWriter(StringBuilder stringBuilder, MoveAction moveAction)
    {
        StringBuilder = stringBuilder;
        MoveAction = moveAction;
    }

   
  
    public void WriteTransition(JourneyCollection collection)
    {
        var from = MoveAction.From;
        var to = MoveAction.To;
        StringBuilder.Append($@"<transition angle=""0"" displayName=""true"" id=""{MoveAction.Name}"" infiniteServer=""false"" name=""{MoveAction.Name}"" nameOffsetX=""0"" nameOffsetY=""0"" player=""0"" positionX=""50"" positionY=""50"" priority=""0"" urgent=""false""> <condition>");


        var ands = AppendText(collection);
        AppendStructures(ands);
        StringBuilder.Append("</condition> </transition>");
    }

    private void AppendStructures(List<And> ands)
    {
        StringBuilder.Append("<structure>");

        var orStartTags = Enumerable.Repeat(@"<or> <subterm>", ands.Count() - 1).Aggregate((prev,curr)=>  prev+curr);
        StringBuilder.Append(orStartTags);
        StringBuilder.Append(ands.First().ToXmlString());
        StringBuilder.Append("</subterm> <subterm>");
        

        foreach (var and in ands.Skip(1))
        {
            if (and.Equals(ands.Last()))
            {
                StringBuilder.Append(and.ToXmlString());
                StringBuilder.Append("</subterm> </or> ");
            }
            else
            {
                StringBuilder.Append(and.ToXmlString());
                StringBuilder.Append("</subterm> </or> </subterm> <subterm>");
            }
            
           
        }

        StringBuilder.Append(@"</structure>");

    }

    private List<And> AppendText(JourneyCollection collection)
    {
        StringBuilder.Append($@"<text>");
        var andss = CreateOrs(collection);
        var parens = Enumerable.Repeat("(", andss.Count() - 1).Aggregate((prev,curr)=>  prev+curr);
        StringBuilder.Append(parens);
        StringBuilder.Append(andss.First());
        StringBuilder.Append(" or ");
        
        var ands = andss.Skip(1);
        var first = andss.First();
        foreach (var and in ands.Skip(1))
        {
            
            StringBuilder.Append(and);
            StringBuilder.Append(") or ");
        }

        StringBuilder.Append(first + ")");
        StringBuilder.Append("</text>");
        return andss;
    }
    
    

    private List<And> CreateOrs(JourneyCollection collection)
    {
      var ands = new List<And>();

      var longestJourney = collection.Max(e => e.Value.Count());
      
      

      var kvp1 = collection.First();
          var firstPartName = kvp1.Key;
          var firstPartJourney = kvp1.Value;

          foreach (var kvp2 in collection.Skip(1))
          {
              if (kvp2.Key == kvp1.Key) continue;
              var secondPartName = kvp2.Key;
              var secondPartJourney = kvp2.Value;

              foreach (var first in firstPartJourney)
              {
                  foreach (var second in secondPartJourney)
                  {
                      var lhs = new Eq(first.Key, Colours.VariableIdForPart(firstPartName), first.Key,longestJourney);
                      var rhs = new Eq(second.Key, Colours.VariableIdForPart(secondPartName), second.Key,longestJourney);
                      ands.Add(new And(lhs, rhs));
                  }
              }
          }

          return ands;
    }
}