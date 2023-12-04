// See https://aka.ms/new-console-template for more information

using ConsoleApp.ProgramBuilder;


var process = await TofmsToTapaal
    .GetInOutFiles()
    .ParseAndValidateInputSystem()
    .TranslateTofmsToTacpnComponents()
    .TranslateToTapaalXml()
    ;

await process.WriteToOutputPath();
return 0;