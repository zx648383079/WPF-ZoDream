﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace ZoDream.Helper
{
    public class StringHelper
    {
        public static string GetStr(string s, int l, string endStr)
        {
            string temp = s.Substring(0, (s.Length < l) ? s.Length : l);

            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l)
            {
                return temp;
            }
            for (int i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l - endStr.Length)
                {
                    return temp + endStr;
                }
            }
            return endStr;
        }

        public static string GetStr2(string s, int l, string endStr)
        {
            string temp = s.Substring(0, (s.Length < l + 1) ? s.Length : l + 1);
            byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(temp);

            string outputStr = "";
            int count = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                if ((int)encodedBytes[i] == 63)
                    count += 2;
                else
                    count += 1;

                if (count <= l - endStr.Length)
                    outputStr += temp.Substring(i, 1);
                else if (count > l)
                    break;
            }

            if (count <= l)
            {
                outputStr = temp;
                endStr = "";
            }

            outputStr += endStr;

            return outputStr;
        }


        public static Color ToColor(string color)
        {
            var returnColor = Colors.Black;
            if (string.IsNullOrEmpty(color)) return returnColor;
            if (color[0] == '#')
            {
                // #fff;
                var convertFromString = ColorConverter.ConvertFromString(color);
                if (convertFromString != null)
                {
                    return (Color)convertFromString;
                }
                return returnColor;
            }
            var ms = Regex.Matches(color, @"\d+");
            switch (ms.Count)
            {
                case 3:
                    //255,255,255
                    returnColor = Color.FromRgb(Convert.ToByte(ms[0].Value), Convert.ToByte(ms[1].Value),
                        Convert.ToByte(ms[2].Value));
                    break;
                case 4:
                    //0,0,0,0.1
                    returnColor = Color.FromArgb(Convert.ToByte(ms[0].Value), Convert.ToByte(ms[1].Value),
                        Convert.ToByte(ms[2].Value), Convert.ToByte(ms[3].Value));
                    break;
            }
            return returnColor;
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDbc(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
    }
}
