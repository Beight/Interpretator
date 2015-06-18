using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    //Hold a blueprint on how a block should look like.
    //The AR class copy data from the template class to create executable blocks.
    class Template
    {
        private static Block m_block;

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
