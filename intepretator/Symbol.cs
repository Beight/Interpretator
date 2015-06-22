using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class Symbol
    {
        ///<summary>
        ///Symbol id(1, 2, ...).
        ///</summary>
        private int m_id;
        ///<summary>
        ///Symbol type(int, float, ...).
        ///</summary>
        private int m_type;
        ///<summary>
        ///Symbol Kind(simple, array, func).
        ///</summary>
        private int m_kind;
        ///<summary>
        ///<para>Info depending on kind.</para>
        ///<para>Kind = simple, not used.</para>
        ///<para>Kind = array, number of indices.</para>
        ///<para>Kind = function, number of parameters.</para>
        ///</summary>
        private int m_info;
        ///<summary>
        ///<para>Info depending on kind.</para>
        ///<para>Kind = simple, not used.</para>
        ///<para>Kind = array, not used.</para>
        ///<para>Kind = function, defining block index.</para>
        ///</summary>
        private int m_info2;
        ///<summary>
        ///<para>Info depending on kind</para>
        ///<para>Kind = simple, not used.</para>
        ///<para>Kind = array, qualification(if ref)/0.</para>
        ///<para>Kind = function, qualification(if ref)/0.</para>
        ///</summary>
        private int m_info3;
        ///<summary>
        ///Relative address or zero.
        ///</summary>
        private int m_address;
        ///<summary>
        ///Unknown.
        ///</summary>
        private int m_boundry;
        ///<summary>
        ///Symbol value if it has one.
        ///</summary>
        private int m_value;
        ///<summary>
        ///Symbol name in string form.
        ///</summary>
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
        public int getId()
        {
            return m_id;
        }
        public int getKind()
        {
            return m_kind;
        }
        public int getInfo(int p_nr)
        {
            if(p_nr == 1)
                return m_info;
            if (p_nr == 2)
                return m_info2;
            if (p_nr == 3)
                return m_info3;

            return -1;
        }

        public int getAddress()
        {
            return m_address;
        }

        public int getType()
        {
            return m_type;
        }
    }
}
