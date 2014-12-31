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
using System.Xml;
using jabber.protocol.stream;
using System.Net;
using System;
using System.Text;

namespace jabber.connection.sasl
{
    /// <summary>
	/// Live Messenge Authentication Mechanism 
    /// </summary>
    
    public class XOAuth2Processor : SASLProcessor
    {
		private string applicationID;
        private string accessToken;

		public XOAuth2Processor(string applicationID, string accessToken)
			: base()
		{
			this.applicationID = applicationID;
            this.accessToken = accessToken;
		}

        /// <summary>
        /// Perform the next step
        /// </summary>
        /// <param name="s">Null if it's the initial response</param>
        /// <param name="doc">Document to create Steps in</param>
        /// <returns></returns>
        public override Step step(Step s, XmlDocument doc)
        {
			WebClient client = new WebClient();

			if (s == null)
			{
				// first step
				Auth a = new Auth(doc);
				a.Mechanism = MechanismType.XOAUTH2;
				a.SetAttribute("service", "http://www.google.com/talk/protocol/auth", "oauth2");
				a.InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes("\u0000" + this[USERNAME] + "\u0000" + accessToken));
				return a;
			}

			throw new AuthenticationFailedException();
		}
	}
}
