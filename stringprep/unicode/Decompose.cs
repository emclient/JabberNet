/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Collections.Generic;

namespace stringprep.unicode
{
    /// <summary>
    /// Decomposition data for NFKC.
    /// </summary>
    public class Decompose
    {
        /// <summary>
        /// Look up the expansion, if any, for the given character.
        /// </summary>
        /// <param name="ch">The character to find</param>
        /// <returns>the expansion, or null if none found.</returns>
        public static string Find(char ch)
        {
            int offset = Array.BinarySearch(DecomposeData.OffsetsKey, ch);
            if (offset < 0)
                return null;

            return DecomposeData.Expansion[DecomposeData.OffsetsValue[offset]];
        }
    }
}
