using tabuleiro;

namespace xadrez
{
    class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        /// <summary>
        /// Cria matriz de posições possíveis para a peça.
        /// </summary>
        /// <returns></returns>
        public override bool[,] movimentosPossiveis()
        {
            bool[,] matrizValida = new bool[tab.linhas, tab.colunas]; //Matriz de booleanos é inicializada como false.

            Posicao pos = new Posicao(0, 0);

            // Acima:
            pos.definirValores(posicao.Linha - 1, posicao.Coluna);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.Linha--;    // Diminui linha (sobre no tabuleiro).
            }
            // Acima:
            pos.definirValores(posicao.Linha + 1, posicao.Coluna);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.Linha++;    // Aumenta linha (desce no tabuleiro).
            }
            // Direita:
            pos.definirValores(posicao.Linha, posicao.Coluna + 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.Coluna++;    // Aumenta coluna (vai para direita).
            }
            // Esquerda:
            pos.definirValores(posicao.Linha, posicao.Coluna - 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.Coluna--;    // Diminui coluna (vai para esquerda).
            }
            
            return matrizValida;
        }

        public override string ToString()
        {
            return "T";
        }
    }
}
