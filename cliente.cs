import socket
import threading

SERVER_IP = '127.0.0.1'
PORTA = 9999

cliente = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
nome = input("Digite seu nome: ")

def receber():
    while True:
        msg, _ = cliente.recvfrom(1024)
        msg = msg.decode()

        if msg.startswith("CARTA:"):
            valor = msg.split(":")[1]
            print(f"Você recebeu uma carta: {valor}")

        elif msg.startswith("RESULTADO:"):
            status = msg.split(":")[1]
            if status == "ganhou":
                print(" Você ganhou!")
            else:
                print("  Você perdeu.")
            break

        elif msg.startswith("MENSAGEM:"):
            print(msg.split(":", 1)[1])

def jogar():
    cliente.sendto(f"ENTRAR:{nome}".encode(), (SERVER_IP, PORTA))

    while True:
        print("\n[1] Pedir carta")
        print("[2] Parar")
        opcao = input("Escolha: ")

        if opcao == "1":
            cliente.sendto("PEDIR_CARTA".encode(), (SERVER_IP, PORTA))
        elif opcao == "2":
            cliente.sendto("PARAR".encode(), (SERVER_IP, PORTA))
            break

threading.Thread(target=receber).start()
jogar()
