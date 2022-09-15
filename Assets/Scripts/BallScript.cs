using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] float speed, acceleration;
    private Vector2 direction;
    private List<Vector> walls;
    private List<Vector> players;
    private GameObject gameManager;
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        walls = FindAllWalls();
        players = FindAllPlayers();
        ThrowBall();
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        players = FindAllPlayers();
        move = direction * speed * Time.deltaTime;
        //Check if moving that collides with a player
        CollisionPlayers();
        //Check if moving that collides with a wall
        CollisionWalls();
        transform.Translate(move);
        if (transform.position.x <= -10)
        {
            gameManager.GetComponent<GameManagerScript>().p1scored();
            ThrowBall();
        }
        else if (transform.position.x >= 10)
        {
            gameManager.GetComponent<GameManagerScript>().p2scored();
            ThrowBall();
        }
    }

    private Vector2 Collide(List<Vector> edges, out Vector2 edgeVector)
    {
        Vector2 ballPosNow = transform.position;
        Vector2 ballPosNext = ballPosNow + move;
        Vector2 closestIntersect = ballPosNext;
        edgeVector = new Vector2(0, 0);

        foreach (var edge in edges)
        {
            Vector2 edgeP1 = edge.p1;
            Vector2 edgeP2 = edge.p2;
            float denominator = (edgeP2.y - edgeP1.y) * (ballPosNext.x - ballPosNow.x) - (edgeP2.x - edgeP1.x) * (ballPosNext.y - ballPosNow.y);

            if (denominator == 0) continue;
            float u_a = ((edgeP2.x - edgeP1.x) * (ballPosNow.y - edgeP1.y) - (edgeP2.y - edgeP1.y) * (ballPosNow.x - edgeP1.x)) / denominator;
            float u_b = ((ballPosNext.x - ballPosNow.x) * (ballPosNow.y - edgeP1.y) - (ballPosNext.y - ballPosNow.y) * (ballPosNow.x - edgeP1.x)) / denominator;

            if (!(u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)) continue;
            float x = ballPosNow.x + u_a * (ballPosNext.x - ballPosNow.x);
            float y = ballPosNow.y + u_a * (ballPosNext.y - ballPosNow.y);
            Vector2 intersection = new Vector2(x, y);
            float distanceTemp = Vector2.Distance(intersection, ballPosNow);
            float closestTemp = Vector2.Distance(ballPosNow, closestIntersect);
            if (distanceTemp < closestIntersect.magnitude)
            {
                edgeVector = edgeP2 - edgeP1;
                closestIntersect = 99*(intersection - ballPosNow)/100;
            }            

        }
        Debug.Log("AB" + closestIntersect);
        return closestIntersect;
    }

    private void CollisionPlayers()
    {
        Vector2 ballPosNow = transform.position;
        Vector2 ballPosNext = ballPosNow + new Vector2(move.x, move.y);
        Vector2 closestIntersect = ballPosNext;
        Vector2 edgeVector;

        closestIntersect = Collide(players, out edgeVector);
        //foreach (var player in players)
        //{
        //    Vector2 playerP1 = player.p1;
        //    Vector2 playerP2 = player.p2;
        //    float denominator = (playerP2.y - playerP1.y) * (ballPosNext.x - ballPosNow.x) - (playerP2.x - playerP1.x) * (ballPosNext.y - ballPosNow.y);

        //    if (denominator == 0) continue;
        //    float u_a = ((playerP2.x - playerP1.x) * (ballPosNow.y - playerP1.y) - (playerP2.y - playerP1.y) * (ballPosNow.x - playerP1.x)) / denominator;
        //    float u_b = ((ballPosNext.x - ballPosNow.x) * (ballPosNow.y - playerP1.y) - (ballPosNext.y - ballPosNow.y) * (ballPosNow.x - playerP1.x)) / denominator;

        //    if (!(u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)) continue;
        //    float x = ballPosNow.x + u_a * (ballPosNext.x - ballPosNow.x);
        //    float y = ballPosNow.y + u_a * (ballPosNext.y - ballPosNow.y);

        //    Vector2 intersection = new Vector2(x, y);
        //    float distanceTemp = Vector2.Distance(intersection, ballPosNow);
        //    if (distanceTemp < Vector2.SqrMagnitude(closestIntersect))
        //    {
        //        edgeVector = playerP2 - playerP1;
        //        closestIntersect = 99 * (intersection - ballPosNow) / 100;
        //    }
        //}
        if (Vector2.Distance(closestIntersect, ballPosNext) >= 0.01f)
        {
            Debug.Log(direction);
            Vector2 normal = new Vector2(edgeVector.y, -edgeVector.x).normalized;
            direction = (direction - 2 * (Vector2.Dot(direction, normal) * normal)).normalized;
            move = closestIntersect + new Vector2(this.transform.localScale.x, this.transform.localScale.y)*direction;
            move = closestIntersect;
        }

    }

    private void CollisionWalls()
    {
        Vector2 ballPosNow = transform.position;
        Vector2 ballPosNext = ballPosNow + new Vector2(move.x, move.y);
        Vector2 closestIntersect;
        Vector2 edgeVector;

        closestIntersect = Collide(walls, out edgeVector);

        if (closestIntersect != ballPosNext)
        {
            Vector2 normal = new Vector2(edgeVector.y, -edgeVector.x);
            direction = direction - 2 * (Vector2.Dot(direction, normal) * normal).normalized;
            Debug.Log("Çarpma" + closestIntersect);
            Debug.Log("direction" + direction);
        }
    }

    //private void IntersectTest()
    //{
    //    Vector2 player1 = new Vector2(-5, 0);
    //    Vector2 player2 = new Vector2(5, 0);
    //    Vector2 player2 = new Vector2(5, 0);
    //}

    private void ThrowBall()
    {
        transform.position = Vector2.zero;
        int temp = Random.Range(0, 2);
        float angle = Random.Range(45, 60);
        if (temp == 0)
        {
            direction = new Vector2(-1, angle / 360).normalized;
        }
        else
        {
            direction = new Vector2(1, angle / 360).normalized;
        }
        direction = new Vector2(1, 0).normalized;

    }

    private List<Vector> FindAllWalls()
    {
        List<Vector> vectors = new List<Vector>();
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            WallScript s = wall.GetComponent<WallScript>();
            List<Vector> v = s.GetWalls();
            foreach (Vector v2 in v)
            {
                vectors.Add(v2);
            }
        }
        return vectors;
    }

    private List<Vector> FindAllPlayers()
    {
        List<Vector> vectors = new List<Vector>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            WallScript s = player.GetComponent<WallScript>();
            List<Vector> v = s.GetWalls();
            foreach (Vector v2 in v)
            {
                vectors.Add(v2);
            }
        }
        return vectors;
    }
}
