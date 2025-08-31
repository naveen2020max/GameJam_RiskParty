using UnityEngine;

namespace CatlikeCoding.PingPong
{
    public class PP_LivelyCamera : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        float
            jostleStrength = 40f,
            pushStrength = 1f,
            springStrength = 100f,
            dampingStrength = 10f;

        Vector3 velocity, anchorPosition;

        float maxDeltaTime = 1f / 60f;

        void Awake() => anchorPosition = transform.localPosition;
        public void Jostley() => velocity.y += jostleStrength;

        public void PushXZ(Vector2 impulse)
        {
            velocity.x += pushStrength * impulse.x;
            velocity.z += pushStrength * impulse.y;
        }

        private void LateUpdate()
        {
            float dt = Time.deltaTime;
            while (dt > maxDeltaTime)
            {
                TimeStep(maxDeltaTime);
                dt -= maxDeltaTime;
            }
            TimeStep(dt);
        }

        private void TimeStep(float dt)
        {
            Vector3 displacement = anchorPosition - transform.localPosition;
            Vector3 acceleration = springStrength * displacement - dampingStrength * velocity;
            velocity += acceleration * dt;
            transform.localPosition += velocity * dt;
        }
    } 
}
