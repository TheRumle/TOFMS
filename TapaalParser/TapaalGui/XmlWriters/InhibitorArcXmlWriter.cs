﻿using System.Text;
using TACPN.Net.Arcs;
using TapaalParser.TapaalGui.XmlWriters.Symbols;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

namespace TapaalParser.TapaalGui.XmlWriters;

public class InhibitorArcXmlWriter : IGuiTranslater<InhibitorArc>
{
    private readonly StringBuilder _builder;

    public InhibitorArcXmlWriter(StringBuilder stringBuilder)
    {
        this._builder = stringBuilder;
    }
    
    public InhibitorArcXmlWriter(): this(new StringBuilder())
    {}

    public string XmlString(InhibitorArc arc)
    {
        ArcInfoAppender arcInfoAppender = new(_builder);
        arcInfoAppender.AppendArcInfo(arc);
        _builder.Append(@" <hlinscription> <text>1'dot.all</text> <structure> <numberof> <subterm> <numberconstant value=""1""> <positive/> </numberconstant> </subterm> <subterm> <all> <usersort declaration=""dot""/> </all> </subterm> </numberof> </structure> </hlinscription>");
        arcInfoAppender.EndArc(arc);
        return _builder.ToString();
    }


}