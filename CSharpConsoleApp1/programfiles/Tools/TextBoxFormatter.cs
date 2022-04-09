using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsciiProgram
{
    //fits text into bounds
    //returns a list of each line of text formatted to fit in box
    class TextBoxFormatter
    {
        List<string> m_text;

        public TextBoxFormatter()
        {
            m_text = new List<string>();
        }

        public List<string> FormatText(string text, Vector2 bounds, bool textWrap)
        {
            List<string> formattedString = new List<string>();
            int index = 0;

            string[] lines = text.Split('\n');

            foreach (string line in lines)
            {
                if (line.Length > bounds.x)//line too long
                {
                    formattedString.Add("");
                    string[] words = line.Split(' ');

                    foreach (string word in words)//go through words
                    {
                        if (word.Length > bounds.x)//word too long
                        {
                            string[] cutWord;

                            //cut word into segments that fit
                            if (formattedString[index].Length == 0)
                                cutWord = FitWordToLine(word, bounds);
                            else
                                cutWord = FitWordToLine(word, bounds, bounds.x - formattedString[index].Length);

                            //add int cut up word
                            for (int i = 0; i < cutWord.Length; ++i)
                            {
                                if ((cutWord[i] + formattedString[index]).Length > bounds.x)
                                {
                                    formattedString.Add(cutWord[i]);
                                    ++index;
                                }
                                else
                                    formattedString[index] += cutWord[i];
                            }

                        }
                        else if ((formattedString[index] + word + " ").Length > bounds.x)//word and space wont fit on line
                        {
                            formattedString[index] = formattedString[index].Trim(' ');
                            formattedString.Add(word);
                            ++index;
                        }
                        else//word fits
                        {
                            if (formattedString[index].Length == 0)
                                formattedString[index] += word;
                            else
                                formattedString[index] += " " + word;
                        }

                    }

                }
                else//line fits
                {
                    formattedString.Add(line);
                    ++index;
                }

            }


            m_text = formattedString;
            return m_text;
        }

        public List<string> GetFormattedText()
        {
            return m_text;
        }

        string[] FitWordToLine(string word, Vector2 bounds, int firstLineSpace = 0)
        {
            FastConsole.FastWrite fastWrite = FastConsole.FastWrite.GetInstance();
            fastWrite.SetCursorPosition(new Vector2(0, 0));

            List<string> result = new List<string>();
            while(word.Length > 0)
            {
                if(firstLineSpace != 0)
                {
                    result.Add(word.Substring(0, firstLineSpace));
                    word = word.Remove(0, firstLineSpace);
                    firstLineSpace = 0;
                }
                else
                {
                    if(word.Length > bounds.x)
                    {
                        result.Add(word.Substring(0, bounds.x));
                        word = word.Remove(0, bounds.x);
                    }
                    else
                    {
                        result.Add(word.Substring(0, word.Length));
                        word = word.Remove(0, word.Length);
                        break;
                    }
                }
            }

            return result.ToArray();
        }

    }
}
