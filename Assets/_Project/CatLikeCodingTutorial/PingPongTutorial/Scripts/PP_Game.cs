using System;
using TMPro;
using UnityEngine;

namespace CatlikeCoding.PingPong
{
    public class PP_Game : MonoBehaviour
    {
        [SerializeField]
        PP_Ball ball;
        [SerializeField]
        PP_Paddle topPaddle, bottomPaddle;

        [SerializeField]
        Vector2 arenaExtents = new Vector2(10f, 10f);

        [SerializeField]
        int pointsToWin = 3;

        [SerializeField]
        TextMeshPro countdownText;

        [SerializeField]
        float newgameDelay = 3f;

        [SerializeField]
        PP_LivelyCamera livelyCamera;

        float countdownUntilNewGame;


        void Awake() => countdownUntilNewGame = newgameDelay;

        void StartNewGame()
        {
            ball.StartNewGame();
            topPaddle.StartNewGame();
            bottomPaddle.StartNewGame();
        }

        private void Update()
        {
            bottomPaddle.Move(ball.Position.x, arenaExtents.x);
            topPaddle.Move(ball.Position.x, arenaExtents.x);
            if (countdownUntilNewGame <= 0f)
            {
                UpdateGame();
            }
            else
            {
                UpdateCountdown();
            }
        }

        private void UpdateCountdown()
        {
            countdownUntilNewGame -= Time.deltaTime;
            if (countdownUntilNewGame <= 0f)
            {
                countdownText.gameObject.SetActive(false);
                StartNewGame();
            }
            else
            {
                float displayvalue = Mathf.Ceil(countdownUntilNewGame);
                if(displayvalue < newgameDelay)
                {
                    countdownText.SetText("{0}", displayvalue);
                }
            }
        }

        private void UpdateGame()
        {
            ball.Move();
            BounceYIfNeeded();
            BounceXIfNeeded(ball.Position.x);
            ball.UpdateVisualization();
        }

        void BounceYIfNeeded()
        {
            float yExtents = arenaExtents.y - ball.Extents;
            if(ball.Position.y < -yExtents)
            {
                BounceY(-yExtents, bottomPaddle, topPaddle);
            }
            if(ball.Position.y > yExtents)
            {
                BounceY(yExtents, topPaddle, bottomPaddle);
            }
        }

        void BounceY(float boundary, PP_Paddle defender, PP_Paddle attacker)
        {
            float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
            float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;

            BounceXIfNeeded(bounceX);
            bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
            livelyCamera.PushXZ(ball.Velocity);
            ball.BounceY(boundary);

            livelyCamera.Jostley();
            if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
            {
                //float speedFactor = 1.1f + Mathf.Abs(hitFactor) * 0.5f;
                ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
            }
            else if(attacker.ScorePoint(pointsToWin))
            {
                EndGame();
            }
            
        }

        private void EndGame()
        {
            countdownUntilNewGame = newgameDelay;
            countdownText.SetText("GameOver");
            countdownText.gameObject.SetActive(true);
            ball.EndGame();
        }

        void BounceXIfNeeded(float x)
        {
            float xExtents = arenaExtents.x - ball.Extents;
            if (x < -xExtents)
            {
                livelyCamera.PushXZ(ball.Velocity);
                ball.BounceX(-xExtents);
            }
            if (x > xExtents)
            {
                livelyCamera.PushXZ(ball.Velocity);
                ball.BounceX(xExtents); 
            }
        }
    } 
}
