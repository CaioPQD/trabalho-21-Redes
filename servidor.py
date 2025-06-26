import socket
import threading
import random

clientes = {}
pontuacoes = {}
jogo_ativo = True

SERVER_IP = '127.0.0.1'
PORTA = 9999

servidor = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
servidor.bind((SERVER_IP, PORTA))

print(f"[SERVIDOR INICIADO] Escutando em {SERVER_IP}:{PORTA}")

def enviar(msg, endereco):
    servidor.sendto(msg.encode(), endereco)

def sortear_carta():
    return random.randint(1, 11)

def distribuir_carta(endereco):
    carta = sortear_carta()
    pontuacoes[endereco] += carta
    enviar(f"CARTA:{carta}", endereco)
    if pontuacoes[endereco] > 21:
        enviar("RESULTADO:perdeu", endereco)

def avaliar_resultado():
    vencedor = None
    maior = 0
    for end, pontos in pontuacoes.items():
        if pontos <= 21 and pontos > maior:
            maior = pontos
            vencedor = end

    for end in pontuacoes:
        if end == vencedor:
            enviar("RESULTADO:ganhou", end)
        else:
            enviar("RESULTADO:perdeu", end)

def ouvir():
    global jogo_ativo
    while True:
        msg, endereco = servidor.recvfrom(1024)
        msg = msg.decode()

        if msg.startswith("ENTRAR:"):
            nome = msg.split(":")[1]
            clientes[endereco] = nome
            pontuacoes[endereco] = 0
            print(f"[{nome}] entrou no jogo.")
            enviar("MENSAGEM:Bem-vindo ao jogo!", endereco)

            if len(clientes) >= 2:
                for end in clientes:
                    distribuir_carta(end)

        elif msg == "PEDIR_CARTA":
            if endereco in pontuacoes:
                distribuir_carta(endereco)

        elif msg == "PARAR":
            pontuacoes[endereco] += 0
            enviar("MENSAGEM:Jogador parou. Aguardando os outros...", endereco)

            if all(p > 21 for p in pontuacoes.values()):
                avaliar_resultado()

threading.Thread(target=ouvir).start()