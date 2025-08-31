using TMPro;
using UnityEngine;

namespace CatlikeCoding.PingPong
{
    public class PP_Paddle : MonoBehaviour
    {
        static readonly int 
            timeOfLastHitID = Shader.PropertyToID("_timeOfLastHit"),
            emissionColorID = Shader.PropertyToID("_EmissionColor"),
            faceColorID = Shader.PropertyToID("_FaceColor");

        [SerializeField, Min(0f)]
        float
            speed = 10f,
            minExtents = 4f,
            maxExtents = 4f,
            maxTargetingBias = 0.75f;
        [SerializeField]
        bool isAI = false;

        [SerializeField]
        TextMeshPro scoreText;

        [SerializeField]
        MeshRenderer goalRenderer;

        [SerializeField, ColorUsage(true, true)]
        Color goalColor = Color.white;

        int score = 0;
        float targetingBias, extents;

        Material
            paddleMaterial,
            goalMaterial,
            scoreMaterial;

        void Awake()
        {
            paddleMaterial = GetComponent<Renderer>().material;
            goalMaterial = goalRenderer.material;
            goalMaterial.SetColor(emissionColorID, goalColor);
            scoreMaterial = scoreText.fontMaterial;
            SetScore(0);
        }

        public void StartNewGame()
        {
            SetScore(0);
            ChangeTargeringBias();
        }

        public void Move(float target, float arenaExtents)
        {
            Vector3 p = transform.localPosition;
            p.x = isAI ? AdjustByAI(p.x, target) : AdjustByPlayer(p.x);
            float limit = arenaExtents - extents;
            p.x = Mathf.Clamp(p.x, -limit, limit);
            transform.localPosition = p;
        }

        float AdjustByAI(float x, float target)
        {
            target += targetingBias * extents;
            if (x < target)
                return Mathf.Min(x + speed * Time.deltaTime, target);
           return Mathf.Max(x - speed * Time.deltaTime, target);
        }

        float AdjustByPlayer(float x)
        {
            bool goright = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            bool goleft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            if(goright && !goleft)
                return x + speed * Time.deltaTime;
            if (goleft && !goright)
                return x - speed * Time.deltaTime;
            return x;
        }

        public bool HitBall(float ballX, float ballExtents,out float hitforce)
        {
            hitforce = (ballX - transform.localPosition.x) / (extents + ballExtents);
            ChangeTargeringBias();
            //return -1f <= hitforce && hitforce <= 1f;
            bool success = -1f <= hitforce && hitforce <= 1f;
            if(success)
                paddleMaterial.SetFloat(timeOfLastHitID, Time.time);
            return success;
        }

        void SetScore(int newScore, float PointsToWin = 1000f)
        {
            score = newScore;
            scoreText.SetText("{0}", score);
            scoreMaterial.SetColor(faceColorID, goalColor * (newScore/PointsToWin));
            SetExtents(Mathf.Lerp(maxExtents, minExtents, newScore / (PointsToWin - 1f)));
        }

        public bool ScorePoint(int PointTOWin)
        {
            SetScore(score + 1, PointTOWin);
            goalMaterial.SetFloat(timeOfLastHitID, Time.time);
            return score >= PointTOWin;
        }

        void ChangeTargeringBias()
        {
            targetingBias = Random.Range(-maxTargetingBias, maxTargetingBias);
        }

        void SetExtents(float newExtents)
        {
            extents = newExtents;
            Vector3 s = transform.localScale;
            s.x = 2f * newExtents;
            transform.localScale = s;
        }
    } 
}
