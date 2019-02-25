namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qtdMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; } // Protegida para só ser setada pela classe Peca ou por suas subclasses.

        public Peca(Tabuleiro tab, Cor cor)
        {
            this.posicao = null;
            this.cor = cor;
            this.tab = tab;
            this.qtdMovimentos = 0;
        }

        public void incrementarQtdMovimentos()
        {
            qtdMovimentos++;
        }

        public void decrementarQtdMovimentos()
        {
            qtdMovimentos--;
        }

        /// <summary>
        /// Testa se peça pode mover para posição 'pos', ou seja,
        /// se posição está vazia ou ocupada por peça adversária (outra cor).
        /// </summary>
        /// <param name="pos">Posição a ser testada.</param>
        /// <returns></returns>
        protected bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return (p == null) || (p.cor != cor);
        }

        /// <summary>
        /// Cria matriz de posições possíveis para a peça.
        /// Sobrescrita em cada peça para as devidas particularidades.
        /// </summary>
        /// <returns></returns>
        public abstract bool[,] movimentosPossiveis();

        /// <summary>
        /// Verifica se há pelo menos 1 movimento possível ou se peça está bloqueada.
        /// </summary>
        /// <returns></returns>
        public bool existemMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();
            for (int i = 0; i < tab.linhas; i++)
            {
                for (int j = 0; j < tab.colunas; j++)
                {
                    if (mat[i,j] == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Retorna se é possível mover pela para posição 'pos'.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool podeMoverPara(Posicao pos)
        {
            return movimentosPossiveis()[pos.Linha, pos.Coluna];
        }
    }
}
