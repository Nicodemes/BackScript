# BackScript interpreter
This project is an interpreter for a language called "BackScript" I wrote when I was a teen.
## BackScript
BackScript is a language base on the concept of reverse polish notation.
### Mathematical expressions
In a normal programming languange you write mathematical expression like so

```1 + 1```

In Backscript you write it like so

```1 1 +```

More complex expressions are written in normal language like so

```(1 + 1) * 4```

In Backscript it is written like so

```1 1 + 4 *```

### Function Calling
Instead of 

```foo(x)```

it is 

```x <- foo```

### Hello World
Here is how you print hello world to the cout
`"Hello World" <- Print"

## Reasoning
The reason I wrote this project is to experiment with language creation after a failed attempt on writing a compiler compiler.
This project was relativly simple because the language is stack based and didnt require building a parse tree.
Also, I lied to myself and claimed that this was a very efficient interpreted langauge because it did not require parse trees, 
But this efficiency was mitigated due to the fact that it was written in C#
