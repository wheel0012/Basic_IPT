using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_IPT.Core
{
    public partial class Parser
    {
        private Var Variable()
        {
            var node = new Var(this.curr_token);
            this.GetToken(TokenType.ID);
            return node;
        }
        private Block Block()
        {
            var declaration_nodes = this.Declarations();
            var compound_statement_node = this.CompoundStatement();
            var node = new Block(declaration_nodes, compound_statement_node);
            return node;
        }
        private List<VarDecl> Declarations()
        {
            var declarations = new List<VarDecl>();
            if (this.curr_token.status == TokenType.VAR)
            {
                this.GetToken(TokenType.VAR);
                while(this.curr_token.status == TokenType.ID)
                {
                    var var_decl = this.VariableDeclaration();
                    declarations.AddRange(var_decl);
                }
            }
            return declarations;
        }
        private List<VarDecl> VariableDeclaration()
        {
            var var_nodes = new List<Var>() { new Var(this.curr_token) };
            this.GetToken(TokenType.ID);
            while(this.curr_token.status == TokenType.COMMA)
            {
                this.GetToken(TokenType.COMMA);
                var_nodes.Add(new Var(this.curr_token));
                this.GetToken(TokenType.ID);
            }
            this.GetToken(TokenType.COLON);

            var type_node = this.TypeSpec();
            var var_declarations = new List<VarDecl>();
            foreach(var var_node in var_nodes)
            {
                var_declarations.Add(new VarDecl(var_node, type_node));
            }
            return var_declarations;
        }
        private VarType TypeSpec()
        {
            var token = this.curr_token;
            if(this.curr_token.status == TokenType.INTEGER)
            {
                this.GetToken(TokenType.INTEGER);
            }
            else
            {
                this.GetToken(TokenType.REAL);
            }
            var node = new VarType(token);
            return node;
        }
    }
}
