namespace WebAPIServer.Game;

public class Item
{
    Int16 _code { get; set; } = 0;
    string _name { get;set; } = string.Empty;
    Int16 _attribute { get; set; } = 0;
    Int16 _buy {get; set; } = 0;
    Int16 _sell { get; set; } = 0;
    Int16 UseLv { get; set; } = 0;
    Int16 _attack { get; set; } = 0;
    Int16 _defence { get; set; } = 0;
    Int16 _magic { get; set; } = 0;
    Int16 _enhanceMaxCount { get; set; } = 0;

}
