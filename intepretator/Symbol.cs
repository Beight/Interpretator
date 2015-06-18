using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Symbol
    {
        private int m_id;
        private int m_type;
        private int m_kind;
        private int m_info;
        private int m_info2;
        private int m_info3;
        private int m_address;
        private int m_boundry;
        private int m_value;
        private string m_text;

        public Symbol(int p_id, int p_type, int p_kind, int p_info, int p_info2, int p_info3, int p_address, string p_text, int p_boundry)
        {
            m_id = p_id;
            m_type = p_type;
            m_kind = p_kind;  
            m_info = p_info;
            m_info2 = p_info2;
            m_info3 = p_info3;
            m_address = p_address;
            m_text = p_text;
            m_boundry = p_boundry;
        }

        public string getText()
        {
            return m_text;
        }
        public int getValue()
        {
            return m_value;
        }

        public void setValue(int p_value)
        {
            m_value = p_value;
        }
    }
}
