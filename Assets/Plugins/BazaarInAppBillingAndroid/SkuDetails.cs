public class SkuDetails
{

    private string productId;
    private string price;
    private string title;
    private string description;

    //==========================================

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

    public string Price
    {
        get
        {
            return price;
        }
        set
        {
            price = value;
        }
    }

    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }
}
