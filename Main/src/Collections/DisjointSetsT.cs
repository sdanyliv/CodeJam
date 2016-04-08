using System.Collections.Generic;
using System.Linq;

namespace CodeJam.Collections
{
	/// <summary>
	/// Реализация классической структуры Disjoint sets
	/// Generic - версия, позволяющая хранить данные внутри
	/// </summary>
	///  <remarks>
	/// http://en.wikipedia.org/wiki/Disjoint-set_data_structure
	/// </remarks>
	public sealed class DisjointSets<T> : DisjointSetsBase<DisjointSets<T>.Node>
	{
		/// <summary>Создает пустой DS</summary>
		public DisjointSets() { }

		/// <summary>Создает DS со значениями из перечисления</summary>
		/// <param name="values">Перечисление значений для добавления</param>
		public DisjointSets(IEnumerable<T> values)
		{
			Add(values);
		}

		/// <summary>Получение значения элемента по индексу</summary>
		/// <param name="index">Индекс элемента</param>
		public T this[int index] => Nodes_[index].Value;

		/// <summary>Добавление перечисления элементов</summary>
		/// <param name="values">Элементы</param>
		public void Add(IEnumerable<T> values)
		{
			var initialNodesCount = Nodes_.Count;
			Nodes_.AddRange(values.Select(_ => new Node { Value = _, ParentIndex = -1, Rank = 0 }));
			SetsCount += Nodes_.Count - initialNodesCount;
		}

		/// <summary>Добавление одного элемента</summary>
		/// <param name="value">Элемент</param>
		public void Add(T value)
		{
			Nodes_.Add(new Node { Value = value, ParentIndex = -1, Rank = 0 });
			++SetsCount;
		}

		/// <summary>Узел дерева</summary>
		public class Node : BasicNode
		{
			/// <summary>Собственно полезные данные</summary>
			public T Value;
		}
	}
}
