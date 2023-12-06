using Tofms.Common;
using Tofms.Common.JsonTofms;
using Tofms.Common.JsonTofms.ConsistencyCheck.Validators;
using Xml;

namespace ConsoleApp.ProgramBuilder;

public class ParseAndValidateTofmSystem
{
    private readonly JsonTofmToDomainTofmParser _parser;
    public readonly string System;
    public readonly string InputFile;
    public readonly string OutputFile;

    public ParseAndValidateTofmSystem(string inputFile, string outputFile, string tofmSystem)
    {
        this.System = tofmSystem;
        this.InputFile = inputFile;
        this.OutputFile = outputFile;
        this._parser = new JsonTofmToDomainTofmParser(TofmSystemValidator.Default());
    }

    public TranslateToTacpn ParseAndValidateInputSystem()
    {
        ValidatedTofmSystem system =  _parser.ParseTofmsSystemJsonString(System).Result;
        return new (system, system.MoveActions, system.Journeys, this);
    }
    
    public WriteToFile DirectTranslate()
    {
        ValidatedTofmSystem system =  _parser.ParseTofmsSystemJsonString(System).Result;
        TofmSystemParser tofmSystemParser = new TofmSystemParser(system);
        var a = tofmSystemParser.Parse();
        return new WriteToFile(a, OutputFile);
    }
}