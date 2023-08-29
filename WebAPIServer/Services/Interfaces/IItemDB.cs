using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIServer.TableModel;

namespace WebAPIServer.Services.Interfaces;
public interface IItemDB
{
    public Task<ErrorCode> InsertItem(string id, short itemCode);
    public Task<(ErrorCode, List<ItemInfo>)> LoadItem(string id);
}
