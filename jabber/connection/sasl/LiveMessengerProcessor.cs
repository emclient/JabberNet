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
	/// Live Messenge Authentication Mechanism 
    /// </summary>
    
    public class LiveMessengerProcessor : SASLProcessor
    {
		private static readonly string oAuthTokenUri = "https://oauth.live.com/token";
		private static readonly string redirectUri = "https://oauth.live.com/desktop";

		private string applicationID;
		private string refreshToken;
		private string accessToken;
		public LiveMessengerProcessor(string applicationID, string refreshToken)
			: base()
		{
			this.applicationID = applicationID;
			this.refreshToken = refreshToken;
			this.accessToken = string.Empty;
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


			try
			{
				Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
				TokenInfo info = serializer.Deserialize<TokenInfo>(new Newtonsoft.Json.JsonTextReader(new StringReader(client.DownloadString(this.BuildOAuthRefreshTokenUri(refreshToken)))));
				this.accessToken = info.Access_Token;
			}
			catch
			{
				//fix me, handle oauth errors somehow
			}
			if (s == null)
			{
				// first step
				Auth a = new Auth(doc);
				a.Mechanism = MechanismType.LIVEMESSENGER;
				a.InnerText = accessToken;
				return a;
			}
			else
			{
				Response r = new Response(doc);
				r.InnerText = accessToken;
				return r;
			}
        }
		private Uri BuildOAuthRefreshTokenUri(string refreshToken)
		{
			List<string> paramList = new List<string>();
			
			paramList.Add("client_id=" + Uri.EscapeDataString(applicationID));
			paramList.Add("grant_type=" + Uri.EscapeDataString("refresh_token"));
			paramList.Add("refresh_token=" + Uri.EscapeDataString(refreshToken));
			paramList.Add("redirect_uri=" + Uri.EscapeDataString(redirectUri));

			UriBuilder authorizeUri = new UriBuilder(oAuthTokenUri);
			authorizeUri.Query = String.Join("&", paramList.ToArray());
			return authorizeUri.Uri;
		}
    }
	public class TokenInfo
	{
		public string Access_Token
		{
			get;
			set;
		}
		public string Refresh_Token
		{
			get;
			set;
		}
		public int Expires_In
		{
			get;
			set;
		}
		public string Token_Type
		{
			get;
			set;
		}
		public string Scope
		{
			get;
			set;
		}

	}
}
