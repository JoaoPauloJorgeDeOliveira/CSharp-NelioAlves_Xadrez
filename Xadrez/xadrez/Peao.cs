using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {

        private PartidaDeXadrez partida;

        public Peao(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "P";
        }

        /// <summary>
        /// Retorna true se casa possui peça de cor diferente (e se não está vazia).
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool existeInimigo(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p != null && p.cor != cor;
        }

        /// <summary>
        /// Retorna true se posição 'pos' está livre de peças.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool livre(Posicao pos)
        {
            return tab.peca(pos) == null;
        }

        /// <summary>
        /// Cria matriz de posições possíveis para a peça.
        /// </summary>
        /// <returns></returns>
        public override bool[,] movimentosPossiveis()
        {
            bool[,] matrizValida = new bool[tab.linhas, tab.colunas]; //Matriz de booleanos é inicializada como false.

            Posicao pos = new Posicao(0, 0);


            if (cor == Cor.Branca) // Se peça for branca, sobe (linha diminui):
            {
                // Se não existe inimigo nas casa a frente:
                pos.definirValores(posicao.Linha - 1, posicao.Coluna);
                if (tab.posicaoValida(pos) && livre(pos))
                {
                    matrizValida[pos.Linha, pos.Coluna] = true;

                    // Se primeira casa é válida, testa segunda:
                    pos.definirValores(posicao.Linha - 2, posicao.Coluna);
                    if (tab.posicaoValida(pos) && livre(pos) && qtdMovimentos == 0) // Se for o primeiro movimento do peão, pode andar 2 casas. Não vê se tem peça na frente?
                    {
                        matrizValida[pos.Linha, pos.Coluna] = true;
                    }
                }

                // Se existe inimigo nas diagonais imediatas:
                pos.definirValores(posicao.Linha - 1, posicao.Coluna - 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    matrizValida[pos.Linha, pos.Coluna] = true;
                }
                pos.definirValores(posicao.Linha - 1, posicao.Coluna + 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    matrizValida[pos.Linha, pos.Coluna] = true;
                }

                // Jogada especial: En Passant:
                if (posicao.Linha == 3) // Só é possível nesta linha.
                {
                    // À esquerda:
                    Posicao esquerda = new Posicao(posicao.Linha, posicao.Coluna - 1);
                    if (tab.posicaoValida(esquerda) && existeInimigo(esquerda) && tab.peca(esquerda) == partida.vulneravelEnPassant)
                    {
                        matrizValida[esquerda.Linha - 1, esquerda.Coluna] = true;
                    }
                    // À direita:
                    Posicao direita = new Posicao(posicao.Linha, posicao.Coluna + 1);
                    if (tab.posicaoValida(direita) && existeInimigo(direita) && tab.peca(direita) == partida.vulneravelEnPassant)
                    {
                        matrizValida[direita.Linha - 1, direita.Coluna] = true;
                    }
                }
            }
            else // Se peça for branca, desce (linha aumenta):
            {
                // Se não existe inimigo nas casas a frente:
                pos.definirValores(posicao.Linha + 1, posicao.Coluna);
                if (tab.posicaoValida(pos) && livre(pos))
                {
                    matrizValida[pos.Linha, pos.Coluna] = true;

                    // Se primeira casa é válida, testa segunda:
                    pos.definirValores(posicao.Linha + 2, posicao.Coluna);
                    if (tab.posicaoValida(pos) && livre(pos) && qtdMovimentos == 0) // Se for o primeiro movimento do peão, pode andar 2 casas. Não vê se tem peça na frente?
                    {
                        matrizValida[pos.Linha, pos.Coluna] = true;
                    }
                }
                
                // Se existe inimigo nas diagonais imediatas:
                pos.definirValores(posicao.Linha + 1, posicao.Coluna - 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    matrizValida[pos.Linha, pos.Coluna] = true;
                }
                pos.definirValores(posicao.Linha + 1, posicao.Coluna + 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    matrizValida[pos.Linha, pos.Coluna] = true;
                }

                // Jogada especial: En Passant:
                if (posicao.Linha == 4) // Só é possível nesta linha.
                {
                    // À esquerda:
                    Posicao esquerda = new Posicao(posicao.Linha, posicao.Coluna - 1);
                    if (tab.posicaoValida(esquerda) && existeInimigo(esquerda) && tab.peca(esquerda) == partida.vulneravelEnPassant)
                    {
                        matrizValida[esquerda.Linha + 1, esquerda.Coluna] = true;
                    }
                    // À direita:
                    Posicao direita = new Posicao(posicao.Linha, posicao.Coluna + 1);
                    if (tab.posicaoValida(direita) && existeInimigo(direita) && tab.peca(direita) == partida.vulneravelEnPassant)
                    {
                        matrizValida[direita.Linha + 1, direita.Coluna] = true;
                    }
                }

            }

            return matrizValida;
        }


    }
}
