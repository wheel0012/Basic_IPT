using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_IPT.Core
{
    public partial class Parser
    {
        private Compound CompoundStatement()
        {
            //this.GetToken(TokenType.BEGIN);
            var nodes = StatementList();
            //this.GetToken(TokenType.END);
            var root = new Compound();
            foreach (var node in nodes)
            {
                root.children.Add(node);
            }
            return root;
        }
        private IFState IFStatement()
        {
            var cases = new List<IFCase>();
            this.GetToken(TokenType.IF);
            var condition = BoolExpress();
            this.GetToken(TokenType.THEN);
            var execute = Statement();
            cases.Add(new IFCase(condition, execute));
            while(curr_token.status != TokenType.ENDIF)
            {
                switch(curr_token.status)
                {
                    case TokenType.ELSEIF:
                        this.GetToken(TokenType.ELSEIF);
                        var elifCondition = BoolExpress();
                        this.GetToken(TokenType.THEN);
                        var elifResult = Statement();
                        cases.Add(new IFCase(elifCondition, elifResult));
                        break;
                    case TokenType.ELSE:
                        this.GetToken(TokenType.ELSE);
                        var elseResult = Statement();
                        cases.Add(new IFCase(new BoolNode(Lexer.keywords["TRUE"]) , elseResult));
                        break;
                }
                GetToken(TokenType.EOL);
            }
            this.GetToken(TokenType.ENDIF);
            return new IFState(cases);
        }
        private List<object> StatementList()
        {
            var node = Statement();
            var results = new List<object>() { node };
            while (this.curr_token.status == TokenType.EOL)
            {
                this.GetToken(TokenType.EOL);
                results.Add(this.Statement());
            }
            if (curr_token.status == TokenType.ID)
                throw new Exception("No right-node error");
            return results;
        }/*
        private object ProcessIFState()
        {
            if()
        }*/

        private object Statement()
        {
            object result = null;
            switch (curr_token.status)
            {
                case TokenType.BEGIN:
                    result = this.CompoundStatement();
                    break;
                case TokenType.ID:
                    result = this.AssignmentStatement();
                    break;
                case TokenType.IF:
                    result = this.IFStatement();
                    break;
                case TokenType.RETURN:
                    result = this.ReturnValueStatement();
                    break;
                default:
                    result = this.Empty();
                    break;
            }
            return result;
        }
        private ReturnValue ReturnValueStatement()
        {
            this.GetToken(TokenType.RETURN);
            var result = Express();
            return new ReturnValue(result);
        }
        private Assign AssignmentStatement()
        {
            var left = this.Variable();
            var token = this.curr_token;
            GetToken(TokenType.ASSIGN);
            var right = this.Express();
            var node = new Assign(left, token, right);
            return node;
        }
    }
}
