using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
    //BILLING_RESPONSE_RESULT_USER_CANCELED("The process was canceled by the user", 1),  
    //BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE("Your request is not supported by this version of the API", 3),  
    //BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE("The requested product is not registered in the system", 4),  
    //BILLING_RESPONSE_RESULT_DEVELOPER_ERROR("Developer errors can occur in the following circumstances :\n\nSending invalid parameters to methods\nApplication not registered in Iranapps\nNot including permissions in the Manifest file", 5),
    //BILLING_RESPONSE_RESULT_ERROR("operation failed.", 6), 
    //BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNED("occurs when the product has already benn purchased(to buy it again you must consume it first).", 7), 
    //BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED("User is not the owner of the product so it cannot be consumed.", 8),  
    //BILLING_RESPONSE_USER_NOT_LOGIN("User has not logged in to Iranapps.", 9),  
    //BILLING_RESPONSE_IRANAPPS_NOT_AVAILABLE("IranApps app is not installed", 10), 
    //LOCAL_CANT_CONNECT_TO_IAB_SERVICE("Helper can't connect to the in-app billing service", 11), 
    //LOCAL_HELPER_NOT_CONNECTED_TO_SERVICE("helper isn't connected to the service", 12), 
    //LOCAL_EXCEPTION("any error or exception that happens inside the helper", 13);



    /// <summary>
    /// Representing an API error. 
    /// Errors Are:<br></br>
    ///   1-  BILLING_RESPONSE_RESULT_USER_CANCELED (code 1): means that the process is canceled by the user.<br></br><br></br>
    ///   2-  BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE (code 3): means that the request is not supported by this version of the API.<br></br><br></br>
    ///   3-  BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE (code 4): means that the requested product is not registered in the system.<br></br><br></br>
    ///   4-  BILLING_RESPONSE_RESULT_DEVELOPER_ERROR (code 5): means that a Developer error occured. Developer error are translated as:<br></br>
    ///         4.a- Invalid parameters are sent to the API and In-App Billing provider server. for example invalid product ID or etc.<br></br>
    ///         4.b- Application is not registered in IranApps.<br></br>
    ///         4.c- IranApps billing permission is not enetered in the AndroidManifest.xml<br></br><br></br>
    ///   5-  BILLING_RESPONSE_RESULT_ERROR (code 6): means that the operation is failed    <br></br> <br></br>
    ///   6-  BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNED (code 7): means that the item is already owned by the current user logged in in IranApps. ( The item must be consumed first ).<br></br><br></br>
    ///   7-  BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED (code 8): means that the current logged in user in IranApps does not own the product to consume it.<br></br><br></br>
    ///   8-  BILLING_RESPONSE_USER_NOT_LOGIN (code 9): means that there is no user logged in in IranApps to do the operation.<br></br><br></br>
    ///   9-  BILLING_RESPONSE_IRANAPPS_NOT_AVAILABLE (code 10): means that IranApps is not installed on the device.<br></br><br></br>
    ///   10- LOCAL_CANT_CONNECT_TO_IAB_SERVICE (code 11): means that the helper API could not connect to IranApps In-App Billing service.<br></br><br></br>
    ///   11- LOCAL_HELPER_NOT_CONNECTED_TO_SERVICE (code 12): means that the helper is not already connected to In-App Billing service.<br></br><br></br>
    ///   12- LOCAL_EXCEPTION (code 13): an error happened inside the helper that prevents the operation to finish.<br></br><br></br>
    /// </summary>
    public class InAppError
    {
        private const string BillingResponseResultUserCanceled = "BILLING_RESPONSE_RESULT_USER_CANCELED";
        private const string BillingResponseResultBillingUnavailable = "BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE";
        private const string BillingResponseResultItemUnavailable = "BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE";
        private const string BillingResponseResultDeveloperError = "BILLING_RESPONSE_RESULT_DEVELOPER_ERROR";
        private const string BillingResponseResultError = "BILLING_RESPONSE_RESULT_ERROR";
        private const string BillingResponseResultItemAlreadyOwned = "BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNED";
        private const string BillingResponseResultItemNotOwned = "BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED";
        private const string BillingResponseUserNotLogin = "BILLING_RESPONSE_USER_NOT_LOGIN";
        private const string BillingResponseIranappsNotAvailable = "BILLING_RESPONSE_IRANAPPS_NOT_AVAILABLE";
        private const string LocalCantConnectToIabService = "LOCAL_CANT_CONNECT_TO_IAB_SERVICE";
        private const string LocalHelperNotConnectedToService = "LOCAL_HELPER_NOT_CONNECTED_TO_SERVICE";
        private const string LocalException = "LOCAL_EXCEPTION";


        private readonly string _error;
        private readonly int _code;

        private InAppError(string errorString)
        {
            this._error = errorString;
            if (_error.Equals(BillingResponseResultUserCanceled))
            {
                this._code = 1;
            }
            else if (_error.Equals(BillingResponseResultBillingUnavailable))
            {
                this._code = 3;
            }
            else if (_error.Equals(BillingResponseResultItemUnavailable))
            {
                this._code = 4;
            }
            else if (_error.Equals(BillingResponseResultDeveloperError))
            {
                this._code = 5;
            }
            else if (_error.Equals(BillingResponseResultError))
            {
                this._code = 6;
            }
            else if (_error.Equals(BillingResponseResultItemAlreadyOwned))
            {
                this._code = 7;
            }
            else if (_error.Equals(BillingResponseResultItemNotOwned))
            {
                this._code = 8;
            }
            else if (_error.Equals(BillingResponseUserNotLogin))
            {
                this._code = 9;
            }
            else if (_error.Equals(BillingResponseIranappsNotAvailable))
            {
                this._code = 10;
            }
            else if (_error.Equals(LocalCantConnectToIabService))
            {
                this._code = 11;
            }
            else if (_error.Equals(LocalHelperNotConnectedToService))
            {
                this._code = 12;
            }
            else if (_error.Equals(LocalException))
            {
                this._code = 13;
            }
        }

        public static InAppError Create(string errorString)
        {
            var error = new InAppError(errorString);
            return error;
        }

        public string GetErrorName()
        {
            return _error;
        }

        public int GetErrorCode()
        {
            return _code;
        }
    }
}


