using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;
using System.Web;

namespace FlickrResearcherLibrary
{
    public class FlickrManager
    {
        private static Flickr _flickr;

        // Insert your keys and enjoy the simple app :)

        /// <summary>
        /// Own ApiKey and Secret. You can get them on Flickr.com
        /// </summary>
        public static string ApiKey = "";
        public static string Secret = "";

        /// <summary>
        /// Connect to service Flickr by ApiKey and Secret
        /// </summary>
        /// <returns></returns>
        public static Flickr GetInstance()
        {
            return new Flickr(ApiKey, Secret);
        }

        /// <summary>
        /// GetAuthInstance from service 
        /// </summary>
        /// <returns></returns>
        public static Flickr GetAuthInstance()
        {
            _flickr = new Flickr(ApiKey, Secret);

            if (OAuthToken != null)
            {
                _flickr.OAuthAccessToken = OAuthToken.Token;
                _flickr.OAuthAccessTokenSecret = OAuthToken.TokenSecret;

            }
            return _flickr;
        }

        /// <summary>
        /// Get token
        /// </summary>
        public static OAuthAccessToken OAuthToken
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["OAuthToken"] == null)
                    return null;

                var values = HttpContext.Current.Request.Cookies["OAuthToken"].Values;
                return new OAuthAccessToken()
                {
                    FullName = values["FullName"],
                    Token = values["Token"],
                    TokenSecret = values["TokenSecret"],
                    UserId = values["UserId"],
                    Username = values["Username"]
                };
            }
            set
            {
                var cookie = new HttpCookie("OAuthToken") { Expires = DateTime.UtcNow.AddHours(1) };

                cookie.Values["FullName"] = value.FullName;
                cookie.Values["Token"] = value.Token;
                cookie.Values["TokenSecret"] = value.TokenSecret;
                cookie.Values["UserId"] = value.UserId;
                cookie.Values["Username"] = value.Username;

                HttpContext.Current.Response.AppendCookie(cookie);

            }
        }
    }
}
