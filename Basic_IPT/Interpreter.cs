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
    public class Interpreter :  NodeVisitor
    {
        string[] source_lines;
        string source_code;
        Parser parser;
        public Dictionary<string, object> GLOBAL_SCOPE;
        public Interpreter(Parser parser)
        {
            this.parser= parser;
            this.GLOBAL_SCOPE = new Dictionary<string, object>() { { "Result", null } };
        }
        public int Visit_Num(object node)
        {
            var typed_node = (Num)node;
            return typed_node.value;
        }
        public float Visit_Real(object node)
        {
            var typed_node = (Real)node;
            return typed_node.value;
        }
        public bool Visit_BoolNode(object node)
        {
            var typed_node = (BoolNode)node;
            return typed_node.value;
        }
        public void Visit_Program(object node)
        {
            var typed_node = (Program)node;
            this.Visit(typed_node.block);
        }
        public object Visit_IFState(object node)
        {
            var typed_node = (IFState)node;
            foreach(var if_case in typed_node.cases)
            {
                if ((bool)Visit(if_case.condition))
                    return Visit(if_case.execute);
            }

            return null;

        }
        public void Visit_Block(object node)
        {
            var typed_node = (Block)node;
            foreach(var decl in typed_node.declarations)
            {
                this.Visit(decl);
            }
            this.Visit(typed_node.compound_statement);
        }
        public void Visit_VarDecl()
        {
            //KEEP IT VOID
        }
        public void Visit_Type()
        {
            //KEEP IT VOID
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
        public object Visit_BinOP(object node)
        {
            var typed_node = (BinOP)node;
            switch (typed_node.op.status)
            {
                case TokenType.PLUS:
                    return this.Visit(typed_node.left) + this.Visit(typed_node.right);
                case TokenType.MINUS:
                    return this.Visit(typed_node.left) - this.Visit(typed_node.right);
                case TokenType.MUL:
                    return this.Visit(typed_node.left) * this.Visit(typed_node.right);
                case TokenType.DIV:
                    return this.Visit(typed_node.left) / this.Visit(typed_node.right);
            }
            throw new Exception("Visit_BinOP method error");
        }
        public float Visit_FloatOP(object node)
        {
            var typed_node = (FloatOP)node;
            var left = this.Visit(typed_node.left);
            var right = this.Visit(typed_node.right);
            switch (typed_node.op.status)
            {
                case TokenType.PLUS:
                    return (float)left + (float)right;
                case TokenType.MINUS:
                    return (float)left - (float)right;
                case TokenType.MUL:
                    return (float)left * (float)right;
                case TokenType.DIV:
                    return (float)left / (float)right;
            }
            throw new Exception("Visit_FloatOP method error");
        }
        public bool Visit_BoolOP(object node)
        {
            var typed_node = (BoolOP)node;
            switch (typed_node.op.status)
            {
                case TokenType.ISEQUAL:
                    return this.Visit(typed_node.left) == this.Visit(typed_node.right);

                case TokenType.ISLESS:
                    return this.Visit(typed_node.left) < this.Visit(typed_node.right);

                case TokenType.ISMORE:
                    return this.Visit(typed_node.left) > this.Visit(typed_node.right);

                case TokenType.ISLESSOREQUAL:
                    return this.Visit(typed_node.left) <= this.Visit(typed_node.right);

                case TokenType.ISMOREOREQUAL:
                    return this.Visit(typed_node.left) >= this.Visit(typed_node.right);
            }
            throw new Exception("Visit_BoolOP method error");
        }
        public void Visit_Compound(object node)
        {
            var typed_node = (Compound)node;
            foreach(var child in typed_node.children)
            {
                this.Visit(child);
            }
        }
        public void Visit_NoOP(object node) 
        {

        }
        public void Visit_Assign(object node)
        {
            var typed_node = (Assign)node;
            var name = ((Var)typed_node.left).value.ToString();
            this.GLOBAL_SCOPE.Add(name, this.Visit(typed_node.right));
        }
        public object Visit_Var(object node)
        {
            var typed_node = (Var)node;
            var var_name = typed_node.value.ToString();
            object var;
            if (this.GLOBAL_SCOPE.TryGetValue(var_name, out var))
            {
                return var;
            }
            else throw new Exception("Undefinded variable name");
        }
        public void Visit_ReturnValue(object node)
        {
            var typed_node = (ReturnValue)node;
            if(typed_node.node.GetType().IsEquivalentTo(typeof(Num)))
            {
                GLOBAL_SCOPE["Result"] = ((Num)typed_node.node).value;
            }
            else if (typed_node.node.GetType().IsEquivalentTo(typeof(Real)))
            {
                GLOBAL_SCOPE["Result"] = ((Real)typed_node.node).value;
            }
            else if (typed_node.node.GetType().IsEquivalentTo(typeof(BoolNode)))
            {
                GLOBAL_SCOPE["Result"] = ((BoolNode)typed_node.node).value;
            }
        }
        public object Process()
        {
            var ASTtree = this.parser.Parse();
            return this.Visit(ASTtree);
        }
    }
}