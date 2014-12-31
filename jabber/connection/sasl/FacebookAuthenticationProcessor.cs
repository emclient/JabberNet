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
    
    public class FacebookAuthenticationProcessor : SASLProcessor
    {
		private string applicationID;
		private string authToken;

		public FacebookAuthenticationProcessor(string applicationID, string authToken)
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
				a.Mechanism = MechanismType.FACEBOOK;
				return a;
			}
			string challenge = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(s.InnerText));

			NameValueCollection qs = parseQueryString(challenge);
			string nonce = qs["nonce"];
			string method = qs["method"];

			Dictionary<String, String> myparams;
			myparams = GetParams(method, nonce);
			
			StringBuilder sb = new StringBuilder();
			foreach (String myKey in myparams.Keys)
			{
				sb.Append(String.Format("{0}={1}&", myKey, myparams[myKey]));
			}
			
			Byte[] myResponseByte = Encoding.UTF8.GetBytes(sb.ToString().TrimEnd('&'));

			Response r = new Response(doc);
			r.Bytes = myResponseByte;
			return r;
            
        }
		private NameValueCollection parseQueryString(string s)
		{
			NameValueCollection nvc = new NameValueCollection();

			// remove anything other than query string from url
			if (s.Contains("?"))
			{
				s = s.Substring(s.IndexOf('?') + 1);
			}

			foreach (string vp in Regex.Split(s, "&"))
			{
				string[] singlePair = Regex.Split(vp, "=");
				if (singlePair.Length == 2)
				{
					nvc.Add(singlePair[0], singlePair[1]);
				}
				else
				{
					// only one key with no value specified in query string
					nvc.Add(singlePair[0], string.Empty);
				}
			}

			return nvc;
		}

		private Dictionary<string, string> GetParams(string method, string nonce)
		{
			Dictionary<string, string> myparams = new Dictionary<string, string>();
			myparams.Add("method", method);
			myparams.Add("nonce", nonce);
			myparams.Add("access_token", authToken); 
			myparams.Add("api_key", applicationID); 
			myparams.Add("call_id", System.DateTime.Now.Ticks.ToString());
			myparams.Add("v", "1.0");

			return myparams;	 
		}

		private string ComputeHash(SortedDictionary<String, String> myparams, String AppSecretKey)
		{
			StringBuilder parametersForSig = new StringBuilder();
			foreach (String myKey in myparams.Keys)
			{
				parametersForSig.Append(String.Format("{0}={1}", myKey, myparams[myKey]));
			}
			parametersForSig.Append(AppSecretKey);

			System.Security.Cryptography.MD5CryptoServiceProvider myMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			Byte[] myData = myMD5.ComputeHash(Encoding.UTF8.GetBytes(parametersForSig.ToString()));

			StringBuilder md5HashToReturn = new StringBuilder();

			for (int i = 0; i < myData.Length; i++)
			{
				md5HashToReturn.Append(myData[i].ToString("x2"));
			}

			return md5HashToReturn.ToString().ToLower();

		}
    }
}
