using System.Collections.Generic;

namespace Common.Utilities.Formatter
{
  /// <summary>
  /// Given a delimited string of records, break the records up by a given delimiter.
  /// </summary>
  public interface IRecordFormatter
  {
    /// <summary>
    /// Given a delimited string of records, break them up by the delimiter.  Optionally remove blank records (any that trigger string.IsNullOrWhitespace())
    /// </summary>
    /// <param name="records">The string of records to format</param>
    /// <param name="recordDelimiter">The record delimiter</param>
    /// <param name="removeBlankRecords">Should empty, null, and whitespace only records be removed from the final results?</param>
    /// <returns>An enumerable of strings representing the records.</returns>
    public IEnumerable<string> FormatRecord(string records, string recordDelimiter, bool removeBlankRecords);

    /// <summary>
    /// Given a delimited string of records, break them up by the delimiter.  Optionally remove blank records (any that trigger string.IsNullOrWhitespace()), and optionally normalize the line endings
    /// </summary>
    /// <param name="records">The string of records to format</param>
    /// <param name="recordDelimiter">The record delimiter</param>
    /// <param name="removeBlankRecords">Should empty, null, and whitespace only records be removed from the final results?</param>
    /// <param name="normalizeLineEndings">Should \r\n, \r, and \n\r get replaced with \n?</param>
    /// <returns>An enumerable of strings representing the records.</returns>
    public IEnumerable<string> FormatRecord(string records, string recordDelimiter, bool removeBlankRecords, bool normalizeLineEndings);

    /// <summary>
    /// Given an enumerable of records, break them up into further enumerables based on the subrecord delimiter
    /// </summary>
    /// <param name="records">An enumerable of records to format into subrecords</param>
    /// <param name="subRecordDelimiter">The subrecord delimiter</param>
    /// <param name="removeBlankRecords">Should empty, null, and whitespace only records and subrecords be removed from the final results?</param>
    /// <returns>An enumerable of an enumerable of strings representing the records first, and within their subrecords</returns>
    public IEnumerable<IEnumerable<string>> FormatSubRecords(IEnumerable<string> records, string subRecordDelimiter, bool removeBlankRecords);

    /// <summary>
    /// Given an enumerable of records, break them up into further enumerables based on the subrecord delimiter, and optionally normalize the line endings
    /// </summary>
    /// <param name="records">An enumerable of records to format into subrecords</param>
    /// <param name="subRecordDelimiter">The subrecord delimiter</param>
    /// <param name="removeBlankRecords">Should empty, null, and whitespace only records and subrecords be removed from the final results?</param>
    /// <param name="normalizeLineEndings">Should \r\n, \r, and \n\r get replaced with \n?</param>
    /// <returns>An enumerable of an enumerable of strings representing the records first, and within their subrecords</returns>
    public IEnumerable<IEnumerable<string>> FormatSubRecords(IEnumerable<string> records, string subRecordDelimiter, bool removeBlankRecords, bool normalizeLineEndings);

    /// <summary>
    /// Given a file path, read in the file and break it apart by the record delimiter.  Optionally, remove the blank, null, or whitespace only records.
    /// </summary>
    /// <param name="filePath">The path to the file to read.</param>
    /// <param name="recordDelimiter">The record delimiter</param>
    /// <param name="removeBlankRecords">Should empty, null, and whitespace only records be removed from the final results?</param>
    /// <returns>An enumerable of strings representing the records.</returns>
    public IEnumerable<string> FormatFile(string filePath, string recordDelimiter, bool removeBlankRecords);

    /// <summary>
    /// Given a file path, read in the file and break it apart by the record delimiter.  Optionally, remove the blank, null, or whitespace only records.
    /// </summary>
    /// <param name="filePath">The path to the file to read.</param>
    /// <param name="recordDelimiter">The record delimiter</param>
    /// <param name="removeBlankRecords">Should empty, null, and whitespace only records be removed from the final results?</param>
    /// <param name="normalizeLineEndings">Should \r\n, \r, and \n\r get replaced with \n?</param>
    /// <returns>An enumerable of strings representing the records.</returns>
    public IEnumerable<string> FormatFile(string filePath, string recordDelimiter, bool removeBlankRecords, bool normalizeLineEndings);

  }
}

