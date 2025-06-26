using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Servidor
{
    static Dictionary<IPEndPoint, int> pontuacoes = new();
    static Dictionary<IPEndPoint, string> jogadores = new();
    static UdpClient udp = new(9999);
    static Random random = new();

    static void Main()
    {
        Console.WriteLine("[SERVIDOR INICIADO] Porta 9999");
        new Thread(() => Ouvir()).Start();
    }

    static void Ouvir()
    {
        while (true)
        {
            IPEndPoint origem = new(IPAddress.Any, 0);
            byte[] dados = udp.Receive(ref origem);
            string msg = Encoding.UTF8.GetString(dados);

            if (msg.StartsWith("ENTRAR:"))
            {
                string nome = msg.Split(":")[1];
                jogadores[origem] = nome;
                pontuacoes[origem] = 0;
                Console.WriteLine($"[{nome}] entrou no jogo.");
                Enviar("MENSAGEM:Bem-vindo ao jogo!", origem);

                if (jogadores.Count >= 2)
                {
                    foreach (var jogador in jogadores.Keys)
                        DistribuirCarta(jogador);
                }
            }
            else if (msg == "PEDIR_CARTA")
            {
                if (pontuacoes.ContainsKey(origem))
                    DistribuirCarta(origem);
            }
            else if (msg == "PARAR")
            {
                Enviar("MENSAGEM:Jogador parou. Aguardando os outros...", origem);
                VerificarFinal();
            }
        }
    }

    static void DistribuirCarta(IPEndPoint jogador)
    {
        int carta = random.Next(1, 12);
        pontuacoes[jogador] += carta;
        Enviar($"CARTA:{carta}", jogador);

        if (pontuacoes[jogador] > 21)
        {
            Enviar("RESULTADO:perdeu", jogador);
            VerificarFinal();
        }
    }

    static void VerificarFinal()
    {
        if (TodosFinalizaram())
        {
            int maior = 0;
            IPEndPoint vencedor = null;

            foreach (var kv in pontuacoes)
            {
                if (kv.Value <= 21 && kv.Value > maior)
                {
                    maior = kv.Value;
                    vencedor = kv.Key;
                }
            }

            foreach (var jogador in pontuacoes.Keys)
            {
                string resultado = jogador.Equals(vencedor) ? "ganhou" : "perdeu";
                Enviar($"RESULTADO:{resultado}", jogador);
            }
        }
    }

    static bool TodosFinalizaram()
    {
        foreach (var pontos in pontuacoes.Values)
        {
            if (pontos <= 21)
                return false;
        }
        return true;
    }

    static void Enviar(string msg, IPEndPoint destino)
    {
        byte[] dados = Encoding.UTF8.GetBytes(msg);
        udp.Send(dados, dados.Length, destino);
    }
}
