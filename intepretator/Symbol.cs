using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Symbol
    {
        private int m_Id;
        private int m_Type;
        private int m_Kind;
        private int m_Info;
        private int m_Info2;
        private int m_Info3;
        private int m_Address;
        private int m_Boundry;
        private int m_Value;
        private string m_Text;

        public Symbol(int p_Id, int p_Type, int p_Kind, int p_Info, int p_Info2, int p_Info3, int p_Address, string p_Text, int p_Boundry)
        {
            m_Id = p_Id;
            m_Type = p_Type;
            m_Kind = p_Kind;  
            m_Info = p_Info;
            m_Info2 = p_Info2;
            m_Info3 = p_Info3;
            m_Address = p_Address;
            m_Text = p_Text;
            m_Boundry = p_Boundry;
        }

        public string getText()
        {
            return m_Text;
        }
        public int getValue()
        {
            return m_Value;
        }

        public void setValue(int p_Value)
        {
            m_Value = p_Value;
        }
    }
}
