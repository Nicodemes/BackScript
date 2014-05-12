using System;
using System.Collections.Generic;


namespace Interpreter {
    class Tokenizer {
        //the input of the tokenizer
        string input;
        //the output of the tokenizer. aka the tokens
        LinkedList<object> output;

        //from where we should scan a  string.
        int start;
        int end;
        
        //carry things
        string carry;
        Token.Type carryType;

        //constructor
        public Tokenizer(string input) {
            start = 0;
            end = input.Length;
            this.output = new LinkedList<object>();
            this.input = input;
            this.carry = "";
            carryType = Token.Type.EOS;
        }

        //input navigation
        Token LookBack() {
            return (Token)output.First.Value;
        }
        char LookAhead() {
            int next = start + 1;
            if (next >= input.Length)
                return '$';
            return input[start + 1];
        }
        void Next() {
            start++;
        }

        //these methods used to switch the carry type and insert into the input the current carry
        void BreakCarry(){
            if (carryType == Token.Type.EOS) 
                return;
            if (carryType == Token.Type.IdTail)
                carryType = Token.Type.IdEnd;
            if (carryType == Token.Type.IdHead)
                carryType = Token.Type.IdSingle;
            output.AddLast(new Token(carry, carryType));
            carry = "";
            carryType = Token.Type.EOS;
        }
        void BreakCarry(Token.Type with) {

            if (carryType == Token.Type.EOS) {
                carryType = with;
                return;
            }
            if (carryType == Token.Type.IdTail && with!=Token.Type.IdTail)
                carryType = Token.Type.IdEnd;
            if (carryType == Token.Type.IdHead && with != Token.Type.IdTail)
                carryType = Token.Type.IdSingle; 

            output.AddLast(new Token(carry, carryType));
            carry = "";
            carryType = with;
        }

       //the function that converts the iinput string into tokens
        public LinkedList<object> Tokenize() {
            while (start < end) {
                //bolleans
                if (ReadFrom(input, start, "true")) {
                    start += 4;
                    this.output.AddLast(new Token("true", Token.Type.Boolean));
                    continue;
                }
                if (ReadFrom(input, start, "false")) {
                    start += 5;
                    this.output.AddLast(new Token("false", Token.Type.Boolean));
                    continue;
                }
                
                char cur = input[start];
                
                //number literals
                if (cur == '0' ||
                    cur == '1' ||
                    cur == '2' ||
                    cur == '3' ||
                    cur == '4' ||
                    cur == '5' ||
                    cur == '6' ||
                    cur == '7' || 
                    cur == '8' ||
                    cur == '9') {
                    //if there is no carry - that means that this is an integer 
                    if (carryType == Token.Type.EOS)
                        carryType = Token.Type.Integer;
                }
                
                switch (cur) {
                    //saperators
                    case '$':
                        BreakCarry();
                        break;
                    case ' ':
                        BreakCarry();
                        break;
                    
                    case '\t':
                        BreakCarry();
                        break;
                    case '\n':
                        BreakCarry();
                        break;
                    //full stop saperator 
                    case '.':
                        //if the point is comming after idHead that is mean that you got idtail after this token
                        if (carryType == Token.Type.IdHead) 
                            BreakCarry(Token.Type.IdTail);
                        //if you are now carrying tail - that means that there is more tail after him
                        else if (carryType == Token.Type.IdTail)
                            BreakCarry(Token.Type.IdTail);
                        //it may be inside of an integer-then it converts to doublee.
                        else if (carryType == Token.Type.Integer) {
                            carryType = Token.Type.Double;
                            carry += '.';
                        }
                        else
                            throw new Exception("You cannot place '.' after " + carryType.ToString()); 
                       break;

                    
                    //operators
                    case ';':
                        BreakCarry();
                        output.AddLast(new Token(";",Token.Type.Operator));
                        break;
                    case '+':
                        BreakCarry();
                        output.AddLast(new Token("+", Token.Type.Operator));
                        break;
                    case '-':
                        BreakCarry();
                        output.AddLast(new Token("-", Token.Type.Operator));
                        break;
                    case '*':
                        BreakCarry();
                        output.AddLast(new Token("*", Token.Type.Operator));
                        break;
                    case '/':
                        BreakCarry();
                        output.AddLast(new Token("/", Token.Type.Operator));
                        break;
                    
                    case '=':
                        BreakCarry();
                        //check if the next charecter is = , if it is, that is mean that this is an boolean operator.
                        if (LookAhead() == '=') {
                            output.AddLast(new Token("==", Token.Type.Operator));
                            Next();
                        }
                        else
                            output.AddLast(new Token("=", Token.Type.Operator));
                        break;
                    case '!':
                        BreakCarry();
                        //NOT boolean operator.
                        output.AddLast(new Token("!", Token.Type.Operator));
                        break;
                    case '|':
                        BreakCarry();
                        //OR boolean operator
                        if (LookAhead() == '|'){
                            output.AddLast(new Token("||", Token.Type.Operator));
                            Next();
                        }
                        //OR  operator.
                        else
                            output.AddLast(new Token("|", Token.Type.Operator));
                        
                        break;
                    case '^':
                        BreakCarry();
                        //XOR boolean operator
                        output.AddLast(new Token("^", Token.Type.Operator));
                        break;
                    case '&':
                        BreakCarry();
                        //AND boolean operator
                        if (LookAhead() == '&'){
                            output.AddLast(new Token("&&", Token.Type.Operator));
                            Next();
                        }
                        else
                            output.AddLast(new Token("&", Token.Type.Operator));
                            break;
                    
                   
                    case '{':
                        BreakCarry();
                        output.AddLast(new Token("{", Token.Type.Operator));
                        break;
                    case '}':
                        BreakCarry();
                        output.AddLast(new Token("}", Token.Type.Operator));
                        break;
                    
                    case ',':
                        output.AddLast(new Token(",", Token.Type.Operator));
                        BreakCarry();
                       
                        int count = 1;

                        Next();
                        while (input[start] == ',' || input[start] == '@' ) {
                            if (input[start] == '@') {
                                output.AddLast(new Token(""+count, Token.Type.Integer));
                                output.AddLast(new Token("array", Token.Type.FunctionCall));
                                break;
                            }
                            else {
                                count++;
                                output.AddLast(new Token(",", Token.Type.Operator));
                            }
                                Next();
                            
                        }
                        break;

                    //indexer
                    case '[':
                        Next();
                        BreakCarry(Token.Type.Indexer);
                        while (input[start] != ']') {
                            carry += input[start];
                            Next();
                        }
                        BreakCarry();
                        break;

                    //string literals
                    case '\"':
                       
                        BreakCarry(Token.Type.String);
                        Next();
                        while (input[start] != '\"') {
                            
                            //if it is an escape charecter.
                            if (input[start] == '\\') {
                                //TODO: escape charecter extantion for hex dec and other stuff.
                                switch (LookAhead()) {
                                    case 'n':
                                        carry += '\n';
                                        break;
                                    case 't':
                                        carry += '\t';
                                        break;
                                    case '\"':
                                        carry += '\"';
                                        break;
                                    case '\'':
                                        carry += '\'';
                                        break;
                                    default:
                                        throw new Exception("unrecognized charecter after escape charecter");
                                }
                                Next();
                            }
                            else {
                                carry += input[start];
                            }
                            Next();
                        }
                        BreakCarry();
                        break;
                    //escape charecters
                    
                    //function call
                    case '<':
                        BreakCarry();
                        char lookAhead=LookAhead();
                        //boolean operator
                        if (lookAhead == '<'){
                            output.AddLast(new Token("<<", Token.Type.Operator));
                            Next();
                        }
                        else{
                            if (lookAhead == '-')
                                Next();
                            
                            //function call operator.
                            output.AddLast(new Token("<-", Token.Type.Operator));
                            BreakCarry();
                        }
                       break;
                    case '>':
                       BreakCarry();
                        if(LookAhead()!='>')
                            throw new Exception("Invalid operator >, must be followed by >");
                        Next();
                        output.AddLast(new Token(">>", Token.Type.Operator));
                        break;
                    case '~':
                       throw new Exception("Reserved Operator used!");
                    
                    //default
                    default:
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.IdHead;
                        carry+=cur;
                        break;
                }
                Next();
            }
            BreakCarry();
            return output;
        }

        //used to check if a word exists. need to improce in here.
        public static bool ReadFrom(string input, int index, string word) {
            try {
                int j = 0;
                for (int i = index; j < word.Length; i++,j++) {
                    if (input[i] != word[j])
                        break;
                }
                if (j == word.Length)
                    return true;
            }
            catch {
                return false;
            }
            return false;
        }

    }
}
