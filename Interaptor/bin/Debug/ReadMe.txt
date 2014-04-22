backScript is writen in Reverse Polish notation;

#Using Fnctions 
{parameter1} {parameter2} {parameter3} <- {function_name};
OR
{parameter1} {parameter2} {parameter3} <{function_name};

#Printing on Screan
"Hello world!" <- PrintL;
"Hello World!" <- Print;

#Declearing a variable 
<var_name> <value> <- Def;

#Reading input into a variable
<var_name> <ReadLine <- Def;

#Declearing a list
{<var1> <var2> <var3>} <- list;
#using indaxer
myList {"a" "b" "c"} <- list <- Def
myList[1] <- PrintL

#(the output will be "b")


# functions that are supported by now - >

#constructs a list
{...}<-list
#
{var_name} {value} <- Def
#sets value of an id ( the id must be defined first)
{var_name} {value} <- Set

#adding 
{value} {value} <-Add 
#clears the screen
<Clear
#reads a file into a string 
< ReadFile
# executes a string 
{string} <- Eval

