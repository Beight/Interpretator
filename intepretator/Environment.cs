using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Intepretator
{
    class Environment
    {
        private int m_blockCounter;
        private int m_current;
        private FileManager m_file;
        private List<Token> m_operators;
        private List<Token> m_operands;
        private List<Template> m_blockTemplates;
        private List<AR> m_arStack;
        private RichTextBox m_tbout;
        private MyControl m_matrix;


        public Environment(FileManager p_filemanager, RichTextBox p_tb)
        {
            m_file = p_filemanager;
            m_tbout = p_tb;
            m_blockCounter = 0;
            m_operators = new List<Token>();
            m_operands = new List<Token>();
            m_arStack = new List<AR>();
            m_matrix = new MyControl();
            //m_blockTemplates.Clear();
            m_current = 0;
        }

        public void allocate(int b_nr, int b_lvl)
        {
        }

        public void setCurrent()
        {
            m_current = m_arStack.Count - 1;
            //sätter current
        }


        public void interpretBlock()
        {
            //Loop through all the tokens
            for (int i = 0; i < m_arStack[m_current].getTokenCount(); i++)
            {
                //Switch case for finding token type.
                switch (m_arStack[m_current].getToken(i).getType())
                {
                    //Symbol
                    case 1:
                        //search for symbol to find if it's a function or not.
                        if (m_arStack[m_current].getToken(i).getCode() > 18)
                        {
                            Symbol tempSym = searchSymbol(m_arStack[m_current].getToken(i).getCode());
                            if (tempSym.getKind() == 2)
                            {
                                actions(m_matrix.GetAction(operatAction(m_operators[m_operators.Count - 1].getText()), operatAction(m_arStack[m_current].getToken(i).getText())), i);
                                break;
                            }
                            else
                            {
                                m_arStack[m_current].getToken(i).setValue(tempSym.getValue());
                                operandAdd(i);
                            }
                        }
                        else
                        {
                            if (m_arStack[m_current].getToken(i).getCode() == 2)
                            {
                                m_tbout.AppendText("Block " + m_current + " Execution Complete\n");
                                break;
                            }

                        }
                        break;
                    //Int
                    case 2:
                        operandAdd(i);                       
                        break;
                    //Float
                    case 3:
                        break;
                    //String
                    case 4:
                        break;
                    //Operator
                    case 5:
                        if (m_operators.Count == 0)
                        {
                            operatAdd(i);
                        }
                        else
                        {
                            actions(m_matrix.GetAction(operatAction(m_operators[m_operators.Count - 1].getText()), operatAction(m_arStack[m_current].getToken(i).getText())), i);
                        }
                        break;
                    //Error code
                    case 6:
                        break;
                    //Line breaks
                    case 7:
                        break;
                    //Block calls
                    case 10:
                        m_tbout.AppendText("Enter Block " + m_arStack[m_current].getToken(i).getCode() + "\n");

                        m_arStack.Add(new AR(m_current, m_blockTemplates[m_arStack[m_current].getToken(i).getCode()]));
                        setCurrent();
                        interpretBlock();
                        m_current = m_arStack[m_current].getDynamicF();
                        m_arStack.RemoveAt(m_arStack.Count - 1);
                        break;
                }
            }
        }

        public void actions(char c, int i)
        {
            switch (c)
            {
                case 'U':
                    m_tbout.AppendText("case U\n");
                    //execute
                    m_tbout.AppendText("execute " + m_operands[m_operands.Count - 2].getText() + " " + m_operators[m_operators.Count - 1].getText() + " " + m_operands[m_operands.Count - 1].getText() + "\n");
                    execute(m_operands[m_operands.Count - 2], m_operators[m_operators.Count - 1], m_operands[m_operands.Count - 1], i);
                    break;
                case 'S':
                    m_tbout.AppendText("case S\n");
                    operatAdd(i);
                    //if (m_operators[m_operators.Count - 1].getText() == "(" && m_operands[m_operands.Count - 1].getCode() > 18)
                    //{
                    //    m_operators.RemoveAt(m_operators.Count - 1);
                    //    m_operators.Add(new Token(0, 0, "F("));
                    //}

                    //stack operator
                    break;
                case 'A':
                    m_tbout.AppendText("case A\n");
                    //Accept
                    break;
                case 'E':
                    m_tbout.AppendText("case E\n");
                    //error
                    break;
                case 'F':
                    m_tbout.AppendText("case F\n");
                    operatAdd(i);
                    //user function
                    break;
                case 'C':
                    m_tbout.AppendText("case C\n");
                    operatAdd(i);
                    if(m_operators[m_operators.Count - 2].getText() == "F")
                    {
                        m_operators.Add(new Token(m_operators[m_operators.Count - 2].getCode(), m_operators[m_operators.Count - 2].getType(), "F("));
                        m_operators.RemoveAt(m_operators.Count - 2);
                        m_operators.RemoveAt(m_operators.Count - 2);    
                    }

                    //function argument
                    break;
                case 'P':
                    m_tbout.AppendText("case P\n");
                    //Remove(from stack and skip)
                    break;
                case 'T':
                    m_tbout.AppendText("case T\n");
                    //transfer parameter
                    break;
                case 'L':
                    m_tbout.AppendText("case L\n");
                    //Symbol tempSym = m_arStack[m_current].searchSymbol(m_operators[m_operators.Count() - 1].getCode());
                    Symbol tempSym = searchSymbol(m_operators[m_operators.Count() - 1].getCode());
                    //Symbol info2 contains information about which block id contains the function.
                    m_arStack.Add(new AR(m_current, m_blockTemplates[tempSym.getInfo(2)]));
                    setCurrent();
                    int nrSymbols = m_arStack[m_current].getSymbolCount();

                    //Find the symbol with the address to the last parameter.
                    for (int j = 0; j < nrSymbols; j++)
                    {
                        //When the symbol corresponding to the last parameter is found, save the value of the parameter to the symbol.
                        if (m_arStack[m_current].getSymbol(j).getAddress() == 1)
                        {
                            //TODO: Need to search for symbol I to get the correct value on token.                           
                            m_arStack[m_current].getSymbol(j).setValue(m_operands[m_operands.Count() - m_arStack[m_current].getSymbol(j).getAddress()].getValue());
                            //not sure if this should be removed here or somewhere else.
                            m_operands.RemoveAt(m_operands.Count() - m_arStack[m_current].getSymbol(j).getAddress());
                            break;
                        }
                    }
                    m_tbout.AppendText("Enter function\n");
                    interpretBlock();
                    //Save return value as operand
                    m_operands.Add(new Token(tempSym.getId(), tempSym.getType(), tempSym.getText()));
                    m_operands[m_operands.Count() - 1].setValue(tempSym.getValue());
                    //Remove function operator
                    m_operators.RemoveAt(m_operators.Count() - 1);
                    m_arStack.RemoveAt(m_arStack.Count - 1);
                    setCurrent();
                    //transfer last parameter
                    break;
            }
        }

        public void execute(Token p_Operand1, Token p_Operat, Token p_Operand2, int p_I)// tillfällig int
        {
            switch (p_Operat.getText())
            {
                case ":=":
                    int temp;
                    temp = p_Operand2.getValue();
                    for (int i = 0; i < m_arStack[m_current].getSymbolCount(); i++)
                    {
                        if (m_arStack[m_current].getSymbol(i).getText() == p_Operand1.getText())
                        {
                            m_arStack[m_current].getSymbol(i).setValue(temp);
                            tboutOperationExec();
                            m_tbout.AppendText(m_arStack[m_current].getSymbol(i).getText() + " = " + m_arStack[m_current].getSymbol(i).getValue() + "\n");
                            removeFromStack(p_Operand1, p_Operand2, p_Operat);
                        }
                        else if (m_current != 0)
                        {
                            for (int j = 0; j < m_arStack[m_arStack[m_current].getStaticF()].getSymbolCount(); j++)
                            {
                                if (m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).getText() == p_Operand1.getText())
                                {
                                    m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).setValue(temp);
                                    tboutOperationExec();
                                    m_tbout.AppendText(m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).getText() + " = " + m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).getValue() + "\n");
                                    removeFromStack(p_Operand1, p_Operand2, p_Operat);
                                }
                            }
                        }
                    }
                    break;
                case "+":
                    addition(p_Operand1, p_Operand2);
                    removeFromStack(p_Operand1, p_Operand2, p_Operat);
                    continueExec(p_I);
                    break;
                case "-":
                    subtraction(p_Operand1, p_Operand2);
                    removeFromStack(p_Operand1, p_Operand2, p_Operat);
                    continueExec(p_I);
                    break;
                case "*":
                    multiplication(p_Operand1, p_Operand2);
                    removeFromStack(p_Operand1, p_Operand2, p_Operat);
                    if (m_arStack[m_current].getToken(p_I).getText() != ";")
                    {
                        operatAdd(p_I);
                    }
                    break;
                case "/":
                    break;
            }

        }


        public int operatAction(string p_operat)
        {
            switch (p_operat)
            { 
                case ":=":
                    return 1;
                case "+":
                    return 3;
                case "-":
                    return 3;
                case "*":
                    return 4;
                case "/":
                    return 4;
                case "=":
                    return 5;
                case "<>":
                    return 5;
                case "<":
                    return 5;
                case ">":
                    return 5;
                case "<=":
                    return 5;
                case ">=":
                    return 5;
                case "F":
                    return 6;
                case ",":
                    return 9;
                case "(":
                    return 10;
                case ")":
                    return 11;
                case "F(":
                    return 12;
                case ";":
                    return 14;
                default:
                    return -1;
            }
        }

        public void setTokenValue(Token p_Operand1, Token p_Operand2)
        {
            for (int i = 0; i < m_arStack.Count; i++)
            {
                for (int j = 0; j < m_arStack[i].getSymbolCount(); j++)
                {
                    if (m_arStack[i].getSymbol(j).getId() == p_Operand1.getCode())
                    {
                        p_Operand1.setValue(m_arStack[i].getSymbol(j).getValue());
                    }
                    //break
                    if (m_arStack[i].getSymbol(j).getId() == p_Operand2.getCode())
                    {
                        p_Operand2.setValue(m_arStack[i].getSymbol(j).getValue());
                    }
                }
            }

        }

        public void removeFromStack(Token p_Operand1, Token p_Operand2, Token p_Operat)
        {
            m_operands.Remove(p_Operand1);
            m_operands.Remove(p_Operand2);
            m_operators.Remove(p_Operat);
        }

        public void continueExec(int p_I)
        {
            if (m_operators.Count > 0 && m_operands.Count > 1)
            {
                execute(m_operands[m_operands.Count - 2], m_operators[m_operators.Count - 1], m_operands[m_operands.Count - 1], p_I);
            }
        }

        public void createBlocks()
        {
            string[][] blockStringArray = m_file.getBlockStringArray();
            for (int i = 0; i < blockStringArray.Length; i++)
            {
                for (int j = 0; j < blockStringArray[i].Length; j++)
                {
                    switch (blockStringArray[i][j])
                    {
                        case "###PROGRAM###":
                            int block_count = int.Parse(blockStringArray[i + 1][0]);
                            m_blockTemplates = new List<Template>(block_count);
                            break;
                        case "##BLOCK##":
                            Block b = new Block(int.Parse(blockStringArray[i + 1][0]),
                                                   int.Parse(blockStringArray[i + 1][1]), int.Parse(blockStringArray[i + 1][2]),
                                                   int.Parse(blockStringArray[i + 1][3]), int.Parse(blockStringArray[i + 1][4]));
                            m_blockTemplates.Add(new Template(b));
                            break;
                        case "#KOD#":
                            int t = -1;
                            for (int l = i; l < i + m_blockTemplates[m_blockCounter].getTokenCount(); l++)
                            {
                                m_blockTemplates[m_blockCounter].setToken(t + 1, new Token(int.Parse(blockStringArray[l + 1][0]),
                                                                                int.Parse(blockStringArray[l + 1][1]), blockStringArray[l + 1][2]));
                                t++;
                            }
                            break;
                        case "#DEKLARATIONER#":
                            int d = -1;
                            for (int l = i; l < i + m_blockTemplates[m_blockCounter].getSymbolCount(); l++)
                            {
                                m_blockTemplates[m_blockCounter].setSymbol(d + 1, new Symbol(int.Parse(blockStringArray[l + 1][0]),
                                                                                int.Parse(blockStringArray[l + 1][1]), int.Parse(blockStringArray[l + 1][2]),
                                                                                int.Parse(blockStringArray[l + 1][3]), int.Parse(blockStringArray[l + 1][4]),
                                                                                int.Parse(blockStringArray[l + 1][5]), int.Parse(blockStringArray[l + 1][6]),
                                                                                blockStringArray[l + 1][9], 0));
                                d++;
                            }
                            break;
                        case "##BLOCKSLUT##":
                            m_blockCounter++;
                            break;
                        case "###PROGRAMSLUT###":
                            m_arStack.Add(new AR(m_current, m_blockTemplates[m_current]));
                            interpretBlock();
                            break;
                    }
                }
            }
        }

        //Methods for Append text in textbox.
        public void tboutOperationExec()
        {
            m_tbout.AppendText("Operation executed \n");
        }

        public void operandAdd(int i)
        {
            m_operands.Add(m_arStack[m_current].getToken(i));
            m_tbout.AppendText("Operand Added: " + m_arStack[m_current].getToken(i).getText() + "\n");
        }

        public void operatAdd(int i)
        {
            m_operators.Add(m_arStack[m_current].getToken(i));
            m_tbout.AppendText("Operator Added: " + m_arStack[m_current].getToken(i).getText() + "\n");
        }

        public void addition(Token p_Operand1, Token p_Operand2)
        {
            //setTokenValue(p_Operand1, p_Operand2);
            m_operands.Add(new Token(p_Operand1.getValue() + p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_operands[m_operands.Count - 1].setText(m_operands[m_operands.Count - 1].getValue().ToString());
            tboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " + " + p_Operand2.getText() + " = " + m_operands[m_operands.Count - 1].getValue() + "\n");
        }

        public void subtraction(Token p_Operand1, Token p_Operand2)
        {
           // setTokenValue(p_Operand1, p_Operand2);
            m_operands.Add(new Token(p_Operand1.getValue() - p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_operands[m_operands.Count - 1].setText(m_operands[m_operands.Count - 1].getValue().ToString());
            tboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " - " + p_Operand2.getText() + " = " + m_operands[m_operands.Count - 1].getValue() + "\n");
        }

        public void multiplication(Token p_Operand1, Token p_Operand2)
        {
           // setTokenValue(p_Operand1, p_Operand2);
            m_operands.Add(new Token(p_Operand1.getValue() * p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_operands[m_operands.Count - 1].setText(m_operands[m_operands.Count - 1].getValue().ToString());
            tboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " * " + p_Operand2.getText() + " = " + m_operands[m_operands.Count - 1].getValue() + "\n");
        }

        public Symbol searchSymbol(int p_id)
        {
            Symbol ret = m_arStack[m_current].searchSymbol(p_id);
            if (ret == null)
            { 
                int sf = m_arStack[m_current].getStaticF();
                if (sf > -1)
                    ret = m_arStack[sf].searchSymbol(p_id);
            }

            return ret;
        }
    }
}
