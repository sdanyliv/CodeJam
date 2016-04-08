using System.Collections.Generic;

namespace CodeJam.Collections
{
	/// <summary>Базовый класс для реализации Disjoint sets</summary>
	///  <remarks>
	/// http://en.wikipedia.org/wiki/Disjoint-set_data_structure
	/// </remarks>
	public class DisjointSetsBase<T> where T : BasicNode
	{
		/// <summary>Список всех узлов</summary>
		protected readonly List<T> Nodes_ = new List<T>();

		/// <summary>Создает пустой DS</summary>
		protected DisjointSetsBase() { }

		/// <summary>Число элементов</summary>
		public int Count => Nodes_.Count;

		/// <summary>Число несвязанных множеств</summary>
		public int SetsCount { get; protected set; }

		/// <summary>Поиск идентификатора множества, которому принадлежит элемент по заданному индексу</summary>
		/// <param name="index">Индекс элемента</param>
		/// <returns>Идентификатор множества, которому принадлежит элемент</returns>
		/// <remarks>Идентификатор множества - это индекс элемента, представляющего множество</remarks>
		public int IndexToSetId(int index)
		{
			// сначала ищем индекс корневого элемента дерева, к которому принадлежит наш узел
			var rootIndex = index;
			for (;;)
			{
				var parentIndex = Nodes_[rootIndex].ParentIndex;
				if (parentIndex == -1)
				{
					break;
				}
				rootIndex = parentIndex;
			}

			// компрессия пути - идем от нашего элемента вверх к корню, обновляя ParentIndex на rootIndex
			while (index != rootIndex)
			{
				var node = Nodes_[index];
				index = node.ParentIndex;
				node.ParentIndex = rootIndex;
			}
			return rootIndex;
		}

		/// <summary>Объединение двух множеств в одно</summary>
		/// <param name="elementOfSet1Index">Индекс какого-то элемента первого множества</param>
		/// <param name="elementOfSet2Index">Индекс какого-то элемента второго можества</param>
		public void Union(int elementOfSet1Index, int elementOfSet2Index)
		{
			elementOfSet1Index = IndexToSetId(elementOfSet1Index);
			elementOfSet2Index = IndexToSetId(elementOfSet2Index);
			if (elementOfSet1Index == elementOfSet2Index)
			{
				return; // уже одно множество
			}

			var set1Root = Nodes_[elementOfSet1Index];
			var set2Root = Nodes_[elementOfSet2Index];
			var rankDifference = set1Root.Rank - set2Root.Rank;
			// Цепляем дерево с меньшим рангом к корню дерева с большим. В этом случае ранг получившегося дерева равен большему, кроме случая, когда ранги равны (тогда будет +1)
			if (rankDifference > 0) // у 1-го ранг больше
			{
				set2Root.ParentIndex = elementOfSet1Index;
			}
			else if (rankDifference < 0) // у 2-го больше
			{
				set1Root.ParentIndex = elementOfSet2Index;
			}
			else // ранги равны. пофигу что к чему цеплять, но нужно увеличить ранг того, к чему прицепили
			{
				set2Root.ParentIndex = elementOfSet1Index;
				++set1Root.Rank;
			}

			// поскольку слили 2 в одно, уменьшаем число сетов
			--SetsCount;
		}
	}

	/// <summary>Базовый класс узла дерева</summary>
	public class BasicNode
	{
		/// <summary>Индекс родителя (после компрессии пути указывает на корень)</summary>
		public int ParentIndex;

		/// <summary>Примерный уровень ноды в дереве (с несжатыми путями), считая от корня</summary>
		public int Rank;
	}
}
