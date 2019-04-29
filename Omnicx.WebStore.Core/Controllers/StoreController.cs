using Omnicx.API.SDK.Api.Commerce;
using Omnicx.WebStore.Core.Controllers;
using Omnicx.WebStore.Models.Keys;
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
        public ActionResult FindNearestStore(string postCode)
        {
            var result = _storeRepository.FindNearestStore(postCode);
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int id)
        {
            var result = _storeRepository.StoreDetail(id);
            if(result.Result == null)
                return RedirectToAction("StoreLocator", "Common");
            return View(CustomViews.STORE_DETAIL, result.Result);
        }
    }
}