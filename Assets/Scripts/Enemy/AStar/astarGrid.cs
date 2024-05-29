using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class astarGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public TilemapCollider2D[] tilemapColliders;

    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;

    public bool displayGridGizmos;
    public bool fourCornerCheck;

    // Represents how many Nodes are on the axes
    int gridSizeX, gridSizeY;
    
    // The dimensions of the grid
    public Vector2 gridWorldSize;
    
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;

        // Calculates size of nodes to fit into the grid
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.up * gridWorldSize.y / 2);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                if (fourCornerCheck)
                {
                    /*
                     * Vector3 topLeft = worldBottomLeft + Vector3.right * (x * nodeDiameter) + Vector3.up * (y * nodeDiameter + nodeRadius * 2);
                     * Vector3 topRight = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius * 2) + Vector3.up * (y * nodeDiameter + nodeRadius * 2);
                     * Vector3 bottomLeft = worldBottomLeft + Vector3.right * (x * nodeDiameter) + Vector3.up * (y * nodeDiameter);
                     * Vector3 bottomRight = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius * 2) + Vector3.up * (y * nodeDiameter);
                     * //bool walkable = !CheckCollisionsFourCornersDeprecated(topLeft, topRight, bottomLeft, bottomRight);
                     */

                    bool walkable = !CheckCollisionsFourCorners(worldPoint);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
                else
                {

                    bool walkable = !CheckCollisionDefault(worldPoint);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }


            }
        }
    }

    bool CheckCollisionsFourCornersDeprecated(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
    {
        foreach (var collider in tilemapColliders)
        {
            if (collider.OverlapPoint(topLeft) || collider.OverlapPoint(topRight) || collider.OverlapPoint(bottomLeft) || collider.OverlapPoint(bottomRight))
                return true;
        }
        return false;
    }

    bool CheckCollisionsFourCorners(Vector3 point)
    {
        float offset = nodeRadius / 8;
        foreach (var collider in tilemapColliders)
        {
            if (collider.OverlapPoint(point + new Vector3(nodeRadius - offset, nodeRadius - offset)) ||
                collider.OverlapPoint(point + new Vector3(-nodeRadius + offset, nodeRadius - offset)) ||
                collider.OverlapPoint(point + new Vector3(nodeRadius - offset, -nodeRadius + offset)) ||
                collider.OverlapPoint(point + new Vector3(-nodeRadius + offset, -nodeRadius + offset)))
            {
                return true;
            }
        }
        return false;
    }

    bool CheckCollisionDefault(Vector3 point)
    {
        foreach(var collider in tilemapColliders)
        {
            if (collider.OverlapPoint(point))
                return true;
        }
        return false;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            { 
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}
