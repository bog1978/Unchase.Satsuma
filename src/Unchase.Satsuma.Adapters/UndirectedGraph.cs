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
# endregion

using Unchase.Satsuma.Core;
using Unchase.Satsuma.Core.Contracts;
using Unchase.Satsuma.Core.Enums;

namespace Unchase.Satsuma.Adapters
{
    /// <inheritdoc cref="UndirectedGraph{TNodeProperty, TArcProperty}"/>
    public sealed class UndirectedGraph :
        UndirectedGraph<object, object>
    {
        /// <summary>
        /// Initialize <see cref="UndirectedGraph"/>.
        /// </summary>
        /// <param name="graph"><see cref="IGraph"/>.</param>
        public UndirectedGraph(
            IGraph graph)
                : base(graph)
        {
        }
    }

    /// <summary>
    /// Adapter showing all arcs of an underlying graph as undirected edges.
    /// </summary>
    /// <remarks>
    /// <para>Node and Arc objects are interchangeable between the adapter and the original graph.</para>
    /// <para>The underlying graph can be freely modified while using this adapter.</para>
    /// </remarks>
    /// <typeparam name="TNodeProperty">The type of stored node properties.</typeparam>
    /// <typeparam name="TArcProperty">The type of stored arc properties.</typeparam>
    public class UndirectedGraph<TNodeProperty, TArcProperty> : 
        IGraph<TNodeProperty, TArcProperty>
    {
        private readonly IGraph<TNodeProperty, TArcProperty> _graph;

        /// <inheritdoc />
        public Dictionary<Node, NodeProperties<TNodeProperty>> NodePropertiesDictionary { get; } = new();

        /// <inheritdoc />
        public Dictionary<Arc, ArcProperties<TArcProperty>> ArcPropertiesDictionary { get; } = new();

        /// <summary>
        /// Initialize <see cref="UndirectedGraph{TNodeProperty, TArcProperty}"/>.
        /// </summary>
        /// <param name="graph"><see cref="IGraph{TNodeProperty, TArcProperty}"/>.</param>
        public UndirectedGraph(
            IGraph<TNodeProperty, TArcProperty> graph)
        {
            _graph = graph;
        }

        /// <inheritdoc />
        public Dictionary<string, TNodeProperty>? GetNodeProperties(Node node)
        {
            return NodePropertiesDictionary.TryGetValue(node, out var p)
                ? p.Properties
                : _graph.GetNodeProperties(node);
        }

        /// <inheritdoc />
        public Dictionary<string, TArcProperty>? GetArcProperties(Arc arc)
        {
            return ArcPropertiesDictionary.TryGetValue(arc, out var p)
                ? p.Properties
                : _graph.GetArcProperties(arc);
        }

        /// <inheritdoc />
        public Node U(Arc arc)
        {
            return _graph.U(arc);
        }

        /// <inheritdoc />
        public Node V(Arc arc)
        {
            return _graph.V(arc);
        }

        /// <inheritdoc />
        public bool IsEdge(Arc arc)
        {
            return true;
        }

        /// <inheritdoc />
        public IEnumerable<Node> Nodes()
        {
            return _graph.Nodes();
        }

        /// <inheritdoc />
        public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
        {
            return _graph.Arcs();
        }

        /// <inheritdoc />
        public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All)
        {
            return _graph.Arcs(u);
        }

        /// <inheritdoc />
        public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
        {
            return _graph.Arcs(u, v);
        }

        /// <inheritdoc />
        public int NodeCount()
        {
            return _graph.NodeCount();
        }

        /// <inheritdoc />
        public int ArcCount(ArcFilter filter = ArcFilter.All)
        {
            return _graph.ArcCount();
        }

        /// <inheritdoc />
        public int ArcCount(Node u, ArcFilter filter = ArcFilter.All)
        {
            return _graph.ArcCount(u);
        }

        /// <inheritdoc />
        public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All)
        {
            return _graph.ArcCount(u, v);
        }

        /// <inheritdoc />
        public bool HasNode(Node node)
        {
            return _graph.HasNode(node);
        }

        /// <inheritdoc />
        public bool HasArc(Arc arc)
        {
            return _graph.HasArc(arc);
        }
    }
}