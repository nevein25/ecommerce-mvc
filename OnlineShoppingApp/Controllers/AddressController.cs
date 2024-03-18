using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
	public class AddressController : Controller
	{
		private readonly IAddressRepo _addressRepo;

		public AddressController(IAddressRepo addressRepo)
        {
			_addressRepo = addressRepo;
		}
        // GET: AddressController
        public ActionResult Index()
		{
			var buyerAddress = _addressRepo.GetAddressByBuyerId(UserHelper.LoggedinUserId);
			var addressList = buyerAddress.Select(Ad => new SelectListItem
			{
				Value = Ad.Id.ToString(),
				Text = $"{Ad.BuildingNumber} - {Ad.Street} - {Ad.City} - {Ad.Country}"
			}).ToList();
			ViewBag.AddressList = new SelectList( addressList, "Value", "Text");
			ViewBag.Buyer = _addressRepo.GetUser(UserHelper.LoggedinUserId);
			return View();
		}


		//public ActionResult AddNewAddress([Bind("BuildingNumber, Street, City, Country, IsMain, BuyerId")] Address address)
		//{
		//	//Address address = new Address(){ BuildingNumber = BuildingNumber, Street = Street, Country = Country, City = City };
		//	if(address != null)
		//	{
		//		_addressRepo.Insert(address);
		//	}
		//	return RedirectToAction(nameof(Index));
		//}
        //public ActionResult AddNewAddress(Address address)
        //{
        //    //Address address = new Address(){ BuildingNumber = BuildingNumber, Street = Street, Country = Country, City = City };
        //    if (address != null)
        //    {
        //        _addressRepo.Insert(address);
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        public ActionResult AddNewAddress(AddAddressViewModel viewModel)
        {
            if (viewModel != null)
            {
                // Create a new Address object and map properties from the view model
                Address address = new Address
                {
                    BuildingNumber = viewModel.BuildingNumber,
                    Street = viewModel.Street,
                    City = viewModel.City,
                    Country = viewModel.Country,
                    IsMain = viewModel.IsMain,
                    BuyerId = viewModel.BuyerId
                };

                _addressRepo.Insert(address);
            }
            Address lastAdrs = _addressRepo.GetAddresses().Last();

            return RedirectToAction(nameof(CheckoutStaticData),lastAdrs);
        }

        public ActionResult GetExistingAddressId(int id)
        {
            Address extAdrs = _addressRepo.GetAddressById(id);
            return RedirectToAction(nameof(CheckoutStaticData), extAdrs);

        }
        public ActionResult CheckoutStaticData(Address address)
        {
			ViewBag.Buyer = _addressRepo.GetUser(UserHelper.LoggedinUserId);
			return View(address);
        }
    }
}
