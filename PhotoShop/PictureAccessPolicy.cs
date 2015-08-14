namespace PhotoShop
{
    public class PictureAccessPolicy : IAccessPolicy
    {
        private readonly Picture _picture;

        public PictureAccessPolicy(Picture picture)
        {
            _picture = picture;
        }

        public void BeforeAccess()
        {
            _picture.Lock();
        }

        public void AfterAccess()
        {
            _picture.Unlock();
        }
    }
}