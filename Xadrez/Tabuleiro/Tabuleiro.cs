namespace tabuleiro
{
    class Tabuleiro
    {
        public int linhas { get; set; }
        public int colunas { get; set; }
        private Peca[,] pecas;              // Matriz de peças. Privativa, pois só o Tabuleiro mexe nas peças.

        public Tabuleiro(int linhas, int colunas)
        {
            this.linhas = linhas;
            this.colunas = colunas;
            this.pecas = new Peca[linhas, colunas];     // Intancia matriz de peças com a quantidade de linhas e colunas do tabuleiro.
        }

        public Peca peca(int Linha, int Coluna)
        {
            return pecas[Linha, Coluna];
        }

        public Peca peca(Posicao pos)
        {
            return pecas[pos.Linha, pos.Coluna];
        }

        public bool existePeca(Posicao pos)
        {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        public void colocarPeca(Peca p, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição");
            }
            pecas[pos.Linha, pos.Coluna] = p;
            p.posicao = pos;
        }

        public Peca retirarPeca(Posicao pos)
        {
            // Se não ha peça, retorna nulo.
            if (peca(pos) == null)
                return null;

            // Se há peça:
            Peca aux = peca(pos);                   // Transfere peça para aux. 
            aux.posicao = null;                     // Marca posição da peça como nula. 
            pecas[pos.Linha, pos.Coluna] = null;    // Marca posição do tabuleiro como nula.
            return aux;                             // Retorna peça retirada.
        }

        /// <summary>
        /// Checa se posição informada é válida no tabuleiro.
        /// </summary>
        /// <param name="pos">Posição a ser checada</param>
        /// <returns></returns>
        public bool posicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Coluna < 0 || pos.Linha > linhas - 1 || pos.Coluna > colunas - 1)
            {
                return false;
            }
            return true;
        }

        public void validarPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
            {
                throw new TabuleiroException("Posição inválida.");
            }
        }
    }
}
