using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsRepo commRepo;
        private readonly IProductRepo productRepo;

        public CommentsController(ICommentsRepo _commRepo, IProductRepo productRepo)
        {
            commRepo = _commRepo;
            this.productRepo = productRepo;
        }

        //public IActionResult GetAll(int prodId)
        //{
        //    if (prodId == 0)
        //    {
        //        return NotFound("No Product exists");
        //    }

        //    var comments = commRepo.GetAllComments(prodId);
        //    var product = productRepo.GetById(prodId); // Assuming a method to get product by ID exists in your repository

        //    if (product == null)
        //    {
        //        return NotFound("Product not found");
        //    }

        //    var prodModelView = new ProductViewModel
        //    {
        //        Comments = comments,
        //        Products = new List<Product> { product }
        //    };

        //    return PartialView(prodModelView);
        //}

        //[HttpPost]
        //public IActionResult InsertComment(string review,int prodID)
        //{
            

        //    var comment = new Comment { Text = review, Date = DateTime.Now,AppUserId= UserHelper.LoggedinUserId /*AppUser = commRepo.GetAppUser(1)*/, ProductId = prodID };

        //    commRepo.AddComment(comment);
        //    return PartialView("~/Views/Product/CommentsPartialView.cshtml", 
        //        new ProductViewModel
        //        {
        //            Products = new List<Product> { productRepo.GetById(prodID) },
        //            Comments = new List<Comment> { comment },
        //        });
        //}
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult EditComment(Comment newComm)
        //{

        //    Comment comm = commRepo.GetComment(newComm.Id);

        //    return PartialView("~/Views/Product/CommentsPartialView.cshtml",
        //       new ProductViewModel
        //       {
        //           Products = new List<Product> { productRepo.GetById(prodID) },
        //           Comments = new List<Comment> { comm },
        //       });

        //    return View();
        //}
    }
}
