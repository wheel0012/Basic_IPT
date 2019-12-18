using System;
using System.Collections.Generic;
using System.Text;

namespace Basic_IPT.Core
{
    public static class TokenTool
    {
        public static Dictionary<string, Token> RegistToken()
        {
            return new Dictionary<string, Token>
            {
                {TokenType.IF.ToString(), new Token(TokenType.IF, TokenType.IF.ToString()) },

                {TokenType.THEN.ToString(), new Token(TokenType.THEN, TokenType.THEN.ToString()) },

                {TokenType.ELSE.ToString(), new Token(TokenType.ELSE, TokenType.ELSE.ToString()) },

                {TokenType.ELSEIF.ToString() , new Token(TokenType.ELSEIF, TokenType.ELSEIF.ToString()) },

                {TokenType.ENDIF.ToString() , new Token(TokenType.ENDIF, TokenType.ENDIF.ToString()) },

                {TokenType.RETURN.ToString(), new Token(TokenType.RETURN, TokenType.RETURN.ToString()) },

                {TokenType.BEGIN.ToString(), new Token(TokenType.BEGIN, TokenType.BEGIN.ToString()) },

                {TokenType.END.ToString(), new Token(TokenType.END, TokenType.END.ToString()) },

                {TokenType.PROGRAM.ToString(), new Token(TokenType.PROGRAM, TokenType.PROGRAM.ToString()) },

                {TokenType.REAL.ToString(), new Token(TokenType.REAL, TokenType.REAL.ToString()) },

                {TokenType.VAR.ToString(), new Token(TokenType.VAR, TokenType.VAR.ToString()) }


            };
        }
    }
    public enum TokenType
    {
        EMPTY,
        LETTER,

        MUL,
        INTEGER_DIV,
        FLOAT_DIV,
        DIV,
        PLUS,
        MINUS,

        STRING,
        INTEGER,
        REAL,
        SPACE,
        EOF,
        LPAREN,
        RPAREN,
        ID,

        IF,
        ELSEIF,
        ELSE,
        THEN,
        ENDIF,

        RETURN,
        ASSIGN,
        DOT,
        BEGIN,
        END,
        VAR,
        EOL,
        PROGRAM,
        COMMA,
        COLON,
        INTEGER_CONST,
        REAL_CONST,

        ISEQUAL,
        ISMORE,
        ISLESS,
        ISMOREOREQUAL,
        ISLESSOREQUAL,
        ISNOT,

        AND,
        OR
    }
    public class Token
    {
        public readonly TokenType status;
        public readonly object value;
        internal Token(TokenType status, object value)
        {
            this.status = status;
            this.value = value;
        }
    }
}
