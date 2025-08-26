using UnityEngine;
using Unity.Netcode;
public class ObjectMovement : NetworkBehaviour
{
    public float movespeed = 2f;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector3.down;
        }

        

        transform.position += moveDir * movespeed * Time.deltaTime;
    }
}
