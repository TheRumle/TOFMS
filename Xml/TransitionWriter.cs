﻿using System.Text;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;

namespace Xml;

public class TransitionWriter
{
    private readonly string transitionName;
    public StringBuilder StringBuilder { get; }
    public MoveAction MoveAction { get; }

    public TransitionWriter(StringBuilder stringBuilder, MoveAction moveAction)
    {
        StringBuilder = stringBuilder;
        MoveAction = moveAction;
        this.transitionName = moveAction.Name;
    }

   
  
    public void WriteTransition(IndexedJourneyCollection collection)
    {
        
        StringBuilder.Append($@"<transition angle=""0"" displayName=""true"" id=""{MoveAction.Name}"" infiniteServer=""false"" name=""{MoveAction.Name}"" nameOffsetX=""0"" nameOffsetY=""0"" player=""0"" positionX=""50"" positionY=""50"" priority=""0"" urgent=""false""> ");
        if (!MoveAction.To.IsProcessing)
        {
            StringBuilder.Append("</transition>");
            return;
        }   

        KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>[] indexesForThisLocation = collection
            .MatchingJourneysFor(MoveAction).ToArray();

        
        this.StringBuilder.Append("<condition>");
        //If we move more than one part type, all parts must have the location as the next journey
        if (indexesForThisLocation.Count() > 1)
        {
            var ands = AppendAndText(indexesForThisLocation);
            AppendAndStructures(ands);
        }
        else
        {
            var part = indexesForThisLocation.First().Key;
            var longest = collection.MaxBy(e => e.Value.Count()).Value.Count();
            List<Eq> ors = AppendOrText(indexesForThisLocation.First().Value, part, longest);
            AppendOrStructures(ors);
        }
        
        StringBuilder.Append("</condition> </transition>");
    }

    private void AppendOrStructures(List<Eq> eqs)
    {
        StringBuilder.Append("<structure>");

        var orStartTags = eqs.Count() > 1
            ? Enumerable.Repeat(@"<or> <subterm>", eqs.Count() - 1).Aggregate((prev, curr) => prev + curr)
            : "";
        StringBuilder.Append(orStartTags);
        StringBuilder.Append(eqs.First().ToXmlTag());
        if (eqs.Count() > 1)
        {
            StringBuilder.Append("</subterm> <subterm>");
            foreach (var eq in eqs.Skip(1))
            {
                if (eq.Equals(eqs.Last()))
                {
                    StringBuilder.Append(eq.ToXmlTag());
                    StringBuilder.Append("</subterm> </or> ");
                }
                else
                {
                    StringBuilder.Append(eq.ToXmlTag());
                    StringBuilder.Append("</subterm> </or> </subterm> <subterm>");
                }
            }
        }
        
        
        StringBuilder.Append(@"</structure>");
    }

    private List<Eq> AppendOrText(IEnumerable<KeyValuePair<int, Location>> journey, string part, int longestJourney)
    {
        StringBuilder.Append($@"<text>");

        var eqs = new List<Eq>();
        foreach (var pair in journey)
            eqs.Add(new Eq(pair.Key, Colours.VariableIdForPart(part), journey.Count(), longestJourney));
        
        
        var startParents = journey.Count() > 1 ? Enumerable.Repeat("(", eqs.Count() -1).Aggregate((prev,curr)=>  prev+curr) : "";
        StringBuilder.Append(startParents);
        
        
        StringBuilder.Append(eqs.First());
        if (journey.Count() > 1)
        {
            StringBuilder.Append(" or ");
            foreach (var eq in eqs.Skip(1))
            {
                if (eq != eqs.Last())
                {
                    StringBuilder.Append(eq);
                    StringBuilder.Append(") or ");
                }
                else
                {
                    StringBuilder.Append(eq + ")");    
                }
                
            }
            
        }
        StringBuilder.Append("</text>");
        return eqs;
    }



    private void AppendAndStructures(List<And> ands)
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

    private List<And> AppendAndText(IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> collection)
    {
        StringBuilder.Append($@"<text>");
        var andss = CreateAnds(collection);
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
    
    

    private List<And> CreateAnds(IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> collection)
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

public static class JourneyCollectionExtension
{
    public static IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> WithPartType (this IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> collection,
        string partType)
    {
        return collection.Where(e => e.Key == partType);
    }
    
    public static IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> WithPartTypes (this IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> collection,
        IEnumerable<string> partTypes)
    {
        return collection.Where(e => partTypes.Contains(e.Key));
    }

    public static IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> MatchingJourneysFor (this IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> collection,
        MoveAction moveAction)
    {
        var a = collection.WithPartTypes(moveAction.PartsToMove.Select(e => e.Key))
            .Select(e =>
            {
                IEnumerable<KeyValuePair<int, Location>> sorted = e.Value.Where(elem => elem.Value.Name == moveAction.To.Name);
                return KeyValuePair.Create(e.Key, sorted);
            });

        return a;
    }
    
}