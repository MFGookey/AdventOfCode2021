using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utilities.IO;
using System.Text.RegularExpressions;

namespace Common.Utilities.Formatter
{
  public class RecordFormatter : IRecordFormatter
  {
    private IFileReader _fileReader;

    /// <summary>
    /// Initializes a new instance of the RecordFormatter class with a given IFileReader
    /// </summary>
    /// <param name="fileReader">The file reader to use for file IO</param>
    public RecordFormatter(IFileReader fileReader)
    {
      _fileReader = fileReader;
    }

    /// <inheritdoc/>
    public IEnumerable<string> FormatFile(string filePath, string recordDelimiter, bool removeBlankRecords)
    {
      return FormatFile(filePath, recordDelimiter, removeBlankRecords, false);
    }

    /// <inheritdoc/>
    public IEnumerable<string> FormatFile(string filePath, string recordDelimiter, bool removeBlankRecords, bool normalizeLineEndings)
    {
      var fileContent = _fileReader.ReadFile(filePath);
      return FormatRecord(fileContent, recordDelimiter, removeBlankRecords, normalizeLineEndings);
    }

    /// <inheritdoc/>
    public IEnumerable<string> FormatRecord(string records, string recordDelimiter, bool removeBlankRecords)
    {
      return FormatRecord(records, recordDelimiter, removeBlankRecords, false);
    }

    /// <inheritdoc/>
    public IEnumerable<string> FormatRecord(string records, string recordDelimiter, bool removeBlankRecords, bool normalizeLineEndings)
    {
      if (normalizeLineEndings)
      {
        records = NormalizeLineEndings(records);
      }

      return records.Split(recordDelimiter, removeBlankRecords ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None).Where(r => { return removeBlankRecords ? string.IsNullOrWhiteSpace(r) == false : true; });
    }

    /// <inheritdoc/>
    public IEnumerable<IEnumerable<string>> FormatSubRecords(IEnumerable<string> records, string subRecordDelimiter, bool removeBlankRecords)
    {
      return FormatSubRecords(records, subRecordDelimiter, removeBlankRecords, false);
    }

    /// <inheritdoc/>
    public IEnumerable<IEnumerable<string>> FormatSubRecords(IEnumerable<string> records, string subRecordDelimiter, bool removeBlankRecords, bool normalizeLineEndings)
    {
      return records.Where(r => { return removeBlankRecords ? string.IsNullOrWhiteSpace(r) == false : true; }).Select(r => FormatRecord(r, subRecordDelimiter, removeBlankRecords, normalizeLineEndings));
    }

    private string NormalizeLineEndings(string toNormalize, string normalizeTo = "\n")
    {
      return Regex.Replace(toNormalize, "\r\n|\r|\n\r|\n", normalizeTo);
    }
  }
}
