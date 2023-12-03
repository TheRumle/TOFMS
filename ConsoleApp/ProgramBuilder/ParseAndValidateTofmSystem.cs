using Tofms.Common.JsonTofms;
using Tofms.Common.JsonTofms.ConsistencyCheck.Validators;

namespace ConsoleApp.ProgramBuilder;

public class ParseAndValidateTofmSystem
{
    private readonly JsonTofmToDomainTofmParser _parser;
    private readonly string _system;
    public readonly string InputFile;
    public readonly string OutputFile;

    public ParseAndValidateTofmSystem(string inputFile, string outputFile, string tofmSystem)
    {
        this._system = tofmSystem;
        this.InputFile = inputFile;
        this.OutputFile = outputFile;
        this._parser = new JsonTofmToDomainTofmParser(TofmSystemValidator.Default());
    }

    public TranslateToTacpn ParseAndValidateInputSystem()
    {
        var moveActions =  _parser.ParseTofmsSystemJsonString(_system).Result;
        return new TranslateToTacpn(moveActions, this);
    }
}