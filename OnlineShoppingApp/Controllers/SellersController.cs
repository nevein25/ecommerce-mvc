using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class SellersController : Controller
    {
        private ISellerRepo _sellerRepo;

        public SellersController(ISellerRepo sellerRepo)
        {
            _sellerRepo = sellerRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View(_sellerRepo.GetProfileData(User.GetUserId()));
        }

        [HttpPost]
        public IActionResult Profile(UpdateSellerProfileViewModel updateProfile)
        {
            if (ModelState.IsValid)
            {
                bool res = _sellerRepo.UpdateProfile(updateProfile, User.GetUserId());
                if (res)
                {
                    ViewBag.MessageSuccess = "Profile updated successfully.";
                    return RedirectToAction("Profile", "Sellers");
                }

            }

            ViewBag.MessageFail = "Failed to update profile.";

            return View(updateProfile);
        }
    }
}
