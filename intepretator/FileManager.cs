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
        private StreamReader m_Filereader;
        private OpenFileDialog m_BrowseFile;
        private List<string> m_FileStringList;
        private List<string> m_TempStringList;
        private string[][] m_TempStringArray;
        private string[][] m_BlockString_Array;


        public FileManager()
        {
            m_BrowseFile = new OpenFileDialog();
            m_FileStringList = new List<string>();
            m_TempStringList = new List<string>();
        }

        public void Loadfile(RichTextBox tbfile)
        {
            tbfile.Clear();
            m_BrowseFile.Title = "Choose a file";
            m_BrowseFile.InitialDirectory = "\\.";
            m_BrowseFile.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            m_BrowseFile.ShowDialog();
            m_Filereader = new StreamReader(m_BrowseFile.OpenFile());

            while (!m_Filereader.EndOfStream)
            {
                m_FileStringList.Add(m_Filereader.ReadLine());

            }
            for (int i = 0; i < m_FileStringList.Count; i++)
            {
                tbfile.AppendText(m_FileStringList[i] + "\n");
            }
            m_TempStringArray = new string[m_FileStringList.Count][];
            m_BlockString_Array = new string[m_FileStringList.Count][];

            for (int i = 0; i < m_FileStringList.Count; i++)
            {
                m_TempStringArray[i] = m_FileStringList[i].Split(' ');
            }
            for (int i = 0; i < m_TempStringArray.Length; i++)
            {
                for (int j = 0; j < m_TempStringArray[i].Length; j++)
                {
                    if (m_TempStringArray[i][j] != "")
                    {
                        m_TempStringList.Add(m_TempStringArray[i][j]);
                    }
                }
                m_BlockString_Array[i] = m_TempStringList.ToArray();
                m_TempStringList.Clear();
            }
        }

        public string[][] getBlockStringArray()
        {
            return m_BlockString_Array;
        }
    }   
}
