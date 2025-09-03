using CatlikeCoding.PingPong;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace BAF.Network
{
    [RequireComponent(typeof(PP_Paddle), typeof(NetworkTransform))]
    public class NetworkPaddle : NetworkBehaviour
	{
		private PP_Paddle paddle;

        private NetworkGhostPlayer controllingGhostPlayer;

        NetworkGame game;

        private void Awake()
        {
            paddle = GetComponent<PP_Paddle>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (controllingGhostPlayer != null)
            {
                MovePaddle(controllingGhostPlayer.moveInput.Value); 
            }
        }

        public void Init(NetworkGame game)
        {
            this.game = game;
        }

        

        void MovePaddle(float moveInput)
        {
            float ext = 10f;
            if (game != null)
                ext = game.ArenaExtents.x;
            else
                Debug.LogError("NetworkGame is null in NetworkPaddle");
            paddle.Move(0f, ext, moveInput);
        }

        public void ChangeOwnership(ulong clientId)
        {
                Debug.Log($"Changing ownership of {gameObject.name} to client {clientId}");
            if (IsServer)
            {
                NetworkObject.ChangeOwnership(clientId);
                
            }
        }

        public void SetControllingPlayer(NetworkGhostPlayer ghostPlayer)
        {
            this.controllingGhostPlayer = ghostPlayer;
        }
    } 
}
