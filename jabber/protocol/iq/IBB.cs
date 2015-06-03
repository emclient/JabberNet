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
	<iq from='romeo@montague.net/orchard' id='jn3h8g65' to='juliet@capulet.com/balcony' type='set'>
	   <open xmlns='http://jabber.org/protocol/ibb' block-size='4096' sid='i781hf64' stanza='iq'/>
	</iq>

	<iq from='romeo@montague.net/orchard' 
     id='kr91n475'
     to='juliet@capulet.com/balcony'
     type='set'>
      <data xmlns='http://jabber.org/protocol/ibb' seq='0' sid='i781hf64'>
         qANQR1DBwU4DX7jmYZnncmUQB/9KuKBddzQH+tZ1ZywKK0yHKnq57kWq+RFtQdCJ
         WpdWpR0uQsuJe7+vh3NWn59/gTc5MDlX8dS9p0ovStmNcyLhxVgmqS8ZKhsblVeu
         IpQ0JgavABqibJolc3BKrVtVV1igKiX/N7Pi8RtY1K18toaMDhdEfhBRzO/XB0+P
         AQhYlRjNacGcslkhXqNjK5Va4tuOAPy2n1Q8UUrHbUd0g+xJ9Bm0G0LZXyvCWyKH
         kuNEHFQiLuCY6Iv0myq6iX6tjuHehZlFSh80b5BVV9tNLwNR5Eqz1klxMhoghJOA
	  </data>
	</iq>
	 
	<iq from='romeo@montague.net/orchard'
	 id='us71g45j'
     to='juliet@capulet.com/balcony'
     type='set'>
       <close xmlns='http://jabber.org/protocol/ibb' sid='i781hf64'/>
    </iq>
	 */
	/// <summary>
    /// IQ packet with an OpenIBB element inside.
    /// </summary>
    
    public class OpenIBBIQ : jabber.protocol.client.TypedIQ<OpenIBB>
    {
        /// <summary>
        /// Open an IBB stream IQ.
        /// </summary>
        /// <param name="doc"></param>
        public OpenIBBIQ(XmlDocument doc) : base(doc)
        {
        }
		
		public OpenIBB Open
		{
			get { return (OpenIBB)this["open"]; }
		}
    }

	/// <summary>
	/// IQ packet with an CloseIBB element inside.
	/// </summary>

	public class CloseIBBIQ : jabber.protocol.client.TypedIQ<CloseIBB>
	{
		/// <summary>
		/// Open an IBB stream IQ.
		/// </summary>
		/// <param name="doc"></param>
		public CloseIBBIQ(XmlDocument doc)
			: base(doc)
		{
		}
		
		public CloseIBB Close
		{
			get { return (CloseIBB)this["close"]; }
		}
	}

	/// <summary>
	/// IQ packet with an IBBData element inside.
	/// </summary>

	public class IBBDataIQ : jabber.protocol.client.TypedIQ<IBBData>
	{
		/// <summary>
		/// IBB Data IQ.
		/// </summary>
		/// <param name="doc"></param>
		public IBBDataIQ(XmlDocument doc)
			: base(doc)
		{
		}
		public IBBData Data
		{
			get { return (IBBData) this["data"]; }
		}
	}

    /// <summary>
    /// An open element for file transfer.
    /// </summary>
    
    public class OpenIBB : Element
    {
		
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public OpenIBB(XmlDocument doc) : base("open", URI.IBB, doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
		public OpenIBB(string prefix, XmlQualifiedName qname, XmlDocument doc) :
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

		public string Stanza
		{
			get { return GetAttr("stanza"); }
			set { SetAttr("stanza", value); }
		}

		public int BlockSize
		{
			get 
			{
				return int.Parse(GetAttr("block-size"));
			}
			set { SetAttr("block-size", value.ToString()); }
		}	
    }
	
	/// <summary>
	/// A close element for file transfer.
	/// </summary>
	public class CloseIBB : Element
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="doc"></param>
		public CloseIBB(XmlDocument doc)
			: base("close", URI.IBB, doc)
		{
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public CloseIBB(string prefix, XmlQualifiedName qname, XmlDocument doc) :
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
	}

	/// <summary>
	/// A data element for file transfer.
	/// </summary>
	public class IBBData : Element
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="doc"></param>
		public IBBData(XmlDocument doc)
			: base("data", URI.IBB, doc)
		{
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public IBBData(string prefix, XmlQualifiedName qname, XmlDocument doc) :
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

		
		public int Seq
		{
			get
			{
				return int.Parse(GetAttr("seq"));
			}
			set { SetAttr("seq", value.ToString()); }
		}
		public byte[] Data
		{
			get
			{
				return System.Convert.FromBase64String(this.InnerText);
			}
			set
			{
				this.InnerText = System.Convert.ToBase64String(value);
			}
		}
	}
}
