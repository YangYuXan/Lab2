using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private float distance=0;
    private List<GameObject> pickItemsComponents = new List<GameObject>();
    private PlayerController playerController;
    private Rigidbody playerRigidbody;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        playerController = GetComponent<PlayerController>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerController.gameMode)
        {
            case PlayerController.GameMode.Normal:
                SetUIVisible(playerController, false);
                lineRenderer.enabled = false;
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("PickUp").Length; i++)
                {
                    GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<Renderer>().material.color = Color.white;
                }
                break;

            case PlayerController.GameMode.Distance:
                SetUIVisible(playerController, true);
                lineRenderer.enabled = true;
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("PickUp").Length; i++)
                {
                    GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<Renderer>().material.color = Color.white;
                    if (GameObject.FindGameObjectsWithTag("PickUp")[i].activeSelf == true)
                    {
                        pickItemsComponents.Add(GameObject.FindGameObjectsWithTag("PickUp")[i]);
                    }
                }

                distance = Vector3.Distance(transform.position, pickItemsComponents[0].transform.position);
                float distance_buff;
                int index=0;

                for (int i = 1; i < pickItemsComponents.Count; i++)
                {
                    distance_buff = Vector3.Distance(transform.position, pickItemsComponents[i].transform.position);
                    if (distance_buff<distance)
                    {
                        distance = distance_buff;
                        index = i;
                    }
                    playerController.distance.text = "Distance: " + distance;
                }
                pickItemsComponents[index].GetComponent<Renderer>().material.color = Color.blue;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, pickItemsComponents[index].transform.position);
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;

                pickItemsComponents.Clear();
                break;

            case PlayerController.GameMode.Vision:
                SetUIVisible(playerController,false);
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                Vector3 velocity = playerRigidbody.velocity;
                lineRenderer.SetPosition(1, transform.position + velocity);
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;

                for (int i = 0; i < GameObject.FindGameObjectsWithTag("PickUp").Length; i++)
                {
                    GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<Renderer>().material.color = Color.white;
                    Vector3 toPlayer = GameObject.FindGameObjectsWithTag("PickUp")[i].transform.position-transform.position;
                    toPlayer.Normalize();
                    float dotProduct = Vector3.Dot(playerRigidbody.velocity.normalized, toPlayer);
                    if (dotProduct > 0.5f)
                    {
                        GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<Renderer>().material.color = Color.green;
                        GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<Rotater>().shouldRotate = false;
                        GameObject.FindGameObjectsWithTag("PickUp")[i].transform.LookAt(transform.position);
                    }
                    else
                    {
                        GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<Rotater>().shouldRotate = true;
                    }
                }

                break;
        }
        
    }

    void SetUIVisible(PlayerController controller,bool visible)
    {
        controller.distance.gameObject.SetActive(visible);
        controller.position.gameObject.SetActive(visible);
        controller.velocity.gameObject.SetActive(visible);
    }
}
