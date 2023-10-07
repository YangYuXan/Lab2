using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Vector2 moveValue;
    public float speed;

    public TextMeshProUGUI score;
    public TextMeshProUGUI win;
    public TextMeshProUGUI position;
    public TextMeshProUGUI velocity;
    public TextMeshProUGUI distance;


    private int count;
    private int numPickups = 8;
    private Vector3 curPos;
    private Vector3 lasPos;
    private float Show_speed;

    public enum GameMode
    {
        Normal,
        Distance,
        Vision
    }

    public int currentIndex;
    public GameMode gameMode = GameMode.Normal;

    // Start is called before the first frame update
    void Start()
    {
        //Initial Value
        count = 0;
        win.text = "";
        currentIndex = (int)gameMode;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);

        switch (gameMode)
        {
            case GameMode.Normal:
                break;
            case GameMode.Distance:
                position.text="Position: "+transform.position.ToString();
                curPos = transform.position;
                Show_speed = Vector3.Magnitude((curPos - lasPos) / Time.deltaTime);
                velocity.text = "Velocity: " +Show_speed.ToString();
                lasPos = curPos;
                break;
            case GameMode.Vision:
                break;
        }

        
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void OnSwitchMode(InputValue value)
    {
        currentIndex = (currentIndex+1)% System.Enum.GetValues(typeof(GameMode)).Length;
        gameMode = (GameMode)currentIndex;
        switch (gameMode)
        {
            case GameMode.Normal:
                velocity.gameObject.SetActive(false);
                position.gameObject.SetActive(false);
                break;
            case GameMode.Distance:
                velocity.gameObject.SetActive(true);
                position.gameObject.SetActive(true);
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }

        
    }



    private void SetCountText()
    {
        score.text = " Score : " + count.ToString();
        if (count >= numPickups)
        {
            score.text = "";
            win.text = " You win ! ";
        }
    }
}