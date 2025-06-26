# Jogo 21 com Sockets UDP

Este é um projeto acadêmico desenvolvido para a disciplina de Redes de Computadores. Ele simula o jogo de cartas “21” (Blackjack), utilizando sockets UDP para comunicação entre cliente e servidor.

## 🎮 Funcionalidades

- Comunicação entre cliente e servidor via UDP.
- Suporte a múltiplos jogadores.
- Lógica completa do jogo 21 (Blackjack).
- Interface no terminal para os jogadores.
- Exibição de cartas, pontuação parcial e resultado final.

## 🧠 Protocolo de Comunicação

| Comando | Origem → Destino | Descrição |
|--------|------------------|------------|
| `ENTRAR:<nome>` | Cliente → Servidor | Solicita entrada no jogo |
| `CARTA:<valor>` | Servidor → Cliente | Envia carta sorteada |
| `PEDIR_CARTA` | Cliente → Servidor | Jogador pede nova carta |
| `PARAR` | Cliente → Servidor | Jogador decide parar |
| `RESULTADO:<ganhou/perdeu>` | Servidor → Cliente | Envia resultado da rodada |
| `MENSAGEM:<texto>` | Servidor → Cliente | Comunicação geral |

## 🚀 Como Executar

### Requisitos

- Instale o .NET SDK (recomendo o .NET 6 ou 8)
- Terminal ou prompt de comando

### Passos

1. Clone este repositório:

```bash
git clone https://github.com/seuusuario/jogo-21-udp.git
cd jogo-21-udp
```

2. Execute o servidor:

```bash
cd Servidor
dotnet run

```

3. Em outros terminais, execute os clientes:

```bash
cd ../Cliente
dotnet run

```

4. Siga as instruções na tela para jogar.

## 📁 Estrutura do Projeto

```
📦 jogo-21-udp
┣ 📄 servidor.cs
┣ 📄 cliente.cs
┣ 📄 README.md
```

## 👥 Autores

- Caio Maciel Magalhães

## 🛡️ Aviso

Este projeto é de uso exclusivamente acadêmico. Cópias ou distribuições não autorizadas podem ser consideradas plágio.
