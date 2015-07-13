using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Intepretator
{
    public partial class Form1 : Form
    {
        Environment m_environment;
        FileManager m_file;
        bool m_fileloaded;

        public Form1()
        {
            InitializeComponent();
            m_fileloaded = false;
        }

        private void loadFile_Click(object sender, EventArgs e)
        {
            tbfile.Clear();
            m_file = new FileManager();
            m_fileloaded = m_file.Loadfile(tbfile);
            tbconverted.Clear();
            
        }

        private void Convert_Click(object sender, EventArgs e)
        {
            if (m_fileloaded)
            {
                m_environment = new Environment(m_file, tbconverted);
                m_environment.createBlocks();
            }
        }

        public void tbfile_TextChanged(object sender, EventArgs e)
        {
            
                
        }

        private void tbconverted_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
