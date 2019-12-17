using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_IPT.Core
{
    public partial class Parser
    {
        private object CompoundStatement()
        {
            this.GetToken(TokenType.BEGIN);
            var nodes = StatementList();
            this.GetToken(TokenType.END);
            var root = new Compound();
            foreach (var node in nodes)
            {
                root.children.Add(node);
            }
            return root;
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
        }
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
                default:
                    result = this.Empty();
                    break;
            }
            return result;
        }
        private object AssignmentStatement()
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
