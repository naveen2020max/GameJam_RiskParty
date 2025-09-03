using BAF.Network;
using CatlikeCoding.PingPong;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

namespace BAF.Network
{
    [RequireComponent(typeof(PP_Game))]
    public class NetworkGame : NetworkBehaviour
    {
        [SerializeField]
        NetworkPaddle
            topPaddle,
            bottomPaddle;


        PP_Game game;

        List<ulong> connectedPlayerIDs = new List<ulong>();

        //public Vector2 ArenaExtents => game.ArenaExtents;
        public Vector2 ArenaExtents = new Vector2(10f, 10f);

        void Awake() => game = GetComponent<PP_Game>();

        public override void OnNetworkSpawn()
        {
            Debug.Log("NetworkGame OnNetworkSpawn");
            if (IsServer)
            {
                Debug.Log("IsServer - registering connection callbacks");
                NetworkManager.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.OnClientDisconnectCallback += HandleClientDisconnected;

                foreach (var client in NetworkManager.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            }
            topPaddle.Init(this);
            bottomPaddle.Init(this);
        }

        void HandleClientConnected(ulong clientId)
        {
            if (!connectedPlayerIDs.Contains(clientId))
            {
                connectedPlayerIDs.Add(clientId);
                NetworkGhostPlayer ghostPlayer = NetworkManager.Singleton
                    .ConnectedClients[clientId].PlayerObject.GetComponent<NetworkGhostPlayer>();
                if (connectedPlayerIDs.Count == 1)
                {
                    bottomPaddle.SetControllingPlayer(ghostPlayer);
                }
                else if (connectedPlayerIDs.Count == 2)
                {
                    topPaddle.SetControllingPlayer(ghostPlayer);

                }
            }

        }

        void HandleClientDisconnected(ulong clientId)
        {
            if (connectedPlayerIDs.Contains(clientId))
            {
                connectedPlayerIDs.Remove(clientId);

                // need logic to reassign paddles or diassign (if that is a word) if the player controlling them disconnects
                //if (topPaddle.OwnerClientId == clientId)
                //{
                //    topPaddle.ChangeOwnership(0);
                //}
                //else if (bottomPaddle.OwnerClientId == clientId)
                //{
                //    bottomPaddle.ChangeOwnership(0);
                //}
            }
        }


    } 
}
