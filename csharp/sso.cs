using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace FluidReview {
    public class AuthResponse {
        public string token { get; set; }
    }

    public class SyncResponse {
        public string token { get; set; }
        public string return_path { get; set; }
    }

    class SSO {
        static void Main(string[] args) {
            string fluidreview_url = "http://[YOUR SUBDOMAIN].fluidreview.com"; // Your FluidReview URL
            string signin_path = "/api/signin/";
            string auth_path = "/api/authenticate/";

            string signin_url = fluidreview_url + signin_path;
            string auth_url = fluidreview_url + auth_path;

            string admin_email = "you@example.com"; // Your site's owner's email address
            string admin_password = "password"; // Your site's owner's password

            string token;
            string syncURL;

            /* First, authenticate with the API. This will return a rolling token
            that should be included with all subsequent requests.

            API authentication details can be found at:
            https://test01.fluidreview.com/api/docs/#authentication
             */
    
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(auth_url);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";

            using (var writer = new StreamWriter(request.GetRequestStream())) {
                writer.Write("email=" + admin_email + "&" + "password=" + admin_password);
            }

            // Send our credentials via HTTP POST, this will authenticate with the FluidReview API.

            using (WebResponse response = request.GetResponse()) {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AuthResponse));
                AuthResponse serialized = (AuthResponse)serializer.ReadObject(response.GetResponseStream());
                token = serialized.token;
            }

            HttpWebRequest syncRequest = (HttpWebRequest)WebRequest.Create(signin_url);

            syncRequest.Method = "POST";
            syncRequest.ContentType = "application/x-www-form-urlencoded";
            syncRequest.Accept = "application/json";

            using (var writer = new StreamWriter(syncRequest.GetRequestStream())) {
                /* The following values are required by the `sign in` API method.
                The most important field being `email`, which will govern what
                user this is. */
                writer.Write("email=applicant@example.com&group=2835&first_name=John&last_name=Doe&token=" + token);
            }

            /* Send the information about our user to the FluidReview API's sign-in method.
            If a user with the given email exists, a one-time-login URL will be returned.

            If a user account with the given email does not exist, a new account will be created.

            Both scenarios will return the same response. */

            using (WebResponse response = syncRequest.GetResponse()) {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SyncResponse));
                SyncResponse serialized = (SyncResponse)serializer.ReadObject(response.GetResponseStream());
                token = serialized.token;
                syncURL = serialized.return_path;
            }

            Console.WriteLine(syncURL);

            Thread.Sleep(4000);
        }
    }
}
