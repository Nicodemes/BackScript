using System;
using System.Collections.Generic;


namespace Interpreter {
    class Tokenizer {
        /*
        public static void Main() {
            string inr = Console.ReadLine();
            Tokenizer obj = new Tokenizer(inr);
            LinkedList<Token> output = obj.Tokenize();
            foreach (Token i in output)
                Console.WriteLine("[" + i.type + "] " + i.lexema);
            Console.ReadLine();
        }*/
        string input;
        LinkedList<object> output;

        int start;
        int end;
        
        Token LookBack() {
            return (Token)output.First.Value;
        }
        char LookAhead() {
            int next=start+1;
            if (next >= input.Length)
                return '$';
            return input[start+1];
        }
        void Next() {
            start++;
        }


        string carry;
        Token.Type carryType;

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

        public Tokenizer(string input) {
            start = 0;
            end = input.Length;
            this.output = new LinkedList<object>();
            this.input = input;
            this.carry = "";
            carryType = Token.Type.EOS;
        }
        
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
        public static LinkedList<Token> TokenizeOld (string input,int start, int end){
            
            
            LinkedList<Token> output=new LinkedList<Token>();
            string carry="";
            //end of stream carry is an emty carry.
            Token.Type carryType= Token.Type.EOS;

            while (start < end) {

                switch(input[start]){
                    //escape charecter
                    case '[':
                        if (carryType == Token.Type.String)
                            carry += '[';
                        else if (carryType != Token.Type.EOS) {
                            output.AddLast(new Token(carry, carryType));
                            carry = "";
                            carryType = Token.Type.EOS;
                        }
                        string carry2 = "";
                        while (start < end) {
                            start++;
                            if (input[start] != ']')
                                carry2 += input[start];
                            else {
                                output.AddLast(new Token(carry2, Token.Type.Indexer));
                                break;
                            }       
                        }
                        break;
                    case '\\':
                        if (carryType != Token.Type.String)
                            throw new Exception("escape char outside of a string defention");
                        start++;
                        if (start >= end)
                            throw new Exception("end of string toosoon");

                        switch(input[start]){
                            case 'n':
                                carry += '\n';
                                break;
                            case 't':
                                carryType += '\t';
                                break;
                            case '\"':
                                carryType += '\"';
                                break;
                            case '\'':
                                carryType += '\'';
                                break;
                            default:
                                throw new Exception("unrecognized charecter after escape charecter");
                                

                        }
                        break;
                    case '\n':
                         if (carryType != Token.Type.String) {
                            if (carryType == Token.Type.FunctionCall && ((carry == "") || (carry == "<"))) {
                                //ignore uneeded spaces
                            }
                            else {
                                if (carryType != Token.Type.EOS) {

                                    output.AddLast(new Token(carry, carryType));
                                    carry = "";
                                    carryType = Token.Type.EOS;
                                }
                            }
                        }
                        
                        break;
                    
                    case ' ':
                        
                        if (carryType != Token.Type.String) {
                            if (carryType == Token.Type.FunctionCall && ((carry == "") || (carry == "<"))) {
                                //ignore uneeded spaces
                            }
                            else {
                                if (carryType != Token.Type.EOS) {

                                    output.AddLast(new Token(carry, carryType));
                                    carry = "";
                                    carryType = Token.Type.EOS;
                                }
                            }
                        }
                        else
                            carry += ' ';
                        break;
                    case '\"':
                        if (carryType == Token.Type.String) {
                            output.AddLast(new Token(carry, carryType));
                            carryType = Token.Type.EOS;
                        }
                        else if (carryType == Token.Type.EOS) {
                            carryType = Token.Type.String;
                        }
                        else
                            throw new Exception("invalid operator placement");
                        break;
                    case '.':
                        switch(carryType){
                            case Token.Type.String:
                                carry += '.';
                                break;
                            case Token.Type.Integer:
                                carryType=Token.Type.Double;
                                carry+='.';
                                break;
                            case Token.Type.IdHead:
                                output.AddLast(new Token(carry, carryType));
                                carryType=Token.Type.IdTail;
                                carry="";
                                break;
                            case Token.Type.IdTail:
                                output.AddLast(new Token(carry, carryType));
                                carryType=Token.Type.IdTail;
                                carry="";
                                break;
                            default:
                                throw new Exception("invalid dot position");
                        }
                        break;
                    case '0':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='0';
                        break;
                    case '1':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='1';
                        break;
                    case '2':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='2';
                        break;
                    case '3':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='3';
                        break;
                    case '4':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='4';
                        break;
                    case '5':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='5';
                        break;
                    case '6':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='6';
                        break;
                    case '7':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='7';
                        break;
                    case '8':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='8';
                        break;
                    case '9':
                        if(carryType==Token.Type.EOS)
                            carryType=Token.Type.Integer;
                        carry+='9';
                        break;
                    
                    
                    case '<':
                        if (carryType != Token.Type.EOS) {
                            if (carryType == Token.Type.String)
                                carry += "<";
                            else
                                throw new Exception("invalid placement");
                        }
                        else {
                            carryType = Token.Type.FunctionCall;
                            carry = "<";
                        }
                        break;
                    case '-':
                        if(carryType==Token.Type.EOS)
                            output.AddLast(new Token("-", Token.Type.Operator));
                        else if(carryType == Token.Type.FunctionCall){
                            if(carry!="<")
                                throw new Exception("invalid placement");
                            carry="";
                        }
                        else
                            throw new Exception("invalid placement");
                        break;
                    case '{':
                         if (carryType == Token.Type.String)
                            carry += '{';
                        else if (carryType != Token.Type.EOS) {
                            output.AddLast(new Token(carry, carryType));
                            carry = "";
                            carryType = Token.Type.EOS;
                        }
                        output.AddLast(new Token("{", Token.Type.Operator));
                        break;
                    case '}':
                         if (carryType == Token.Type.String)
                            carry += '}';
                        else if (carryType != Token.Type.EOS) {
                            output.AddLast(new Token(carry, carryType));
                            carry = "";
                            carryType = Token.Type.EOS;
                        }
                        output.AddLast(new Token("}", Token.Type.Operator));
                        
                        break;
                    case '+':
                        if(carryType==Token.Type.EOS)
                            output.AddLast(new Token("+", Token.Type.Operator));
                        else
                            throw new Exception("invalid placement");
                        break;
                    case '*':
                        if(carryType==Token.Type.EOS)
                            output.AddLast(new Token("*", Token.Type.Operator));
                        else
                            throw new Exception("invalid placement");
                        break;
                    case ';':
                        
                        if (carryType == Token.Type.String)
                            carry += ';';
                        else if (carryType != Token.Type.EOS) {
                            output.AddLast(new Token(carry, carryType));
                            carry = "";
                            carryType = Token.Type.EOS;
                        }
                        output.AddLast(new Token(";", Token.Type.Operator));
                        break;
                    

                    default:
                        if (carryType == Token.Type.EOS)
                            carryType = Token.Type.IdHead;
                        if (carryType == Token.Type.FunctionCall && carry == "<") {
                            carry = "";
                            
                        }
                        carry+=input[start];
                        break;
                }
                start++;
            }
            if (carry != "") {
                output.AddLast(new Token(carry, carryType));
            }
            output.AddLast(new Token("$", Token.Type.EOS));
            return output;
        }
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
