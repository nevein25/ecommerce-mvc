using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class SellerRepo : ISellerRepo
    {

        private ShoppingContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public SellerRepo(ShoppingContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public UpdateSellerProfileViewModel GetProfileData(int sellerId)
        {
            Seller seller = _context.Sellers.Where(b => b.Id == sellerId).FirstOrDefault();
            UpdateSellerProfileViewModel sellerProfile = null;
            if (seller != null)
            {
                sellerProfile = new()
                {
                    PhoneNumber = seller.PhoneNumber,
                    Paper = seller.Paper,
                    Description = seller.Description,
                    BusinessName  = seller.BusinessName,
                    VAT = seller.VAT,
                    Image = seller.Image,
                };

            }
            return sellerProfile;
        }

        public bool UpdateProfile(UpdateSellerProfileViewModel oldData, int sellerId)
        {
            Seller seller = _context.Sellers.Where(b => b.Id == sellerId).FirstOrDefault();

            if (seller != null)
            {

                seller.PhoneNumber = oldData.PhoneNumberDb;
                seller.Paper = oldData.Paper;
                seller.Description = oldData.Description;
                seller.BusinessName = oldData.BusinessName;
                seller.VAT = oldData.VAT;
                seller.Image = seller.Image;

                if (oldData.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImage");
                    string fileNameWithExtention = $"{sellerId}{Path.GetExtension(oldData.ImageFile.FileName)}";

                    string filePath = Path.Combine(uploadsFolder, fileNameWithExtention);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        oldData.ImageFile.CopyTo(fileStream);
                    }

                    seller.Image = fileNameWithExtention;
                }

                if (oldData.PaperFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "PaperImage");
                    string fileNameWithExtention = $"{sellerId}{Path.GetExtension(oldData.PaperFile.FileName)}";

                    string filePath = Path.Combine(uploadsFolder, fileNameWithExtention);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        oldData.PaperFile.CopyTo(fileStream);
                    }

                    seller.Paper = fileNameWithExtention;
                }

            }
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
