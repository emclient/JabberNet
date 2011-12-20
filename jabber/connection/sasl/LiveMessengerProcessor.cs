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
using System.IO;
using System.Diagnostics;
using System.Xml;

using bedrock.util;
using jabber.protocol.stream;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace jabber.connection.sasl
{
    /// <summary>
	/// Facebook authentication Mechanism  http://developers.facebook.com/docs/chat/
    /// </summary>
    
    public class LiveMessengerProcessor : SASLProcessor
    {
		private string applicationID;
		private string authToken;
		public LiveMessengerProcessor(string applicationID, string authToken)
			: base()
		{
			this.applicationID = applicationID;
			this.authToken = authToken;
		}
        /// <summary>
        /// Perform the next step
        /// </summary>
        /// <param name="s">Null if it's the initial response</param>
        /// <param name="doc">Document to create Steps in</param>
        /// <returns></returns>
        public override Step step(Step s, XmlDocument doc)
        {
			if (s == null)
			{
				// first step
				Auth a = new Auth(doc);
				a.Mechanism = MechanismType.LIVEMESSENGER;
				a.InnerText = authToken;
				return a;
			}
			else
			{
				Response r = new Response(doc);
				r.InnerText = authToken;
				return r;
			}
        }
    }
}
