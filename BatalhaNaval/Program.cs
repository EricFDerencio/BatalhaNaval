const int TAMANHO_CAMPO = 10;

const int TAMANHO_ENCOURACADO = 4;
const int TAMANHO_CRUZADOR = 3;
const int TAMANHO_SUBMARINO = 2;
const int TAMANHO_DESTROYER = 1;

int[,] campoJogador = new int[TAMANHO_CAMPO, TAMANHO_CAMPO];
int[,] campoMaquina = new int[TAMANHO_CAMPO, TAMANHO_CAMPO];

int[] tamanhosNavios = { TAMANHO_ENCOURACADO, TAMANHO_CRUZADOR, TAMANHO_SUBMARINO, TAMANHO_DESTROYER };
string[] nomesNavios = { "Encouraçado", "Cruzador", "Submarino", "Destroyer" };




bool PodePosicionar(int[,] matriz, int linha, int coluna, int tamanho, bool horizontal)
{
    for (int i = 0; i < tamanho; i++)
    {
        int linhaAtual;
        int colunaAtual;

        if (horizontal)
        {
            linhaAtual = linha;
            colunaAtual = coluna + i;
        }
        else
        {
            linhaAtual = linha + i;
            colunaAtual = coluna;
        }

        if (linhaAtual >= TAMANHO_CAMPO || colunaAtual >= TAMANHO_CAMPO)
        {
            return false;
        }

        if (matriz[linhaAtual, colunaAtual] != 0)
        {
            return false;
        }
    }

    return true;
}
void ExibirCampo(int[,] matriz)
{
    Console.Write("    ");
    for (int coluna = 0; coluna < TAMANHO_CAMPO; coluna++)
    {
        Console.Write($"{coluna + 1,2} ");
    }
    Console.WriteLine();

    for (int linha = 0; linha < TAMANHO_CAMPO; linha++)
    {
        Console.Write($"{linha + 1,2}  ");
        for (int coluna = 0; coluna < TAMANHO_CAMPO; coluna++)
        {
            string simbolo = matriz[linha, coluna] == 0 ? "~" : matriz[linha, coluna].ToString();
            Console.Write($"{simbolo,2} ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
int[,] PosicionarNavio(int[,] matriz, int linha, int coluna, int tamanho, bool horizontal)
{
    for (int i = 0; i < tamanho; i++)
    {
        int linhaAtual;
        int colunaAtual;

        if (horizontal)
        {
            linhaAtual = linha;
            colunaAtual = coluna + i;
        }
        else
        {
            linhaAtual = linha + i;
            colunaAtual = coluna;
        }

        matriz[linhaAtual, colunaAtual] = tamanho;
    }

    return matriz;
}
int[,] PopularCampoAleatorio(int[,] matriz)
{
    for (int navio = 0; navio < tamanhosNavios.Length; navio++)
    {
        bool posicionado = false;

        do
        {
            int linha = Random.Shared.Next (TAMANHO_CAMPO);
            int coluna = Random.Shared.Next(TAMANHO_CAMPO);
            bool horizontal = Random.Shared.Next(2) == 0;

            if (PodePosicionar(matriz, linha, coluna, tamanhosNavios[navio], horizontal))
            {
                matriz = PosicionarNavio(matriz, linha, coluna, tamanhosNavios[navio], horizontal);
                posicionado = true;
            }
        } while (!posicionado);
    }

    return matriz;
}
int[,] PopularCampoManual(int[,] matriz)
{
    for (int navio = 0; navio < tamanhosNavios.Length; navio++)
    {
        bool posicionado = false;

        do
        {
            ExibirCampo(matriz);
            Console.WriteLine($"Posicionando {nomesNavios[navio]} (tamanho {tamanhosNavios[navio]})");

            int linha;
            do
            {
                Console.Write($"Linha (1-{TAMANHO_CAMPO}): ");
            } while (!int.TryParse(Console.ReadLine(), out linha) || linha < 1 || linha > TAMANHO_CAMPO);

            int coluna;
            do
            {
                Console.Write($"Coluna (1-{TAMANHO_CAMPO}): ");
            } while (!int.TryParse(Console.ReadLine(), out coluna) || coluna < 1 || coluna > TAMANHO_CAMPO);

            int direcao = 1;
            if (tamanhosNavios[navio] > 1)
            {
                do
                {
                    Console.Write("Direção (1 - Horizontal, 2 - Vertical): ");
                } while (!int.TryParse(Console.ReadLine(), out direcao) || direcao < 1 || direcao > 2);
            }

            linha--;
            coluna--;
            bool horizontal = direcao == 1;

            if (PodePosicionar(matriz, linha, coluna, tamanhosNavios[navio], horizontal))
            {
                matriz = PosicionarNavio(matriz, linha, coluna, tamanhosNavios[navio], horizontal);
                posicionado = true;
            }
            else
            {
                Console.WriteLine("Posição inválida! O navio sai do campo ou sobrepõe outro. Tente novamente.\n");
            }
        } while (!posicionado);
    }

    return matriz;
}

int opcao;

do
{
    Console.WriteLine("===== BATALHA NAVAL =====");
    Console.WriteLine("1 - Jogar com tabuleiro aleatório");
    Console.WriteLine("2 - Jogar posicionando as peças");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha: ");

    if (!int.TryParse(Console.ReadLine(), out opcao))
    {
        opcao = -1;
    }

    switch (opcao)
    {
        case 1:
            campoJogador = PopularCampoAleatorio(new int[TAMANHO_CAMPO, TAMANHO_CAMPO]);
            campoMaquina = PopularCampoAleatorio(new int[TAMANHO_CAMPO, TAMANHO_CAMPO]);
            Console.WriteLine("\nSeu campo:");
            ExibirCampo(campoJogador);
            // próximo passo: IniciarJogo(campoJogador, campoMaquina);
            break;

        case 2:
            campoJogador = PopularCampoManual(new int[TAMANHO_CAMPO, TAMANHO_CAMPO]);
            campoMaquina = PopularCampoAleatorio(new int[TAMANHO_CAMPO, TAMANHO_CAMPO]);
            Console.WriteLine("\nSeu campo:");
            ExibirCampo(campoJogador);
            // próximo passo: IniciarJogo(campoJogador, campoMaquina);
            break;

        case 0:
            break;

        default:
            Console.WriteLine("Opção inválida!\n");
            break;
    }
} while (opcao != 0);