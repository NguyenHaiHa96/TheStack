using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private float offset = 0.3f;
    private Vector3 desiredPosition;
    private UIManager UI_Manager;
    private float desiredDuration = 1f;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        UI_Manager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        MovingBlock.OnBlockSpawned += MovingBlock_OnBlockSpawned;
    }

    private void OnDestroy()
    {
        MovingBlock.OnBlockSpawned -= MovingBlock_OnBlockSpawned;
    }

    private void MovingBlock_OnBlockSpawned()
    { 
        if (UI_Manager.GetCurrentScore() >= 3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);           
        }       
    }

    // Update is called once per frame
    void Update()
    {
    }
}
