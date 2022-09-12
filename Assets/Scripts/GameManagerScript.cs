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

    float ballSpeedInit;
    float playerSpeedInit;
    // Start is called before the first frame update
    void Start()
    {
        p1score = 0;
        p2score = 0;
        ballSpeedInit = ballSpeed;
        ThrowBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) && p1.transform.position.y + p1.transform.localScale.y/2 <= upWall.transform.position.y - upWall.transform.localScale.y/2){
            p1.transform.Translate(new Vector2(0, playerSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.UpArrow) && p2.transform.position.y + p2.transform.localScale.y/2 <= upWall.transform.position.y - upWall.transform.localScale.y/2){
            p2.transform.Translate(new Vector2(0, playerSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.S) && p1.transform.position.y - p1.transform.localScale.y/2 >= downWall.transform.position.y + downWall.transform.localScale.y/2){
            p1.transform.Translate(new Vector2(0, -playerSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.DownArrow) && p2.transform.position.y - p2.transform.localScale.y/2 >= downWall.transform.position.y + downWall.transform.localScale.y/2){
            p2.transform.Translate(new Vector2(0, -playerSpeed * Time.deltaTime));
        }
        
        ball.transform.Translate(direction * ballSpeed * Time.deltaTime);
        Bounce();
        //ballSpeed += 0.0001f;
        playerSpeed += 0.00002f;
    }

    public void Bounce(){
        if(ball.transform.position.y + ball.transform.localScale.y/2 >= upWall.transform.position.y - upWall.transform.localScale.y/2 && ball.transform.position.x + ball.transform.localScale.x/2 >= upWall.transform.position.x - upWall.transform.localScale.x/2 && ball.transform.position.x + ball.transform.localScale.x/2 <= upWall.transform.position.x + upWall.transform.localScale.x/2){
            direction = new Vector2(direction.x, -direction.y);
        }else if(ball.transform.position.y - ball.transform.localScale.y/2 <= downWall.transform.position.y + downWall.transform.localScale.y/2 && ball.transform.position.x + ball.transform.localScale.x/2 >= downWall.transform.position.x - downWall.transform.localScale.x/2 && ball.transform.position.x + ball.transform.localScale.x/2 <= downWall.transform.position.x + downWall.transform.localScale.x/2){
            direction = new Vector2(direction.x, -direction.y);
        }
        
        if(ball.transform.position.x - ball.transform.localScale.x/2 <= p1.transform.position.x + p1.transform.localScale.x/2 && ball.transform.position.y - ball.transform.localScale.y/2 <= p1.transform.position.y + p1.transform.localScale.y/2 && ball.transform.position.y + ball.transform.localScale.y/2 >= p1.transform.position.y - p1.transform.localScale.y/2){
            direction = GenerateAngle(p1);
        }else if(ball.transform.position.x + ball.transform.localScale.x/2 >= p2.transform.position.x - p2.transform.localScale.x/2 && ball.transform.position.y - ball.transform.localScale.y/2 <= p2.transform.position.y + p2.transform.localScale.y/2 && ball.transform.position.y + ball.transform.localScale.y/2 >= p2.transform.position.y - p2.transform.localScale.y/2){
            direction = GenerateAngle(p2);
        }

        if(ball.transform.position.x <= leftWall.transform.position.x){
            p1scored();
        }else if(ball.transform.position.x >= rightWall.transform.position.x){
            p2scored();
        }
    }

    public Vector2 GenerateAngle(GameObject player){
        Vector2 vector;
        float angle = (ball.transform.position.y - player.transform.position.y)/(player.transform.localScale.y/2) * hitAngle;
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

    public void p1scored(){
        p1score++;
        p1text.text = "" + p1score;
        ball.transform.position = new Vector3(0, 0, ball.transform.position.z);
        ThrowBall();
        ballSpeed = ballSpeedInit;
        playerSpeed = playerSpeedInit;
    }

    public void p2scored(){
        p2score++;
        p2text.text = "" + p2score;
        ball.transform.position = new Vector3(0, 0, ball.transform.position.z);
        ThrowBall();
        ballSpeed = ballSpeedInit;
        playerSpeed = playerSpeedInit;
    }
}
