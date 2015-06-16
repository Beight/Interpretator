using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class AR
    {
        private int m_StaticF;
        private int m_DynamicF;
        private int m_ReturnAddress;
        private Template m_TemplateAddress;

        public AR(int p_DynamicF, Template p_TemplateAddress)
        {
            m_DynamicF = p_DynamicF;
            m_TemplateAddress = p_TemplateAddress;
            m_StaticF = m_TemplateAddress.getBlock().getStaticF();
        }

        public Block getTempBlock()
        {
            return m_TemplateAddress.getBlock();
        }

        public int getStaticF()
        {
            return m_StaticF;
        }
        public int getDynamicF()
        {
            return m_DynamicF;
        }
    }
}
