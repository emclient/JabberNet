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
using bedrock.util;

namespace jabber.connection.sasl
{
    /// <summary>
    /// Authentication failed.
    /// </summary>
    [SVN(@"$Id: SASLProcessor.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class AuthenticationFailedException : Sasl.SaslException
	{
        /// <summary>
        ///
        /// </summary>
        public AuthenticationFailedException() : base()
        {}

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public AuthenticationFailedException(string message) : base(message)
        {}
    }
}
