using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [Header("Настройки поля")]
    [SerializeField] private int fieldSize = 4;           // Размер поля (4×4)
    [SerializeField] private CellView cellPrefab;         // Ссылка на префаб клетки (CellView)

    [Header("Ссылки на UI")]
    [SerializeField] private Transform gridContainer;     // Родитель, в котором лежат плейсхолдеры

    // Массив плейсхолдеров
    private Transform[,] placeholders;

    // Сетка (модель) для быстрого поиска пустых ячеек
    private Cell[,] cellGrid;

    // Список всех клеток (если нужно)
    private List<Cell> cells = new List<Cell>();

    private void Awake()
    {
        // 1) Инициализируем массивы
        placeholders = new Transform[fieldSize, fieldSize];
        cellGrid = new Cell[fieldSize, fieldSize];

        // 2) Заполняем массив placeholders, берём детей из gridContainer
        int childIndex = 0;
        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                placeholders[x, y] = gridContainer.GetChild(childIndex);
                childIndex++;
            }
        }
    }

    private void Start()
    {
        // Пример: сразу создаём 2 клетки в начале
        CreateCell();
        CreateCell();
    }

    /// <summary>
    /// Возвращает координаты случайной пустой клетки на поле.
    /// Если пустых клеток нет, возвращаем (-1, -1).
    /// </summary>
    public Vector2Int GetEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                if (cellGrid[x, y] == null)
                {
                    emptyPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        if (emptyPositions.Count == 0)
        {
            return new Vector2Int(-1, -1);
        }

        int randomIndex = Random.Range(0, emptyPositions.Count);
        return emptyPositions[randomIndex];
    }

    /// <summary>
    /// Создаёт новую клетку (Cell и CellView) в случайной пустой позиции.
    /// Значение: 1 (90%) или 2 (10%).
    /// </summary>
    public void CreateCell()
    {
        Vector2Int emptyPos = GetEmptyPosition();
        if (emptyPos.x < 0)
        {
            return;
        }

        int newValue = (Random.value < 0.9f) ? 2 : 4;
        Cell newCell = new Cell(emptyPos, newValue);

        cellGrid[emptyPos.x, emptyPos.y] = newCell;
        cells.Add(newCell);

        Transform placeholder = placeholders[emptyPos.x, emptyPos.y];
        CellView newCellView = Instantiate(cellPrefab, placeholder);
        newCellView.Init(newCell, this);

        // Выбираем цвет 50/50: жёлтый или бежевый
        Color cellColor = (Random.value < 0.5f) 
            ? new Color(0.95f, 0.91f, 0.74f)
            : new Color(0.96f, 0.96f, 0.86f); // бежевый оттенок

        newCellView.SetBackgroundColor(cellColor);

        RectTransform childRect = newCellView.GetComponent<RectTransform>();
        if (childRect != null)
        {
            childRect.anchorMin = Vector2.zero;
            childRect.anchorMax = Vector2.one;
            childRect.offsetMin = Vector2.zero;
            childRect.offsetMax = Vector2.zero;
            childRect.localPosition = Vector3.zero;
        }
    }


    /// <summary>
    /// Возвращает плейсхолдер (Transform), соответствующий позиции на поле (x, y).
    /// Используется внутри CellView при перемещении клетки.
    /// </summary>
    public Transform GetPlaceholderAt(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= fieldSize || pos.y < 0 || pos.y >= fieldSize)
        {
            return null;
        }
        return placeholders[pos.x, pos.y];
    }

    /// <summary>
    /// Метод, растягивающий RectTransform дочернего объекта на весь размер родителя.
    /// </summary>
    private void StretchToParent(RectTransform rect)
    {
        // Важно: эти настройки заставят ребёнка занять всю площадь родителя
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localPosition = Vector3.zero;  // На всякий случай
    }
}
