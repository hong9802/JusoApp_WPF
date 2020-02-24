using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JusoApp_WPF
{
    class RequestHandler
    {
        private string resdata;
        public string[] jusoarray = new string[5];
        public string[] jusobdMgtSn = new string[5];
        public string[] jusolndn = new string[5];
        public string KorNM;
        public string telCn;

        public string getResdata()
        {
            return this.resdata;
        }
        public void send_RequestAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        this.resdata = sr.ReadToEnd();
                    }
                    if (url.Contains("getAddrJuminDetailRenew")) getCn();
                    else convertJuso();
                }
            } catch (Exception e)
            {
                resdata = "Error : " + e.Message;
            }
        }

        private void convertJuso()
        {
            string tmpdata = resdata;
            resdata = tmpdata.Substring(resdata.IndexOf("<ol>"), resdata.LastIndexOf("</ol>"));
            resdata = resdata.Replace("\r", "");
            resdata = resdata.Replace("\n", "");
            resdata = resdata.Replace("\t", "");
            for (int i = 0; i < jusoarray.Length; i++)
            {
                if (!resdata.Contains("rnAddr" + (i + 1).ToString()))
                    break;
                tmpdata = resdata.Substring(resdata.IndexOf("id=\"rnAddr" + (i + 1).ToString()) + 20);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("id=\"lndnAddr" + (i + 1).ToString()) - 25);
                tmpdata = tmpdata.Replace("<b>", "");
                tmpdata = tmpdata.Replace("</b>", "");
                jusoarray[i] = tmpdata;
                tmpdata = resdata.Substring(resdata.IndexOf("id=\"lndnAddr" + (i + 1).ToString()) + 22);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("id=\"bdNm" + (i + 1).ToString()) - 25);
                tmpdata = tmpdata.Replace("<b>", "");
                tmpdata = tmpdata.Replace("</b>", "");
                jusolndn[i] = tmpdata;
                tmpdata = resdata.Substring(resdata.IndexOf("id=\"bdMgtSn" + (i + 1).ToString()) + 21);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("id=\"histRnAddr" + (i + 1).ToString()) - 24);
                tmpdata = tmpdata.Replace("<b>", "");
                tmpdata = tmpdata.Replace("</b>", "");
                jusobdMgtSn[i] = tmpdata;
                if (resdata.Contains("id=\"bdNm" + (i + 1).ToString() + "\" value=\"\""))
                {
                    continue;
                }
                tmpdata = resdata.Substring(resdata.IndexOf("id=\"bdNm" + (i + 1).ToString()) + 18);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("id=\"bsiZonNo" + (i + 1).ToString()) - 25);
                tmpdata = tmpdata.Replace("<b>", "");
                tmpdata = tmpdata.Replace("</b>", "");
                jusoarray[i] += " " + tmpdata;
            }
        }

        private void getCn()
        {
            string tmpdata = resdata;
            if (!resdata.Contains("ctpKorNm"))
            {
                KorNM = "None"; telCn = "None";
            }
            else
            {
                tmpdata = resdata.Substring(resdata.IndexOf("<ctpKorNm>") + 10);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("</"));
                KorNM = tmpdata;
                tmpdata = resdata.Substring(resdata.IndexOf("<sigKorNm>") + 10);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("</"));
                KorNM += " " + tmpdata;
                tmpdata = resdata.Substring(resdata.IndexOf("<emdKorNm>") + 10);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("</"));
                KorNM += " " + tmpdata;
                tmpdata = resdata.Substring(resdata.IndexOf("<telCn>") + 7);
                tmpdata = tmpdata.Substring(0, tmpdata.IndexOf("</"));
                telCn = tmpdata;
            }
        }
    }
}
