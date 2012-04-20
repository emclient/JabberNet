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

using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
	/*
	 * <iq from='chat.shakespeare.lit'
	 *	to='juliet@capulet.lit/chamber'
	 *	type='get' 
	 *	id='comp1'>
	 *	<ping xmlns='urn:xmpp:ping'/>
	 * </iq>
	 */
	/// <summary>
    /// IQ packet with an oob query element inside.
    /// </summary>
    [SVN(@"$Id: OOB.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class PingIQ : jabber.protocol.client.TypedIQ<Ping>
    {
        /// <summary>
        /// Create an OOB IQ.
        /// </summary>
        /// <param name="doc"></param>
        public PingIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// An oob query element for file transfer.
    /// </summary>
 
    public class Ping : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Ping(XmlDocument doc) : base("ping", URI.PING, doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Ping(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }
}
