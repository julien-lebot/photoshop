using AcensiPhotoShop;
using NUnit.Framework;

namespace AcensiPhotoshop.Tests
{
    [TestFixture]
    public class PictureTests
    {
        [Test]
        public void Should_Have_Sensible_Defaults()
        {
            var picture = new Picture(Color.White);

            Assert.That(picture.Width, Is.EqualTo(1));
            Assert.That(picture.Height, Is.EqualTo(1));
            Assert.That(picture[0, 0], Is.EqualTo(Color.White));
        }
        
    }
}
