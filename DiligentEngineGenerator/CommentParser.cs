using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    public static class CommentParser
    {
        public static bool Find(String line, StringBuilder currentComment)
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("///"))
            {
                currentComment.AppendLine(trimmed.Substring(3).Trim());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
