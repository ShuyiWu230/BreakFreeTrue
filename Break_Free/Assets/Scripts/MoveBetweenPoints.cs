using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool pingPong = true;
    [SerializeField] private bool startAtPointB = false;

    private Transform currentTarget;
    private float journeyLength;
    private float startTime;
    private bool movingToB = true;
    public GameObject panel;

    public bool moving;//是否要移动
    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Points A and B must be assigned!");
            enabled = false;
            return;
        }

        if (startAtPointB)
        {
            transform.position = pointB.position;
            currentTarget = pointA;
            movingToB = false;
        }
        else
        {
            transform.position = pointA.position;
            currentTarget = pointB;
        }

        journeyLength = Vector3.Distance(pointA.position, pointB.position);
        startTime = Time.time;
    }

    void Update()
    {
        if (currentTarget == null) return;

        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        if (moving == true) 
        {
            transform.position = Vector3.Lerp(transform.position, currentTarget.position, fractionOfJourney * Time.deltaTime * speed);

        }

        // 检测是否到达目标点附近
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            if (pingPong)
            {
                // 切换目标点
                movingToB = !movingToB;
                currentTarget = movingToB ? pointB : pointA;
                startTime = Time.time;
                journeyLength = Vector3.Distance(transform.position, currentTarget.position);
            }
            else
            {
                // 如果不是pingpong模式，到达终点后禁用脚本
                enabled = false;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<HourglassController>() != null)
        {
            other.gameObject.SetActive(false);
            panel.SetActive(true);
        }
    }
}