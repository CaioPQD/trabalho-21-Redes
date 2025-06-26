using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Cliente
{
    static UdpClient cliente = new();
    static IPEndPoint servidor = new("127.0.0.1", 9999);

    static void Main()
    {
        Console.Write("Digite seu nome: ");
        string nome = Console.ReadLine();
        Enviar($"ENTRAR:{nome}");

        new Thread(Receber).Start();

        while (true)
        {
            Console.WriteLine("\n[1] Pedir carta\n[2] Parar");
            string opcao = Console.ReadLine();

            if (opcao == "1")
                Enviar("PEDIR_CARTA");
            else if (opcao == "2")
            {
                Enviar("PARAR");
                break;
            }
        }
    }

    static void Enviar(string msg)
    {
        byte[] dados = Encoding.UTF8.GetBytes(msg);
        cliente.Send(dados, dados.Length, servidor);
    }

    static void Receber()
    {
        IPEndPoint origem = new(IPAddress.Any, 0);
        while (true)
        {
            byte[] dados = cliente.Receive(ref origem);
            string msg = Encoding.UTF8.GetString(dados);

            if (msg.StartsWith("CARTA:"))
                Console.WriteLine($"Você recebeu uma carta: {msg.Split(":")[1]}");
            else if (msg.StartsWith("RESULTADO:"))
            {
                string status = msg.Split(":")[1];
                Console.WriteLine(status == "ganhou" ? "🎉 Você ganhou!" : "😞 Você perdeu.");
                break;
            }
            else if (msg.StartsWith("MENSAGEM:"))
                Console.WriteLine(msg.Split(":", 2)[1]);
        }
    }
}
