using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface ISellerRepo
    {
        public UpdateSellerProfileViewModel GetProfileData(int sellerId);
        public GetSellerProfileViewModel GetProfileDataAsViewer(int sellerId);
        public bool UpdateProfile(UpdateSellerProfileViewModel oldData, int sellerId);
    }
}
