// See https://aka.ms/new-console-template for more information

using ConsoleApp.ProgramBuilder;


await TofmsToTapaal
    .GetInOutFiles()
    .DirectTranslate()
    .WriteToOutputPath()
    ;

return 0;