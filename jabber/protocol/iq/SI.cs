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
using System.Globalization;

namespace jabber.protocol.iq
{
	/*
	 * <iq type='set' id='offer1' to='receiver@jabber.org/resource'>
  <si xmlns='http://jabber.org/protocol/si' 
	  id='a0'
	  mime-type='text/plain'
	  profile='http://jabber.org/protocol/si/profile/file-transfer'>
	<file xmlns='http://jabber.org/protocol/si/profile/file-transfer'
		  name='test.txt'
		  size='1022'
		  hash='552da749930852c69ae5d2141d3766b1'
		  date='1969-07-21T02:56:15Z'>
	  <desc>This is a test. If this were a real file...</desc>
	</file>
	<feature xmlns='http://jabber.org/protocol/feature-neg'>
	  <x xmlns='jabber:x:data' type='form'>
		<field var='stream-method' type='list-single'>
		  <option><value>http://jabber.org/protocol/bytestreams</value></option>
		  <option><value>http://jabber.org/protocol/ibb</value></option>
		</field>
	  </x>
	</feature>
  </si>
</iq>

	 */
	/// <summary>
    /// IQ packet with SI query element inside.
    /// </summary>
    public class SIIQ : jabber.protocol.client.TypedIQ<SI>
    {
        /// <summary>
        /// Create an OOB IQ.
        /// </summary>
        /// <param name="doc"></param>
        public SIIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// SI element for file transfer.
    /// </summary>
 
    public class SI : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public SI(XmlDocument doc) : base("si", URI.SI, doc)
        {
			CreateChildElement<SIFile>();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public SI(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// URL to send/receive from
        /// </summary>
        public SIFile File
        {
            get 
			{
				return this["file"] as SIFile;
			}
        }

		public SIFeature Feature
		{
			get
			{
				return this["feature"] as SIFeature;
			}
		}


		public class SIFile : Element
		{
			public SIFile(XmlDocument doc)
				: base("file", URI.SIFile, doc)
			{

			}
			public SIFile(string prefix, XmlQualifiedName qname, XmlDocument doc) :
				base(prefix, qname, doc)
			{
			}
			/// <summary>
			/// File description
			/// </summary>
			public string Desc
			{
				get { return GetElem("desc"); }
				set { SetElem("desc", value); }
			}
			public string FileName
			{
				get
				{
					return GetAttr("name");
				}
				set
				{
					SetAttr("name", value);
				}
			}
			public int Size
			{
				get
				{
					//fix me, handle error
					return Convert.ToInt32(GetAttr("size"));
				}
				set
				{
					SetAttr("size", value.ToString());
				}
			}
			public string Hash
			{
				get
				{
					return GetAttr("hash");
				}
				set
				{
					SetAttr("hash", value);
				}
			}
			public DateTime Date
			{
				get
				{
					return DateTime.ParseExact(GetAttr("date"), "o", CultureInfo.InvariantCulture);
				}
				set
				{
					SetAttr("date", value.ToUniversalTime().ToString("o"));
				}
			}
		}
		public class SIFeature : Element
		{
			public SIFeature(XmlDocument doc)
				: base("feature", URI.SIFeature, doc)
			{
				CreateChildElement<x.Data>();
			}
			public SIFeature(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
			{
			}
			public x.Data X
			{
				get
				{
					return GetChildElement<x.Data>();
				}
			}
			public int Size
			{
				get
				{
					//fix me, handle error
					return Convert.ToInt32(GetAttr("size"));
				}
				set
				{
					SetAttr("size", value.ToString());
				}
			}
			public string Hash
			{
				get
				{
					return GetAttr("hash");
				}
				set
				{
					SetAttr("hash", value);
				}
			}
			public DateTime Date
			{
				get
				{
					return DateTime.ParseExact(GetAttr("date"), "o", CultureInfo.InvariantCulture);
				}
				set
				{
					SetAttr("date", value.ToUniversalTime().ToString("o"));
				}
			}
		}
    }
	

}
