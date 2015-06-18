using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Block
    {
        private int m_nr;
        private int m_staticF;
        private int m_symbolCount;
        private int m_memory;
        private int m_tokenCount;
        private Token[] m_tokens;
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

        public void setToken(int p_index, Token p_token)
        {
            m_tokens[p_index] = p_token;
        }

        public void setSymbol(int p_index, Symbol p_symbol)
        {
            m_symbols[p_index] = p_symbol;
        }
    }
}
