using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Template
    {
        private Block m_block;

        public Template(Block p_Block)
        {
            m_block = p_Block;
        }

        public Block getBlock()
        {
            return m_block;
        }
    }
}
