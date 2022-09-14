using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool p2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p2)
        {
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -4)
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < 4)
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime));
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.S) && transform.position.y > -4)
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.W) && transform.position.y < 4)
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime));
            }
        }
    }

}
