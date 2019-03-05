using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; } // True quando estiver terminada (xeque-mate)
        private HashSet<Peca> pecas;                // Guarda todas as pedas da partida.
        private HashSet<Peca> capturadas;           // Guarda todas as peças capturadas.
        public bool xeque { get; private set; }     // True se alguém está em xeque,
        public Peca vulneravelEnPassant { get; private set; }   // Pega peão se este estiver suscetível à jogada En Passant.


        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;

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
        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);               // Tira a peça que está na origem.
            p.incrementarQtdMovimentos();                    // Incrementa qtd de movimentos daquela peça.
            Peca pecaCapturada = tab.retirarPeca(destino);  // Tira a peça que está no destino.
            tab.colocarPeca(p, destino);                    // Coloca a peça que estava na origem no destino.
            if (pecaCapturada != null)                      // Acrescenta peça capturada ao conjunto das peças acpturadas.
            {
                capturadas.Add(pecaCapturada);
            }

            // Jogadas especiais:

            // Roques:
            if (p is Rei && ((destino.Coluna == origem.Coluna + 2) || (destino.Coluna == origem.Coluna - 2)))
            {

                Posicao origemTorre;
                Posicao destinoTorre;

                // Roque pequeno:
                if (destino.Coluna == origem.Coluna + 2)
                {
                    origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                    destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                }
                // Roque grande:
                else
                {
                    origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                    destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                }

                Peca T = tab.retirarPeca(origemTorre);
                T.incrementarQtdMovimentos();
                tab.colocarPeca(T, destinoTorre);
            }

            // Jogada Especial: En Passant:
            // Se peça é peão, mexeu na diagonal e não houve peça capturada:
            if (p is Peao && origem.Coluna != destino.Coluna && pecaCapturada == null)
            {
                Posicao posPeaoCapturado;
                if (p.cor == Cor.Branca)
                {
                    posPeaoCapturado = new Posicao(destino.Linha + 1, destino.Coluna);
                }
                else
                {
                    posPeaoCapturado = new Posicao(destino.Linha - 1, destino.Coluna);
                }
                pecaCapturada = tab.retirarPeca(posPeaoCapturado);
                capturadas.Add(pecaCapturada);
            }
            
            return pecaCapturada;
        }

        /// <summary>
        /// Desfaz o movimento que foi feito de 'origem' até 'desino'.
        /// Se houve uma peça capturada, a retorna ao jogo.
        /// </summary>
        /// <param name="origem">Posição a partir da qual o movimetno foi originalmente feito.</param>
        /// <param name="destino">Posição para a qual o movimetno foi originalmente feito.</param>
        /// <param name="pecaCapturada"></param>
        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdMovimentos();
            tab.colocarPeca(p, origem);

            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);    // Recoloca peça que foi capturada na posição 'destino'.
                capturadas.Remove(pecaCapturada);           // Remove peça do conjunto de peças capturadas.
            }

            // Jogadas especiais:

            // Roques:
            if (p is Rei && ((destino.Coluna == origem.Coluna + 2) || (destino.Coluna == origem.Coluna - 2)))
            {

                Posicao origemTorre;
                Posicao destinoTorre;

                // Roque pequeno:
                if (destino.Coluna == origem.Coluna + 2)
                {
                    origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                    destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                }
                // Roque grande:
                else
                {
                    origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                    destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                }

                Peca T = tab.retirarPeca(destinoTorre);
                T.decrementarQtdMovimentos();
                tab.colocarPeca(T, origemTorre);
            }

            // Jogada Especial: En Passant:
            // Se peça é peão, mexeu na diagonal e peça capturada foi a vulnerável à jogada En Passant:
            if (p is Peao && origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
            {
                Peca peaoCapturado = tab.retirarPeca(destino); // Retira peão que foi retornado para o local errado.
                Posicao posPeaoCapturou;

                if (p.cor == Cor.Branca)
                {
                    posPeaoCapturou = new Posicao(3, destino.Coluna);
                }
                else
                {
                    posPeaoCapturou = new Posicao(4, destino.Coluna);
                }
                tab.colocarPeca(peaoCapturado, posPeaoCapturou);
                capturadas.Remove(peaoCapturado); // Professor não colocou.
            }

        }

        /// <summary>
        /// Executa o movimento, confere se está em xeque, itera o turno, muda jogador.
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            // Jogada não pode deixar jogador atual em xeque:
            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tab.peca(destino);

            // Jogada especial: En Passant:
            // Se for peão e [moveu duas casas para baixo (preta) ou moveu duas casas para cima (branca)]
            if (p is Peao)
            {
                if ((destino.Linha == origem.Linha - 2) || (destino.Linha == origem.Linha + 2))
                {
                    vulneravelEnPassant = p;
                }
            }
            // Se nessa jogada não há um peão en passant, anula variável novamente (jogada só pode ser executada imediatamente após a movimentação do peão.
            else
            {
                vulneravelEnPassant = null;
            }

            // Jogada Especial: Promoção:
            if (p is Peao)
            {
                if ((p.cor == Cor.Branca && destino.Linha == 0) || (p.cor == Cor.Preta && destino.Linha == 7))
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(tab, p.cor);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }

            // Checando se adversário está em xeque:
            if (estaEmXeque(corAdversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            // Se realizei a jogada e o adversário do jagador atual está em xeque-mate:
            if (testeXequeMate(corAdversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                // Itera turno:
                turno++;

                // Muda Jogador:
                if (jogadorAtual == Cor.Branca)
                {
                    jogadorAtual = Cor.Preta;
                }
                else
                {
                    jogadorAtual = Cor.Branca;
                }
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
            if (!tab.peca(origem).movimentoPossivel(destino))
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
        /// Retorna cor adversária.
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Cor corAdversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        /// <summary>
        /// Retorna rei de uma dada cor.
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if(x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        /// <summary>
        /// Verifica se rei da cor informada está em xeque.
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não há rei da cor " + cor + " no tabuleiro");
            }

            foreach(Peca x in pecasEmJogo(corAdversaria(cor))) // Para cada peça em jogo da cor adversária.
            {
                bool[,] mat = x.movimentosPossiveis();  // Pegando movimentos possíveis para esta peça.
                if (mat[R.posicao.Linha, R.posicao.Coluna] == true)
                {
                    return true; // Está em xeque.
                }
            }

            return false; // Não está em xeque.
        }

        /// <summary>
        ///  Testa se jogador 'cor' está em xeque-mate.
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool testeXequeMate(Cor cor)
        {
            // Se não está nem em xeque, também não esta em xeque-mate.
            if (!estaEmXeque(cor))
            {
                return false;
            }
            
            foreach (Peca x in pecasEmJogo(cor))            // Para cada peça em jogo dessa cor:
            {
                bool[,] mat = x.movimentosPossiveis();      // Pega movimentos possiveis da peça.
                for (int i = 0; i < tab.linhas; i++)        // Percore linhas do tabuleiro.
                {
                    for (int j = 0; j < tab.colunas; j++)   // Percorre colunas d tabuleiro.
                    {
                        if (mat[i,j] == true)                // Se movimento for possível:
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);    // Simulando movimento da peça para nova posição.
                            bool testeXeque = estaEmXeque(cor);                           // Testa se ainda estaria em xeque.
                            desfazMovimento(origem, destino, pecaCapturada);           // Desfaz movimento simulado.
                            if (!testeXeque)                                              // Se saiu do xeque, não está em xeque-mate.
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            // Se nenhuma peça em nenhuma posição quebrou o xeque, está em xeque-mate.
            return true;
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
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));

            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branca, this));

            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));

            colocarNovaPeca('a', 7, new Peao(tab, Cor.Preta, this));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Preta, this));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Preta, this));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Preta, this));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Preta, this));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Preta, this));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Preta, this));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Preta, this));
        }

    }
}
