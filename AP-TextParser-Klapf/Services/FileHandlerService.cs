using AP_TextParser_Klapf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("AP-TextParser-Klapf-Tests")]

namespace AP_TextParser_Klapf.Services
{
    internal class FileHandlerService
    {
        /// <summary>
        /// Extracts all words from a string by removing all control and special characters.
        /// </summary>
        /// <param name="data">The string to process</param>
        /// <returns>A string array with all words found in the given string.</returns>
        internal String[] ExtractWordsFromString(string data)
        {
            if (!String.IsNullOrEmpty(data))
            {
                var stripped = string.Join(" ", Regex.Split(data, @"(?:\r\n|\n|\r)")).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return stripped;
            }
            return null;
        }

        /// <summary>
        /// Reads in an array of strings and returns a list of worddata each containing one word and its occurrence in the parsed text.
        /// </summary>
        /// <param name="collection">String array to process</param>
        /// <returns>A list of worddata each containing one word and its occurrence in the parsed text</returns>
        private List<WordData> GetWordsWithTheirOccurences(string[] collection)
        {
            if (collection == null) { return null; }

            var res = (from word in collection
                       group word by word into groupedWords
                       select new
                       {
                           Word = groupedWords.Key,
                           Occurrence = groupedWords.Count()
                       }).OrderByDescending(x => x.Occurrence);
            return res.Select(row => new WordData { Word = row.Word, WordCount = row.Occurrence }).ToList();
        }

        /// <summary>
        /// Reads a text file and stores its content in a string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Content of the text file as string</returns>
        private string GetTextFileAsString(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }

            try
            {
                if (!IsBinary(path))
                {
                    return File.ReadAllText(path);
                }
                else
                {
                    return null;
                }
            }
            catch (IOException e)
            {
                if (e.Source != null)
                    Console.WriteLine("Error reading from {0}. Message = {1}", path, e.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                    Console.WriteLine("Exception: {0}", ex.Source);
            }
            return null;
        }

        /// <summary>
        /// Processes a given file.
        /// </summary>
        /// <param name="path">Full path to the file being processed</param>
        /// <returns>A list of worddata each containing one word and its occurrence in the parsed text</returns>
        public List<WordData> ProcessAnsiFile(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Console.WriteLine("Started processing file!");

                string RawText = GetTextFileAsString(path);
                if (RawText == null)
                {
                    return null;
                }

                var data = GetWordsWithTheirOccurences(ExtractWordsFromString(RawText));

                if (data == null)
                {
                    return null;
                }
                watch.Stop();
                Console.WriteLine($"Finished processing. Total Processing Time: {watch.ElapsedMilliseconds} ms");
                return data;
            }

            return null;
        }

        /// <summary>
        /// Simple check if a given file is a binary. The first 8000 bytes of the file are checked, if one contains a nulChar it can be assumed that it is a binary.
        /// </summary>
        /// <param name="path">Full path to the file</param>
        /// <returns></returns>
        private bool IsBinary(string path)
        {
            const int charsAmountToCheck = 8000;
            const char nulChar = '\0';

            using (var streamReader = new StreamReader(path))
            {
                for (var i = 0; i < charsAmountToCheck; i++)
                {
                    if (streamReader.EndOfStream)
                    {
                        return false;
                    }

                    if ((char)streamReader.Read() == nulChar)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}