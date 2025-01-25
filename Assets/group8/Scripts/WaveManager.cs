using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Phase { EnemyPhase, EnemyPhaseEnded, BombPhase, BombPhaseEnded, PlayerPhase, PlayerPhaseEnded, GameOverPhase };

namespace BombGame
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField]
        int waveNumber = 0;

        public int maximumBomb = 8;
        public bool RequirePressBarForNextPhase = false;
        public TMP_Text scoreText;
        public TMP_Text bombText;
        public TMP_Text timerText;
        public TMP_Text waveText; // shows the wave we are at
        public TMP_Text gameState1Text; // GameOver text
        public TMP_Text gameState2Text; // GameOver text
        public bool textReady = false; // depends on existance of text objects

        private int score = 0;

        [SerializeField]
        int[] numberOfSpawnEnemy = { 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 };

        private float playerPhaseLength = 25f;
        private float bombPhaseLength = 10f;
        private float enemyPhaseLength = 2f;
        private float bombCleanUpHeight = 3f;
        private SpawnSurface[] spawnEnemySurfaces;
        private Throw hand;
        private OverflowDetector overflowDetector;
        private BombPusher bombPusher;
        private EnemyReachingPlane enemyReachingPlane;
        private CameraView cameraView;
        private ScoreManager scoreManager;
        private float phaseStartedAt;
        private bool hasNotGameOver = true;
        private int highestBombLevelRead = 0;

        // controlling bomb canvas
        public Image bombCanvasImage;
        public Sprite[] bombCanvasSprites;
        public Image timerBarSprite;
        public Image maxBombCanvasImage;
        public Sprite[] maxBombSprites;


        // during player phase, player plays combining game
        // during bombphase, bomb starts falling
        // during enemy phase, enemy starts climbing

        public Phase currentPhase = Phase.EnemyPhaseEnded;

        // Start is called before the first frame update
        void Start()
        {
            spawnEnemySurfaces = FindObjectsOfType<SpawnSurface>();
            hand = FindObjectOfType<Throw>();
            overflowDetector = FindObjectOfType<OverflowDetector>();
            bombPusher = FindObjectOfType<BombPusher>();
            enemyReachingPlane = FindObjectOfType<EnemyReachingPlane>();
            cameraView = FindObjectOfType<CameraView>();
            scoreManager = FindObjectOfType<ScoreManager>();
            phaseStartedAt = Time.time;


            // display text only if text objects are all set correctly
            textReady = ((scoreText != null) &&
                (bombText != null) &&
                (timerText != null) &&
                (gameState2Text != null) &&
                (gameState1Text != null));

            // start by disabling game over text
            if (gameState1Text != null) gameState1Text.enabled = false;
            if (gameState2Text != null) gameState2Text.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            float currentTime = Time.time;
            int nBomb = 0;
            string timerValue = "x";

            // Check if space is pressed and the object is not already moving
            switch (currentPhase)
            {
                case Phase.EnemyPhaseEnded:
                    if (!RequirePressBarForNextPhase || Input.GetKeyDown(KeyCode.Space))
                    {
                        StartPlayerPhase();

                    }
                    break;
                case Phase.PlayerPhaseEnded:
                    if (!RequirePressBarForNextPhase || Input.GetKeyDown(KeyCode.Space))
                    {
                        StartBombPhase();
                    }
                    break;
                case Phase.BombPhaseEnded:
                    if (!RequirePressBarForNextPhase || Input.GetKeyDown(KeyCode.Space))
                    {
                        StartEnemyPhase();
                    }
                    break;
            }

            // manually ending phase by condition
            if (currentPhase == Phase.PlayerPhase)
            {
                // calculate time left (+1 is so that the count down begins with maximum number 14.9s -> 15s
                timerValue = ((int)(playerPhaseLength - (currentTime - phaseStartedAt) + 1)).ToString();
                nBomb = GameObject.FindGameObjectsWithTag("Bomb").Length;
                if (nBomb == maximumBomb)
                {
                    endPhase(Phase.PlayerPhase, "Bomb exceeded Length (" + nBomb + "/" + maximumBomb + ")");
                    /* needs to be called if terminate Detection no longer used */
                    if (overflowDetector != null) overflowDetector.TerminateDetection();
                }
                else if (currentTime > phaseStartedAt + playerPhaseLength)
                {
                    endPhase(Phase.PlayerPhase, "Time is up");
                    if (overflowDetector != null) overflowDetector.TerminateDetection();
                }
                else if (overflowDetector.BombOverflowDetected())
                {
                    endPhase(Phase.PlayerPhase, "Overflow detected");
                    if (overflowDetector != null) overflowDetector.TerminateDetection();
                }
                else if (Input.GetKey("space"))
                {
                    endPhase(Phase.PlayerPhase, "User Pressed Space Bar");
                    if (overflowDetector != null) overflowDetector.TerminateDetection();
                }

                float timeLeft = 1 - (currentTime - phaseStartedAt) / playerPhaseLength;
                if (timerBarSprite != null) timerBarSprite.fillAmount = timeLeft > 0 ? timeLeft : 0;

            }
            if (currentPhase == Phase.BombPhase)
            {
                if (currentTime > phaseStartedAt + bombPhaseLength)
                    endPhase(Phase.BombPhase, "Time is up");
            }
            if (currentPhase == Phase.EnemyPhase)
            {
                if (currentTime > phaseStartedAt + enemyPhaseLength)
                    endPhase(Phase.EnemyPhase, "Time is up");
            }

            // handle scores
            if (scoreManager != null) score = scoreManager.getScore();
            if (textReady)
            {
                scoreText.text = "Score: " + score.ToString();
                bombText.text = "MAX Bomb: " + nBomb + " / " + maximumBomb;
                timerText.text = "Time: " + timerValue.ToString();
                maxBombCanvasImage.sprite = maxBombSprites[nBomb];
            }

            if (enemyReachingPlane != null)
                if (hasNotGameOver && enemyReachingPlane.enemyReachedPlane)
                    triggerGameOver();


            /* restart game */
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            // update bomb canvas
            if (scoreManager != null)
            {
                int level = scoreManager.getHighestBombLevel();
                if (level > highestBombLevelRead)
                {
                    ChangeBombCanvasImage(level);
                    highestBombLevelRead = level;

                    // increase bomb creation level by set amount depending on wave and unlocked bombs
                    if (hand != null) hand.setBombCreationLevel(Mathf.Max(highestBombLevelRead - 2, 1));
                }
            }

        }

        void StartPlayerPhase()
        {
            /* runs exactly once at the start of enemy phase */
            currentPhase = Phase.PlayerPhase;
            phaseStartedAt = Time.time;
            Debug.Log("Player Phase started");

            // increase player phase length by set amount
            playerPhaseLength += (waveNumber - 1) * 3;

            // activate hand if it exists
            if (hand != null) hand.activateHand(true);
            if (cameraView != null) cameraView.ZoomIn();

            // get rid of bombs not on tower
            foreach (GameObject bomb in GameObject.FindGameObjectsWithTag("Bomb"))
                // code block to be executed
                if (bomb.transform.position.y < bombCleanUpHeight)
                    Destroy(bomb);

            // start bomb overflow detection
            if (overflowDetector != null) overflowDetector.InitiateDetection();

            // terminate enemy reaching plane
            if (enemyReachingPlane != null) enemyReachingPlane.TerminateDetection();



        }

        void StartBombPhase()
        {
            /* runs exactly once at the start of enemy phase */
            currentPhase = Phase.BombPhase;
            phaseStartedAt = Time.time;
            Debug.Log("Bomb Phase started");

            // deactivate hand if it exists
            if (hand != null) hand.activateHand(false);
            if (cameraView != null) cameraView.ZoomOut();

            // push bomnbs off
            if (bombPusher != null)
            {
                //bombPushingObject.pushBombsOff(bombPhaseLength * 2/3);
                bombPusher.pushBombsOff(10, 3f, 5);
            }
        }

        void StartEnemyPhase()
        {
            /* runs exactly once at the start of enemy phase */
            currentPhase = Phase.EnemyPhase;
            phaseStartedAt = Time.time;
            Debug.Log("Enemy Phase started");

            // deactivate hand if it exists
            if (hand != null) hand.activateHand(false);

            // enemy reaching plane starts detecting enemy
            if (enemyReachingPlane != null) enemyReachingPlane.InitiateDetection();

            // increase your wave track
            waveNumber += 1;
            waveText.text = "Wave: " + waveNumber.ToString();

            // make enemy climb up
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Enemy>().ClimbUp(enemyPhaseLength);
            }

            // spawn new enemy
            StartCoroutine(SpawnEnemyAfterDelay(enemyPhaseLength / 2));
        }

        void endPhase(Phase phase, string why)
        {
            switch (phase)
            {
                case Phase.PlayerPhase:
                    if (currentPhase == Phase.PlayerPhaseEnded)
                        Debug.Log("Player phase already ended, ignoring " + why);
                    else
                    {
                        currentPhase = Phase.PlayerPhaseEnded;
                        Debug.Log("Player phase ended " + why);
                    }
                    break;
                case Phase.EnemyPhase:
                    if (currentPhase == Phase.EnemyPhaseEnded)
                        Debug.Log("Enemy phase already ended, ignoring " + why);
                    else
                    {
                        currentPhase = Phase.EnemyPhaseEnded;
                        Debug.Log("Enemy phase ended " + why);
                    }
                    break;
                case Phase.BombPhase:
                    if (currentPhase == Phase.BombPhaseEnded)
                        Debug.Log("Bomb phase already ended, ignoring " + why);
                    else
                    {
                        currentPhase = Phase.BombPhaseEnded;
                        Debug.Log("Bomb phase ended " + why);
                    }
                    break;
            }

        }

        void triggerGameOver()
        {
            hasNotGameOver = false;
            currentPhase = Phase.GameOverPhase;
            StartCoroutine(showDelayedGameOverText());
        }

        IEnumerator SpawnEnemyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            int nEnemy = numberOfSpawnEnemy[numberOfSpawnEnemy.Length - 1];
            if (waveNumber < numberOfSpawnEnemy.Length)
                nEnemy = numberOfSpawnEnemy[waveNumber];
            for (int i = 0; i < spawnEnemySurfaces.Length; i++)
                spawnEnemySurfaces[i].Spawn(nEnemy);

        }


        IEnumerator showDelayedGameOverText()
        {
            float delay = 1;
            yield return new WaitForSeconds(delay);
            if (gameState1Text != null) gameState1Text.enabled = true;
            if (gameState2Text != null) gameState2Text.enabled = true;
        }

        void ChangeBombCanvasImage(int level)
        {
            if (bombCanvasImage != null)
            {
                if (level < bombCanvasSprites.Length)
                {
                    bombCanvasImage.sprite = bombCanvasSprites[level];
                }
            }
        }
    }
}
