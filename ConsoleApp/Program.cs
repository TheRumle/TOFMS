// See https://aka.ms/new-console-template for more information

using ConsoleApp;


await TofmsToTapaal
    .GetInputFiles()
    .ParseAndValidateInputSystem()
    .TranslateTofmsToTacpnComponent()
    .TranslateToTapaalXml()
    .WriteToOutputPath();
    