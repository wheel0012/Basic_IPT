using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_IPT.Core
{
    public class AST
    {
    }
    class NoOP : AST
    {

    }
    public class Var : AST
    {
        public readonly Token token;
        public readonly object value;
        public Var(Token token)
        {
            this.token = token;
            this.value = token.value;
        }
    }
    class BinOP : AST
    {
        public readonly dynamic left;
        public readonly dynamic right;
        public readonly Token op;
        public BinOP(object left, Token op, object right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }

    class FloatOP : AST
    {
        public readonly dynamic left;
        public readonly dynamic right;
        public readonly Token op;
        public FloatOP(object left, Token op, object right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }
    class BoolOP : AST
    {
        public readonly object left;
        public readonly object right;
        public readonly Token op;
        public BoolOP(object left, Token op, object right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }
    class Num : AST
    {
        public readonly Token token;
        public readonly int value;
        public Num(Token token)
        {
            this.token = token;
            this.value = Convert.ToInt32(token.value);
        }
    }
    class Real : AST
    {
        public readonly Token token;
        public readonly float value;
        public Real(Token token)
        {
            this.token = token;
            this.value = Convert.ToSingle(token.value);
        }
    }
    class UnaryOP : AST
    {
        public readonly Token token;
        public readonly object expr;
        public UnaryOP(Token op, object expr)
        {
            this.token = op;
            this.expr = expr;
        }
    }
    public class Compound : AST
    {
        public List<object> children;
        public Compound()
        {
            children = new List<object>();
        }
    }
    public class Assign : AST
    {
        public object left;
        public Token token;
        public object right;
        public Assign(object left, Token token, object right)
        {
            this.left = left;
            this.token = token;
            this.right = right;
        }
    }
    public class Program : AST
    {
        public readonly string name;
        public readonly Block block;
        public Program(string name, Block block)
        {
            this.name = name;
            this.block = block;
        }
    }
    public class Block : AST
    {
        public readonly List<VarDecl> declarations;
        public readonly object compound_statement;
        public Block(List<VarDecl> declarations, object compound_statement)
        {
            this.declarations = declarations;
            this.compound_statement = compound_statement;
        }
    }
    public class VarDecl : AST
    {
        public readonly object var_node;
        public readonly object type_node;
        public VarDecl(object var_node, object type_node)
        {
            this.var_node = var_node;
            this.type_node = type_node;
        }
    }
    public class VarType : AST
    {
        public readonly Token token;
        public readonly object value;
        public VarType(Token token)
        {
            this.token = token;
            this.value = token.value;
        }
    }
}
