using Omnicx.API.SDK.Api.Commerce;
using Omnicx.WebStore.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omnicx.WebStore.Core.Controllers
{
    public class StoreController : BaseController
    {
        private readonly IStoreApi _storeRepository;
        public StoreController(IStoreApi storeRepository)
        {
            _storeRepository = storeRepository;
        }
        public ActionResult StoreStockAvailability(string stockCode,string postCode)
        {
            var result = _storeRepository.CheckStoreStockAvailability(stockCode, postCode);
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }
    }
}