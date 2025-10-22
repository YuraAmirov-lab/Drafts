using System;
using System.Collections.Generic;

public class Heap<T> where T : IComparable<T>
{
    private List<T> elements;
    private bool isMinHeap;

    // Конструктор, создающий пустую кучу
    public Heap(bool isMinHeap = false)
    {
        this.elements = new List<T>();
        this.isMinHeap = isMinHeap;
    }

    // Конструктор, принимающий массив элементов и создающий из них кучу
    public Heap(T[] array, bool isMinHeap = false)
    {
        this.elements = new List<T>(array);
        this.isMinHeap = isMinHeap;
        BuildHeap();
    }

    // Нахождение максимума/минимума без удаления
    public T Peek()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("Куча пуста");

        return elements[0];
    }

    // Удаление и возврат максимума/минимума
    public T Pop()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("Куча пуста");

        T result = elements[0];
        elements[0] = elements[elements.Count - 1];
        elements.RemoveAt(elements.Count - 1);
        HeapifyDown(0);

        return result;
    }

    // Изменение ключа элемента по индексу
    public void ChangeKey(int index, T newValue)
    {
        if (index < 0 || index >= elements.Count)
            throw new ArgumentOutOfRangeException("Неверный индекс");

        T oldValue = elements[index];
        elements[index] = newValue;

        if (Compare(newValue, oldValue) < 0)
            HeapifyUp(index);
        else
            HeapifyDown(index);
    }

    // Добавление нового элемента
    public void Push(T item)
    {
        elements.Add(item);
        HeapifyUp(elements.Count - 1);
    }

    // Слияние с другой кучей
    public Heap<T> Merge(Heap<T> otherHeap)
    {
        var mergedArray = new T[elements.Count + otherHeap.elements.Count];
        elements.CopyTo(mergedArray, 0);
        otherHeap.elements.CopyTo(mergedArray, elements.Count);

        return new Heap<T>(mergedArray, isMinHeap);
    }

    public int Count => elements.Count;
    public bool IsEmpty => elements.Count == 0;

    // Вспомогательные методы
    private void BuildHeap()
    {
        for (int i = elements.Count / 2 - 1; i >= 0; i--)
        {
            HeapifyDown(i);
        }
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (Compare(elements[index], elements[parentIndex]) >= 0)
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallestOrLargest = index;

            if (leftChild < elements.Count &&
                Compare(elements[leftChild], elements[smallestOrLargest]) < 0)
            {
                smallestOrLargest = leftChild;
            }

            if (rightChild < elements.Count &&
                Compare(elements[rightChild], elements[smallestOrLargest]) < 0)
            {
                smallestOrLargest = rightChild;
            }

            if (smallestOrLargest == index)
                break;

            Swap(index, smallestOrLargest);
            index = smallestOrLargest;
        }
    }

    private int Compare(T a, T b)
    {
        int result = a.CompareTo(b);
        return isMinHeap ? result : -result;
    }

    private void Swap(int i, int j)
    {
        T temp = elements[i];
        elements[i] = elements[j];
        elements[j] = temp;
    }

    // Для отладки - вывод кучи в виде строки
    public override string ToString()
    {
        return string.Join(", ", elements);
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        // Тестирование min-кучи
        Console.WriteLine("Min-Heap:");
        var minHeap = new Heap<int>(new int[] { 5, 3, 8, 1, 9 }, true);

        Console.WriteLine($"Куча: {minHeap}");
        Console.WriteLine($"Минимум: {minHeap.Peek()}");

        minHeap.Push(0);
        Console.WriteLine($"После добавления 0: {minHeap}");

        Console.WriteLine($"Извлеченный минимум: {minHeap.Pop()}");
        Console.WriteLine($"После извлечения: {minHeap}");

        minHeap.ChangeKey(2, 2);
        Console.WriteLine($"После изменения ключа: {minHeap}");

        // Тестирование max-кучи
        Console.WriteLine("\nMax-Heap:");
        var maxHeap = new Heap<int>(new int[] { 1, 5, 3, 7, 2 });

        Console.WriteLine($"Куча: {maxHeap}");
        Console.WriteLine($"Максимум: {maxHeap.Peek()}");

        maxHeap.Push(10);
        Console.WriteLine($"После добавления 10: {maxHeap}");

        // Слияние куч
        var heap1 = new Heap<int>(new int[] { 1, 3, 5 }, true);
        var heap2 = new Heap<int>(new int[] { 2, 4, 6 }, true);
        var merged = heap1.Merge(heap2);

        Console.WriteLine($"\nСлияние: {heap1} + {heap2} = {merged}");
    }
}