using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class KeyCreator
    {
        static Random random;

        static KeyCreator()
        {
            DateTime now = DateTime.Now;
            int seedMod = (int)Math.Pow(now.Day & now.Month & now.Year & now.Hour & now.Minute & now.Second, 0.2);
            random = new Random(seedMod);
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

        public static String createKey(String appName)
        {
            long appVal = 0;
            for (int i = 0; i < appName.Length; ++i)
            {
                appVal += appName[i];
            }

            String keyString = "";
            for (int i = 1; i < 21; ++i)
            {
                if (random.Next(10000) / 10000.0f < 0.5f) //Do a letter
                {
                    keyString += new String((char)(random.Next(25) + 65), 1);
                }
                else
                {
                    keyString += random.Next(9).ToString();
                }
                if (i % 5 == 0)
                {
                    keyString += "-";
                }
            }

            long genVal = 0;
            for (int i = 0; i < keyString.Length; ++i)
            {
                if (i < appName.Length)
                {
                    if (getPlusMinus(appName[i]) == false)
                    {
                        genVal += keyString[i];
                    }
                    else
                    {
                        genVal -= keyString[i];
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        genVal -= keyString[i];
                    }
                    else
                    {
                        genVal += keyString[i];
                    }
                }
            }
            if (genVal < 0)
            {
                genVal = -genVal;
            }

            //Console.WriteLine(String.Format("GenVal {0} AppVal {1} KeyString {2}", genVal, appVal, keyString));

            keyString += ((genVal * appVal).ToString() + "JSDEU").Substring(0, 5);
            return keyString.ToUpper();
        }
    }
}
