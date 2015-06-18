using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Intepretator
{
    class Token
    {
        private int m_code;
        private int m_type;
        private int m_value;
        private string m_text;


        public Token(int p_code, int p_type, string p_text)
        {
            m_code = p_code;
            m_type = p_type;
            m_text = p_text;

            if (p_type == 2)
                m_value = int.Parse(m_text);
        }

        public Token(int p_value, int p_code, int p_type)
        {
            m_code = p_code;
            m_type = p_type;
            m_value = p_value;
        }

        public int getType()
        {
            return m_type;
        }
        public int getCode()
        {
            return m_code;
        }
        public int getValue()
        {
            return m_value;
        }
        public void setValue(int p_value)
        {
            m_value = p_value;
        }
        public string getText()
        {
            return m_text;
        }
        public void setText(string p_text)
        {
            m_text = p_text;
        }

    }
}
