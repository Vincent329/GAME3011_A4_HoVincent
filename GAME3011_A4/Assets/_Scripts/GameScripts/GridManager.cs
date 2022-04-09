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
    [SerializeField] private List<GameObject> startPipePrefabs; // create the logic to have one of these spawn on the grid
    [SerializeField] private List<GameObject> endPipePrefabs; // create the logic to have one of these spawn on the grid

    // Pipe Array and stocked piece
    // when you click on the pipe array, it will take the pipe from that position and swap it with the stocked piece
    private Pipe[,] pipeArray;
    [SerializeField] private Pipe stockedPipe;
    [SerializeField] private Transform stockedPipeTransform;
    
    // Keep track of pipes that are of importance
    // Start Pipe
    // pipe currently being filled
    // End pipe
    [SerializeField] private StartPipe startingPipe;
    [SerializeField] private Pipe fillInProgressPipe;
    [SerializeField] private EndPipe endPipe;

    // Keep track of difficulty locally
    [SerializeField] private DifficultyEnum difficultySet;

    private void OnEnable()
    {
        fillInProgressPipe = null;
        InitializeGameBoard();
    }

    private void OnDisable()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void InitializeGameBoard()
    {
        difficultySet = GameManager.Instance.difficultyEnum;
        if (difficultySet == DifficultyEnum.EASY)
        {
            gridX = 6;
            gridY = 6;
        } else if (difficultySet == DifficultyEnum.NORMAL)
        {
            gridX = 10;
            gridY = 10;
        } else if (difficultySet == DifficultyEnum.HARD)
        {
            gridX = 13;
            gridY = 13;
        }

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

        // initialize the start pipe
        int startPosition = (int)Random.Range(1, gridY - 1);
        GameObject startPipe = Instantiate(startPipePrefabs[0], GetGridPosition(0, startPosition), Quaternion.identity);
        startingPipe = startPipe.GetComponent<StartPipe>();
        startingPipe.InitPipe(0, startPosition, this, PipeEnum.START);
        startingPipe.transform.SetParent(this.transform);
        pipeArray[0, startPosition] = startingPipe;
        
        // initialize the end pipe
        int endPosition = (int)Random.Range(1, gridY - 1);
        GameObject finishPipe = Instantiate(endPipePrefabs[0], GetGridPosition(gridX - 1, endPosition), Quaternion.identity);
        endPipe = finishPipe.GetComponent<EndPipe>();
        endPipe.InitPipe(gridX - 1, endPosition, this, PipeEnum.FINISH);
        endPipe.transform.SetParent(this.transform);
        pipeArray[gridX - 1, endPosition] = endPipe;
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

    public Vector2 GetGridPosition(int x, int y)
    {
        Vector2 GridPos = new Vector2(transform.position.x - ((gridX / 2) * pieceDimension) + x * pieceDimension,
                                    transform.position.y - ((gridY / 2) * pieceDimension) + y * pieceDimension);
        return GridPos;
    }
   
    public void CheckExit(Pipe finishedPipe, int xPos, int yPos)
    {
        foreach(PipeOpenings openings in finishedPipe.GetPipeOpenings)
        {
            // between the two openings, check which one is the 
            if (finishedPipe.PipeType == PipeEnum.START)
            {
                Debug.Log("START!?");
                CheckNextPipe(xPos, yPos);
            }
            else if (finishedPipe.PipeType != PipeEnum.START && finishedPipe.GetEntryPoint != openings)
            {
                Debug.Log("BYPASS!?");
                pipeArray[xPos, yPos].ExitPoint = openings; // assign the exit point
                CheckNextPipe(xPos, yPos);
            } 
        }
    }

    // go through the grid, and check the current pipe that has just finished
    // called from the pipe in question
    private void CheckNextPipe(int xPos, int yPos)
    {
        bool exitFound = false;
        // take the current exit point of the pipe, and check the array
        switch(pipeArray[xPos, yPos].ExitPoint)
        {
            case PipeOpenings.UP:
                if (pipeArray[xPos, yPos + 1] != null)
                {
                    foreach (PipeOpenings pipeOpenings in pipeArray[xPos, yPos + 1].GetPipeOpenings)
                    {
                        if (pipeOpenings == PipeOpenings.DOWN)
                        {
                            Debug.Log("Entry");

                            pipeArray[xPos, yPos + 1].CheckOpenings(pipeArray[xPos, yPos].ExitPoint);
                            fillInProgressPipe = pipeArray[xPos, yPos + 1];
                            exitFound = true;
                            break;
                        }
                    }
                    if (!exitFound)
                    {
                        Debug.Log("GameOver");
                    }
                } 
                else // out of bounds case
                {
                    Debug.Log("GameOver");
                }
                break;
            case PipeOpenings.DOWN:
                if (pipeArray[xPos, yPos - 1] != null)
                {
                    foreach (PipeOpenings pipeOpenings in pipeArray[xPos, yPos - 1].GetPipeOpenings)
                    {
                        if (pipeOpenings == PipeOpenings.UP)
                        {
                            Debug.Log("Entry");

                            pipeArray[xPos, yPos - 1].CheckOpenings(pipeArray[xPos, yPos].ExitPoint);
                            fillInProgressPipe = pipeArray[xPos, yPos - 1];
                            exitFound = true;
                            break;
                        }
                    }
                    if (!exitFound)
                    {
                        Debug.Log("GameOver");
                    }
                }
                else
                {
                    Debug.Log("GameOver");
                }
                break;
            case PipeOpenings.LEFT:
                if (pipeArray[xPos - 1, yPos] != null)
                {
                    foreach (PipeOpenings pipeOpenings in pipeArray[xPos - 1, yPos].GetPipeOpenings)
                    {
                        if (pipeOpenings == PipeOpenings.RIGHT)
                        {
                            pipeArray[xPos - 1, yPos].CheckOpenings(pipeArray[xPos, yPos].ExitPoint);
                            fillInProgressPipe = pipeArray[xPos - 1, yPos];

                            exitFound = true;
                            break;
                        }
                    }
                    if (!exitFound)
                    {
                        Debug.Log("GameOver");
                    }
                }
                else
                {
                    Debug.Log("GameOver");
                }
                break;
            case PipeOpenings.RIGHT:
                if (pipeArray[xPos + 1, yPos] != null)
                {
                    foreach (PipeOpenings pipeOpenings in pipeArray[xPos + 1, yPos].GetPipeOpenings)
                    {
                        if (pipeOpenings == PipeOpenings.LEFT)
                        {
                            pipeArray[xPos + 1, yPos].CheckOpenings(pipeArray[xPos, yPos].ExitPoint);
                            fillInProgressPipe = pipeArray[xPos + 1, yPos];
                            exitFound = true;
                            break;
                        }
                    }
                    if (!exitFound)
                    {
                        Debug.Log("GameOver");
                    }
                }
                else
                {
                    Debug.Log("GameOver");
                }
                break;
            default:
                Debug.Log("Error Case");
                break;
        }
    }

}
