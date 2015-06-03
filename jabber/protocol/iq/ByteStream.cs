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
	 <iq type='set' 
			from='initiator@example.com/foo' 
			to='target@example.org/bar' 
			id='initiate'>
		  <query xmlns='http://jabber.org/protocol/bytestreams' 
				 sid='vxf9n471bn46'
				 mode='tcp'>
			<streamhost 
				jid='initiator@example.com/foo' 
				host='192.168.4.1' 
				port='5086'/>
			<streamhost 
				jid='streamhostproxy.example.net' 
				host='24.24.24.1' 
				zeroconf='_jabber.bytestreams'/>
		  </query>
		</iq>

	 */
	/// <summary>
    /// IQ packet with an socks bytestreams query element inside.
    /// </summary>
    
    public class ByteStreamIQ : jabber.protocol.client.TypedIQ<ByteStream>
    {
        /// <summary>
        /// Create an ByteStream IQ.
        /// </summary>
        /// <param name="doc"></param>
        public ByteStreamIQ(XmlDocument doc) : base(doc)
        {
        }

		public ByteStream ByteStream
		{
			get { return Instruction; }
		}
    }

    /// <summary>
    /// An bytestream query element for file transfer.
    /// </summary>
    
    public class ByteStream : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public ByteStream(XmlDocument doc) : base("query", URI.SOCKSByteStreams, doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public ByteStream(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Unique Stream ID
        /// </summary>
        public string SID
        {
            get { return GetAttr("sid"); }
            set { SetAttr("sid", value); }
        }

		public string Mode
		{
			get { return GetAttr("mode"); }
			set { SetElem("mode", value); }
		}

		public JID StreamHostUsed
		{
			get 
			{
				if (this["streamhost-used"] != null && this["streamhost-used"].Attributes["jid"] != null)
				{
					return new JID(this["streamhost-used"].Attributes["jid"].Value);
				}
				else
				{
					return null;
				}
			}
			set 
			{ 
				if (this["streamhost-used"] ==null)
				{
					XmlElement elem=this.OwnerDocument.CreateElement("streamhost-used");
					this.AddChild(elem);
				}
				this["streamhost-used"].SetAttribute("jid", value.ToString());
			}
		}
		public JID Activate
		{
			get { return new JID( GetElem("activate")); }
			set { SetElem("activate", value.ToString()); }
		}

		public StreamHost[] GetStreamHosts()
		{
			return new TypedElementList<StreamHost>(this).ToArray();
		}
        public void AddStreamHost(string host, JID jid, int port)
        {
            StreamHost streamHost = CreateChildElement<StreamHost>();
            streamHost.JID = jid;
            streamHost.Host = host;
            streamHost.Port = port;
          
        }
		public class StreamHost : Element
		{
			public StreamHost(XmlDocument doc)
				: base("streamhost", URI.SOCKSByteStreams, doc)
			{

			}
			public StreamHost(string prefix, XmlQualifiedName qname, XmlDocument doc) :
				base(prefix, qname, doc)
			{
			}
			/// <summary>
			/// JID
			/// </summary>
			public JID JID
			{
				get { return new JID(GetAttr("jid")); }
				set { SetAttr("jid", value.ToString()); }
			}
			public string Host
			{
				get
				{
					return GetAttr("host");
				}
				set
				{
					SetAttr("host", value);
				}
			}
			public int Port
			{
				get
				{
					return int.Parse(GetAttr("port"));
				}
				set
				{
					SetAttr("port", value.ToString());
				}
			}
			public string ZeroConf
			{
				get
				{
					return GetAttr("zeroconf");
				}
				set
				{
					SetAttr("zeroconf", value);
				}
			}
		}
    }
}
