using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Block
    {
        ///<summary>
        ///Block nr, Identifies the block.
        ///</summary>
        private int m_nr;
        ///<summary>
        ///Static father, id to block surrounding this block. Equals -1 if this is the first block.
        ///</summary>
        private int m_staticF;
        ///<summary>
        ///Number of symbols contained in the block.
        ///</summary>
        private int m_symbolCount;
        ///<summary>
        ///Memory required by the block. Not used at the moment.
        ///</summary>
        private int m_memory;
        ///<summary>
        ///Number of tokens contained in the block.
        ///</summary>
        private int m_tokenCount;
        ///<summary>
        ///List of the block's tokens.
        ///</summary>
        private Token[] m_tokens;
        ///<summary>
        ///List of the block's symbols.
        ///</summary>
        private Symbol[] m_symbols;

        public Block(int p_nr, int p_staticF, int p_memory, int p_symbolCount, int p_tokenCount)
        {
            m_nr = p_nr;
            m_staticF = p_staticF;
            m_symbolCount = p_symbolCount;
            m_memory = p_memory;
            m_tokenCount = p_tokenCount;
            m_tokens = new Token[m_tokenCount];
            m_symbols = new Symbol[m_symbolCount];

        }

        public Block(Block p_block)
        {
            m_nr = p_block.getBlockNr();
            m_staticF = p_block.getStaticF();
            m_symbolCount = p_block.getSymbolCount();
            m_memory = p_block.getMemory();
            m_tokenCount = p_block.getTokenCount();
            m_tokens = new Token[m_tokenCount];
            m_symbols = new Symbol[m_symbolCount];
            p_block.getTokens().CopyTo(m_tokens, 0);
            p_block.getSymbols().CopyTo(m_symbols, 0);
        }

        //public void SearchID(){ }
        //public void GetToken(){ }
        //public void ParameterSearch() { }

        public int getBlockNr()
        {
            return m_nr;
        }
        public int getStaticF()
        {
            return m_staticF;
        }
        public int getSymbolCount()
        {
            return m_symbolCount;
        }
        public int getTokenCount()
        {
            return m_tokenCount;
        }
        public int getMemory()
        {
            return m_memory;
        }
        public Token getToken(int p_index)
        {
            return m_tokens[p_index];
        }
        public Symbol getSymbol(int p_index)
        {
            return m_symbols[p_index];
        }
        public Symbol searchSymbol(int p_id)
        {
            for(int i = 0; i < m_symbolCount; i++)
            {
                if(m_symbols[i].getId() == p_id)
                    return m_symbols[i];
            }
            return null;
        }

        public void setToken(int p_index, Token p_token)
        {
            m_tokens[p_index] = p_token;
        }

        public void setSymbol(int p_index, Symbol p_symbol)
        {
            m_symbols[p_index] = p_symbol;
        }

        private Token[] getTokens()
        {
            return m_tokens;
        }

        private Symbol[] getSymbols()
        {
            return m_symbols;
        }
    }
}
