using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Intepretator
{
    class Token
    {
        private int m_Code;
        private int m_Type;
        private int m_Value;
        private string m_Text;


        public Token(int p_Code, int p_Type, string p_Text)
        {
            m_Code = p_Code;
            m_Type = p_Type;
            m_Text = p_Text;

            if (p_Type == 2)
                m_Value = int.Parse(m_Text);
        }

        public Token(int p_Value, int p_Code, int p_Type)
        {
            m_Code = p_Code;
            m_Type = p_Type;
            m_Value = p_Value;
        }

        public int getType()
        {
            return m_Type;
        }
        public int getCode()
        {
            return m_Code;
        }
        public int getValue()
        {
            return m_Value;
        }
        public void setValue(int p_Value)
        {
            m_Value = p_Value;
        }
        public string getText()
        {
            return m_Text;
        }
        public void setText(string p_Text)
        {
            m_Text = p_Text;
        }

    }
}
