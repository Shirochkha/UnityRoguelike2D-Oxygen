using UnityEngine;
using System;
//Для Списков(листов).
using System.Collections.Generic;
//Сообщает Random, что нужно использовать генератор случайных чисел Unity Engine.
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    // Использование Serializable позволяет встроить в инспектор класс с подсвойствами.
    [Serializable]
    public class Count
    {
        public int minimum;             //Минимальное значение для класса Count.
        public int maximum;             //Максимальное значение для класса Count.


        //Конструктор.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    [HideInInspector]public int columns = 8;             //Количество столбцов на игровой доске.
    [HideInInspector] public int rows = 8;                //Количество строк на игровой доске.
                                        
    public Count wallCount = new(5, 14);  //Количество стен на каждом уровне.
    public Count resourcesCount = new(1, 7);  //Количество еды на каждом уровне.
    public GameObject exit;             //Выход. Т.к. только один на уровень, то задается отдельно.
    public GameObject[] floorTiles;     //Массив floor prefabs.
    public GameObject[] wallTiles;      //Массив wall prefabs.
    public GameObject[] resourcesTiles; //Массив food prefabs.
    public GameObject[] enemyTiles;     //Массивy enemy prefabs.
    public GameObject[] outerWallTiles; //Массив outer tile prefabs.

    //Переменная для хранения ссылки на преобразование Board.
    private Transform boardHolder;
    //Список возможных мест для размещения плитки.
    private readonly List<Vector3> gridPositions = new();

    public AudioClip backSound1; //Фоновая музыка 1.
    public AudioClip backSound2; //Фоновая музыка 2.
    public AudioClip backSound3; //Фоновая музыка 3.
    public AudioClip backSound4; //Фоновая музыка 4.
    public AudioClip backSound5; //Фоновая музыка 5.

    //Очищает список gridPositions и подготавливает его к генерации новой board.
    void InitialiseList()
    {
        //Очищение gridPositions.
        gridPositions.Clear();

        //Задание координат клеткам.
        // Цикл по оси x (столбцы).
        for (int x = 1; x < columns - 1; x++)
        {
            //Внутри каждого столбца цикл по оси Y (строкам).
            for (int y = 1; y < rows - 1; y++)
            {
                // По каждому индексу добавляем новый Vector3 в наш список с координатами x и y этой позиции.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Установка внешней стены и пола (фона) игрового поля.
    void BoardSetup()
    {
        //Создание экземпляра Board и установка в boardHolder его transform.
        boardHolder = new GameObject("Board").transform;

        // Цикл вдоль оси x, начиная с -1 (для заполнения угла) плитками пола или внешней стены.
        for (int x = -1; x < columns + 1; x++)
        {
            // Цикл вдоль оси Y, начиная с -1, чтобы разместить плитки пола или внешней стены.
            for (int y = -1; y < rows + 1; y++)
            {
                //Выбор случайной плитки из массива префабов напольной плитки и создание ее экземпляр.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //Проверка, находится ли текущая позиция на краю доски, если да, то выбирается случайный префаб внешней стены из массива тайлов внешней стены.
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                //Создается экземпляр GameObject, используя префаб, выбранный для toInstantiate в Vector3,
                //соответствующем текущей позиции сетки в цикле, и приводится его к GameObject.
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Установка родителем созданный экземпляр объекта со значением boardHolder, чтобы не загромождать иерархию.
                instance.transform.SetParent(boardHolder);
            }
        }
    }


    //RandomPosition возвращает случайную позицию из списка gridPositions.
    Vector3 RandomPosition()
    {
        // Объявление целого числа randomIndex, установка для него случайного числа от 0 до количества элементов в списке gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        // Объявление переменной типа Vector3 с именем randomPosition, установка ее значения на запись в randomIndex из списка gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Удаление записи в randomIndex из списка, чтобы ее нельзя было использовать повторно.
        gridPositions.RemoveAt(randomIndex);

        // Возвращение случайно выбранной позиции Vector3.
        return randomPosition;
    }


    //LayoutObjectAtRandom принимает массив игровых объектов на выбор, вместе с минимальным и максимальным диапазоном количества создаваемых объектов.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Выбор случайного количества объектов для их создания в пределах минимального и максимального пределов.
        int objectCount = Random.Range(minimum, maximum + 1);

        //Создание объектов до тех пор, пока не будет достигнут случайно выбранный предел objectCount.
        for (int i = 0; i < objectCount; i++)
        {
            //Выбор позиции для randomPosition, получение случайной позиции из списка доступных Vector3, хранящихся в gridPosition.
            Vector3 randomPosition = RandomPosition();

            //Выбор случайной плитки из массива плиток.
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Создание экземпляра tileChoice в позиции, возвращаемой RandomPosition, без изменения rotation.
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    //SetupScene инициализирует уровень и вызывает предыдущие функции для разметки игрового поля.
    public void SetupScene(int level)
    {
        //Установка случайной фоновой музыки.
        SoundManager.instance.RandomizeBackgroundMusic(backSound1, backSound2, backSound3, backSound4, backSound5);

        //columns = Random.Range(6, 9);
        //rows = columns;

        //Создание внешней стены и пола.
        BoardSetup();

        //Сброс списка позиций сетки.
        InitialiseList();

        //Создание случайного количества тайлов стены на основе минимума и максимума в случайных позициях.
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //Создание случайного количества плиток еды на основе минимума и максимума в случайных позициях.
        LayoutObjectAtRandom(resourcesTiles, resourcesCount.minimum, resourcesCount.maximum);

        //Определение количества врагов на основе номера текущего уровня, на основе логарифмической прогрессии.
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Создание случайного числа врагов на основе минимума и максимума в случайных позициях.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Создание тайла выхода в правом верхнем углу игрового поля.
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
