using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPIServer.Services;
public interface IItemDB
{
    public Task<ErrorCode> InsertItem(string id, Int16 itemCode);
}
