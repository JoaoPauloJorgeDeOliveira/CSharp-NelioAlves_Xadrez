using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            colocarPecas();
        }

        /// <summary>
        /// Executa o movimento da peça.
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);               // Tira a peça que está na origem.
            p.incrementaQtdMovimentos();                    // Incrementa qtd de movimentos daquela peça.
            Peca pecaCapturada = tab.retirarPeca(destino);  // Tira a peça que está no destino.
            tab.colocarPeca(p, destino);                    // Coloca a peça que estava na origem no destino.
        }

        /// <summary>
        /// Executa o movimento, itera o turno, muda jogador.
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;

            // Muda Jogador
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        /// <summary>
        /// Testa se posição de origem é válida.
        /// </summary>
        /// <param name="pos"></param>
        public void validaPosicaOrigem(Posicao origem)
        {
            // Testa se existe peça na posição.
            if (tab.peca(origem) == null)
                throw new TabuleiroException("Não existe peça nesta posição.");
            // Testa se peça é da mesma cor do jogador atual.
            if (jogadorAtual != tab.peca(origem).cor)
                throw new TabuleiroException("Peça selecionada não é da mesma cor do jogador.");
            // Testa se há movimetos possíveis para esta peça.
            if (!tab.peca(origem).existemMovimentosPossiveis())
                throw new TabuleiroException("Não há movimetos possíveis para esta peça.");
        }

        /// <summary>
        /// Testa se posição de destino é válida.
        /// </summary>
        /// <param name="pos"></param>
        public void validaPosicaDestino(Posicao origem, Posicao destino)
        {
            // Testa se movimentação é possível:
            if (!tab.peca(origem).podeMoverPara(destino))
                throw new TabuleiroException("Peça não pode mover para esta posição.");
        }

        /// <summary>
        /// Método auxiliar, apenas para testes.
        /// Coloca diversas peças no tabuleiro.
        /// </summary>
        private void colocarPecas()
        {
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c', 1).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c', 2).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('d', 2).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e', 2).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e', 1).toPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Branca), new PosicaoXadrez('d', 1).toPosicao());

            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 7).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 8).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('d', 7).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 7).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 8).toPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Preta), new PosicaoXadrez('d', 8).toPosicao());
        }

    }
}
