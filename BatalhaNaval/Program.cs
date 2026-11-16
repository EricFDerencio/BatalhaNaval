const int TAMANHO_CAMPO = 6;

const int TAMANHO_ENCOURACADO = 4;
const int TAMANHO_CRUZADOR = 3;
const int TAMANHO_SUBMARINO = 2;
const int TAMANHO_DESTROYER = 1;

const int TOTAL_PARTES_NAVIOS = TAMANHO_ENCOURACADO + TAMANHO_CRUZADOR + TAMANHO_SUBMARINO + TAMANHO_DESTROYER;
const int TIRO_AGUA = -9;

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
void ExibirCampo(int[,] matriz, bool mostrarNavios)
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
            Console.ForegroundColor = ConsoleColor.Blue;
            string simbolo = "~";
            

            if (matriz[linha, coluna] == TIRO_AGUA)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                simbolo = "o";
            }
            else if (matriz[linha, coluna] < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                simbolo = "X";
            }
            else if (mostrarNavios && matriz[linha, coluna] > 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                simbolo = matriz[linha, coluna].ToString();
            }
      
            Console.Write($"{simbolo,2} ");
            Console.ForegroundColor = ConsoleColor.White;
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
            int linha = Random.Shared.Next(TAMANHO_CAMPO);
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
            ExibirCampo(matriz, true);
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
bool Atirar(int[,] matriz, int linha, int coluna)
{
    if (matriz[linha, coluna] > 0)
    {
        matriz[linha, coluna] = -matriz[linha, coluna];
        Console.WriteLine("Acertou!\n");
        return true;
    }

    matriz[linha, coluna] = TIRO_AGUA;
    Console.WriteLine("Água!\n");
    return false;
}
bool TurnoJogador(int[,] matriz)
{
    int linha;
    int coluna;

    do
    {
        do
        {
            Console.Write($"Linha do tiro (1-{TAMANHO_CAMPO}): ");
        } while (!int.TryParse(Console.ReadLine(), out linha) || linha < 1 || linha > TAMANHO_CAMPO);

        do
        {
            Console.Write($"Coluna do tiro (1-{TAMANHO_CAMPO}): ");
        } while (!int.TryParse(Console.ReadLine(), out coluna) || coluna < 1 || coluna > TAMANHO_CAMPO);

        linha--;
        coluna--;

        if (matriz[linha, coluna] < 0)
        {
            Console.WriteLine("Você já atirou nessa posição! Escolha outra.\n");
        }
    } while (matriz[linha, coluna] < 0);

    return Atirar(matriz, linha, coluna);
}
bool TurnoMaquina(int[,] matriz)
{
    int linha;
    int coluna;

    do
    {
        linha = Random.Shared.Next(TAMANHO_CAMPO);
        coluna = Random.Shared.Next(TAMANHO_CAMPO);
    } while (matriz[linha, coluna] < 0);

    Console.WriteLine($"A máquina atirou na linha {linha + 1}, coluna {coluna + 1}.");

    return Atirar(matriz, linha, coluna);
}
void IniciarJogo(int[,] matrizJogador, int[,] matrizMaquina)
{
    int acertosJogador = 0;
    int acertosMaquina = 0;

    do
    {
        Console.WriteLine("===== CAMPO DA MÁQUINA =====");
        ExibirCampo(matrizMaquina, false);

        Console.WriteLine("===== SEU CAMPO =====");
        ExibirCampo(matrizJogador, true);

        if (TurnoJogador(matrizMaquina))
        {
            acertosJogador++;
        }

        if (acertosJogador < TOTAL_PARTES_NAVIOS && TurnoMaquina(matrizJogador))
        {
            acertosMaquina++;
        }
    } while (acertosJogador < TOTAL_PARTES_NAVIOS && acertosMaquina < TOTAL_PARTES_NAVIOS);

    if (acertosJogador == TOTAL_PARTES_NAVIOS)
    {
        Console.WriteLine("Parabéns, você venceu!\n");
    }
    else
    {
        Console.WriteLine("A máquina venceu!\n");
    }
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
            IniciarJogo(campoJogador, campoMaquina);
            break;

        case 2:
            campoJogador = PopularCampoManual(new int[TAMANHO_CAMPO, TAMANHO_CAMPO]);
            campoMaquina = PopularCampoAleatorio(new int[TAMANHO_CAMPO, TAMANHO_CAMPO]);
            IniciarJogo(campoJogador, campoMaquina);
            break;

        case 0:
            break;

        default:
            Console.WriteLine("Opção inválida!\n");
            break;
    }
} while (opcao != 0);