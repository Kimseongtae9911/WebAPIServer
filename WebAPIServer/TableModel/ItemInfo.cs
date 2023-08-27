namespace WebAPIServer.TableModel;

public class ItemInfo
{
    public ItemInfo(Int16 type, Int16 detail) { this.ItemType = type; this.ItemDetail = detail; }
    public Int16 ItemType;
    public Int16 ItemDetail;
}
