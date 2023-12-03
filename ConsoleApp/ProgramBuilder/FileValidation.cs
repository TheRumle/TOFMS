namespace ConsoleApp.ProgramBuilder;

public class FileValidation
{
    
    public static bool IsValidFilePath(string filePath)
    {
        try
        {
            // Check if the file path is rooted
            if (!Path.IsPathRooted(filePath))
            {
                return false;
            }

            // Check if the file name is valid
            string fileName = Path.GetFileName(filePath);
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return false;
            }

            // Check if the directory path is valid
            string directoryPath = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directoryPath) || directoryPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }

            // Additional checks can be added as needed

            // If all checks pass, the file path is considered valid
            return true;
        }
        catch (Exception)
        {
            // An exception occurred, indicating the file path is not valid
            return false;
        }
    }



}