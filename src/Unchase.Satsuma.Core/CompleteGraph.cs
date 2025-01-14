﻿#region License
/*This file is part of Satsuma Graph Library
Copyright © 2013 Balázs Szalkai

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.

Updated by Unchase © 2022*/
#endregion

using Unchase.Satsuma.Core.Contracts;
using Unchase.Satsuma.Core.Enums;

namespace Unchase.Satsuma.Core
{
    /// <inheritdoc cref="CompleteGraph{TNodeProperty, TArcProperty}"/>
    public sealed class CompleteGraph :
        CompleteGraph<object, object>
    {
        /// <summary>
        /// Initialize <see cref="CompleteGraph"/>.
        /// </summary>
        /// <param name="nodeCount">Node count.</param>
        /// <param name="directedness"><see cref="Directedness"/>.</param>
        public CompleteGraph(
            int nodeCount,
            Directedness directedness)
                : base(nodeCount, directedness)
        {
        }
    }

    /// <summary>
    /// A complete undirected or directed graph on a given number of nodes.
    /// </summary>
    /// <remarks>
    /// <para>A complete undirected graph is defined as a graph which has all the possible edges.</para>
    /// <para>A complete directed graph is defined as a graph which has all the possible directed arcs.</para>
    /// <para>Memory usage: O(1).</para>
    /// <para>This type is thread safe.</para>
    /// </remarks>
    /// <typeparam name="TNodeProperty">The type of stored node properties.</typeparam>
    /// <typeparam name="TArcProperty">The type of stored arc properties.</typeparam>
    public class CompleteGraph<TNodeProperty, TArcProperty> : 
        IGraph<TNodeProperty, TArcProperty>
	{
		/// <inheritdoc />
        public Dictionary<Node, NodeProperties<TNodeProperty>> NodePropertiesDictionary { get; } = new();

        /// <inheritdoc />
        public Dictionary<Arc, ArcProperties<TArcProperty>> ArcPropertiesDictionary { get; } = new();

		/// <summary>
		/// True if the graph contains all the possible directed arcs, 
		/// false if it contains all the possible edges.
		/// </summary>
		public bool Directed { get; }

		private readonly int _nodeCount;

		/// <summary>
		/// Initialize <see cref="CompleteGraph{TNodeProperty, TArcProperty}"/>.
		/// </summary>
		/// <param name="nodeCount">Node count.</param>
		/// <param name="directedness"><see cref="Directedness"/>.</param>
		public CompleteGraph(
            int nodeCount, 
            Directedness directedness)
		{
			_nodeCount = nodeCount;
			Directed = directedness == Directedness.Directed;

            if (nodeCount <= 0)
            {
                throw new ArgumentException("Invalid node count: " + nodeCount);
            }

			var arcCount = (long)nodeCount * (nodeCount - 1);
            if (!Directed)
            {
                arcCount /= 2;
            }

            if (arcCount > int.MaxValue)
            {
                throw new ArgumentException("Too many nodes: " + nodeCount);
            }
		}

		/// <summary>
		/// Gets a node of the complete graph by its index.
		/// </summary>
		/// <param name="index">An integer between 0 (inclusive) and NodeCount() (exclusive).</param>
        public static Node GetNode(int index)
		{
			return new(1L + index);
        }

		/// <summary>
		/// Gets the index of a graph node.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <returns>An integer between 0 (inclusive) and NodeCount() (exclusive).</returns>
		public static int GetNodeIndex(Node node)
		{
			return (int)(node.Id - 1);
		}

		/// <summary>
		/// Gets the unique arc between two nodes.
		/// </summary>
		/// <param name="u">The first node.</param>
		/// <param name="v">The second node.</param>
		/// <returns>
		/// The arc that goes from u to v, or <see cref="Arc.Invalid"/> if u equals v.
		/// </returns>
		public Arc GetArc(Node u, Node v)
		{
			var x = GetNodeIndex(u);
			var y = GetNodeIndex(v);

			if (x == y) return Arc.Invalid;
			if (!Directed && x > y)
			{
				(x, y) = (y, x);
            }

			return GetArcInternal(x, y);
		}

		/// <summary>
		/// If !directed, then x less y must be true.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
        private Arc GetArcInternal(int x, int y)
		{
			return new(1L + (long)y * _nodeCount + x);
		}

		/// <inheritdoc />
        public Dictionary<string, TNodeProperty>? GetNodeProperties(Node node)
        {
            return NodePropertiesDictionary.TryGetValue(node, out var p)
                ? p.Properties
                : null;
        }

        /// <inheritdoc />
        public Dictionary<string, TArcProperty>? GetArcProperties(Arc arc)
        {
            return ArcPropertiesDictionary.TryGetValue(arc, out var p)
                ? p.Properties
                : null;
        }

		/// <inheritdoc />
		public Node U(Arc arc)
		{
			return new(1L + (arc.Id - 1) % _nodeCount);
		}

        /// <inheritdoc />
		public Node V(Arc arc)
		{
			return new(1L + (arc.Id - 1) / _nodeCount);
		}

        /// <inheritdoc />
		public bool IsEdge(Arc arc)
		{
			return !Directed;
		}

        /// <inheritdoc />
		public IEnumerable<Node> Nodes()
		{
            for (var i = 0; i < _nodeCount; i++)
            {
                yield return GetNode(i);
            }
		}

        /// <inheritdoc />
		public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
		{
			if (Directed)
			{
				for (var i = 0; i < _nodeCount; i++)
                {
                    for (var j = 0; j < _nodeCount; j++)
                    {
                        if (i != j)
                        {
                            yield return GetArcInternal(i, j);
                        }
                    }
                }
			}
			else
			{
				for (var i = 0; i < _nodeCount; i++)
                {
                    for (var j = i + 1; j < _nodeCount; j++)
                    {
                        yield return GetArcInternal(i, j);
                    }
                }
			}
		}

        /// <inheritdoc />
		public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All)
		{
			if (Directed)
			{
                if (filter == ArcFilter.Edge)
                {
                    yield break;
                }

				if (filter != ArcFilter.Forward)
                {
                    foreach (var w in Nodes())
                    {
                        if (w != u)
                        {
                            yield return GetArc(w, u);
                        }
                    }
                }
			}

			if (!Directed || filter != ArcFilter.Backward)
            {
                foreach (var w in Nodes())
                {
                    if (w != u)
                    {
                        yield return GetArc(u, w);
                    }
                }
            }
		}

        /// <inheritdoc />
		public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
		{
			if (Directed)
			{
                if (filter == ArcFilter.Edge)
                {
                    yield break;
                }

                if (filter != ArcFilter.Forward)
                {
                    yield return GetArc(v, u);
                }
			}

            if (!Directed || filter != ArcFilter.Backward)
            {
                yield return GetArc(u, v);
            }
		}

        /// <inheritdoc />
		public int NodeCount()
		{
			return _nodeCount;
		}

        /// <inheritdoc />
		public int ArcCount(ArcFilter filter = ArcFilter.All)
		{
			var result = _nodeCount * (_nodeCount - 1);
            if (!Directed)
            {
                result /= 2;
            }

			return result;
		}

        /// <inheritdoc />
		public int ArcCount(Node u, ArcFilter filter = ArcFilter.All)
        {
            if (!Directed)
            {
                return _nodeCount - 1;
            }

            return filter switch
            {
                ArcFilter.All => 2 * (_nodeCount - 1),
                ArcFilter.Edge => 0,
                _ => _nodeCount - 1
            };
        }

        /// <inheritdoc />
		public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All)
        {
            if (!Directed)
            {
                return 1;
            }

            return filter switch
            {
                ArcFilter.All => 2,
                ArcFilter.Edge => 0,
                _ => 1
            };
        }

        /// <inheritdoc />
		public bool HasNode(Node node)
		{
			return node.Id >= 1 && node.Id <= _nodeCount;
		}

		/// <inheritdoc />
		public bool HasArc(Arc arc)
		{
			var v = V(arc);
            if (!HasNode(v))
            {
                return false;
            }

			var u = U(arc);

			// HasNode(u) is always true
			return Directed || u.Id < v.Id;
		}
    }
}