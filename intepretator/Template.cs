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
        private Block m_block;

        public Template(Block p_block)
        {
            m_block = p_block;
        }

        public Block getBlock()
        {
            return m_block;
        }

        public void setToken(int p_index, Token  p_token)
        {
            m_block.setToken(p_index, p_token);
        }

        public void setSymbol(int p_index, Symbol p_symbol)
        {
            m_block.setSymbol(p_index, p_symbol);
        }

        public void addSymbol(Symbol p_symbol)
        {
            m_block.addSymbol(p_symbol);
        }

        public void addToken(Token p_token)
        {
            m_block.addToken(p_token);
        }
        
        public int getTokenCount()
        {
            return m_block.getTokenCount();
        }

        public int getSymbolCount()
        {
            return m_block.getSymbolCount();
        }
    }
}
