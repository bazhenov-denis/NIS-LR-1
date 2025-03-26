using System;
using UnityEngine;

public class Cell
{
    // События, которые будут вызываться при изменении значения и позиции
    public event Action<int> OnValueChanged;
    public event Action<Vector2Int> OnPositionChanged;

    private int _value;
    private Vector2Int _position;

    // Свойство для значения
    public int Value
    {
        get => _value;
        set
        {
            // Если значение реально изменилось – вызываем событие
            if (_value != value)
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    // Свойство для позиции
    public Vector2Int Position
    {
        get => _position;
        set
        {
            // Аналогично, если позиция меняется – вызываем событие
            if (_position != value)
            {
                _position = value;
                OnPositionChanged?.Invoke(_position);
            }
        }
    }

    // Конструктор для инициализации клетки
    public Cell(Vector2Int startPosition, int startValue)
    {
        _position = startPosition;
        _value = startValue;
    }
}