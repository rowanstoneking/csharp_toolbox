namespace CSharpToolbox;

public partial class ToolboxFunc
{
    /// <summary>
    /// Gets portion of string from original
    /// </summary>
    /// <param name="input">Given string</param>
    /// <param name="indexSubMin">First index of subsection to be retrieved; assumed to be first of original if null</param>
    /// <param name="indexSubMax">Last index of subsection to be retrieved; assumed to be last of original if null</param>
    /// <param name="inclusive">Whether first and last indices are to be included in result</param>
    /// <returns>String subsection (or empty string, if no material found)</returns>
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
}
