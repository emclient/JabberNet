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
using System.Reflection;
using System.Diagnostics;

namespace jabber.protocol.iq
{
	/*
	 * <iq type="set" to="horatio@denmark" from="sailor@sea" id="i_oob_001">
	 *   <query xmlns="jabber:iq:oob">
	 *     <url>http://denmark/act4/letter-1.html</url>
	 *     <desc>There's a letter for you sir.</desc>
	 *   </query>
	 * </iq>
	 */
	/// <summary>
	/// IQ packet with an oob query element inside.
	/// </summary>
	[SVN(@"$Id: OOB.cs 724 2008-08-06 18:09:25Z hildjj $")]
	public class PrivacyIQ : jabber.protocol.client.TypedIQ<Privacy>
	{
		/// <summary>
		/// Create an OOB IQ.
		/// </summary>
		/// <param name="doc"></param>
		public PrivacyIQ(XmlDocument doc)
			: base(doc)
		{
		}
	}

	/// <summary>
	/// An oob query element for file transfer.
	/// </summary>
	[SVN(@"$Id: OOB.cs 724 2008-08-06 18:09:25Z hildjj $")]
	public class Privacy : Element
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="doc"></param>
		public Privacy(XmlDocument doc)
			: base("query", URI.PRIVACY, doc)
		{
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public Privacy(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
		}

		/// <summary>
		/// Active privacy list name
		/// </summary>
		public string ActiveName
		{
			get 
			{ 

				 XmlElement e = this["active"];
				 if (e == null)
					return null;
				 XmlAttribute nameAttr = e.Attributes["name"];
				if (nameAttr==null)
				{
					return null;
				}
				else
				{
					return nameAttr.Value;
				}
			
			}
			set 
			{
				XmlElement e = GetOrCreateElement("active", null, null);
				e.RemoveAll();

				if (value != null)
					e.SetAttribute("name", value);
			}
		}

		/// <summary>
		/// Active privacy list name
		/// </summary>
		public string PrivacyListName
		{
			get
			{

				XmlElement e = this["list"];
				if (e == null)
					return null;
				XmlAttribute nameAttr = e.Attributes["name"];
				if (nameAttr == null)
				{
					return null;
				}
				else
				{
					return nameAttr.Value;
				}

			}
			set
			{
				XmlElement e = GetOrCreateElement("list", null, null);
				e.RemoveAll();

				if (value != null)
					e.SetAttribute("name", value);
			}
		}
		/// <summary>
		/// Create an element that is a child of this element, of the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		protected T CreateChildElement<T>(XmlNode node)
			where T : Element
		{
			// Note: It would be cool to just do new T(OwnerDocument), but you can only call
			// parameter-less constructors in generic-land.
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(XmlDocument) });
			Debug.Assert(constructor != null, "Type " + typeof(T).ToString() + " does not have a constructor taking an XmlDocument");
			T c = (T)constructor.Invoke(new object[] { this.OwnerDocument });
			node.AppendChild(c);
			return c;
		}
		/// <summary>
		/// Add an privacy item that shows or hides this particular JID from list when in invisible mode
		/// </summary>
		/// <returns></returns>
		public PrivacyItem AddPrivacyItem(JID jid, PrivacyItemAction action, PrivacyItemRights right)
		{
			
			XmlElement e = GetOrCreateElement("list", null, null);

			PrivacyItem p = CreateChildElement<PrivacyItem>(e);

			p.Value = jid.Bare;
			p.Type = PrivacyItemType.jid;
			p.Action = action;
			p.Order = GetPrivacyItems().Length+1;
			
			switch (right)
			{
				case PrivacyItemRights.PresenceIn:
					p.AllowPresenceIn = true;
					break;
				case PrivacyItemRights.PresenceOut:
					p.AllowPresenceOut = true;
					break;
				case PrivacyItemRights.Messages:
					p.AllowMessages = true;
					break;
			}
			
			return p;
		}

		/// <summary>
		/// Add an privacy item that shows or hides all other items
		/// </summary>
		/// <returns></returns>
		public PrivacyItem AddPrivacyItem(PrivacyItemAction action, PrivacyItemRights right)
		{

			XmlElement e = GetOrCreateElement("list", null, null);
			PrivacyItem p = CreateChildElement<PrivacyItem>(e);

			p.Action = action;
			p.Order = GetPrivacyItems().Length + 1;

			switch (right)
			{
				case PrivacyItemRights.PresenceIn:
					p.AllowPresenceIn = true;
					break;
				case PrivacyItemRights.PresenceOut:
					p.AllowPresenceOut = true;
					break;
				case PrivacyItemRights.Messages:
					p.AllowMessages = true;
					break;
			}
		
			return p;
		}

		/// <summary>
		/// Remove a privacy item of the given name.  Does nothing if that group is not found.
		/// </summary>
		/// <param name="name"></param>
		public void RemovePrivacyItem(JID jid)
		{
			foreach (PrivacyItem p in GetElements<PrivacyItem>())
			{
				if (p.Value == jid.Bare)
				{
					this.RemoveChild(p);
					return;
				}
			}
		}

		/// <summary>
		/// List of privacy items
		/// </summary>
		/// <returns></returns>
		public PrivacyItem[] GetPrivacyItems()
		{
			XmlElement e = GetOrCreateElement("list", null, null);
			return new TypedElementList<PrivacyItem>(e).ToArray();
		}

		/// <summary>
		/// Is this jid in the list?
		/// </summary>
		/// <param name="name">The name of the group to check</param>
		/// <returns></returns>
		public bool HasJID(JID jid)
		{
			foreach (PrivacyItem p in GetElements<PrivacyItem>())
			{
				if (p.Value == jid.Bare)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Get the group object of the given name in this item.
		/// If there is no group of that name, returns null.
		/// </summary>
		/// <param name="jid">The jid of the privacy list to return</param>
		/// <returns>null if none found.</returns>
		public PrivacyItem GetPrivacyItem(JID jid)
		{
			foreach (PrivacyItem p in GetElements<PrivacyItem>())
			{
				if (p.Value == jid.Bare)
					return p;
			}
			return null;
		}
	}

	public class PrivacyItem : Element
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="doc"></param>
		public PrivacyItem(XmlDocument doc)
			: base("item", URI.PRIVACY, doc)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="qname"></param>
		/// <param name="doc"></param>
		public PrivacyItem(string prefix, XmlQualifiedName qname, XmlDocument doc) :
			base(prefix, qname, doc)
		{
			
		}
		/// <summary>
		/// Item type (jid, group or subscription)
		/// </summary>
		public PrivacyItemType Type
		{
			get { return GetEnumAttr<PrivacyItemType>("type"); }
			set { SetEnumAttr("type", value); }
		}

		/// <summary>
		/// Item Value
		/// </summary>
		public string Value
		{
			get { return GetAttr("value"); }
			set { this.SetAttr("value", value); }
		}

		

		/// <summary>
		/// Action (deny or allow)
		/// </summary>
		public PrivacyItemAction Action
		{
			get { return GetEnumAttr<PrivacyItemAction>("action"); }
			set { SetEnumAttr("action", value); }
		}

		/// <summary>
		/// Allow presence in
		/// </summary>
		public bool AllowPresenceIn
		{
			get 
			{ 
				return GetElementsByTagName("presence-in").Count > 0 ? true : false; 
			}
			set 
			{ 
				if (GetElementsByTagName("presence-in").Count == 0)
					this.AddChild(this.OwnerDocument.CreateElement("presence-in")); 
			}
		}

		/// <summary>
		/// Allow presence in
		/// </summary>
		public bool AllowPresenceOut
		{
			get
			{
				return GetElementsByTagName("presence-out").Count > 0 ? true : false;
			}
			set
			{
				if (GetElementsByTagName("presence-out").Count == 0)
					this.AddChild(this.OwnerDocument.CreateElement("presence-out"));
			}
		}
		/// <summary>
		/// Allow messages
		/// </summary>
		public bool AllowMessages
		{
			get
			{
				return GetElementsByTagName("message").Count > 0 ? true : false;
			}
			set
			{
				if (GetElementsByTagName("message").Count == 0)
					this.AddChild(this.OwnerDocument.CreateElement("message"));
			}
		}

		

		/// <summary>
		/// Order
		/// </summary>
		public int Order
		{
			get { return int.Parse(GetAttr("order")); }
			set { SetAttr("order", value.ToString()); }
		}
	}
	public enum PrivacyItemType
	{
		/// <summary>
		/// Privacy based on JID.
		/// </summary>
		jid,
		/// <summary>
		/// Privacy based on user groups
		/// </summary>
		group,
		/// <summary>
		/// Privacy based on subscription type (unavailable etc)
		/// </summary>
		subscription,
	}
	public enum PrivacyItemAction
	{
		/// <summary>
		/// Show this item
		/// </summary>
		allow,
		/// <summary>
		/// Hide this item
		/// </summary>
		deny,
	}

	public enum PrivacyItemRights
	{
		/// <summary>
		/// ignoration
		/// </summary>
		PresenceIn = 1,
		/// <summary>
		/// invisibility
		/// </summary>
		PresenceOut = 2,
		/// <summary>
		/// messages ignoration
		/// </summary>
		Messages = 4
	}

	
}
