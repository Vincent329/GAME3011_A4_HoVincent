using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Dimensions")]
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    [SerializeField] private float fillDuration;

    // Pieces
    [SerializeField] private GameObject backgroundPrefabObject;
    private float pieceDimension;

    // Pipe Array and stocked piece
    // when you click on the pipe array, it will take the pipe from that position and swap it with the stocked piece
    private Pipe[,] pipeArray;
    private Pipe stockedPiece;

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
