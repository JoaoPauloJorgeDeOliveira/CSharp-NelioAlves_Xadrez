﻿using System.Collections.Generic;
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


        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            
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
            colocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('h', 7, new Torre(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));

            colocarNovaPeca('b', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('a', 8, new Rei(tab, Cor.Preta));
        }

    }
}
