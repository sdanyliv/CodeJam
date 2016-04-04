﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if FW40
/*============================================================
**
** Interface:  IReadOnlyCollection<T>
** 
** 
**
** Purpose: Base interface for read-only generic lists.
** 
===========================================================*/

using JetBrains.Annotations;

namespace System.Collections.Generic
{
	/// <summary>
	/// Represents a strongly-typed, read-only collection of elements.
	/// </summary>
	/// <typeparam name="T">The type of the elements.</typeparam>
	[PublicAPI]
	public interface IReadOnlyCollection<out T> : IEnumerable<T>
	{
		/// <summary>
		/// Gets the number of elements in the collection.
		/// </summary>
		int Count { get; }
	}
}
#endif