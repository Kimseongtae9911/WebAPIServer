using Microsoft.AspNetCore.Mvc;
using WebAPIServer.ModelReqRes;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    readonly IItemDB _itemDB;

    public ItemController(IItemDB itemDB)
    {
        _itemDB = itemDB;
    }

    [HttpPost]
    public async Task<InsertItemResponse> InsertItem(InsertItemRequest request)
    {
        var response = new InsertItemResponse();

        var errorCode = await _itemDB.InsertItem(request.ID, request.ItemCode);

        response.Result = errorCode;
        return response;
    }
}
