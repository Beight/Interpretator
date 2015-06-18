using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class AR
    {
        private int m_staticF;
        private int m_dynamicF;
        private int m_returnAddress;
        private Template m_templateAddress;

        public AR(int p_dynamicF, Template p_templateAddress)
        {
            m_dynamicF = p_dynamicF;
            m_templateAddress = p_templateAddress;
            m_staticF = m_templateAddress.getBlock().getStaticF();
        }

        public Block getTempBlock()
        {
            return m_templateAddress.getBlock();
        }

        public int getStaticF()
        {
            return m_staticF;
        }
        public int getDynamicF()
        {
            return m_dynamicF;
        }
    }
}
