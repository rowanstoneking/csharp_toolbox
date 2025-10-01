using System.Threading;

namespace CSharpToolbox;

public partial class ToolboxFunc
{
    /// <summary>
    /// Deletes subsection of string by indices.
    /// </summary>
    /// <param name="src"> Given string </param>
    /// <param name="indexDeleteStart"> First index to delete </param>
    /// <param name="indexDeleteEnd"> Last index to delete </param>
    /// <returns> N/A </returns>
    internal static void DeleteSubStrByIndex(ref string src, int indexDeleteStart, int indexDeleteEnd)
    {
        if ((src.Length < 1) || (indexDeleteStart >= indexDeleteEnd)) { return; }  // No deletions if original input if empty, or if there isn't space between start and end of delete section.
        // ---
        indexDeleteStart = (indexDeleteStart < 0) ? 0 : indexDeleteStart;  // Sets delete start to beginning if too small.
        indexDeleteEnd = (indexDeleteEnd >= src.Length) ? src.Length - 1 : indexDeleteEnd;  // Sets delete end to end if too large.
        // ---
        src = src.Remove(indexDeleteStart, (indexDeleteEnd - indexDeleteStart + 1));  // Removes section
    }


    /// <summary>
    /// Extracts substring between head string and tail string; if indicated, will delete 1+ sections of source string.
    /// FUNCTIONS REQUIRED:  DeleteSubStrByIndex, GetStrSect
    /// </summary>
    /// <param name="src"> Given string </param>
    /// <param name="head"> First delimiter string (before string being extracted); if empty or not found, position is considered before source string </param>
    /// <param name="tail"> Second delimiter string (after string being extracted); if empty or not found, position is considered after source string </param>
    /// <param name="deleteExtracted"> Whether to delete the string between the head and tail in the original source string </param>
    /// <param name="deleteBeforeHead"> Whether to delete before the head in the original source string </param>
    /// <param name="deleteHead"> Whether to delete the head in the original source string</param>
    /// <param name="deleteTail"> Whether to delete the tail in the original source string </param>
    /// <param name="deleteAfterTail"> Whether to delete after the tail in the original source string </param>
    /// <returns> Portion of original string between tail and head </returns>
    internal static string ExtractBtwnStrings(ref string src, string head, string tail,
        bool deleteExtracted = false, bool deleteBeforeHead = false, bool deleteHead = false, bool deleteTail = false, bool deleteAfterTail = false)
    {
        int headStart, headEnd, tailStart, tailEnd;
        string result;
        const int indexBeforeStr = -1;  // Value for index before start of string (not a valid index, but distinct from after the end of a string).
        // ---
        if (src.Length < 1) { return String.Empty; }  // If source string is empty, returns empty string
        // ---
        // Finds start/end position of head string.
        if (head.Length > 0)
        {
            headStart = src.IndexOf(head);  // start of head
            if (headStart >= 0) { headEnd = headStart + head.Length - 1; }  // head found, calculates end of head 
            else { headStart = headEnd = indexBeforeStr; }  // head not found 
        }
        else  // empty head, position considered before src
        {
            headStart = headEnd = indexBeforeStr;
        }
        // ---
        // Finds start/end position of tail string.
        if (tail.Length > 0)
        {
            tailStart = src.IndexOf(tail);  // start of tail
            if (tailStart >= 0) { tailEnd = tailStart + tail.Length - 1; }  // tail found, calculates end of tail
            else { tailStart = tailEnd = tail.Length; }  // tail not found
        }
        else  // empty tail, position considered after src
        {
            tailStart = tailEnd = tail.Length;
        }
        // ---
        // Gets string between head and tail
        result = GetStrSect(src, headEnd, tailStart, false);
        // ---
        // Deletes all specified items from source string (checks whether they exist first).
        // NOTE:  deletes in backwards order so we can reference indices.
        if (deleteAfterTail)
        {
            if ((src.Length - 1) > tailEnd) { DeleteSubStrByIndex(ref src, (tailStart + 1), (src.Length - 1)); }
        }
        if (deleteTail)
        {
            if (tailStart < src.Length) { DeleteSubStrByIndex(ref src, tailStart, tailEnd); }
        }
        if (deleteExtracted)
        {
            if (result.Length > 0) { DeleteSubStrByIndex(ref src, (headEnd + 1), (tailStart - 1)); }
        }
        if (deleteHead)
        {
            if (headStart > indexBeforeStr) { DeleteSubStrByIndex(ref src, headStart, headEnd); }
        }
        if (deleteBeforeHead)
        {
            if (headStart > 0) { DeleteSubStrByIndex(ref src, 0, (headStart + 1)); }
        }
        // ---
        return result;  // Returns string between head and tail
    }


    /// <summary>
    /// Gets portion of string from original.
    /// </summary>
    /// <param name="input"> Given string </param>
    /// <param name="indexSubMin"> First index of subsection to be retrieved; assumed to be first of original if null </param>
    /// <param name="indexSubMax"> Last index of subsection to be retrieved; assumed to be last of original if null </param>
    /// <param name="inclusive"> Whether first and last indices are to be included in result </param>
    /// <returns> String subsection (or empty string, if no material found) </returns>
    internal static string GetStrSect(string input, int? indexSubMin = null, int? indexSubMax = null, bool inclusive = true)
    {
        const int indexMin = 0;  // min index of original string
        int indexMax = input.Length - 1;  // max index of original string
        if (indexMax < 0) { return string.Empty; }  // If original string is empty, returns empty string.
        int indexSubMin_ = indexSubMin ?? indexMin;  // First index of subsection being retrieved (start of original if not given).
        int indexSubMax_ = indexSubMax ?? indexMax;  // Last index of subsection being retrieved (end of original if not given).
        // ---
        // If given subsection indices aren't meant to be included, reassigns them to the actual indices being handled.
        if (!inclusive)
        {
            indexSubMin_ += 1;
            indexSubMax_ -= 1;
        }
        // If start and end go beyond original bounds, sets to bounds.
        indexSubMin_ = (indexSubMin_ < indexMin) ? indexMin : indexSubMin_;
        indexSubMax_ = (indexSubMax_ > indexMax) ? indexMax : indexSubMax_;
        // ---
        return input.Substring(indexSubMin_, indexSubMax_);  // Returns desired string subsection
    }


    /// <summary>
    /// Determines whether given string value is numeric
    /// </summary>
    internal static bool IsNumeric(string value)
    {
        return int.TryParse(value, out _);
    }


    /// <summary>
    /// Removes all instances of a substring from the end of a source string.
    /// </summary>
    /// <param name="src"> Given string being updated </param>
    /// <param name="substrToRemove"> Substring being removed from end </param>
    internal static void RemoveSubStrFromEnd(ref string src, string substrToRemove)
    {
        while (src.EndsWith(substrToRemove))
        {
            src = src.Substring(0, src.Length - substrToRemove.Length);
        }
    }


    /// <summary>
    /// Removes all instances of a substring from the start of a source string.
    /// </summary>
    /// <param name="src"> Given string being updated </param>
    /// <param name="substrToRemove"> Substring being removed from start </param>
    internal static void RemoveSubStrFromStart(ref string src, string substrToRemove)
    {
        while (src.StartsWith(substrToRemove))
        {
            src = src.Substring(substrToRemove.Length);
        }
    }
}
