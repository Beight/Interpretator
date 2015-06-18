using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Intepretator
{
    class Environment
    {
        private Block[] m_blockArray;
        private int m_blockCounter;
        private int m_current;
        private FileManager m_file;
        private List<Token> m_operators;
        private List<Token> m_operands;
        private Template[] m_blockTemplates;
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
            m_current = 0;
        }
        
        //Ett block
        public void Enter(int b_nr)
        {
            m_arStack.Add(new AR(m_current, m_blockTemplates[b_nr]));
            //Allocate(b_nr, 0);
            SetCurrent();
            InterpretBlock(b_nr);
        }


        public void Allocate(int b_nr, int b_lvl)
        {
        }

        public void SetCurrent()
        {
            m_current = m_arStack.Count - 1;
            //sätter current
        }


        public void InterpretBlock(int b_nr)
        {
            //Loop through all the tokens
            for (int i = 0; i < m_arStack[m_current].getTokenCount(); i++)
            {
                //Switch case for finding token type.
                switch (m_arStack[m_current].getToken(i).getType())
                {
                    //Symbol
                    case 1:
                        if (m_arStack[m_current].getToken(i).getCode() > 18)
                        {
                            OperandAdd(i);
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
                        OperandAdd(i);                       
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
                            OperatAdd(i);
                        }
                        else
                        {
                            Actions(m_matrix.GetAction(operatAction(m_operators[m_operators.Count - 1].getText()), operatAction(m_arStack[m_current].getToken(i).getText())), i);
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
                        Enter(m_arStack[m_current].getToken(i).getCode());
                        m_current = m_arStack[m_current].getStaticF();
                        m_arStack.RemoveAt(m_arStack.Count - 1);
                        break;
                }
            }
        }

        public void Actions(char c, int i)
        {
            switch (c)
            {
                case 'U':
                    m_tbout.AppendText("case U\n");
                    //execute
                    m_tbout.AppendText("Execute " + m_operands[m_operands.Count - 2].getText() + " " + m_operators[m_operators.Count - 1].getText() + " " + m_operands[m_operands.Count - 1].getText() + "\n");
                    Execute(m_operands[m_operands.Count - 2], m_operators[m_operators.Count - 1], m_operands[m_operands.Count - 1], i);
                    break;
                case 'S':
                    m_tbout.AppendText("case S\n");
                    OperatAdd(i);
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
                    //user function
                    break;
                case 'C':
                    m_tbout.AppendText("case C\n");
                    //function argument
                    break;
                case 'P':
                    m_tbout.AppendText("case P\n");
                    //Remove
                    break;
                case 'T':
                    m_tbout.AppendText("case T\n");
                    //transfer parameter
                    break;
                case 'L':
                    m_tbout.AppendText("case L\n");
                    m_arStack.Add(new AR(0, m_blockTemplates[1]));
                    //int temp_par = m_arStack[m_arStack.Count - 1].getBlock().getBlockNr();
                    //AR_stack[AR_stack.Count - 1].GetTempBlock().symbols[i]
                    //transfer last parameter
                    break;
            }
        }

        public void Execute(Token p_Operand1, Token p_Operat, Token p_Operand2, int p_I)// tillfällig int
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
                            TboutOperationExec();
                            m_tbout.AppendText(m_arStack[m_current].getSymbol(i).getText() + " = " + m_arStack[m_current].getSymbol(i).getValue() + "\n");
                            RemoveFromStack(p_Operand1, p_Operand2, p_Operat);
                        }
                        else if (m_current != 0)
                        {
                            for (int j = 0; j < m_arStack[m_arStack[m_current].getStaticF()].getSymbolCount(); j++)
                            {
                                if (m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).getText() == p_Operand1.getText())
                                {
                                    m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).setValue(temp);
                                    TboutOperationExec();
                                    m_tbout.AppendText(m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).getText() + " = " + m_arStack[m_arStack[m_current].getStaticF()].getSymbol(j).getValue() + "\n");
                                    RemoveFromStack(p_Operand1, p_Operand2, p_Operat);
                                }
                            }
                        }
                    }
                    break;
                case "+":
                    Addition(p_Operand1, p_Operand2);
                    RemoveFromStack(p_Operand1, p_Operand2, p_Operat);
                    ContinueExec(p_I);
                    break;
                case "-":
                    Subtraction(p_Operand1, p_Operand2);
                    RemoveFromStack(p_Operand1, p_Operand2, p_Operat);
                    ContinueExec(p_I);
                    break;
                case "*":
                    Multiplication(p_Operand1, p_Operand2);
                    RemoveFromStack(p_Operand1, p_Operand2, p_Operat);
                    if (m_arStack[m_current].getToken(p_I).getText() != ";")
                    {
                        OperatAdd(p_I);
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

        public void SetTokenValue(Token p_Operand1, Token p_Operand2)
        {
            for (int i = 0; i < m_arStack.Count; i++)
            {
                for (int j = 0; j < m_arStack[i].getSymbolCount(); j++)
                {
                    if (m_arStack[i].getSymbol(j).getText() == p_Operand1.getText())
                    {
                        p_Operand1.setValue(m_arStack[i].getSymbol(j).getValue());
                    }
                    //break
                    if (m_arStack[i].getSymbol(j).getText() == p_Operand2.getText())
                    {
                        p_Operand2.setValue(m_arStack[i].getSymbol(j).getValue());
                    }
                }
            }

        }

        public void RemoveFromStack(Token p_Operand1, Token p_Operand2, Token p_Operat)
        {
            m_operands.Remove(p_Operand1);
            m_operands.Remove(p_Operand2);
            m_operators.Remove(p_Operat);
        }

        public void ContinueExec(int p_I)
        {
            if (m_operators.Count > 0 && m_operands.Count > 1)
            {
                Execute(m_operands[m_operands.Count - 2], m_operators[m_operators.Count - 1], m_operands[m_operands.Count - 1], p_I);
            }
        }

        public void CreateBlocks()
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
                            m_blockArray = new Block[block_count];
                            m_blockTemplates = new Template[block_count];
                            break;
                        case "##BLOCK##":
                            m_blockArray[m_blockCounter] = new Block(int.Parse(blockStringArray[i + 1][0]),
                                int.Parse(blockStringArray[i + 1][1]), int.Parse(blockStringArray[i + 1][2]),
                                int.Parse(blockStringArray[i + 1][3]), int.Parse(blockStringArray[i + 1][4]));
                            m_blockTemplates[m_blockCounter] = new Template(m_blockArray[m_blockCounter]);
                            break;
                        case "#KOD#":
                            int t = -1;
                            for (int l = i; l < i + m_blockArray[m_blockCounter].getTokenCount(); l++)
                            {
                                m_blockArray[m_blockCounter].setToken(t + 1, new Token(int.Parse(blockStringArray[l + 1][0]),
                                                                                int.Parse(blockStringArray[l + 1][1]), blockStringArray[l + 1][2]));
                                t++;
                            }
                            break;
                        case "#DEKLARATIONER#":
                            int d = -1;
                            for (int l = i; l < i + m_blockArray[m_blockCounter].getSymbolCount(); l++)
                            {
                                m_blockArray[m_blockCounter].setSymbol(d + 1, new Symbol(int.Parse(blockStringArray[l + 1][0]),
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
                            Enter(m_current);
                            break;
                    }
                }
            }
        }

        //Methods for Append text in textbox.
        public void TboutOperationExec()
        {
            m_tbout.AppendText("Operation Executed \n");
        }

        public void OperandAdd(int i)
        {
            m_operands.Add(m_arStack[m_current].getToken(i));
            m_tbout.AppendText("Operand Added: " + m_arStack[m_current].getToken(i).getText() + "\n");
        }

        public void OperatAdd(int i)
        {
            m_operators.Add(m_arStack[m_current].getToken(i));
            m_tbout.AppendText("Operator Added: " + m_arStack[m_current].getToken(i).getText() + "\n");
        }

        public void Addition(Token p_Operand1, Token p_Operand2)
        {
            SetTokenValue(p_Operand1, p_Operand2);
            m_operands.Add(new Token(p_Operand1.getValue() + p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_operands[m_operands.Count - 1].setText(m_operands[m_operands.Count - 1].getValue().ToString());
            TboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " + " + p_Operand2.getText() + " = " + m_operands[m_operands.Count - 1].getValue() + "\n");
        }

        public void Subtraction(Token p_Operand1, Token p_Operand2)
        {
            SetTokenValue(p_Operand1, p_Operand2);
            m_operands.Add(new Token(p_Operand1.getValue() - p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_operands[m_operands.Count - 1].setText(m_operands[m_operands.Count - 1].getValue().ToString());
            TboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " - " + p_Operand2.getText() + " = " + m_operands[m_operands.Count - 1].getValue() + "\n");
        }

        public void Multiplication(Token p_Operand1, Token p_Operand2)
        {
            SetTokenValue(p_Operand1, p_Operand2);
            m_operands.Add(new Token(p_Operand1.getValue() * p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_operands[m_operands.Count - 1].setText(m_operands[m_operands.Count - 1].getValue().ToString());
            TboutOperationExec();
            m_tbout.AppendText(p_Operand1.getText() + " * " + p_Operand2.getText() + " = " + m_operands[m_operands.Count - 1].getValue() + "\n");
        }
    }
}
