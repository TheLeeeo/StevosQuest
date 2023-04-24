public class Stack<T>
{
    private T[] _stack;

    public int Length { get; private set; }

    public Stack(int size)
    {
        _stack = new T[size];
    }

    public T this[int index]
    {
        get => _stack[index];
    }

    public void Push(T Value)
    {
        _stack[Length] = Value;
        Length++;
    }

    public void Pop(int NumberOfValues = 1)
    {
        if(NumberOfValues > Length)
        {
            NumberOfValues = Length;
        }

        Length -= NumberOfValues;
    }
}
