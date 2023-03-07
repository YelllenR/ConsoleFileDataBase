namespace ConsoleFileDataBase;

public class Program {
    static void Main(string[] args) {
        string path = GetPathFromUser();

        if (PathExists(path) is false) {
            Console.WriteLine("Path doesn't exist, please indicate a correct one or come back later");
            return;
        }

        List<string> paths = GetPaths(path);

        List<string> files = GetFiles(path);

        List<FileData> results = new();
        foreach (string filePath in files) {
            FileData result = GetFileInfos(filePath);
            results.Add(result);
        }

        // Test later !
        Stream memoryFile = PrintFileDatas(results);
        string savePath = GetSavePathFromUser();

        bool success = SaveFilesData(memoryFile, savePath);
    }


    public static string GetPathFromUser() {
        Console.WriteLine("Please give the file path to analyse");
        string? path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path) == false) {
            return path;
        }

        // Recursive function
        Console.WriteLine("Sorry BROOOOOOOO, you haven't put a valid path!!!!");
        string result = GetPathFromUser();
        return result;
    }

    public static bool PathExists(string path) {
        bool doesExist = Directory.Exists(path);
        return doesExist;
    }

    public static List<string> GetPaths(string path) {
        List<string> listOfPaths = new();
        string[] directories = Directory.GetDirectories(path);
        listOfPaths.AddRange(directories);

        return listOfPaths;
    }

    public static List<string> GetFiles(string path) {
        List<string> listOfFiles = new();
        string[] files = Directory.GetFiles(path);
        listOfFiles.AddRange(files);

        return listOfFiles;
    }

    public static FileData GetFileInfos(string path) {
        FileData finalFileData = new();

        finalFileData.FileCreationDate = File.GetCreationTimeUtc(path);
        finalFileData.FileLastModifiedDate = File.GetLastWriteTimeUtc(path);

        FileInfo infoFile = new(path);

        finalFileData.FileWeight = infoFile.Length;
        finalFileData.FileExtension = infoFile.Extension;
        finalFileData.FileName = infoFile.Name;
        finalFileData.FileParentDirectoryName = infoFile.DirectoryName;

        return finalFileData;
    }
    public static Stream PrintFileDatas(List<FileData> files) {
        // create a memory stream to write into
        MemoryStream stream = new();
        using StreamWriter writer = new(stream);

        // get header of FileData class properties and write it (1rst row)
        string fileHeader = FileData.ToStringPropertiesNameWithReflection();
        writer.WriteLine(fileHeader);

        foreach (FileData file in files) {
            /*
            // Naive
            string concatUn = file.FileParentDirectoryName + file.FileExtension;

            // Intermédiaire
            string concatDeux = string.Concat(file.FileParentDirectoryName, file.FileExtension);

            // Avancé 
            StringBuilder stringBuilder = new();
            stringBuilder.Append(file.FileParentDirectoryName);
            stringBuilder.Append(file.FileName);
            string concatTrois = stringBuilder.ToString();
            */

            // Expert == Récursion
            string fileRow = file.ToStringPropertiesValueWithReflection();
            writer.WriteLine(fileRow);
        }

        // Clean buffer and return a stream at the begining
        writer.Flush();
        writer.BaseStream.Position = 0;
        return writer.BaseStream;
    }

    public static string GetSavePathFromUser() {
        Console.WriteLine("Please give the file path to log");
        string? path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path) == false) {
            return path;
        }

        // Recursive function
        Console.WriteLine("Sorry GIIIRLLLS, you haven't put a valid path!!!!");
        string result = GetPathFromUser();
        return result;
    }

    public static bool SaveFilesData(Stream stream, string savePath) {
        string fileName = "LogsFichiers.csv";
        string finalDirectory = Path.Combine(savePath, fileName);
        
        FileStream fileStream = new(finalDirectory, FileMode.CreateNew);
        try {
            stream.CopyTo(fileStream);
            fileStream.Flush();
            return true;
        }
        catch (Exception exception) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
            Console.ResetColor();
            return false;
        }
    }
}
