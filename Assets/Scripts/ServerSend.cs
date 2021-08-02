using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }

    public static void Welcome(int toClient)
    {
        using (Packet packet = new Packet((int)ServerPackets.welcomeToGame))
        {
            packet.Write(toClient);
            Debug.Log("Sending welcome packet");
            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            packet.Write(player.id);
            packet.Write(player.username);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write(player.stats.deaths);
            packet.Write(player.stats.score);
            packet.Write(player.stats.kills);
            packet.Write(player.stats.hasWeapon);
            packet.Write(player.stats.health);
            Debug.Log($"Sent spawn {player.id} to {toClient}");
            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerDisconnected(int playerId)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerMovement(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerMovement))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write(player.keysPressed.Length);
            foreach (bool key in player.keysPressed)
            {
                packet.Write(key);
            }
            packet.Write(player.isGrounded);
            packet.Write(player.spineAngle);
            SendUDPDataToAll(player.id, packet);
        }
    }

    public static void CreateItemSpawner(int toClient, int spawnerId, Vector3 position, bool hasItem)
    {
        using (Packet packet = new Packet((int)ServerPackets.createItemSpawner))
        {
            packet.Write(spawnerId);
            packet.Write(position);
            packet.Write(hasItem);

            SendTCPData(toClient, packet);
        }
    }

    public static void ItemSpawned(int spawnerId)
    {
        using (Packet packet = new Packet((int)ServerPackets.itemSpawned))
        {
            packet.Write(spawnerId);
            SendTCPDataToAll(packet);
        }

    }

    public static void ItemPickedUp(int spawnerId, int byPlayer)
    {
        using (Packet packet = new Packet((int)ServerPackets.itemPickedUp))
        {
            packet.Write(spawnerId);
            packet.Write(byPlayer);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHealth(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerHealth))
        {
            packet.Write(player.id);
            packet.Write(player.stats.health);

            SendTCPDataToAll(packet);
        }
    }

}
