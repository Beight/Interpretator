using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    //Activation Record
    //represents blocks that has been created by copying data from a template block.
    //A template is an description of what a block contains at creation, this data is always static. An AR is then created in the image of the template and the AR then executes the code.
    //This way multiple instances of a block can be created and used with AR.
    class AR
    {
        private int m_dynamicF;
        private int m_returnAddress;
        private Block m_block;

        public AR(int p_dynamicF, Template p_template)
        {
            m_dynamicF = p_dynamicF;
            m_block = p_template.getBlock();
        }
        public Block getBlock()
        {
            return m_block;
        }
        public int getStaticF()
        {
            return m_block.getStaticF();
        }
        public Symbol getSymbol(int p_index)
        {
            return m_block.getSymbol(p_index);
        }
        public Token getToken(int p_index)
        {
            return m_block.getToken(p_index);
        }
        public int getSymbolCount()
        {
            return m_block.getSymbolCount();
        }
        public int getTokenCount()
        {
            return m_block.getTokenCount();
        }
        public int getDynamicF()
        {
            return m_dynamicF;
        }
    }
}
