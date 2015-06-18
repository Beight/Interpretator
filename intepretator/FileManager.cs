using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Intepretator
{
    class FileManager
    {
        private StreamReader m_filereader;
        private OpenFileDialog m_browseFile;
        private List<string> m_fileStringList;
        private List<string> m_tempStringList;
        private string[][] m_tempStringArray;
        private string[][] m_blockStringArray;


        public FileManager()
        {
            m_browseFile = new OpenFileDialog();
            m_fileStringList = new List<string>();
            m_tempStringList = new List<string>();
        }

        public void Loadfile(RichTextBox p_tbfile)
        {
            m_browseFile.Title = "Choose a file";
            m_browseFile.InitialDirectory = "\\.";
            m_browseFile.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            m_browseFile.ShowDialog();

            try
            {

                m_filereader = new StreamReader(m_browseFile.OpenFile());

                while (!m_filereader.EndOfStream)
                {
                    m_fileStringList.Add(m_filereader.ReadLine());

                }
                for (int i = 0; i < m_fileStringList.Count; i++)
                {
                    p_tbfile.AppendText(m_fileStringList[i] + "\n");
                }
                m_tempStringArray = new string[m_fileStringList.Count][];
                m_blockStringArray = new string[m_fileStringList.Count][];

                for (int i = 0; i < m_fileStringList.Count; i++)
                {
                    m_tempStringArray[i] = m_fileStringList[i].Split(' ');
                }
                for (int i = 0; i < m_tempStringArray.Length; i++)
                {
                    for (int j = 0; j < m_tempStringArray[i].Length; j++)
                    {
                        if (m_tempStringArray[i][j] != "")
                        {
                            m_tempStringList.Add(m_tempStringArray[i][j]);
                        }
                    }
                    m_blockStringArray[i] = m_tempStringList.ToArray();
                    m_tempStringList.Clear();
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                return;
            }
        }

        public string[][] getBlockStringArray()
        {
            return m_blockStringArray;
        }
    }   
}
