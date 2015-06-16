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
        Environment enviro;
        FileManager file;

        public Form1()
        {
           
            InitializeComponent();
            file = new FileManager();
            enviro = new Environment(file, tbconverted);
        }

        private void loadFile_Click(object sender, EventArgs e)
        {
            file.Loadfile(tbfile);

        }

        private void Convert_Click(object sender, EventArgs e)
        {
            enviro.CreateBlocks();
        }

        public void tbfile_TextChanged(object sender, EventArgs e)
        {
            
                
        }

        private void tbconverted_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
