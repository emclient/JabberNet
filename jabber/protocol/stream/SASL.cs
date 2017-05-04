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
using System.Collections.Generic;

namespace jabber.protocol.stream
{
    /// <summary>
    /// SASL mechanisms in stream features.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Mechanisms : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Mechanisms(XmlDocument doc) :
            base("", new XmlQualifiedName("mechanisms", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Mechanisms(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The SASL mechanisms valid for this stream.
        /// </summary>
        /// <returns></returns>
        public Mechanism[] GetMechanisms()
        {
            XmlNodeList nl = GetElementsByTagName("mechanism", URI.SASL);
			Dictionary<string, Mechanism> items = new Dictionary<string, Mechanism>();
			
            foreach (XmlNode n in nl)
            {
				string name = ((Mechanism)n).MechanismName;

				if (!items.ContainsKey(name))
					items.Add(name, (Mechanism)n);
            }
			Mechanism[] array = new Mechanism[items.Count];

			items.Values.CopyTo(array, 0);
			return array;
        }
    }

    /// <summary>
    /// Stores SASL mechanisms in stream features.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Mechanism : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Mechanism(XmlDocument doc) :
            base("", new XmlQualifiedName("mechanism", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Mechanism(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The IANA-registered SASL mechanism name.
        /// </summary>
        public string MechanismName
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }

    /// <summary>
    /// Auth, Challenge, and Response.
    /// </summary>
    public abstract class Step : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Step(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The innards of the step.  If it is "=", it
        /// means an intentionally blank response, not one waiting for a challenge.
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                string it = this.InnerText;
                if (it == "")
                    return null;
                if (it == "=")
                    return new byte[0];
                return Convert.FromBase64String(it);
            }
            set
            {
                if (value == null)
                    this.InnerText = "";
                else if (value.Length == 0)
                    this.InnerText = "=";
                else
                    this.InnerText = Convert.ToBase64String(value);
            }
        }
    }

    /// <summary>
    /// First phase of SASL auth.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Auth : Step
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Auth(XmlDocument doc) :
            base("", new XmlQualifiedName("auth", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Auth(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The chosen mechanism
        /// </summary>
        public string MechanismName
        {
            get
            {
                return GetAttribute("mechanism");
            }
            set
            {
                SetAttribute("mechanism", value);
            }
        }
    }
    /// <summary>
    /// Subsequent phases of SASL auth sent by server.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Challenge : Step
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Challenge(XmlDocument doc) :
            base("", new XmlQualifiedName("challenge", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Challenge(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// First phase of SASL auth.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Response : Step
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Response(XmlDocument doc) :
            base("", new XmlQualifiedName("response", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Response(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// SASL auth failed.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class SASLFailure : Step
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public SASLFailure(XmlDocument doc) :
            base("", new XmlQualifiedName("failure", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public SASLFailure(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// Abort SASL auth.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Abort : Step
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Abort(XmlDocument doc) :
            base("", new XmlQualifiedName("abort", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Abort(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }

    /// <summary>
    /// SASL auth successfult.
    /// </summary>
    [SVN(@"$Id: SASL.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Success : Step
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Success(XmlDocument doc) :
            base("", new XmlQualifiedName("success", jabber.protocol.URI.SASL), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Success(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }
}
