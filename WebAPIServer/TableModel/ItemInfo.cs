namespace WebAPIServer.TableModel;

public class ItemInfo
{
    public ItemInfo() { ItemCode = -1; Count = -1; }
    public ItemInfo(Int16 code, Int16 count) { this.ItemCode = code; this.Count = count; }
    public Int16 ItemCode;
    public Int16 Count;
}
