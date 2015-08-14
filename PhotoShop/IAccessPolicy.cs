namespace PhotoShop
{
    public interface IAccessPolicy
    {
        void BeforeAccess();
        void AfterAccess();
    }
}