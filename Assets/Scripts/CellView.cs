using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellView : MonoBehaviour
{
    [SerializeField] private Text cellText;
    // Если у вас нет ссылки на Image, попробуем получить его автоматически.
    // Если объект не имеет компонента Image, можно добавить его вручную на префаб.
    private Image cellImage;

    private Cell cell;
    private GameField gameField;

    public void Init(Cell cell, GameField gameField)
    {
        this.cell = cell;
        this.gameField = gameField;

        // Пробуем получить компонент Image с этого объекта
        cellImage = GetComponent<Image>();
        if(cellImage == null)
        {
            Debug.LogWarning("Компонент Image не найден на " + gameObject.name);
        }

        cell.OnValueChanged += UpdateValue;
        cell.OnPositionChanged += UpdatePosition;

        UpdateValue(cell.Value);
        UpdatePosition(cell.Position);
    }

    private void OnDestroy()
    {
        if (cell != null)
        {
            cell.OnValueChanged -= UpdateValue;
            cell.OnPositionChanged -= UpdatePosition;
        }
    }

    /// <summary>
    /// Устанавливает цвет фона клетки.
    /// </summary>
    public void SetBackgroundColor(Color color)
    {
        if(cellImage == null)
        {
            // Пытаемся получить компонент ещё раз, если вдруг не был получен ранее
            cellImage = GetComponent<Image>();
        }
        if (cellImage != null)
        {
            cellImage.color = color;
        }
        else
        {
            Debug.LogWarning("Невозможно изменить цвет, компонент Image отсутствует на " + gameObject.name);
        }
    }

    private void UpdateValue(int newValue)
    {
        double displayedNumber = System.Math.Pow(newValue, 2);
        cellText.text = displayedNumber.ToString();
    }

    private void UpdatePosition(Vector2Int newPosition)
    {
        Transform placeholder = gameField.GetPlaceholderAt(newPosition);
        if (placeholder == null)
        {
            Debug.LogWarning("Placeholder is null for position: " + newPosition);
            return;
        }
        transform.SetParent(placeholder, false);

        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localPosition = Vector3.zero;
        }
    }
}
