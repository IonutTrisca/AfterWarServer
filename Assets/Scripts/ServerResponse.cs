using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerResponse
{
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        int clientIdCheck = packet.ReadInt();
        string username = packet.ReadString();

        Debug.Log($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }
        Server.clients[fromClient].SendIntoGame(username);
    }

    public static void PlayerMovement(int fromClient, Packet packet)
    {
        try
        {
            Vector3 position = packet.ReadVector3();
            Quaternion bodyRotation = packet.ReadQuaternion();
            Quaternion cameraRotation = packet.ReadQuaternion();
            float spineAngle = packet.ReadFloat();
            bool[] keysPressed = new bool[packet.ReadInt()];
            bool isGrounded = packet.ReadBool();
            for (int i = 0; i < keysPressed.Length; i++)
            {
                keysPressed[i] = packet.ReadBool();
            }

            Server.clients[fromClient].player.transform.position = position;
            Server.clients[fromClient].player.transform.rotation = bodyRotation;
            Server.clients[fromClient].player.spineAngle = spineAngle;
            Server.clients[fromClient].player.SetKeysPressed(keysPressed);
            Server.clients[fromClient].player.isGrounded = isGrounded;
            Server.clients[fromClient].player.shootOrigin.rotation = cameraRotation;
        } catch (Exception e)
        {
            Debug.LogError("Some error");
        }
        
    }
}