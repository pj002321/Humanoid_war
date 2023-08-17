using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMap : MonoBehaviour
{
    public int MazeWidth;   // �̷��� �ʺ�
    public int MazeHeight;  // �̷��� ����
    public float WallSize; // ���� �β�
    public GameObject wallPrefab; // �� ������
    public GameObject PlanPrefab; // �ٴ� ������
    public GameObject TreePrefab; // ���� ������
    private GameObject RotateCubePrefab; // ���� ������

    private GameObject planeHex;
    private int[,] maze; // �̷� �迭

    private void Start()
    {
        maze = new int[MazeHeight, MazeWidth];
        CrateMaze();
        RotateCubePrefab = GameObject.Find("RotateCube");
    }
    void Update()
    {
        RotateCubePrefab.transform.Rotate(0,5,0);
    }
    private void CrateMaze()
    {
        // Init maze[,]
        for (int row = 0; row < MazeHeight; row++)
        {
            for (int col = 0; col < MazeWidth; col++)
            {
                maze[row, col] = 1;
            }
        }

        // ��/�Ա� ������ֱ�
        maze[0, 1] = 0;
        maze[MazeHeight - 1, MazeWidth - 2] = 0;

        GenerateMazeRecursively(1, 1);
        // Create MazeMap Plane
        for (int row = 0; row < 100; row++)
        {
            for (int col = 0; col < 100; col++)
            {
              
                Vector3 position = new Vector3(-50.0f+col *5.0f, 0, -50.0f+row * 5.0f);
                GameObject plane = Instantiate(PlanPrefab,transform);
                plane.transform.position = position;
                plane.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // ������ Plane ũ�� ����
            }
        }
        // Create walls 
        for (int row = 0; row < MazeHeight; row++)
        {
            for (int col = 0; col < MazeWidth; col++)
            {
                // Create Tree
                GameObject ProbObject = Instantiate(TreePrefab, new Vector3((col/2), 0, (row/2)), Quaternion.identity);
                ProbObject.transform.position = new Vector3(col * WallSize+ Random.Range(-20, 20),2, row * WallSize+ Random.Range(-20, 20));
                ProbObject.transform.Rotate(0.0f,0.0f, 0.0f);
                ProbObject.transform.localScale = new Vector3(2.0f, 2.0f,2.0f);
              
                if (maze[row, col] == 1)
                {
                    // Create wall's Objects
                    GameObject wall = Instantiate(wallPrefab, new Vector3(col * WallSize, WallSize/2, row * WallSize), Quaternion.identity);
                    wall.transform.localScale = new Vector3(WallSize, WallSize, WallSize);
                    wall.transform.Rotate(0.0f, Random.Range(0, 4) * 90.0f, 0.0f);
                    wall.transform.parent = transform;

                    // Create CheckBox - Collider
                    GameObject CheckBox = new GameObject();
                    CheckBox.AddComponent<BoxCollider>();
                    CheckBox.GetComponent<BoxCollider>().size = new Vector3(WallSize, WallSize, WallSize);
                    CheckBox.transform.position = new Vector3(col * WallSize, WallSize / 2, row * WallSize);
                }
            }
        }
    }
    // Array Shuffle
    private void ArrayShuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int rd = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[rd];
            array[rd] = temp;
        }
    }

    // �̷θ� ����� ���� ��� �˰���
    private void GenerateMazeRecursively(int row, int col)
    {
        // ���� �迭 ->      Up    Down    Left    Right
        int[] directions = { 0,     1,      2,      3 };
        // ���� Shuffle
        ArrayShuffle(directions);

        foreach (int dir in directions)
        {
            int newRow = row;
            int newCol = col;

            // ���� �̵�
            switch (dir)
            {
                case 0: 
                    newRow-=2;  // Up
                    break;
                case 1: 
                    newRow+=2;  // Down
                    break;
                case 2: 
                    newCol-=2;  // Left
                    break;
                case 3: 
                    newCol+=2;  // Right
                    break;
            }

            // ���ο� ���� �̷� �����ȿ� �ִ��� Ȯ��
            if (newRow < 0 || newRow >= MazeHeight || newCol < 0 || newCol >= MazeWidth)
            {
                continue;
            }

            // ���ο� ���� �̹� �湮�� ������ Ȯ��.
            if (maze[newRow, newCol] == 0)
            {
                continue;
            }

            // ���� ���� ���ο� �� ������ ������ �����.
            if (dir == 0)
            { 
                maze[row - 1, col] = 0;// Up
            }
            else if (dir == 1)
            { 
                maze[row + 1, col] = 0;// Down
            }
            else if (dir == 2)
            { 
                maze[row, col - 1] = 0;// Left
            }
            else if (dir == 3)
            { 
                maze[row, col + 1] = 0;// Right
            }
       
            maze[newRow, newCol] = 0;

            // ���ο� ���� ������ ���� ��ġ�Ͽ� ���ȣ��.
            GenerateMazeRecursively(newRow, newCol);
        }
    }
}