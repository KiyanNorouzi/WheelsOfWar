public class PurchasesData
{
    private string orderId;
    private string packageName;
    private string productId;
    private long purchaseTime;
    private int purchaseState;
    private string developerPayload;
    private string purchaseToken;

    //==========================================

    public string OrderId
    {
        get
        {
            return orderId;
        }
        set
        {
            orderId = value;
        }
    }

    public string PackageName
    {
        get
        {
            return packageName;
        }
        set
        {
            packageName = value;
        }
    }

    public string ProductId
    {
        get
        {
            return productId;
        }
        set
        {
            productId = value;
        }
    }

    public long PurchaseTime
    {
        get
        {
            return purchaseTime;
        }
        set
        {
            purchaseTime = value;
        }
    }

    public int PurchaseState
    {
        get
        {
            return purchaseState;
        }
        set
        {
            purchaseState = value;
        }
    }

    public string DeveloperPayload
    {
        get
        {
            return developerPayload;
        }
        set
        {
            developerPayload = value;
        }
    }

    public string PurchaseToken
    {
        get
        {
            return purchaseToken;
        }
        set
        {
            purchaseToken = value;
        }
    }

}
