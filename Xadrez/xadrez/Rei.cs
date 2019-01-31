using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        public Rei(Tabuleiro tab, Cor cor) : base(tab, cor)
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
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal superior direita:
            pos.definirValores(posicao.Linha - 1, posicao.Coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }
            // Direita:
            pos.definirValores(posicao.Linha, posicao.Coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal inferior direita:
            pos.definirValores(posicao.Linha + 1, posicao.Coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }
            // Abaixo:
            pos.definirValores(posicao.Linha + 1, posicao.Coluna);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal inferior esquerda:
            pos.definirValores(posicao.Linha + 1, posicao.Coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }
            // Esquerda:
            pos.definirValores(posicao.Linha, posicao.Coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }
            // Diagonal superior esquerda:
            pos.definirValores(posicao.Linha - 1, posicao.Coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {
                matrizValida[pos.Linha, pos.Coluna] = true;
            }

            return matrizValida;
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
