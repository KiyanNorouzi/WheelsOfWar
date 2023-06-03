using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine;


public class IMEI
{
    public static string GetDeviceID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }


    public static IPAddress GetExternalAddress()
    {
        //<html><head><title>Current IP Check</title></head><body>Current IP Address: 129.98.193.226</body></html>
        var html = new WebClient().DownloadString("http://checkip.dyndns.com/");

        var ipStart = html.IndexOf(": ", StringComparison.OrdinalIgnoreCase) + 2;
        return IPAddress.Parse(html.Substring(ipStart, html.IndexOf("</", ipStart, StringComparison.OrdinalIgnoreCase) - ipStart));
    }
}