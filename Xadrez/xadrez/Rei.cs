using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        private PartidaDeXadrez partida;

        // Construtor:
        public Rei(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "R";
        }

        /// <summary>
        /// Testa se torre na posição 'pos' está apta para executar o roque com o rei.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool testeTorreParaRoque(Posicao pos)
        {
            Peca p = tab.peca(pos);

            // Pode fazer roque se: 
            //  Há uma peça em 'pos';
            //  Esta peça é uma torre;
            //  Esta peça é da mesma cor que o Rei; e
            //  Esta peça não se moveu ainda.
            return p != null && p is Torre && p.cor == cor && p.qtdMovimentos == 0;
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

            // Jogadas Especiais:
            if (qtdMovimentos == 0 && partida.xeque == false)
            {
                // Roque pequeno:
                Posicao posT1 = new Posicao(posicao.Linha, posicao.Coluna + 3); // Posição suposta da torre.

                if (testeTorreParaRoque(posT1)) // Se posição 'pos' é elegível para roque:
                {
                    // Se não há peças entre rei e torre:
                    Posicao p1 = new Posicao(posicao.Linha, posicao.Coluna + 1);
                    Posicao p2 = new Posicao(posicao.Linha, posicao.Coluna + 2);
                    if (tab.peca(p1) == null && tab.peca(p2) == null)
                    {
                        matrizValida[posicao.Linha, posicao.Coluna + 2] = true;
                    }
                }

                // Roque grande:
                Posicao posT2 = new Posicao(posicao.Linha, posicao.Coluna - 4); // Posição suposta da torre.

                if (testeTorreParaRoque(posT2)) // Se posição 'pos' é elegível para roque:
                {
                    // Se não há peças entre rei e torre:
                    Posicao p1 = new Posicao(posicao.Linha, posicao.Coluna - 1);
                    Posicao p2 = new Posicao(posicao.Linha, posicao.Coluna - 2);
                    Posicao p3 = new Posicao(posicao.Linha, posicao.Coluna - 3);
                    if (tab.peca(p1) == null && tab.peca(p2) == null && tab.peca(p3) == null)
                    {
                        matrizValida[posicao.Linha, posicao.Coluna - 2] = true;
                    }
                }
            }

            return matrizValida;
        }


    }
}
