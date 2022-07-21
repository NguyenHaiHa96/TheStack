using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingBlock : MonoBehaviour
{
    public static event Action IsOverTheEdge = delegate { };

    public static event Action OnBlockSpawned = delegate { };

    public static MovingBlock CurrentBlock { get; set; }
    public static MovingBlock LastBlock { get; set; }

    public const float ACCEPTABLE_HANGOVER = 0.1f;

    private float boundsLimit = 4f;
    private float blockTransition = 0.0f;
    private float moveSpeed = 1.5f;

    private bool isGameOver = false;
    private bool isMovingOnZ;

    private void OnEnable()
    {
        if (LastBlock == null)
        {
            LastBlock = GameObject.FindGameObjectWithTag("GroundBlock").GetComponent<MovingBlock>();
            LastBlock.transform.position = GameObject.FindGameObjectWithTag("GroundBlock").GetComponent<Transform>().transform.position;
        }
        CurrentBlock = this;
        GetComponent<Renderer>().material.color = GetRandomColor();
        transform.localScale = new Vector3(MovingBlock.LastBlock.transform.localScale.x, transform.localScale.y, MovingBlock.LastBlock.transform.localScale.z);
        isMovingOnZ = FindObjectOfType<BlockSpawner>().GetIsMovingOnZ();
        GetComponent<BoxCollider>().enabled = true;
    }

    void Update()
    {
        if (moveSpeed != 0)
        {
            Move();
        }
    }

    public void Move()
    {
        blockTransition += Time.deltaTime * moveSpeed;
        if (gameObject.CompareTag("MovingBlock"))
        {
            if (isMovingOnZ)
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Sin(blockTransition) * boundsLimit);
            else
                transform.position = new Vector3(Mathf.Sin(blockTransition) * boundsLimit, transform.position.y, transform.position.z);
        }
        if (gameObject.CompareTag("GroundBlock"))
        {
            transform.position = GameObject.FindGameObjectWithTag("GroundBlock").GetComponent<Transform>().transform.position;
        }
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    public void Reset()
    {
        CurrentBlock = null;
        LastBlock = null;
    }

    private float GetHangover()
    {
        if (isMovingOnZ)
            return transform.position.z - LastBlock.transform.position.z;
        else
            return transform.position.x - LastBlock.transform.position.x;
    }

    private float GetLastBlockScale()
    {
        if (isMovingOnZ)
            return LastBlock.transform.localScale.z;
        else
            return LastBlock.transform.localScale.x;
    }
    
    internal void Stop()
    {
        moveSpeed = 0f;
        float _hangover = GetHangover();
        float _lastBlockScale = GetLastBlockScale();
        float _direction = (_hangover > 0) ? 1.0f : -1.0f;

        if (CurrentBlock != null && LastBlock != null)
        {
            if (Mathf.Abs(_hangover) <= ACCEPTABLE_HANGOVER)
            {
                _hangover = 0f;           
            }
            if (Mathf.Abs(_hangover) > _lastBlockScale)
            {
                gameObject.AddComponent<Rigidbody>();
                Reset();
                IsOverTheEdge();
            }
            if (isMovingOnZ)
                SplitBlockOnZ(_hangover, _direction);
            else
                SplitBlockOnX(_hangover, _direction);         
        }
        LastBlock = this;
    }

    private void SplitBlockOnX(float hangover, float direction)
    {
        if (LastBlock != null)
        {
            float _stayingBlockPositionOnX = LastBlock.transform.position.x + (hangover / 2f);
            float _stayingBlockScaleOnX = transform.localScale.x - Mathf.Abs(hangover);

            float _cuttingEdgePosition = _stayingBlockPositionOnX + (_stayingBlockScaleOnX / 2f) * direction;
            float _fallingBlockSizeScaleOnX = Mathf.Abs(hangover);
            float _fallingBlockPositionOnX = _cuttingEdgePosition + (_fallingBlockSizeScaleOnX / 2f) * direction;

            SpawnStayingBlockOnX(_stayingBlockPositionOnX, _stayingBlockScaleOnX);

            if (Mathf.Abs(hangover) > ACCEPTABLE_HANGOVER) 
                SpawnFallingBlockOnX(_fallingBlockPositionOnX, _fallingBlockSizeScaleOnX);
        }
    }

    private void SpawnStayingBlockOnX(float stayingBlockPositionOnX, float stayingBlockScaleOnX)
    {
        OnBlockSpawned();
        transform.position = new Vector3(stayingBlockPositionOnX, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(stayingBlockScaleOnX, transform.localScale.y, transform.localScale.z);
    }

    private void SpawnFallingBlockOnX(float fallingBlockPositionOnX, float fallingBlockSizeScaleOnX)
    {
        var _fallingBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _fallingBlock.transform.position = new Vector3(fallingBlockPositionOnX, transform.position.y, transform.position.z);
        _fallingBlock.transform.localScale = new Vector3(fallingBlockSizeScaleOnX, transform.localScale.y, transform.localScale.z);
        _fallingBlock.AddComponent<Rigidbody>();
        Destroy(_fallingBlock, 2f);
    }

    private void SplitBlockOnZ(float hangover, float direction)
    {
        if (LastBlock != null)
        {
            float _stayingBlockPositionOnZ = LastBlock.transform.position.z + (hangover / 2f);
            float _stayingBlockScaleOnZ = transform.localScale.z - Mathf.Abs(hangover);

            float _cuttingEdgePosition = _stayingBlockPositionOnZ + (_stayingBlockScaleOnZ / 2f) * direction;
            float _fallingBlockSizeScaleOnZ = Mathf.Abs(hangover);
            float _fallingBlockPositionOnZ = _cuttingEdgePosition + (_fallingBlockSizeScaleOnZ / 2f) * direction;

            SpawnStayingBlockOnZ(_stayingBlockPositionOnZ, _stayingBlockScaleOnZ);

            if (Mathf.Abs(hangover) > ACCEPTABLE_HANGOVER)
                SpawnFallingBlockOnZ(_fallingBlockPositionOnZ, _fallingBlockSizeScaleOnZ);
        }         
    }

    private void SpawnStayingBlockOnZ(float stayingBlockPositionOnZ, float stayingBlockScaleOnZ)
    {
        OnBlockSpawned();
        transform.position = new Vector3(transform.position.x, transform.position.y, stayingBlockPositionOnZ);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, stayingBlockScaleOnZ);   
    }

    private void SpawnFallingBlockOnZ(float fallingBlockPositionOnZ, float fallingBlockSizeScaleOnZ)
    {
        var _fallingBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _fallingBlock.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPositionOnZ);
        _fallingBlock.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSizeScaleOnZ);
        _fallingBlock.AddComponent<Rigidbody>();
        Destroy(_fallingBlock, 2f);      
    }
}
