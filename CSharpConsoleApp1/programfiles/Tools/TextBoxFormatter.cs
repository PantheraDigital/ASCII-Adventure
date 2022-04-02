﻿using System;
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

        public List<string> FormatText(string text, Vector2 bounds, bool textWrapping)
        {
            string[] words = text.Split(' ');

            if (m_text.Count == 0)
                m_text.Add("");

            int lineIndex = m_text.Count - 1;

            if (lineIndex < 0)
                lineIndex = 0;


            foreach (string word in words)
            {
                if (textWrapping && m_text[lineIndex].Length == bounds.x)
                {
                    m_text[lineIndex] = m_text[lineIndex].TrimEnd(' ');
                    m_text.Add("");
                    ++lineIndex;
                }

                if (!textWrapping && (word + m_text[lineIndex]).Length > bounds.x && !String.IsNullOrEmpty(m_text[0]))
                    break;

                if (word.Length > bounds.x || word.Contains('\n'))
                {
                    //cut out newlines and make word fit on line
                    string wordCopy = word;

                    do//while wordCopy contains'\n' or is longer than bounds.x
                    {
                        if (textWrapping && m_text[lineIndex].Length == bounds.x)
                        {
                            m_text[lineIndex] = m_text[lineIndex].TrimEnd(' ');
                            m_text.Add("");
                            ++lineIndex;
                        }

                        if (wordCopy.Contains('\n'))
                        {
                            int cutWordLength;
                            string cutWord;
                            bool add1 = false;
                            bool longWord = false;//if a word longer than bounds has a '/n'

                            if (wordCopy.IndexOf('\n') + m_text[lineIndex].Length > bounds.x)
                            {
                                longWord = true;
                                cutWordLength = bounds.x - m_text[lineIndex].Length;
                            }
                            else
                            {
                                add1 = true;
                                cutWordLength = wordCopy.IndexOf('\n');
                            }

                            cutWord = wordCopy.Substring(0, cutWordLength);
                            m_text[lineIndex] = m_text[lineIndex] + cutWord;
                            m_text[lineIndex] = m_text[lineIndex].TrimEnd(' ');
                            m_text.Add("");
                            ++lineIndex;

                            if (add1)
                                cutWordLength += 1;

                            wordCopy = wordCopy.Remove(0, cutWordLength);
                            if (longWord && !textWrapping)
                                wordCopy = "";
                        }
                        else
                        {
                            int cutWordLength;
                            string cutWord;

                            if (wordCopy.Length > bounds.x - m_text[lineIndex].Length)
                                cutWordLength = bounds.x - m_text[lineIndex].Length;
                            else
                                cutWordLength = wordCopy.Length;

                            cutWord = wordCopy.Substring(0, cutWordLength);
                            m_text[lineIndex] = m_text[lineIndex] + cutWord;
                            wordCopy = wordCopy.Remove(0, cutWordLength);
                        }

                    } while (wordCopy.Contains('\n') || wordCopy.Length > bounds.x);


                    if (wordCopy.Length > 0)
                    {
                        if ((wordCopy + m_text[lineIndex]).Length + 1 <= bounds.x)
                            m_text[lineIndex] = m_text[lineIndex] + wordCopy + " ";
                        else if ((wordCopy + m_text[lineIndex]).Length <= bounds.x)
                            m_text[lineIndex] = m_text[lineIndex] + wordCopy;
                        else if (textWrapping)
                        {
                            m_text[lineIndex] = m_text[lineIndex].TrimEnd(' ');
                            m_text.Add(wordCopy);
                            ++lineIndex;
                        }
                    }

                }
                else //word can fit in a line and contains no '\n'
                {
                    if ((word + m_text[lineIndex]).Length + 1 <= bounds.x)
                    {
                        m_text[lineIndex] = m_text[lineIndex] + word + " ";
                    }
                    else if ((word + m_text[lineIndex]).Length <= bounds.x)
                    {
                        m_text[lineIndex] = m_text[lineIndex] + word;
                    }
                    else if (textWrapping)
                    {
                        m_text[lineIndex] = m_text[lineIndex].TrimEnd(' ');
                        m_text.Add(word);
                        ++lineIndex;
                    }
                }
            }

            m_text[lineIndex] = m_text[lineIndex].TrimEnd(' ');

            return m_text;
        }

        public List<string> GetFormattedText()
        {
            return m_text;
        }

    }
}