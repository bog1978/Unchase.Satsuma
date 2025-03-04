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

namespace Unchase.Satsuma.Core
{
    /// <inheritdoc cref="NodeProperties{TProperty}"/>
    public class NodeProperties :
        NodeProperties<object>
    {
    }

    /// <summary>
    /// The node properties.
    /// </summary>
    /// <typeparam name="TProperty">The type of stored node properties.</typeparam>
    public class NodeProperties<TProperty>
    {
        /// <summary>
        /// The node properties.
        /// </summary>
        public Dictionary<string, TProperty>? Properties { get; }

        /// <summary>
        /// Initialize <see cref="NodeProperties{TProperty}"/>.
        /// </summary>
        /// <param name="properties">The node properties.</param>
        public NodeProperties(Dictionary<string, TProperty>? properties = default)
        {
            Properties = properties;
        }

        /// <summary>
        /// Initialize <see cref="NodeProperties{TProperty}"/>.
        /// </summary>
        /// <param name="propertyKey">The node property key.</param>
        /// <param name="propertyValue">The node property value.</param>
        public NodeProperties(string propertyKey, TProperty propertyValue)
        {
            Properties ??= new()
            {
                { propertyKey, propertyValue }
            };
        }
    }
}