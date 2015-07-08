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
        private int m_nrOperands;
        private int m_nrOperators;


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
            m_nrOperators = 0;
            m_nrOperands = 0;
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
                            Symbol tempSym = null;

                            //Special case
                            //In some programs the function have the same SymbolId as a local symbol in the funciton block which makes it hard to 
                            //differentiate between the two and that creates a problem when a function is recursive.
                            //This if statement is supposed to go around that problem.
                            if(m_arStack[m_current].getToken(i).getText() == "F" && m_arStack[m_current].getToken(i+1).getText() == "(" && m_current != 0)
                            {
                                for (int j = 0; j < m_arStack.Count; j++)
                                {
                                    tempSym = searchSymbol(m_arStack[m_current].getToken(i).getCode(), m_current - j);

                                    if (tempSym.getKind() == 2)
                                        break;


                                    //if (m_arStack[m_current - 1].getSymbol(j).getId() == m_arStack[m_current].getToken(i).getCode())
                                    //{
                                    //    tempSym = m_arStack[m_current - 1].getSymbol(j);
                                    //    break;
                                    //}
                                }
                                
                            }
                            else
                                tempSym = searchSymbol(m_arStack[m_current].getToken(i).getCode());


                            if (tempSym.getKind() == 2)
                            {
                                    actions(m_matrix.GetAction(operatAction(m_operators[m_nrOperators - 1].getText()), operatAction(m_arStack[m_current].getToken(i).getText())), i);
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
                            if (m_arStack[m_current].getToken(i).getCode() == 10)
                            {
                                //if
                            }
                            if (m_arStack[m_current].getToken(i).getCode() == 11)
                            {
                                //then
                                actions('U', i);
                                if (m_operands[m_nrOperands - 1].getValue() == 1)
                                {
                                    m_operands.RemoveAt(m_nrOperands - 1);
                                    m_nrOperands--;
                                    break;
                                }
                                else 
                                {
                                    m_operands.RemoveAt(m_nrOperands - 1);
                                    m_nrOperands--;
                                    for (i = i + 1;  i < m_arStack[m_current].getTokenCount(); i++)
                                    {
                                        if (m_arStack[m_current].getToken(i).getCode() == 12)
                                            break;
                                    }
                                    break;
                                }
                            }
                            if (m_arStack[m_current].getToken(i).getCode() == 12)
                            {
                                i = m_arStack[m_current].getTokenCount() - 2;
                            }

                        }
                        break;
                    //Int
                    case 2:
                        operandAdd(i);                       
                        break;
                    //Float
                    case 3:
                        operandAdd(i);  
                        break;
                    //String
                    case 4:
                        break;
                    //Operator
                    case 5:
                        if (m_nrOperators == 0)
                        {
                            operatAdd(i);
                        }
                        else
                        {
                            actions(m_matrix.GetAction(operatAction(m_operators[m_nrOperators - 1].getText()), operatAction(m_arStack[m_current].getToken(i).getText())), i);
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
                    m_tbout.AppendText("execute " + m_operands[m_nrOperands - 2].getText() + " " + m_operators[m_nrOperators - 1].getText() + " " + m_operands[m_nrOperands - 1].getText() + "\n");
                    execute(m_operands[m_nrOperands - 2], m_operators[m_nrOperators - 1], m_operands[m_nrOperands - 1], i);
                    break;
                case 'S':
                    m_tbout.AppendText("case S\n");
                    operatAdd(i);

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
                    if(m_operators[m_nrOperators - 2].getText() == "F")
                    {
                        m_operators.Add(new Token(m_operators[m_nrOperators - 2].getCode(), m_operators[m_nrOperators - 2].getType(), "F("));
                        m_nrOperators++;
                        m_operators.RemoveAt(m_nrOperators - 2);
                        m_nrOperators--;
                        m_operators.RemoveAt(m_nrOperators - 2);
                        m_nrOperators--;
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
                    functionCall();

                    //transfer last parameter
                    break;
            }
        }

        public void execute(Token p_operand1, Token p_operator, Token p_operand2, int p_I)// tillfällig int
        {
            //Removes tokens from stack.
            removeFromStack();

            switch (p_operator.getText())
            {
                case ":=":
                    int temp;
                    temp = p_operand2.getValue();
                    for (int i = 0; i < m_arStack[m_current].getSymbolCount(); i++)
                    {
                        if (m_arStack[m_current].getSymbol(i).getText() == p_operand1.getText())
                        {
                            m_arStack[m_current].getSymbol(i).setValue(temp);
                            tboutOperationExec();
                            m_tbout.AppendText(m_arStack[m_current].getSymbol(i).getText() + " = " + m_arStack[m_current].getSymbol(i).getValue() + "\n");
                        }
                        else if (m_current != 0)
                        {
                            //Set value on the symbol in the surrounding block.
                            for (int j = 0; j < m_arStack[m_arStack[m_current].getDynamicF()].getSymbolCount(); j++)
                            {
                                if (m_arStack[m_arStack[m_current].getDynamicF()].getSymbol(j).getText() == p_operand1.getText())
                                {
                                    m_arStack[m_arStack[m_current].getDynamicF()].getSymbol(j).setValue(temp);
                                    tboutOperationExec();
                                    m_tbout.AppendText(m_arStack[m_arStack[m_current].getDynamicF()].getSymbol(j).getText() + " = " + m_arStack[m_arStack[m_current].getDynamicF()].getSymbol(j).getValue() + "\n");
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                case "+":
                    addition(p_operand1, p_operand2);
                    continueExec(p_I);
                    break;
                case "-":
                    subtraction(p_operand1, p_operand2);
                    continueExec(p_I);
                    break;
                case "*":
                    multiplication(p_operand1, p_operand2);
                    if (m_arStack[m_current].getToken(p_I).getText() != ";")
                    {
                        operatAdd(p_I);
                    }
                    break;
                case "/":
                    break;
                case ">":
                    greaterThan(p_operand1, p_operand2);
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

        public void removeFromStack()
        {
            m_operands.RemoveAt(m_nrOperands - 1);
            m_nrOperands--;

            m_operands.RemoveAt(m_nrOperands - 1);
            m_nrOperands--;

            m_operators.RemoveAt(m_nrOperators - 1);
            m_nrOperators--;
        }

        public void continueExec(int p_i)
        {
            if (m_nrOperators > 0 && m_nrOperands > 1)
            {
                if (m_operators[m_nrOperators - 1].getText() == "F(")
                {
                    functionCall();
                    continueExec(p_i);
                }
                else
                    execute(m_operands[m_nrOperands - 2], m_operators[m_nrOperators - 1], m_operands[m_nrOperands - 1], p_i);
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
                            for (int l = i; l < i + m_blockTemplates[m_blockCounter].getTokenCount(); l++)
                            {
                                m_blockTemplates[m_blockCounter].addToken(new Token(int.Parse(blockStringArray[l + 1][0]),
                                                                                int.Parse(blockStringArray[l + 1][1]), blockStringArray[l + 1][2]));
                            }
                            break;
                        case "#DEKLARATIONER#":
                            for (int l = i; l < i + m_blockTemplates[m_blockCounter].getSymbolCount(); l++)
                            {
                                m_blockTemplates[m_blockCounter].addSymbol(new Symbol(int.Parse(blockStringArray[l + 1][0]),
                                                                                int.Parse(blockStringArray[l + 1][1]), int.Parse(blockStringArray[l + 1][2]),
                                                                                int.Parse(blockStringArray[l + 1][3]), int.Parse(blockStringArray[l + 1][4]),
                                                                                int.Parse(blockStringArray[l + 1][5]), int.Parse(blockStringArray[l + 1][6]),
                                                                                blockStringArray[l + 1][9], 0));
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
            m_nrOperands++;
            m_tbout.AppendText("Operand Added: " + m_arStack[m_current].getToken(i).getText() + "\n");
        }

        public void operatAdd(int i)
        {
            m_operators.Add(m_arStack[m_current].getToken(i));
            m_nrOperators++;
            m_tbout.AppendText("Operator Added: " + m_arStack[m_current].getToken(i).getText() + "\n");
        }

        public void addition(Token p_Operand1, Token p_Operand2)
        {
            m_operands.Add(new Token(p_Operand1.getValue() + p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_nrOperands++;
            m_operands[m_nrOperands - 1].setText(m_operands[m_nrOperands - 1].getValue().ToString());
            tboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " + " + p_Operand2.getText() + " = " + m_operands[m_nrOperands - 1].getValue() + "\n");
        }

        public void subtraction(Token p_Operand1, Token p_Operand2)
        {
            m_operands.Add(new Token(p_Operand1.getValue() - p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_nrOperands++;
            m_operands[m_nrOperands - 1].setText(m_operands[m_nrOperands - 1].getValue().ToString());
            tboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " - " + p_Operand2.getText() + " = " + m_operands[m_nrOperands - 1].getValue() + "\n");
        }

        public void multiplication(Token p_Operand1, Token p_Operand2)
        {
            m_operands.Add(new Token(p_Operand1.getValue() * p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_nrOperands++;
            m_operands[m_nrOperands - 1].setText(m_operands[m_nrOperands - 1].getValue().ToString());
            tboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " * " + p_Operand2.getText() + " = " + m_operands[m_nrOperands - 1].getValue() + "\n");
        }
        public void greaterThan(Token p_Operand1, Token p_Operand2)
        {
            if (p_Operand1.getValue() > p_Operand2.getValue())
            {
                m_operands.Add(new Token(0, 0, "true", 1));
                m_nrOperands++;
            }
            else
            {
                m_operands.Add(new Token(0, 0, "false", 0));
                m_nrOperands++;
            }

            m_tbout.AppendText(p_Operand1.getText() + " > " + p_Operand2.getText() + " = " + m_operands[m_nrOperands - 1].getText() + "\n");
        }

        public void functionCall()
        {
            Symbol tempSym = null;
            for (int i = 0; i < m_arStack.Count; i++)
            {
                tempSym = searchSymbol(m_operators[m_nrOperators - 1].getCode(), m_current - i);
                if(tempSym.getKind() == 2)
                    break;
            }
            


            //Symbol info2 contains information about which block id contains the function.
            m_arStack.Add(new AR(m_current, m_blockTemplates[tempSym.getInfo(2)]));
            setCurrent();
            int nrSymbols = m_arStack[m_current].getSymbolCount();

            //Find the symbol with the address to the last parameter.
            for (int i = 0; i < nrSymbols; i++)
            {
                //When the symbol corresponding to the last parameter is found, save the value of the parameter to the symbol.
                if (m_arStack[m_current].getSymbol(i).getAddress() == 1)
                {
                    m_arStack[m_current].getSymbol(i).setValue(m_operands[m_nrOperands - m_arStack[m_current].getSymbol(i).getAddress()].getValue());
                    //not sure if this should be removed here or somewhere else.
                    m_operands.RemoveAt(m_nrOperands - m_arStack[m_current].getSymbol(i).getAddress());
                    m_nrOperands--;
                    break;
                }
            }
            m_tbout.AppendText("Enter function\n");
            interpretBlock();

            tempSym = searchSymbol(m_operators[m_nrOperators - 1].getCode(), m_current);


            //Save return value as operand
            m_operands.Add(new Token(tempSym.getId(), tempSym.getType(), tempSym.getText()));
            m_nrOperands++;
            m_operands[m_nrOperands - 1].setValue(tempSym.getValue());
            //Remove function operator
            m_operators.RemoveAt(m_nrOperators - 1);
            m_nrOperators--;
            m_arStack.RemoveAt(m_arStack.Count - 1);
            setCurrent();
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

        public Symbol searchSymbol(int p_id, int p_blkId)
        {
            Symbol ret = m_arStack[p_blkId].searchSymbol(p_id);

            return ret;
        }
    }
}
