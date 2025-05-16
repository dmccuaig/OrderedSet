using System.Collections;

namespace OrderedSet;

public class OrderedSet<T> : ICollection<T> where T : class
{
	private readonly Dictionary<T, LinkedListNode<T>> _dictionary;
	private readonly LinkedList<T> _linkedList;

	public OrderedSet()
		: this(EqualityComparer<T>.Default)
	{
	}

	public OrderedSet(IEqualityComparer<T> comparer)
	{
		_dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
		_linkedList = [];
	}

	public OrderedSet(IEnumerable<T> collection)
		: this(EqualityComparer<T>.Default)
	{
		ArgumentNullException.ThrowIfNull(collection);

		foreach (T item in collection)
		{
			AddLast(item);
		}
	}

	public int Count => _dictionary.Count;
	public bool IsReadOnly => false;

	public void Clear()
	{
		_linkedList.Clear();
		_dictionary.Clear();
	}

	public IEnumerator<T> GetEnumerator() => _linkedList.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public bool Contains(T item) => _dictionary.ContainsKey(item);

	public void CopyTo(T[] array, int arrayIndex)
	{
		_linkedList.CopyTo(array, arrayIndex);
	}

	public T[] ToArray() => _linkedList.ToArray();

	public T? First => _linkedList.First?.Value;
	public T? Last => _linkedList.Last?.Value;

	void ICollection<T>.Add(T value)
	{
		AddLast(value);
	}

	private LinkedListNode<T>? CreateNewNode(T value)
	{
		if (_dictionary.ContainsKey(value))
		{
			return null;
		}

		var newNode = new LinkedListNode<T>(value);
		_dictionary.Add(value, newNode);

		return newNode;
	}

	public bool AddFirst(T value)
	{
		ArgumentNullException.ThrowIfNull(value);
		var newNode = CreateNewNode(value);
		if (newNode is null)
		{
			return false;
		}

		_linkedList.AddFirst(newNode);
		return true;
	}

	public bool AddLast(T value)
	{
		ArgumentNullException.ThrowIfNull(value);
		var newNode = CreateNewNode(value);
		if (newNode is null)
		{
			return false;
		}

		_linkedList.AddLast(newNode);
		return true;
	}

	private bool RemoveNode(LinkedListNode<T>? node, out T? value)
	{
		if (node is null)
		{
			value = null;
			return false;
		}

		value = node.Value;
		return _dictionary.Remove(value);
	}

	public bool Remove(T item)
	{
		ArgumentNullException.ThrowIfNull(item);
		if (_dictionary.Remove(item, out var node))
		{
			_linkedList.Remove(node);

			return true;
		}

		return false;
	}


	public T? RemoveFirst()
	{
		if (RemoveNode(_linkedList.First, out var value))
		{
			_linkedList.RemoveFirst();
			return value;
		}

		return null;
	}

	public T? RemoveLast()
	{
		if (RemoveNode(_linkedList.Last, out var value))
		{
			_linkedList.RemoveLast();
			return value;
		}

		return null;
	}

}