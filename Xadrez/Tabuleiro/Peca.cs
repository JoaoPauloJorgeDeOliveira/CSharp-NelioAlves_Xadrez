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

        public void incrementaQtdMovimentos()
        {
            qtdMovimentos++;
        }

        /// <summary>
        /// Cria matriz de posições possíveis para a peça.
        /// Sobrescrita em cada peça para as devidas particularidades.
        /// </summary>
        /// <returns></returns>
        public abstract bool[,] movimentosPossiveis();
    }
}
