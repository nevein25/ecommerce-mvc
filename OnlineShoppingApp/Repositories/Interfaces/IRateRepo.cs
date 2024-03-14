namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IRateRepo
    {
        public bool ProductExist(int productId);
        public void Rate(int ProductId, int UserId, int NumOfStars);
    }
}
