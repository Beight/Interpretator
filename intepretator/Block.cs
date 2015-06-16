using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Block
    {
        private int m_Nr;
        private int m_StaticF;
        private int m_SymbolCount;
        private int m_Memory;
        private int m_TokenCount;
        private Token[] m_Tokens;
        private Symbol[] m_Symbols;

        public Block(int p_Nr, int p_StaticF, int p_Memory, int p_SymbolCount, int p_TokenCount)
        {
            m_Nr = p_Nr;
            m_StaticF = p_StaticF;
            m_SymbolCount = p_SymbolCount;
            m_Memory = p_Memory;
            m_TokenCount = p_TokenCount;
            m_Tokens = new Token[m_TokenCount];
            m_Symbols = new Symbol[m_SymbolCount];

        }

        //public void SearchID(){ }
        //public void GetToken(){ }
        //public void ParameterSearch() { }

        public int getBlockNr()
        {
            return m_Nr;
        }
        public int getStaticF()
        {
            return m_StaticF;
        }
        public int getSymbolCount()
        {
            return m_SymbolCount;
        }
        public int getTokenCount()
        {
            return m_TokenCount;
        }
        public int getMemory()
        {
            return m_Memory;
        }
        public Token getToken(int p_Index)
        {
            return m_Tokens[p_Index];
        }
        public Symbol getSymbol(int p_Index)
        {
            return m_Symbols[p_Index];
        }

        public void setToken(int p_Index, Token p_Token)
        {
            m_Tokens[p_Index] = p_Token;
        }

        public void setSymbol(int p_Index, Symbol p_Symbol)
        {
            m_Symbols[p_Index] = p_Symbol;
        }
    }
}
