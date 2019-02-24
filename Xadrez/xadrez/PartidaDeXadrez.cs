using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;        // Guarda todas as pedas da partida.
        private HashSet<Peca> capturadas;   // Guarda todas as peças capturadas.


        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;

            // Instanciando conjuntos:
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();

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
            if (pecaCapturada != null)                      // Acrescenta peça capturada ao conjunto das peças acpturadas.
            {
                capturadas.Add(pecaCapturada);
            }
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
        /// Retorna todas as peças capturadas da cor desejada.
        /// </summary>
        /// <param name="cor">Cor desejada.</param>
        /// <returns></returns>
        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if(x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        /// <summary>
        /// Retorna todas as peças em jogo da cor desejada.
        /// </summary>
        /// <param name="cor">Cor desejada.</param>
        /// <returns></returns>
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)   // Pega todas as peças daquela cor.
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor)); // Retira peças capturadas.
            return aux;
        }

        /// <summary>
        /// Coloca nova peça no tabuleiro e a adiciona ao conjunto de todas as peças.
        /// </summary>
        /// <param name="coluna"></param>
        /// <param name="linha"></param>
        /// <param name="peca"></param>
        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        /// <summary>
        /// Método auxiliar, apenas para testes.
        /// Coloca diversas peças no tabuleiro.
        /// </summary>
        private void colocarPecas()
        {
            colocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('c', 2, new Torre(tab, Cor.Branca));
            colocarNovaPeca('d', 2, new Torre(tab, Cor.Branca));
            colocarNovaPeca('e', 2, new Torre(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));

            colocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));
        }

    }
}
