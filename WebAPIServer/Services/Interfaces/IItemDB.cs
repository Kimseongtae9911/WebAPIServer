using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPIServer.Services.Interfaces;
public interface IItemDB
{
    public Task<ErrorCode> InsertItem(string id, short itemCode);
    public Task<Tuple<ErrorCode, List<Tuple<short, short>>>> LoadItem(string id);
}
