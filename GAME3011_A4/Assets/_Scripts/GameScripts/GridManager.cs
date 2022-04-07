using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Dimensions")]
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    [SerializeField] private float fillDuration;

    // Background Pieces
    [SerializeField] private GameObject backgroundPrefabObject;
    private float pieceDimension;

    // List of pieces to spawn, for every tile element, we're going to keep a list of potential prefabs, then do a random count from there
    [SerializeField] private List<GameObject> pipePrefabs; 

    // Pipe Array and stocked piece
    // when you click on the pipe array, it will take the pipe from that position and swap it with the stocked piece
    private Pipe[,] pipeArray;
    [SerializeField] private Pipe stockedPipe;
    [SerializeField] private Transform stockedPipeTransform;

    private void InitializeGameBoard()
    {
        GameObject gridTile = null;
        pieceDimension = backgroundPrefabObject.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                gridTile = Instantiate(backgroundPrefabObject, GetGridPosition(i, j), Quaternion.identity);
                gridTile.transform.SetParent(this.transform);
            }
        }

        pipeArray = new Pipe[gridX, gridY];

        for (int x = 1; x < gridX - 1; x++)
        {
            for (int y = 1; y < gridY -1; y++)
            {
                SpawnPipe(x, y);
            }
        }

        int stockRandomSelection = (int)Random.Range(0, pipePrefabs.Count);
        if (stockedPipeTransform != null)
        {
            GameObject stockedPiece = Instantiate(pipePrefabs[stockRandomSelection], stockedPipeTransform.position, Quaternion.identity);

            stockedPipe = stockedPiece.GetComponent<Pipe>();
            stockedPipe.transform.SetParent(this.transform);
            stockedPipe.InitPipe(-1, -1, this, PipeEnum.PIPE);
            //stockedPiece.transform.SetParent(this.transform);

        }
    }

    public Pipe SpawnPipe(int x, int y)
    {
        // randomize the pipe prefabs
        int randomSelection = (int)Random.Range(0, pipePrefabs.Count);

        GameObject pipeToSpawn = Instantiate(pipePrefabs[randomSelection], GetGridPosition(x, y), Quaternion.identity);
        
        pipeArray[x, y] = pipeToSpawn.GetComponent<Pipe>();
        pipeArray[x, y].transform.SetParent(this.transform);
        pipeArray[x, y].InitPipe(x, y, this, PipeEnum.PIPE);
        return pipeArray[x, y];
    }

    // taking into account of a pipe's x and y position, this is called from the pipe script
    public void SwapPipe(int x, int y)
    {
        // temp reference to the stock pipe
        Pipe tempPipe = stockedPipe;
        //pipeArray[x, y].transform.position = stockedPipe.transform.position;

        stockedPipe.transform.position = pipeArray[x, y].transform.position;
        stockedPipe = pipeArray[x, y];
        stockedPipe.SetNewPipePosition(-1, -1);
        //stockedPipe.transform.position = tempPipe.transform.position;
        pipeArray[x, y].transform.position = stockedPipeTransform.position;
        pipeArray[x, y] = tempPipe;
        pipeArray[x, y].SetNewPipePosition(x, y);

        //tempPipe = null;
     

        Debug.Log(pipeArray[x, y] + ": Pos (" + x + ", " + y);
        //if (stockedPipe != null)
        //{
        //    hasSwiped = true;
        //}

        //return hasSwiped;
    }

    void ClearPipes()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Destroy(pipeArray[x, y].gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        InitializeGameBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetGridPosition(int x, int y)
    {
        Vector2 GridPos = new Vector2(transform.position.x - ((gridX / 2) * pieceDimension) + x * pieceDimension,
                                    transform.position.y - ((gridY / 2) * pieceDimension) + y * pieceDimension);
        return GridPos;
    }
}
