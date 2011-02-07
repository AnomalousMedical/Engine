using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class KeyChecker
    {
        private KeyChecker()
        {

        }

        public static bool checkValid(String appName, String key)
        {
            long appVal = 0;
            for (int i = 0; i < appName.Length; ++i)
            {
                appVal += appName[i];
            }
            String[] splitArgs = { "-" };
            String[] split = key.Split(splitArgs, StringSplitOptions.None);
            String tempVar = String.Join("-", split, 0, 4) + "-";

            long genVal = 0;
            for (int i = 0; i < tempVar.Length; ++i)
            {
                if (i < appName.Length)
                {
                    if (getPlusMinus(appName[i]) == false)
                    {
                        genVal += tempVar[i];
                    }
                    else
                    {
                        genVal -= tempVar[i];
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        genVal -= tempVar[i];
                    }
                    else
                    {
                        genVal += tempVar[i];
                    }
                }
            }
            if (genVal < 0)
            {
                genVal = -genVal;
            }

            //Console.WriteLine(String.Format("GenVal {0} AppVal {1} KeyString {2}", genVal, appVal, tempVar));

            return ((genVal * appVal).ToString() + "JSDEU").Substring(0, 5) == split[4];
        }

        /// <summary>
        /// Returns true if the character is more than halfway through the
        /// alphabet.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        static bool getPlusMinus(char ch)
        {
            ch = char.ToUpper(ch);
            if (ch - 64 < 12)
            {
                return true;
            }
            return false;
        }
    }
}
