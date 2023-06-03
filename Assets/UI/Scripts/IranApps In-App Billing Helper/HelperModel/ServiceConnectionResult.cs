using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
public class ServiceConnectionResult : MonoBehaviour
{
    private int _status;
    private InAppError _error;

    private bool _isSuccess;

    public const int StatusOk = 0;
    public const int StatusError = 1;
    public const int StatusConnectionLost = 2;
    
    public static ServiceConnectionResult FromJson(string json)
    {
        var result = new ServiceConnectionResult();
        var o = JSON.Parse(json);
        result._status = o["status"].AsInt;
        result._isSuccess = result._status == 0;

        if (result._status==1)
        {
            result._error = InAppError.Create(o["error"]);
        }
        return result;
    }

    /// <summary>
    /// Gets the status of the connecting to IranApps in-app billing services.
    /// </summary>
    /// <returns><see cref="StatusOk"/> when connected to IranApps Service. <see cref="StatusError"/> when connecting to service creates an error. <see cref="StatusConnectionLost"/> when connection to IranApps in-app billing service is lost.</returns>
    public int GetStatus()
    {
        return _status;
    }

    /// <summary>
    /// When <see cref="GetStatus"/> returns <see cref="StatusError"/>, there is an error happened. you can get the error by calling this method. <seealso cref="InAppError"/>
    /// </summary>
    /// <returns>The error that happened when connecting to IranApps in-app billing service</returns>
    public InAppError GetError()
    {
        return _error;
    }

    /// <summary>
    /// If true, the connection to IranApps in-app billing services are successful and you can do the operations of purchasing and cosuming normally.
    /// </summary>
    /// <returns>If true, the connection to IranApps in-app billing services are successful and you can do the operations of purchasing and cosuming normally.</returns>
    public bool IsSuccess()
    {
        return _isSuccess;
    }
}
}

