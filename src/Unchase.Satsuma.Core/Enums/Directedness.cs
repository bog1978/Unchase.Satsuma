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

namespace Unchase.Satsuma.Core.Enums
{
    /// <summary>
    /// Tells whether an arc, an arc set or a graph is directed or undirected.
    /// </summary>
    /// <remarks>
    /// Undirected arcs are referred to as edges.
    /// </remarks>
    public enum Directedness
    {
        /// <summary>
        /// The arc, arc set or graph is directed.
        /// </summary>
        Directed,

        /// <summary>
        /// The arc, arc set or graph is undirected.
        /// </summary>
        Undirected
    }
}