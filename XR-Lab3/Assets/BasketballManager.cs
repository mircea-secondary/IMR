using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BasketballManager : MonoBehaviour
{
    [SerializeField] private Transform hoop;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private Transform despawnLimitTransform;
    
    [SerializeField] private float despawnTimeLimit;
    [SerializeField] private Transform currentBall;
    [SerializeField] private Transform ringPositionsParent;
    [SerializeField] private Transform ring;
    
    [SerializeField] private TMP_Text scoreText;
    private float despawnTimer;

    private Rigidbody ballRb;
    
    private int score = 0;

    private float lastBallYPosition = 0;
    
    private bool isInHoop = false;

    private int hoopPosIndex = 0;
    
    public static BasketballManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ballRb = currentBall.GetComponent<Rigidbody>();

        AddScore(0);
        ring.position = ringPositionsParent.GetChild(0).position;
    }

    private void Update()
    {
        if (currentBall.position.z < despawnLimitTransform.position.z)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer >= despawnTimeLimit || ballRb.linearVelocity.magnitude <= 0.1f)
            {
                currentBall.transform.position = ballSpawnPoint.position;
                currentBall.transform.rotation = Quaternion.identity;
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;
                
                ring.position = ringPositionsParent.GetChild(hoopPosIndex).position;
            }
        }
        else
        {
            despawnTimer = 0;
        }
        
        lastBallYPosition = currentBall.position.y;
    }

    public void OnBasketballEnteredHoop()
    {
        if(lastBallYPosition > hoop.position.y && currentBall.position.y < lastBallYPosition)
        {
            isInHoop = true;
        }
    }
    
    public void OnBasketballExitedHoop()
    {
        if (isInHoop)
        {
            isInHoop = false;

            OnScored();
        }
    }

    private void OnScored()
    {
        hoopPosIndex++;
        hoopPosIndex %= ringPositionsParent.childCount;
        AddScore(hoopPosIndex);
        despawnTimer = despawnTimeLimit - 3f;
    }
    
    private void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
        scoreText.text = score.ToString();
    }
}
