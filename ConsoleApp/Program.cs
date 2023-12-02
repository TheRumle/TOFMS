// See https://aka.ms/new-console-template for more information

using ConsoleApp;


await TofmsToTapaal
    .GetInOutFiles()
    .ParseAndValidateInputSystem()
    .TranslateTofmsToTacpnComponent()
    .TranslateToTapaalXml()
    .WriteToOutputPath();
    