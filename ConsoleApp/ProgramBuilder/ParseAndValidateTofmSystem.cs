using Tmpms.Common;
using Tmpms.Common.Json;
using Tmpms.Common.Json.Validators;
using Xml;

namespace ConsoleApp.ProgramBuilder;

public class ParseAndValidateTofmSystem
{
    private readonly TmpmsJsonInputParser _parser;
    public readonly string System;
    public readonly string InputFile;
    public readonly string OutputFile;

    public ParseAndValidateTofmSystem(string inputFile, string outputFile, string tofmSystem)
    {
        this.System = tofmSystem;
        this.InputFile = inputFile;
        this.OutputFile = outputFile;
        this._parser = new TmpmsJsonInputParser(new TimedMultiPartSystemJsonInputValidator());
    }

    public TranslateToTacpn ParseAndValidateInputSystem()
    {
        TimedMultipartSystem system =  _parser.ParseTofmsSystemJsonString(System).Result;
        return new (system, system.MoveActions, system.Journeys, this);
    }
    
    public WriteToFile DirectTranslate()
    {
        TimedMultipartSystem system =  _parser.ParseTofmsSystemJsonString(System).Result;
        TmpmsParser tmpmsParser = new TmpmsParser(system);
        var a = tmpmsParser.Parse();
        return new WriteToFile(a, OutputFile);
    }
}