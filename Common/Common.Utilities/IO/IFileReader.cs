namespace Common.Utilities.IO
{
  public interface IFileReader
  {
    /// <summary>
    /// Read a file and return the entire contents as a single string
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <returns>The contents of the file as a string</returns>
    string ReadFile(string path);

    /// <summary>
    /// Read a file and return the entire contents as a read-only list of lines
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <returns>The contents of the file as an enumerable of strings</returns>
    string[] ReadFileByLines(string path);
  }
}
