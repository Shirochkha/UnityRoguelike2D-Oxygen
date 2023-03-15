using UnityEngine;
using System;
//��� �������(������).
using System.Collections.Generic;
//�������� Random, ��� ����� ������������ ��������� ��������� ����� Unity Engine.
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    // ������������� Serializable ��������� �������� � ��������� ����� � �������������.
    [Serializable]
    public class Count
    {
        public int minimum;             //����������� �������� ��� ������ Count.
        public int maximum;             //������������ �������� ��� ������ Count.


        //�����������.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    [HideInInspector]public int columns = 8;             //���������� �������� �� ������� �����.
    [HideInInspector] public int rows = 8;                //���������� ����� �� ������� �����.
                                        
    public Count wallCount = new(5, 14);  //���������� ���� �� ������ ������.
    public Count resourcesCount = new(1, 7);  //���������� ��� �� ������ ������.
    public GameObject exit;             //�����. �.�. ������ ���� �� �������, �� �������� ��������.
    public GameObject[] floorTiles;     //������ floor prefabs.
    public GameObject[] wallTiles;      //������ wall prefabs.
    public GameObject[] resourcesTiles; //������ food prefabs.
    public GameObject[] enemyTiles;     //������y enemy prefabs.
    public GameObject[] outerWallTiles; //������ outer tile prefabs.

    //���������� ��� �������� ������ �� �������������� Board.
    private Transform boardHolder;
    //������ ��������� ���� ��� ���������� ������.
    private readonly List<Vector3> gridPositions = new();

    public AudioClip backSound1; //������� ������ 1.
    public AudioClip backSound2; //������� ������ 2.
    public AudioClip backSound3; //������� ������ 3.
    public AudioClip backSound4; //������� ������ 4.
    public AudioClip backSound5; //������� ������ 5.

    //������� ������ gridPositions � �������������� ��� � ��������� ����� board.
    void InitialiseList()
    {
        //�������� gridPositions.
        gridPositions.Clear();

        //������� ��������� �������.
        // ���� �� ��� x (�������).
        for (int x = 1; x < columns - 1; x++)
        {
            //������ ������� ������� ���� �� ��� Y (�������).
            for (int y = 1; y < rows - 1; y++)
            {
                // �� ������� ������� ��������� ����� Vector3 � ��� ������ � ������������ x � y ���� �������.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //��������� ������� ����� � ���� (����) �������� ����.
    void BoardSetup()
    {
        //�������� ���������� Board � ��������� � boardHolder ��� transform.
        boardHolder = new GameObject("Board").transform;

        // ���� ����� ��� x, ������� � -1 (��� ���������� ����) �������� ���� ��� ������� �����.
        for (int x = -1; x < columns + 1; x++)
        {
            // ���� ����� ��� Y, ������� � -1, ����� ���������� ������ ���� ��� ������� �����.
            for (int y = -1; y < rows + 1; y++)
            {
                //����� ��������� ������ �� ������� �������� ��������� ������ � �������� �� ���������.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //��������, ��������� �� ������� ������� �� ���� �����, ���� ��, �� ���������� ��������� ������ ������� ����� �� ������� ������ ������� �����.
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                //��������� ��������� GameObject, ��������� ������, ��������� ��� toInstantiate � Vector3,
                //��������������� ������� ������� ����� � �����, � ���������� ��� � GameObject.
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //��������� ��������� ��������� ��������� ������� �� ��������� boardHolder, ����� �� ������������ ��������.
                instance.transform.SetParent(boardHolder);
            }
        }
    }


    //RandomPosition ���������� ��������� ������� �� ������ gridPositions.
    Vector3 RandomPosition()
    {
        // ���������� ������ ����� randomIndex, ��������� ��� ���� ���������� ����� �� 0 �� ���������� ��������� � ������ gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        // ���������� ���������� ���� Vector3 � ������ randomPosition, ��������� �� �������� �� ������ � randomIndex �� ������ gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //�������� ������ � randomIndex �� ������, ����� �� ������ ���� ������������ ��������.
        gridPositions.RemoveAt(randomIndex);

        // ����������� �������� ��������� ������� Vector3.
        return randomPosition;
    }


    //LayoutObjectAtRandom ��������� ������ ������� �������� �� �����, ������ � ����������� � ������������ ���������� ���������� ����������� ��������.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //����� ���������� ���������� �������� ��� �� �������� � �������� ������������ � ������������� ��������.
        int objectCount = Random.Range(minimum, maximum + 1);

        //�������� �������� �� ��� ���, ���� �� ����� ��������� �������� ��������� ������ objectCount.
        for (int i = 0; i < objectCount; i++)
        {
            //����� ������� ��� randomPosition, ��������� ��������� ������� �� ������ ��������� Vector3, ���������� � gridPosition.
            Vector3 randomPosition = RandomPosition();

            //����� ��������� ������ �� ������� ������.
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //�������� ���������� tileChoice � �������, ������������ RandomPosition, ��� ��������� rotation.
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    //SetupScene �������������� ������� � �������� ���������� ������� ��� �������� �������� ����.
    public void SetupScene(int level)
    {
        //��������� ��������� ������� ������.
        SoundManager.instance.RandomizeBackgroundMusic(backSound1, backSound2, backSound3, backSound4, backSound5);

        //columns = Random.Range(6, 9);
        //rows = columns;

        //�������� ������� ����� � ����.
        BoardSetup();

        //����� ������ ������� �����.
        InitialiseList();

        //�������� ���������� ���������� ������ ����� �� ������ �������� � ��������� � ��������� ��������.
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //�������� ���������� ���������� ������ ��� �� ������ �������� � ��������� � ��������� ��������.
        LayoutObjectAtRandom(resourcesTiles, resourcesCount.minimum, resourcesCount.maximum);

        //����������� ���������� ������ �� ������ ������ �������� ������, �� ������ ��������������� ����������.
        int enemyCount = (int)Mathf.Log(level, 2f);

        //�������� ���������� ����� ������ �� ������ �������� � ��������� � ��������� ��������.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //�������� ����� ������ � ������ ������� ���� �������� ����.
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
