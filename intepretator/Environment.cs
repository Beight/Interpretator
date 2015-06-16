using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Intepretator
{
    class Environment
    {
        private Block[] m_BlockArray;
        private int m_BlockCounter, m_Current;
        private FileManager m_File;
        private List<Token> m_Operat;
        private List<Token> m_Operand;
        private Template[] m_BlockTemplates;
        private List<AR> m_ArStack;
        private RichTextBox m_Tbout;
        private MyControl m_Matrix;


        public Environment(FileManager p_filemanager, RichTextBox p_tb)
        {
            m_File = p_filemanager;
            m_Tbout = p_tb;
            m_BlockCounter = 0;
            m_Operat = new List<Token>();
            m_Operand = new List<Token>();
            m_ArStack = new List<AR>();
            m_Matrix = new MyControl();
            m_Current = 0;
        }
        
        //Ett block
        public void Enter(int b_nr)
        {
            m_ArStack.Add(new AR(m_Current, m_BlockTemplates[b_nr]));
            //Allocate(b_nr, 0);
            SetCurrent();
            IntepretBlock(b_nr);
        }


        public void Allocate(int b_nr, int b_lvl)
        {
        }

        public void SetCurrent()
        {
            m_Current = m_ArStack.Count - 1;
            //sätter current
        }


        public void IntepretBlock(int b_nr)
        {
            for (int i = 0; i < m_BlockArray[m_Current].getTokenCount(); i++)
                switch (m_ArStack[m_Current].getTempBlock().getToken(i).getType())
                {
                    case 1:
                        if (m_ArStack[m_Current].getTempBlock().getToken(i).getCode() > 18)
                        {
                            OperandAdd(i);
                        }
                        else
                        {
                            if (m_ArStack[m_Current].getTempBlock().getToken(i).getCode() == 2)
                            {
                                m_Tbout.AppendText("Block " + m_Current + " Execution Complete\n");
                                break;
                            }

                        }
                        //symId name
                        continue;
                    case 2:
                        OperandAdd(i);
                        //Int
                        continue;
                    case 3:
                        //Float
                        continue;
                    case 4:
                        //Text
                        continue;
                    case 5:
                        if (m_Operat.Count == 0)
                        {
                            OperatAdd(i);
                        }
                        else
                        {
                            Actions(m_Matrix.GetAction(operatAction(m_Operat[m_Operat.Count - 1].getText()), operatAction(m_ArStack[m_Current].getTempBlock().getToken(i).getText())), i);
                        }
                        //opcode
                        continue;
                    case 6:
                        //Error
                        continue;
                    case 7:
                        //line
                        continue;
                    case 10:
                        m_Tbout.AppendText("Enter Block " + m_ArStack[m_Current].getTempBlock().getToken(i).getCode() + "\n");
                        Enter(m_ArStack[m_Current].getTempBlock().getToken(i).getCode());
                        m_Current = m_ArStack[m_Current].getTempBlock().getStaticF();
                        m_ArStack.RemoveAt(m_ArStack.Count - 1);
                        //call block
                        continue;
                }
        }

        public void Actions(char c, int i)
        {
            switch (c)
            {
                case 'U':
                    m_Tbout.AppendText("case U\n");
                    //execute
                    m_Tbout.AppendText("Execute " + m_Operand[m_Operand.Count - 2].getText() + " " + m_Operat[m_Operat.Count - 1].getText() + " " + m_Operand[m_Operand.Count - 1].getText() + "\n");
                    Execute(m_Operand[m_Operand.Count - 2], m_Operat[m_Operat.Count - 1], m_Operand[m_Operand.Count - 1], i);
                    break;
                case 'S':
                    m_Tbout.AppendText("case S\n");
                    OperatAdd(i);
                    if (m_Operat[m_Operat.Count - 1].getText() == "(" && m_Operand[m_Operand.Count - 1].getCode() > 18)
                    {
                        m_Operat.RemoveAt(m_Operat.Count - 1);
                        m_Operat.Add(new Token(0, 0, "F("));
                    }

                    //stack operator
                    break;
                case 'A':
                  //  m_Tbout.AppendText("case A\n");
                    //Accept
                    break;
                case 'E':
                  //  m_Tbout.AppendText("case E\n");
                    //error
                    break;
                case 'F':
                    m_Tbout.AppendText("case F\n");
                    //user function
                    break;
                case 'C':
                    m_Tbout.AppendText("case C\n");
                    //function argument
                    break;
                case 'P':
                    m_Tbout.AppendText("case P\n");
                    //Remove
                    break;
                case 'T':
                    m_Tbout.AppendText("case T\n");
                    //transfer parameter
                    break;
                case 'L':
                    m_Tbout.AppendText("case L\n");
                    m_ArStack.Add(new AR(0, m_BlockTemplates[1]));
                    int temp_par = m_ArStack[m_ArStack.Count - 1].getTempBlock().getBlockNr();
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
                    for (int i = 0; i < m_ArStack[m_Current].getTempBlock().getSymbolCount(); i++)
                    {
                        if (m_ArStack[m_Current].getTempBlock().getSymbol(i).getText() == p_Operand1.getText())
                        {
                            m_ArStack[m_Current].getTempBlock().getSymbol(i).setValue(temp);
                            TboutOperationExec();
                            m_Tbout.AppendText(m_ArStack[m_Current].getTempBlock().getSymbol(i).getText() + " = " + m_ArStack[m_Current].getTempBlock().getSymbol(i).getValue() + "\n");
                            RemoveFromStack(p_Operand1, p_Operand2, p_Operat);
                        }
                        else if (m_Current != 0)
                        {
                            for (int j = 0; j < m_ArStack[m_ArStack[m_Current].getStaticF()].getTempBlock().getSymbolCount(); j++)
                            {
                                if (m_ArStack[m_ArStack[m_Current].getStaticF()].getTempBlock().getSymbol(j).getText() == p_Operand1.getText())
                                {
                                    m_ArStack[m_ArStack[m_Current].getStaticF()].getTempBlock().getSymbol(j).setValue(temp);
                                    TboutOperationExec();
                                    m_Tbout.AppendText(m_ArStack[m_ArStack[m_Current].getStaticF()].getTempBlock().getSymbol(j).getText() + " = " + m_ArStack[m_ArStack[m_Current].getStaticF()].getTempBlock().getSymbol(j).getValue() + "\n");
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
                    if (m_ArStack[m_Current].getTempBlock().getToken(p_I).getText() != ";")
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
            for (int i = 0; i < m_ArStack.Count; i++)
            {
                for (int j = 0; j < m_ArStack[i].getTempBlock().getSymbolCount(); j++)
                {
                    if (m_ArStack[i].getTempBlock().getSymbol(j).getText() == p_Operand1.getText())
                    {
                        p_Operand1.setValue(m_ArStack[i].getTempBlock().getSymbol(j).getValue());
                    }
                    //break
                    if (m_ArStack[i].getTempBlock().getSymbol(j).getText() == p_Operand2.getText())
                    {
                        p_Operand2.setValue(m_ArStack[i].getTempBlock().getSymbol(j).getValue());
                    }
                }
            }

        }

        public void RemoveFromStack(Token p_Operand1, Token p_Operand2, Token p_Operat)
        {
            m_Operand.Remove(p_Operand1);
            m_Operand.Remove(p_Operand2);
            m_Operat.Remove(p_Operat);
        }

        public void ContinueExec(int p_I)
        {
            if (m_Operat.Count > 0 && m_Operand.Count > 1)
            {
                Execute(m_Operand[m_Operand.Count - 2], m_Operat[m_Operat.Count - 1], m_Operand[m_Operand.Count - 1], p_I);
            }
        }

        public void CreateBlocks()
        {
            string[][] blockStringArray = m_File.getBlockStringArray();
            for (int i = 0; i < blockStringArray.Length; i++)
            {
                for (int j = 0; j < blockStringArray[i].Length; j++)
                {
                    switch (blockStringArray[i][j])
                    {
                        case "###PROGRAM###":
                            int block_count = int.Parse(blockStringArray[i + 1][0]);
                            m_BlockArray = new Block[block_count];
                            m_BlockTemplates = new Template[block_count];
                            continue;
                        case "##BLOCK##":
                            m_BlockArray[m_BlockCounter] = new Block(int.Parse(blockStringArray[i + 1][0]),
                                int.Parse(blockStringArray[i + 1][1]), int.Parse(blockStringArray[i + 1][2]),
                                int.Parse(blockStringArray[i + 1][3]), int.Parse(blockStringArray[i + 1][4]));
                            m_BlockTemplates[m_BlockCounter] = new Template(m_BlockArray[m_BlockCounter]);
                            continue;
                        case "#KOD#":
                            int t = -1;
                            for (int l = i; l < i + m_BlockArray[m_BlockCounter].getTokenCount(); l++)
                            {
                                m_BlockArray[m_BlockCounter].setToken(t + 1, new Token(int.Parse(blockStringArray[l + 1][0]),
                                                                                int.Parse(blockStringArray[l + 1][1]), blockStringArray[l + 1][2]));
                                t++;
                            }
                            continue;
                        case "#DEKLARATIONER#":
                            int d = -1;
                            for (int l = i; l < i + m_BlockArray[m_BlockCounter].getSymbolCount(); l++)
                            {
                                m_BlockArray[m_BlockCounter].setSymbol(d + 1, new Symbol(int.Parse(blockStringArray[l + 1][0]),
                                                                                int.Parse(blockStringArray[l + 1][1]), int.Parse(blockStringArray[l + 1][2]),
                                                                                int.Parse(blockStringArray[l + 1][3]), int.Parse(blockStringArray[l + 1][4]),
                                                                                int.Parse(blockStringArray[l + 1][5]), int.Parse(blockStringArray[l + 1][6]),
                                                                                blockStringArray[l + 1][9], 0));
                                d++;
                            }
                            continue;
                        case "##BLOCKSLUT##":
                            m_BlockCounter++;
                            continue;
                        case "###PROGRAMSLUT###":
                            Enter(m_Current);
                            break;
                    }
                }
            }
        }

        //Methods for Append text in textbox.
        public void TboutOperationExec()
        {
            m_Tbout.AppendText("Operation Executed \n");
        }

        public void OperandAdd(int i)
        {
            m_Operand.Add(m_ArStack[m_Current].getTempBlock().getToken(i));
            m_Tbout.AppendText("Operand Added: " + m_ArStack[m_Current].getTempBlock().getToken(i).getText() + "\n");
        }

        public void OperatAdd(int i)
        {
            m_Operat.Add(m_ArStack[m_Current].getTempBlock().getToken(i));
            m_Tbout.AppendText("Operator Added: " + m_ArStack[m_Current].getTempBlock().getToken(i).getText() + "\n");
        }

        public void Addition(Token p_Operand1, Token p_Operand2)
        {
            SetTokenValue(p_Operand1, p_Operand2);
            m_Operand.Add(new Token(p_Operand1.getValue() + p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_Operand[m_Operand.Count - 1].setText(m_Operand[m_Operand.Count - 1].getValue().ToString());
            TboutOperationExec();
            m_Tbout.AppendText(p_Operand1.getText() + " + " + p_Operand2.getText() + " = " + m_Operand[m_Operand.Count - 1].getValue() + "\n");
        }

        public void Subtraction(Token p_Operand1, Token p_Operand2)
        {
            SetTokenValue(p_Operand1, p_Operand2);
            m_Operand.Add(new Token(p_Operand1.getValue() - p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_Operand[m_Operand.Count - 1].setText(m_Operand[m_Operand.Count - 1].getValue().ToString());
            TboutOperationExec();
            m_Tbout.AppendText(p_Operand1.getText() + " - " + p_Operand2.getText() + " = " + m_Operand[m_Operand.Count - 1].getValue() + "\n");
        }

        public void Multiplication(Token p_Operand1, Token p_Operand2)
        {
            SetTokenValue(p_Operand1, p_Operand2);
            m_Operand.Add(new Token(p_Operand1.getValue() * p_Operand2.getValue(), p_Operand1.getCode(), p_Operand1.getType()));
            m_Operand[m_Operand.Count - 1].setText(m_Operand[m_Operand.Count - 1].getValue().ToString());
            TboutOperationExec();
            m_Tbout.AppendText(p_Operand1.getText() + " * " + p_Operand2.getText() + " = " + m_Operand[m_Operand.Count - 1].getValue() + "\n");
        }
    }
}
