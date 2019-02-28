using tabuleiro;

namespace xadrez
{
    class Bispo : Peca
    {
        public Bispo(Tabuleiro tab, Cor cor) : base(tab, cor)
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

            // Diagonal superior esquerda:
            pos.definirValores(posicao.Linha - 1, posicao.Coluna - 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.definirValores(pos.Linha - 1, pos.Coluna - 1); // Itera.
            }
            // Diagonal superior direita:
            pos.definirValores(posicao.Linha - 1, posicao.Coluna + 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.definirValores(pos.Linha - 1, pos.Coluna + 1); // Itera.
            }
            // Diagonal inferior direita:
            pos.definirValores(posicao.Linha + 1, posicao.Coluna + 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.definirValores(pos.Linha + 1, pos.Coluna + 1); // Itera.
            }
            // Diagonal inferior esquerda:
            pos.definirValores(posicao.Linha + 1, posicao.Coluna - 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)  // Se chegou numa posição onde há peça de outra cor, para a conferência de casas.
                    break;
                pos.definirValores(pos.Linha + 1, pos.Coluna - 1); // Itera.
            }
            
            return matrizValida;
        }

        public override string ToString()
        {
            return "B";
        }
    }
}
