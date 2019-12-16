using System;
using System.Collections.Generic;
using System.Reflection;

namespace Basic_IPT.Core
{
    public class NodeVisitor
    {
        public object Visit(object node)
        {
            var methodName = "Visit_" + node.GetType().Name;
            Type type = typeof(Interpreter);
            MethodInfo func = type.GetMethod(methodName);
            var result = func.Invoke(obj: this,parameters: new object[] { node });
            return result;
        }
    }
    public class Interpreter : NodeVisitor
    {
        string[] source_lines;
        string source_code;
        Parser parser;
        Dictionary<string, Var> GLOBAL_SCOPE;
        public Interpreter(Parser parser)
        {
            this.parser= parser;
            this.GLOBAL_SCOPE = new Dictionary<string, Var>();
        }
        public int Visit_Num(object node)
        {
            var typed_node = (Num)node;
            return typed_node.value;
        }
        public int Visit_UnaryOP(object node)
        {
            var typed_node = (UnaryOP)node;
            switch(typed_node.token.status)
            {
                case TokenType.PLUS:
                    return +(int)this.Visit(typed_node.expr);
                case TokenType.MINUS:
                    return -(int)this.Visit(typed_node.expr);
            }
            throw new Exception("Unary type Error" + this.parser.lexer.GetPos());
        }
        public int Visit_BinOP(object node)
        {
            var typed_node = (BinOP)node;
            switch (typed_node.op.status)
            {
                case TokenType.PLUS:
                    return (int)this.Visit(typed_node.left) + (int)this.Visit(typed_node.right);
                case TokenType.MINUS:
                    return (int)this.Visit(typed_node.left) - (int)this.Visit(typed_node.right);
                case TokenType.TERM_OPERATOR:
                    switch(typed_node.op.value)
                    {
                        case "*":
                            return (int)this.Visit(typed_node.left) * (int)this.Visit(typed_node.right);
                        case "/":
                            return (int)this.Visit(typed_node.left) / (int)this.Visit(typed_node.right);
                    }
                    break;
            }
            throw new Exception("Visit_BinOP method error");
        }
        public void Visit_Compound(object node)
        {
            var typed_node = (Compound)node;
            foreach(var child in typed_node.children)
            {
                this.Visit(child);
            }
        }
        public void Visit_Assign(object node)
        {
            var typed_node = (Assign)node;
            var name = ((Var)typed_node.left).value.ToString();
            this.GLOBAL_SCOPE.Add(name, (Var)this.Visit(typed_node.right));
        }
        public Var Visit_Var(object node)
        {
            var typed_node = (Var)node;
            var var_name = typed_node.value.ToString();
            Var var;
            if (this.GLOBAL_SCOPE.TryGetValue(var_name, out var))
            {
                Console.WriteLine(var.value+ ";"+var);
                return var;
            }
            else throw new Exception("Undefinded variable name");
        }
        public object Process()
        {
            var ASTtree = this.parser.Parse();
            return this.Visit(ASTtree);
        }
    }
}