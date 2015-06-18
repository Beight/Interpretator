using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Template
    {
        private Block m_block;

        public Template(Block p_block)
        {
            m_block = p_block;
        }

        public Block getBlock()
        {
            return m_block;
        }
    }
}
