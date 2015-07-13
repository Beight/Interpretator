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
        private String m_output;


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
            m_nrOperators = 0;
            m_nrOperands = 0;
        }
        private void interpretBlock()
        {
            //Loop through all the tokens
            for (int i = 0; i < m_arStack[m_current].getTokenCount(); i++)
            {
                Token token = m_arStack[m_current].getToken(i);
                //Switch case for finding token type.
                switch (token.getType())
                {
                    //Symbol
                    case 1:
                        //If the code is above 18 the token is handled as a symbol or standard procedure.
                        if (token.getCode() > 18)
                        {
                            if (token.getText() == "write")
                            {
                                i = write(i);
                                break;
                            }
                            //Standard procedure that adds a new line to the output string.
                            else if (token.getText() == "writln")
                            {
                                m_output += "\n";
                                break;
                            }
                            //There were no clear explanation on how "readint" was supposed to be handled, the only intructions was that
                            //it would give you an int. Because of that i made it so it just creates an int of the value 1 and puts it on the operand stack.
                            //This is also a standard procedure but it wasn't required for this assignment but i needed to handle it somehow since it was
                            //used in the tests that used write and writeln.
                            else if (token.getText() == "readint")
                            {
                                m_operands.Add(new Token(1, 2, "1", 1));
                                m_nrOperands++;
                                break;
                            }
                            else
                                handleSymbol(token, i);

                        }
                        else
                        {
                            if (token.getCode() == 2)
                            {
                                m_tbout.AppendText("Block execution complete\n");
                                break;
                            }

                            //"if, then" statement, when the intepreter reads "then" it executes the 
                            //if statement on the stack and checks if the statement is true or false
                            if (token.getCode() == 11)
                            {
                                actions('U', i);
                                if (m_operands[m_nrOperands - 1].getValue() == 1)
                                {
                                    //statement was true continue as normal.
                                    operandPop();
                                    break;
                                }
                                else 
                                {
                                    //statement was false, skip all tokens until the else statement is reached and continue from there.
                                    operandPop();
                                    for (i = i + 1;  i < m_arStack[m_current].getTokenCount(); i++)
                                    {
                                        if (token.getCode() == 12)
                                            break;
                                    }
                                    break;
                                }
                            }
                            if (token.getCode() == 12)
                            {
                                //if the intepreter reaches this code it means that it has executed a "then" block of an if statement and the next
                                //part is the "else" statement which should not be executed so we skip past it to the end of the block.(Assuming the "else" statement is the last thing in the block)
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
                            if (token.getText() != ";")
                                operatAdd(i);
                        }
                        else
                        {
                            char c = m_matrix.GetAction(operatAction(m_operators[m_nrOperators - 1].getText()), operatAction(m_arStack[m_current].getToken(i).getText()));
                            actions(c, i);
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
                        m_tbout.AppendText("Enter Block " + token.getCode() + "\n");
                        m_arStack.Add(new AR(m_current, m_blockTemplates[token.getCode()]));
                        m_current = m_arStack.Count - 1;
                        interpretBlock();
                        m_current = m_arStack[m_current].getDynamicF();
                        m_arStack.RemoveAt(m_arStack.Count - 1);
                        break;
                }
            }
        }

        private void actions(char p_c, int p_i)
        {
            switch (p_c)
            {
                case 'U':
                    //execute
                    m_tbout.AppendText("Execute " + m_operands[m_nrOperands - 2].getText() + " " + m_operators[m_nrOperators - 1].getText() + " " + m_operands[m_nrOperands - 1].getText() + "\n");
                    execute(m_operands[m_nrOperands - 2], m_operators[m_nrOperators - 1], m_operands[m_nrOperands - 1], p_i);
                    break;
                case 'S':
                    operatAdd(p_i);
                    //stack operator
                    break;
                case 'A':
                    //Accept
                    break;
                case 'E':
                    //error
                    break;
                case 'F':
                    operatAdd(p_i);
                    //user function
                    break;
                case 'C':
                    //Convert operators "F" and "(" to "F("
                    m_operators.Add(new Token(m_operators[m_nrOperators - 1].getCode(), m_operators[m_nrOperators - 2].getType(), "F("));
                    m_nrOperators++;
                    m_operators.RemoveAt(m_nrOperators - 2);
                    m_nrOperators--;
                    //function argument
                    break;
                case 'P':
                    operatPop();
                    //Remove(from stack and skip)
                    break;
                case 'T':
                    //transfer parameter
                    break;
                case 'L':                   
                    functionCall();
                    break;
            }
        }

        private void execute(Token p_operand1, Token p_operator, Token p_operand2, int p_I)
        {
            //Removes tokens from stack.
            operandPop();
            operandPop();
            operatPop();

            switch (p_operator.getText())
            {
                case ":=":
                    equals(p_operand1, p_operand2);
                    break;
                case "+":
                    addition(p_operand1, p_operand2);
                    if (m_arStack[m_current].getToken(p_I).getText() != ";")
                        operatAdd(p_I);
                    continueExec(p_I);
                    break;
                case "-":
                    subtraction(p_operand1, p_operand2);
                    if (m_arStack[m_current].getToken(p_I).getText() != ";")
                        operatAdd(p_I);
                    continueExec(p_I);
                    break;
                case "*":
                    multiplication(p_operand1, p_operand2);
                    if (m_arStack[m_current].getToken(p_I).getText() != ";")
                        operatAdd(p_I);
                    break;
                case "/":
                    break;
                case ">":
                    greaterThan(p_operand1, p_operand2);
                    break;
            }

        }


        private int operatAction(string p_operat)
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

        private void continueExec(int p_i)
        {
            if (m_nrOperators > 0 && m_nrOperands > 1)
            {
                //remove left over end parenthesis.
                if (m_operators[m_nrOperators - 1].getText() == ")")
                    operatPop();

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
                            if(m_output != null)
                                m_tbout.AppendText("Output:\n" + m_output);
                            break;
                    }
                }
            }
        }



        private void operandAdd(int i)
        {
            m_operands.Add(m_arStack[m_current].getToken(i));
            m_nrOperands++;
        }

        private void operatAdd(int i)
        {
            m_operators.Add(m_arStack[m_current].getToken(i));
            m_nrOperators++;
        }

        private void operatPop()
        {
            m_operators.RemoveAt(m_nrOperators - 1);
            m_nrOperators--;
        }

        public void operandPop()
        {
            m_operands.RemoveAt(m_nrOperands - 1);
            m_nrOperands--;
        }

        private void addition(Token p_operand1, Token p_operand2)
        {
            int result = p_operand1.getValue() + p_operand2.getValue();
            m_operands.Add(new Token(p_operand1.getCode(), p_operand1.getType(), result.ToString(), result));
            m_nrOperands++;
            m_tbout.AppendText(p_operand1.getText() + " + " + p_operand2.getText() + " = " + m_operands[m_nrOperands - 1].getValue() + "\n");
        }

        private void subtraction(Token p_operand1, Token p_operand2)
        {
            int result = p_operand1.getValue() - p_operand2.getValue();
            m_operands.Add(new Token(p_operand1.getCode(), p_operand1.getType(), result.ToString(), result));
            m_nrOperands++;
            m_tbout.AppendText(p_operand1.getText() + " - " + p_operand2.getText() + " = " + m_operands[m_nrOperands - 1].getValue() + "\n");
        }

        private void multiplication(Token p_operand1, Token p_operand2)
        {
            int result = p_operand1.getValue() * p_operand2.getValue();
            m_operands.Add(new Token(p_operand1.getCode(), p_operand1.getType(), result.ToString(), result));
            m_nrOperands++;
            m_tbout.AppendText(p_operand1.getText() + " * " + p_operand2.getText() + " = " + m_operands[m_nrOperands - 1].getValue() + "\n");
        }

        private void handleSymbol(Token p_token, int p_i)
        {
            Symbol tempSym = null;

            //Special case
            //In some tests the function have the same SymbolId as a local symbol in the funciton block which makes it hard to 
            //differentiate between the two and that creates a problem when a function is recursive.
            //This if statement is supposed to locate functions and make sure they're not mistaken for normal symbols.
            if (p_token.getText() == "F" && m_arStack[m_current].getToken(p_i + 1).getText() == "(" && m_current != 0)
            {
                for (int j = 0; j < m_arStack.Count; j++)
                {
                    //Find the function, if it's not in this block look in the surrounding block.
                    tempSym = m_arStack[m_current - j].searchSymbol(p_token.getCode());
                    if (tempSym != null && tempSym.getKind() == 2)
                        break;
                }

            }
            else
            {
                //if it's not a function symbol just search like normal.
                for (int j = 0; j < m_arStack.Count; j++)
                {
                    tempSym = m_arStack[m_current - j].searchSymbol(p_token.getCode());
                    if (tempSym != null)
                        break;
                }

            }

            //Check if symbol is function or not.
            if (tempSym.getKind() == 2)
            {
                actions(m_matrix.GetAction(operatAction(m_operators[m_nrOperators - 1].getText()), operatAction(p_token.getText())), p_i);
            }
            else
            {
                p_token.setValue(tempSym.getValue());
                operandAdd(p_i);
            }
        }

        private int write(int p_i)
        {
            //Standard write procedure. If a "write" is found in a block this function is executed and handles tokens that are sent in as arguments to the "write" function and saves them in a string
            //when all blocks are executed the final string with all the writes are printed in the right textbox as Output.
            for (p_i = p_i + 1; p_i < m_arStack[m_current].getTokenCount(); p_i++)
            {
                Token token = m_arStack[m_current].getToken(p_i);
                if (token.getText() == ";")
                {
                    if(m_nrOperators > 0)
                    {
                        //if the operators that are next on the stack is "()" remove them because this means that all the tokens in the "write" fucnction has been executed.
                        if(m_nrOperators >= 2 && m_operators[m_nrOperators - 1].getText() == ")" && m_operators[m_nrOperators - 2].getText() == "(")
                        {
                            operatPop();
                            operatPop();
                        }
                        else
                            actions(m_matrix.GetAction(operatAction(m_operators[m_nrOperators - 1].getText()), operatAction(token.getText())), p_i);
                    }

                    //save operand value to output string.
                    m_output += m_operands[m_nrOperands - 1].getValue().ToString();
                    operandPop();
                    
                    return p_i + 1;
                }

                switch (token.getType())
                {
                    case 1:

                        if (token.getText() == "readint")
                        {
                                m_operands.Add(new Token(1, 2, "1", 1));
                                m_nrOperands++;
                                break;
                        }
                        else
                            handleSymbol(token, p_i);
                        break;
                    case 2:
                        operandAdd(p_i);
                        break;
                    case 3:
                        operandAdd(p_i);
                        break;
                    case 4:
                        operandAdd(p_i);
                        break;
                    case 5:
                        if(m_nrOperators == 0)
                            operatAdd(p_i);
                        else
                            actions(m_matrix.GetAction(operatAction(m_operators[m_nrOperators - 1].getText()), operatAction(token.getText())), p_i);
                        break;
                }
            }
            return 0;
        }

        private void equals(Token p_operand1, Token p_operand2)
        {
            int temp;
            temp = p_operand2.getValue();
            for (int i = 0; i < m_arStack[m_current].getSymbolCount(); i++)
            {
                if (m_arStack[m_current].getSymbol(i).getText() == p_operand1.getText())
                {
                    m_arStack[m_current].getSymbol(i).setValue(temp);
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
                            break;
                        }
                    }
                    break;
                }
            }
        }
        private void greaterThan(Token p_operand1, Token p_operand2)
        {
            if (p_operand1.getValue() > p_operand2.getValue())
            {
                m_operands.Add(new Token(0, 0, "true", 1));
                m_nrOperands++;
            }
            else
            {
                m_operands.Add(new Token(0, 0, "false", 0));
                m_nrOperands++;
            }

            m_tbout.AppendText(p_operand1.getText() + " > " + p_operand2.getText() + " = " + m_operands[m_nrOperands - 1].getText() + "\n");
        }

        private void functionCall()
        {
            Symbol tempSym = null;
            for (int i = 0; i < m_arStack.Count; i++)
            {
                tempSym = m_arStack[m_current - i].searchSymbol(m_operators[m_nrOperators - 1].getCode());
                if(tempSym != null && tempSym.getKind() == 2)
                    break;
            }

            //Symbol info2 contains information about which block id contains the function.
            m_arStack.Add(new AR(m_current, m_blockTemplates[tempSym.getInfo(2)]));
            m_current = m_arStack.Count - 1;
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
            m_tbout.AppendText("Enter function: " + tempSym.getText() + "\n");
            interpretBlock();

            //Find the return value symbol
            tempSym = m_arStack[m_current].searchSymbol(m_operators[m_nrOperators - 1].getCode());

            //Save return value as operand
            m_operands.Add(new Token(tempSym.getId(), tempSym.getType(), tempSym.getText(), tempSym.getValue()));
            m_nrOperands++;
            //Remove function operator
            operatPop();
            //block execution finished remove it from the stack
            m_arStack.RemoveAt(m_arStack.Count - 1);
            m_current = m_arStack.Count - 1;
        }
    }
}
