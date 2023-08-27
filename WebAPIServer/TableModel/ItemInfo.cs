namespace WebAPIServer.TableModel;

public class ItemInfo
{
    public ItemInfo(Int16 code, Int16 count) { this.ItemCode = code; this.ItemCount = count; }
    public Int16 ItemCode;
    public Int16 ItemCount;
}
