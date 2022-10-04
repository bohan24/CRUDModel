using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CRUDModel.Services
{
    public class HttpRequestService
    {
        public async Task<dynamic> post(string url, object param,dynamic header=null)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                request.ContentType = "application/json";
                if(header!=null)
                {
                    foreach(var row in header)
                    {
                        request.Headers.Add(row.Key, row.Value);
                    }
                }
                byte[] bs = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(param));
                request.ContentLength = bs.Length;
                request.GetRequestStream().Write(bs, 0, bs.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string result = sr.ReadToEnd();
                Console.WriteLine("debug:" + result);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        public async Task<dynamic> get(string url,dynamic header=null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            if(header!=null)
            {
                foreach(var row in header)
                {
                    request.Headers.Add(row.Key, row.Value);
                }
            }
            //request.ContentType = "application/json";
            //byte[] bs = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(param));
            //request.ContentLength = bs.Length;
            //request.GetRequestStream().Write(bs, 0, bs.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string result = sr.ReadToEnd();
            Console.WriteLine("debug:" + result);
            return result;

        }

    }
}
