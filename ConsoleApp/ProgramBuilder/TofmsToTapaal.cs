namespace ConsoleApp.ProgramBuilder;

public class TofmsToTapaal
{
    public static ParseAndValidateTofmSystem GetInOutFiles()
    {
        Console.Write("Path to TOFMS system: ");
        string? inputPath = null;
        do
        {
            try
            {
                inputPath = Path.GetFullPath(Console.ReadLine());
            }
            catch (Exception _ignore)
            {
                Console.WriteLine("Not a valid path");
                Console.WriteLine("");
            }
        } while (String.IsNullOrWhiteSpace(inputPath) );
        string? outputPath= null;
        Console.Write("Where should I place Tapaal file? ");
        do
        {
            try
            {
                var p = Console.ReadLine()!;
                if (FileValidation.IsValidFilePath(p))
                    outputPath = p;
                else
                {
                    Console.WriteLine(p + " was not a valid path to write file");
                }
            }
            catch (Exception _ignore)
            {
                Console.WriteLine("Not a valid path");
            }

        } while (String.IsNullOrWhiteSpace(outputPath));
        
        string text = File.ReadAllText(inputPath);
        return new ParseAndValidateTofmSystem(inputPath, outputPath, text);
    }
}