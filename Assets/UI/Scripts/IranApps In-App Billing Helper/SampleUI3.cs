using UnityEngine;

public class SampleUI3 : MonoBehaviour 
{

    int _row = 50;
    int _column = 90;

    GUIText _text;

    private static SampleUI3 _instance;
    private IranAppsInAppBillingHelper _iranAppsHelper;
    private string _developerPayload = "ir.tgbs.iranapps.inappbilling.sample";
    private string _productId = "product_1";
    private string _consumable = "1";
    private string _purchaseToken = "purchaseToken";
    private string _skusCsv = "product_0,product_1,product_2";

    void Start()
    {
        _text = GameObject.Find("Text").GetComponent<GUIText>();
        //_iranAppsHelper = IranAppsInAppBillingHelper.Instance;
        _iranAppsHelper = GameObject.Find("IranAppsIabObject").GetComponent<IranAppsInAppBillingHelper>();
        _row = Screen.height / 16;
        _column = Screen.width / 5;
        _text.transform.position = Camera.main.ScreenToViewportPoint(new Vector3(0, 9 * _row, 0));
        _text.text = "Debug Text will Appear here.";
        _text.fontSize = 17;
        _instance = this;
    }

    void OnGUI()
    {

        if (GUI.Button(new Rect((float) (0 * _column), 0 * _row, (float) (1.5*_column), _row), "Setup IAB"))
        {
            _iranAppsHelper.SetupInAppBilling();
        }

        if (GUI.Button(new Rect((float) (1.5 * _column), 0 * _row, (float) (1.5*_column), _row), "Dispose"))
        {
            _iranAppsHelper.Dispose();
        }


        if (GUI.Button(new Rect(0 * _column, (float) (1.5 * _row), (float) (1.5*_column), _row), "Buy Product"))
        {
            _iranAppsHelper.BuyProduct(_productId, _developerPayload, int.Parse(_consumable) > 0);
        }
        _developerPayload = GUI.TextArea(new Rect((float)(1.5 * _column), (float)(1.5 * _row), (float)(1.5 * _column), _row), _developerPayload);
        _productId = GUI.TextArea(new Rect((float)(3 * _column), (float)(1.5 * _row), (float)(1.5 * _column), _row), _productId);
        _consumable = GUI.TextArea(new Rect((float)(4.5 * _column), (float)(1.5 * _row), (float)(0.5 * _column), _row), _consumable);

        if (GUI.Button(new Rect(0 * _column, 3 * _row, (float)(1.5 * _column), _row), "Consume Product"))
        {
            _iranAppsHelper.ConsumeProduct(_purchaseToken);
        }
        _purchaseToken = GUI.TextArea(new Rect((float)(1.5 * _column), 3 * _row, (float)(3.5 * _column), _row), _purchaseToken);

        if (GUI.Button(new Rect(0 * _column, (float) (4.5 * _row), (float)(1.5 * _column), _row), "Get Purchases"))
        {
            _iranAppsHelper.GetPurchases();
        }

        if (GUI.Button(new Rect((float) (1.5 * _column), (float)(4.5 * _row), (float)(1.5 * _column), _row), "Get SKU Details"))
        {
            _iranAppsHelper.GetSkusDetails(_skusCsv);
        }

        _skusCsv = GUI.TextArea(new Rect((float)(3 * _column), (float)(4.5 * _row), (float)(1.5 * _column), _row), _skusCsv);

        if (GUI.Button(new Rect((float)(0 * _column), (float)(6 * _row), (float)(1.5 * _column), _row), "Login User"))
        {
            _iranAppsHelper.LoginUser();
        }

        if (GUI.Button(new Rect((float)(1.5 * _column), (float)(6 * _row), (float)(2 * _column), _row), "Is User Logged In"))
        {
            int result = _iranAppsHelper.IsUserLoggedIn();

            if (result == 0)
            {
                PrintText("User Is Logged In");
            }
            else if ( result == 2)
            {
                PrintText("User Is Not Logged In");
            }
            else
            {
                PrintText("API is not instantiated.");
            }
        }
    }

    public static void PrintText(string what)
    {
        _instance._text.text = what;
    }
}
