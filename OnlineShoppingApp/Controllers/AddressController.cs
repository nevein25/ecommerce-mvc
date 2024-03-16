using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

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
			ViewBag.Buyer = _addressRepo.GetUser(UserHelper.LoggedinUserId);
			return View();
		}

		// GET: AddressController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: AddressController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: AddressController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: AddressController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: AddressController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: AddressController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: AddressController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		public ActionResult AddNewAddress(Address address)
		{
			if(address != null)
			{
				_addressRepo.Insert(address);
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
