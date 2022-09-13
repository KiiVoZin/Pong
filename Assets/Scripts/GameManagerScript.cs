using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    private int p1score;
    private int p2score;
    [SerializeField] TextMeshProUGUI p1text;
    [SerializeField] TextMeshProUGUI p2text;
    [SerializeField] float playerSpeed;
    [SerializeField] float ballSpeed;
    [SerializeField] float hitAngle;
    [SerializeField] Camera cam;
    [SerializeField] GameObject p1;
    [SerializeField] GameObject p2;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject upWall;
    [SerializeField] GameObject downWall;
    Vector2 direction;
    Vector2 lastPos;

    float ballSpeedInit;
    float playerSpeedInit;

    Vector3 ballPos;
    Vector3 p1Pos;
    Vector3 p2Pos;
    Vector3 upWallPos;
    Vector3 downWallPos;

    Vector3 ballScl;
    Vector3 p1Scl;
    Vector3 p2Scl;
    Vector3 upWallScl;
    Vector3 downWallScl;
    // Start is called before the first frame update
    void Start()
    {
        p1score = 0;
        p2score = 0;
        ballSpeedInit = ballSpeed;
        playerSpeedInit = playerSpeed;
        ThrowBall();
        lastPos = ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ballPos = ball.transform.position;
        p1Pos = p1.transform.position;
        p2Pos = p2.transform.position;
        upWallPos = upWall.transform.position;
        downWallPos = downWall.transform.position;
        
        ballScl = ball.transform.localScale;
        p1Scl = p1.transform.localScale;
        p2Scl = p2.transform.localScale;
        upWallScl = upWall.transform.localScale;
        downWallScl = downWall.transform.localScale;

        if(Input.GetKey(KeyCode.W) && p1Pos.y + p1Scl.y/2 <= upWallPos.y - upWallScl.y/2){
            p1.transform.Translate(new Vector2(0, playerSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.UpArrow) && p2Pos.y + p2Scl.y/2 <= upWallPos.y - upWallScl.y/2){
            p2.transform.Translate(new Vector2(0, playerSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.S) && p1Pos.y - p1Scl.y/2 >= downWallPos.y + downWallScl.y/2){
            p1.transform.Translate(new Vector2(0, -playerSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.DownArrow) && p2Pos.y - p2Scl.y/2 >= downWallPos.y + downWallScl.y/2){
            p2.transform.Translate(new Vector2(0, -playerSpeed * Time.deltaTime));
        }
        
        Vector2 movement = direction * ballSpeed * Time.deltaTime;
        Vector2 movementUp = IsIntersecting(ballPos, ballPos + new Vector3(movement.x, movement.y, 0), new Vector2(upWallPos.x - upWallScl.x/2, upWallPos.y - upWallScl.y/2), new Vector2(upWallPos.x + upWallScl.x/2, upWallPos.y - upWallScl.y/2));
        Vector2 movementDown = IsIntersecting(ballPos, ballPos + new Vector3(movement.x, movement.y, 0), new Vector2(downWallPos.x - downWallScl.x/2, downWallPos.y + downWallScl.y/2), new Vector2(downWallPos.x + downWallScl.x/2, downWallPos.y + downWallScl.y/2));
        Vector2 movementP1 = IsIntersecting(ballPos, ballPos + new Vector3(movement.x, movement.y, 0), new Vector2(p1Pos.x + p1Scl.x/2, p1Pos.y + p1Scl.y/2), new Vector2(p1Pos.x + p1Scl.x/2, p1Pos.y - p1Scl.y/2));
        Vector2 movementP2 = IsIntersecting(ballPos, ballPos + new Vector3(movement.x, movement.y, 0), new Vector2(p2Pos.x - p2Scl.x/2, p2Pos.y + p2Scl.y/2), new Vector2(p2Pos.x - p2Scl.x/2, p2Pos.y - p2Scl.y/2));
        if(movementUp.x != ballPos.x || movementUp.y != ballPos.y){
            Debug.Log("Yoh");
            ballPos = new Vector3(movementUp.x, movementUp.y, ballPos.z);
        }else if(movementDown.x != ballPos.x || movementDown.y != ballPos.y){
            ballPos = new Vector3(movementDown.x, movementDown.y, ballPos.z);
        }else if(movementP1.x != ballPos.x || movementP1.y != ballPos.y){
            ballPos = new Vector3(movementP2.x, movementP2.y, ballPos.z);
        }else if(movementP2.x != ballPos.x || movementP2.y != ballPos.y){
            ballPos = new Vector3(movementP2.x, movementP2.y, ballPos.z);
        }else{
            ball.transform.Translate(movement);
        }
        Bounce();
        Debug.DrawRay(ballPos, direction*5, Color.blue);
        Debug.DrawRay(ballPos, -direction*5, Color.green);
        //ballSpeed += 0.0001f;
        playerSpeed += 0.00002f;
        lastPos = ballPos;
    }

    public void Bounce(){
        if(ballPos.y + ballScl.y/2 >= upWallPos.y - upWallScl.y/2 && ballPos.x + ballScl.x/2 >= upWallPos.x - upWallScl.x/2 && ballPos.x + ballScl.x/2 <= upWallPos.x + upWallScl.x/2){
            direction = new Vector2(direction.x, -direction.y);
        }else if(ballPos.y - ballScl.y/2 <= downWallPos.y + downWallScl.y/2 && ballPos.x + ballScl.x/2 >= downWallPos.x - downWallScl.x/2 && ballPos.x + ballScl.x/2 <= downWallPos.x + downWallScl.x/2){
            direction = new Vector2(direction.x, -direction.y);
        }
        
        if(ballPos.x - ballScl.x/2 <= p1Pos.x + p1Scl.x/2 && ballPos.y - ballScl.y/2 <= p1Pos.y + p1Scl.y/2 && ballPos.y + ballScl.y/2 >= p1Pos.y - p1Scl.y/2){
            direction = GenerateAngle(p1);
        }else if(ballPos.x + ballScl.x/2 >= p2Pos.x - p2Scl.x/2 && ballPos.y - ballScl.y/2 <= p2Pos.y + p2Scl.y/2 && ballPos.y + ballScl.y/2 >= p2Pos.y - p2Scl.y/2){
            direction = GenerateAngle(p2);
        }

        if(ballPos.x <= leftWall.transform.position.x){
            p1scored();
        }else if(ballPos.x >= rightWall.transform.position.x){
            p2scored();
        }
    }

    public Vector2 GenerateAngle(GameObject player){
        Vector2 vector;
        float angle = (ballPos.y - player.transform.position.y)/(player.transform.localScale.y/2) * hitAngle;
        if(player == p1){
            vector = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)).normalized * ballSpeed;
        }else{
            vector = new Vector2(-Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)).normalized * ballSpeed;
        }
        return vector;
    }
    public void ThrowBall(){

        int temp = Random.Range(0, 2);
        if(temp == 0){
            direction = new Vector2(ballSpeed, 0);
        }else{
            direction = new Vector2(-ballSpeed, 0);
        }
    }

    Vector2 IsIntersecting(Vector2 ball1, Vector2 ball2, Vector2 p1, Vector2 p2)
    {
        float denominator = (p2.y - p1.y) * (ball2.x - ball1.x) - (p2.x - p1.x) * (ball2.y - ball1.y);
        
        if (denominator != 0)
        {
            float u_a = ((p2.x - p1.x) * (ball1.y - p1.y) - (p2.y - p1.y) * (ball1.x - p1.x)) / denominator;
            float u_b = ((ball2.x - ball1.x) * (ball1.y - p1.y) - (ball2.y - ball1.y) * (ball1.x - p1.x)) / denominator;

            if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
            {
                float x = ball1.x + u_a*(ball2.x - ball1.x);
                float y = ball1.y + u_a*(ball2.y - ball1.y);
                return new Vector2(x, y);
            }
        }

        return ball1;
    }

    public void p1scored(){
        p1score++;
        p1text.text = "" + p1score;
        ballPos = new Vector3(0, 0, ballPos.z);
        ThrowBall();
        ballSpeed = ballSpeedInit;
        playerSpeed = playerSpeedInit;
    }

    public void p2scored(){
        p2score++;
        p2text.text = "" + p2score;
        ballPos = new Vector3(0, 0, ballPos.z);
        ThrowBall();
        ballSpeed = ballSpeedInit;
        playerSpeed = playerSpeedInit;
    }
}
