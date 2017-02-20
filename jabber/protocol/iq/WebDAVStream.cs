using System;

using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
	/*
     * <webdavstream xmlns='http://icewarp.com/protocol/webdavstream'>
  			<targeturl>https://www.icewarphttpserver.com/webdav/tonda@icewarp.com/@@UPLOAD@@/test(0).txt</targeturl>
	   </webdavstream>
     */
	/// <summary>
	/// IQ packet with a webdavstream query element inside.
	/// </summary>
	public class WebDAVStreamIQ : jabber.protocol.client.TypedIQ<WebDAVStream>
	{
		/// <summary>
		/// Create a WebDAVStream IQ.
		/// </summary>
		/// <param name="doc"></param>
		public WebDAVStreamIQ(XmlDocument doc) : base(doc)
		{
		}
	}

	/// <summary>
	/// A WebDAVStream query element for file transfer.
	/// </summary>
	public class WebDAVStream : Element
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="doc"></param>
		public WebDAVStream(XmlDocument doc) : base("query", URI.WEBDAVSTREAM, doc)
		{
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public WebDAVStream(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
		}

		/// <summary>
		/// WebDAV URL to send to
		/// </summary>
		public string TargetUrl
		{
			get { return GetElem("targeturl"); }
			set { SetElem("targeturl", value); }
		}
		/// <summary>
		/// Target JID
		/// </summary>
		public JID Target
		{
			get { return GetElem("target"); }
			set { SetElem("target", value); }
		}
		/// <summary>
		/// Original filename (no path just filename and extension)
		/// </summary>
		public string Filename
		{
			get { return GetElem("filename"); }
			set { SetElem("filename", value); }
		}
	}
}
