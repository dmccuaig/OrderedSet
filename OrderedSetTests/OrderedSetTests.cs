using OrderedSet;

namespace OrderedSetTests;

[TestFixture]
public class OrderedSetTests
{
	[Test]
	public void Constructor_Default_CreatesEmptySet()
	{
		var set = new OrderedSet<string>();
		Assert.That(set, Is.Empty);
		Assert.That(set.Count, Is.EqualTo(0));
		Assert.That(set.IsReadOnly, Is.False);
	}

	[Test]
	public void Constructor_WithCollection_AddsAllUniqueItems()
	{
		var set = new OrderedSet<string>(new[] { "A", "B", "B", "C" });
		Assert.That(set.Count, Is.EqualTo(3));
		Assert.That(set.Contains("A"), Is.True);
		Assert.That(set.Contains("B"), Is.True);
		Assert.That(set.Contains("C"), Is.True);
	}

	[Test]
	public void AddFirst_AddsToFront_AndPreventsDuplicates()
	{
		var set = new OrderedSet<string>();
		Assert.That(set.AddFirst("A"), Is.True);
		Assert.That(set.AddFirst("B"), Is.True);
		Assert.That(set.AddFirst("A"), Is.False); // Duplicate
		Assert.That(set.ToArray(), Is.EqualTo(new[] { "B", "A" }));
	}

	[Test]
	public void AddLast_AddsToEnd_AndPreventsDuplicates()
	{
		var set = new OrderedSet<string>();
		Assert.That(set.AddLast("A"), Is.True);
		Assert.That(set.AddLast("B"), Is.True);
		Assert.That(set.AddLast("A"), Is.False); // Duplicate
		Assert.That(set.ToArray(), Is.EqualTo(new[] { "A", "B" }));
	}

	[Test]
	public void ICollectionAdd_AddsToEnd()
	{
		ICollection<string> set = new OrderedSet<string>();
		set.Add("A");
		set.Add("B");
		Assert.That(set.ToArray(), Is.EqualTo(new[] { "A", "B" }));
	}

	[Test]
	public void Remove_RemovesItem_AndReturnsTrue()
	{
		var set = new OrderedSet<string>(["A", "B", "C"]);
		Assert.That(set.Remove("B"), Is.True);
		Assert.That(set.Contains("B"), Is.False);
		Assert.That(set.Remove("D"), Is.False); // Not present
	}

	[Test]
	public void RemoveFirst_RemovesAndReturnsFirst()
	{
		var set = new OrderedSet<string>(["A", "B", "C"]);
		var removed = set.RemoveFirst();
		Assert.That(removed, Is.EqualTo("A"));
		Assert.That(set.ToArray(), Is.EqualTo(new[] { "B", "C" }));
		set.Clear();
		Assert.That(set.RemoveFirst(), Is.EqualTo(null));
	}

	[Test]
	public void RemoveLast_RemovesAndReturnsLast()
	{
		var set = new OrderedSet<string>(["A", "B", "C"]);
		var removed = set.RemoveLast();
		Assert.That(removed, Is.EqualTo("C"));
		Assert.That(set.ToArray(), Is.EqualTo(new[] { "A", "B" }));
		set.Clear();
		Assert.That(set.RemoveLast(), Is.EqualTo(null));
	}

	[Test]
	public void Clear_RemovesAllItems()
	{
		var set = new OrderedSet<string>(["A", "B", "C"]);
		set.Clear();
		Assert.That(set, Is.Empty);
		Assert.That(set.Count, Is.EqualTo(0));
	}

	[Test]
	public void Contains_ReturnsTrueIfPresent()
	{
		var set = new OrderedSet<string>(["A", "B"]);
		Assert.That(set.Contains("A"), Is.True);
		Assert.That(set.Contains("C"), Is.False);
	}

	[Test]
	public void CopyTo_CopiesElementsInOrder()
	{
		var set = new OrderedSet<string>(["A", "B", "C"]);
		var arr = new string[5];
		set.CopyTo(arr, 1);
		Assert.That(arr, Is.EqualTo(new[] { null, "A", "B", "C", null }));
	}

	[Test]
	public void Enumerator_EnumeratesInOrder()
	{
		var set = new OrderedSet<string>(["A", "B", "C"]);
		var list = new List<string>();
		foreach (var item in set)
		{
			list.Add(item);
		}

		Assert.That(list, Is.EqualTo(new[] { "A", "B", "C" }));
	}

	[Test]
	public void First_And_Last_Properties()
	{
		var set = new OrderedSet<string>();
		Assert.That(set.First, Is.EqualTo(null));
		Assert.That(set.Last, Is.EqualTo(null));

		set.AddLast("A");
		set.AddLast("B");
		Assert.That(set.First, Is.EqualTo("A"));
		Assert.That(set.Last, Is.EqualTo("B"));

		set.RemoveFirst();
		Assert.That(set.First, Is.EqualTo("B"));
		Assert.That(set.Last, Is.EqualTo("B"));

		set.RemoveLast();
		Assert.That(set.First, Is.EqualTo(null));
		Assert.That(set.Last, Is.EqualTo(null));
	}

	[Test]
	public void IsReadOnly_IsFalse()
	{
		var set = new OrderedSet<string>();
		Assert.That(set.IsReadOnly, Is.False);
		Assert.That(((ICollection<string>)set).IsReadOnly, Is.False);
	}
}