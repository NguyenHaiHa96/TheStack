using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] MovingBlock blockPrefab;

    public static bool isSpawning = true;

    public bool isMovingOnZ = true;

    private int stackUp = 0;

    public bool GetIsMovingOnZ()
    {
        return isMovingOnZ;
    }

    public bool GetIsSpawning()
    {
        return isSpawning;
    }

    public void SetIsSpawning(bool state)
    {
        isSpawning = state;
    }

    public void SetIsMovingOnZ(bool state)
    {
        isMovingOnZ = state;
    }

    private void Awake()
    {
        SetIsSpawning(true);
        Transform T = GameObject.FindGameObjectWithTag("GroundBlock").GetComponent<Transform>();
        float y = T.position.y + (T.localScale.y / 2f) + (blockPrefab.transform.localScale.y / 2f);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void SpawnBlock()
    {
        if (isSpawning)
        {
            var _movingBlock = Instantiate(blockPrefab);
            if (isMovingOnZ)
            {
                if (MovingBlock.LastBlock != null)
                {
                    if (stackUp <= 0)
                    {
                        stackUp++;
                        isMovingOnZ = false;
                        _movingBlock.transform.position = transform.position;
                    }
                    else
                    {
                        _movingBlock.transform.position = new Vector3(MovingBlock.LastBlock.transform.position.x, transform.position.y + (blockPrefab.transform.localScale.y * stackUp), transform.position.z);
                        stackUp++;
                        isMovingOnZ = false;
                    }
                }
            }
            else
            {
                if (MovingBlock.LastBlock != null)
                {
                    _movingBlock.transform.position = new Vector3(transform.position.x, transform.position.y + (blockPrefab.transform.localScale.y * stackUp), MovingBlock.LastBlock.transform.position.z);
                    stackUp++;
                    isMovingOnZ = true;
                }
            }
        }      
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, blockPrefab.transform.localScale);
    }
}
