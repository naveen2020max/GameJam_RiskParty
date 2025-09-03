using CatlikeCoding.PingPong;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace BAF.Network
{
    
    public class NetworkGhostPlayer : NetworkBehaviour
    {
        public NetworkVariable<float> moveInput =
            new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        private void Update()
        {
            if (!IsOwner) return;

            bool goright = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            bool goleft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            float currentInput = 0f;
            if (goright && !goleft)
                currentInput = 1;
            if (goleft && !goright)
                currentInput = -1;

            if(currentInput != moveInput.Value)
                moveInput.Value = currentInput;

        }
    } 
}
