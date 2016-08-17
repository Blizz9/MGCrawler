using System;
using System.Text;
using SeasideResearch.LibCurlNet;

namespace MGCrawler
{
    internal static class CURLWrapper
    {
        private static Easy _site;
        private static string _pageContents;
        private static bool _receiveFlag;

        internal static string ReadMGURL(string url)
        {
            Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

            _site = new Easy();
            _pageContents = String.Empty;
            _receiveFlag = false;

            Easy.WriteFunction writeFunction = new Easy.WriteFunction(receiveData);
            _site.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, writeFunction);
            _site.SetOpt(CURLoption.CURLOPT_USERAGENT, "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");
            _site.SetOpt(CURLoption.CURLOPT_FOLLOWLOCATION, true);
            _site.SetOpt(CURLoption.CURLOPT_URL, url);

            //if (!string.IsNullOrWhiteSpace(proxy))
            //    _site.SetOpt(CURLoption.CURLOPT_PROXY, proxy);

            //if (!string.IsNullOrWhiteSpace(postData))
            //{
            //    _site.SetOpt(CURLoption.CURLOPT_POST, true);
            //    _site.SetOpt(CURLoption.CURLOPT_POSTFIELDS, postData);
            //}

            _site.Perform();
            while (_receiveFlag == false) { }

            _site.Cleanup();
            Curl.GlobalCleanup();

            return (_pageContents);
        }

        private static int receiveData(byte[] buffer, int count, int size, object extraData)
        {
            _pageContents += Encoding.UTF8.GetString(buffer);
            _receiveFlag = true;

            return (size * count);
        }
    }
}
