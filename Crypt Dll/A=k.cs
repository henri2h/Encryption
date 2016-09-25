﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypt_dll_aplication
{
    public class Data
    {
        public char letter;
        public int Value;
        public int encryptValue = 0;
        public string morseCode = "";
        public int frequency = 0;
        public int total = 0;
        public Data(char letterValue)
        {
            letter = letterValue;
        }
        public Data(char letterValue, int pos)
        {
            letter = letterValue;
            Value = pos;
            encryptValue = pos;
        }
        public Data(char letterValue, int pos, int encryptpos)
        {
            letter = letterValue;
            Value = pos;
            encryptValue = encryptpos;
        }
        public Data(char letterValue, int pos, int encryptpos, string morse)
        {
            letter = letterValue;
            Value = pos;
            encryptValue = encryptpos;
            morseCode = morse;
        }
        public override string ToString()
        {
            StringBuilder SW = new StringBuilder();
            SW.Append("Letter : " + letter + " pos : " + Value + " encrypt value : " + encryptValue);
            return SW.ToString();
        }
    }
    public class A_k
    {
        public static List<Data> letters = new List<Data>();
        public static bool complexe = true;
        public static int total = 0;
        public static string letterSeparator = "/";
        public static string wordSeperator = "//";

        private static void settings(int augment)
        {
            total = defTotal(complexe);
            char[] letter = {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
            };
            string[] morseCode = { "._", "_...", "_._.", "_..", ".", ".._.", "__.", "....", "..", ".___", "_._" , "._..", "__", "_.", "___", ".__.", "__._", "._.", "...", "_", ".._", "..._", ".__","_.._", "_.__", "__..",
                    ".____","..___", "...__", "...._", ".....", "_....", "__...", "___..", "____.", "_____"
            };
            for (int i = 0; i < letter.Length; i++)
            {
                augment = augmentDefine(i + 1, augment);
                Data toAdd = new Data(letter[i], i + 1, i + 1 + augment, morseCode[i]);
                letters.Add(toAdd);
            }

        }
        public static string cryptCustom(string input_public, int augment)
        {
            bool first = true;

            StringBuilder sw = new StringBuilder();
            settings(augment);

            foreach (char ch in input_public)
            {
                if (ch != ' ')
                {
                    if(!first)
                    {
                        sw.Append(letterSeparator);
                    }
                    int pos = findLetterPos(ch);
                    if (pos != -1)
                    {
                        sw.Append(letters[pos].encryptValue);
                    }
                    first = false;
                    
                }
                else
                {
                    sw.Append(wordSeperator);
                    first = true;
                }
               

            }
            return sw.ToString();
        }
        public static string decodeCustom(string input_crypted, int augment)
        {
            // set the value of the letters
            settings(augment);
            StringBuilder Out = new StringBuilder();
            using (StringReader input = new StringReader(input_crypted))
            {
                string line = "";
                while ((line = input.ReadLine()) != null)
                {
                    string[] words = line.Split(new[] { wordSeperator }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string word in words)
                    {
                        string[] chars = word.Split(new[] { letterSeparator }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string letter in chars)
                        {
                            int letterInt = Convert.ToInt32(letter);
                            int letterPos = findLetter(letterInt);
                            Out.Append(letters[letterPos].letter);
                        }
                        Out.Append(" ");
                    }
                    Out.AppendLine();
                }
            }
            return Out.ToString();
        }
        private static int augmentDefine(int pos, int augment)
        {
            int total = defTotal(complexe);
            int result = 0;
            if (pos < (total - augment))
            {
                result = pos + augment;
            }
            else
            {
                result = pos + augment - total;
            }
            return result;
        }
        private static int defTotal(bool complexe)
        {
            int total = 26;
            if (complexe)
            {
                total = letters.ToArray().Length;
            }
            return total;
        }
        public static int findLetterPos(char selectedLetter)
        {
            foreach (Data letter in letters)
            {
                if (letter.letter == selectedLetter)
                {
                    return letters.IndexOf(letter);
                }
            }
            return -1;
        }
        public static int findLetter(int letterCryptValue)
        {
            foreach (Data letter in letters)
            {
                if (letter.encryptValue == letterCryptValue)
                {
                    return letters.IndexOf(letter);
                }
            }
            return -1;
        }
    }
}