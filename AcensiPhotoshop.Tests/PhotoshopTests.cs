using System;
using AcensiPhotoShop;
using NUnit.Framework;

namespace AcensiPhotoshop.Tests
{
    [TestFixture]
    public class PhotoshopTests
    {
        [Test]
        public void Should_Rotate()
        {
            var sut = new ReversiblePicture(
                new Picture(Color.White, 2, 3)
                {
                    [0, 0] = Color.Black,
                    [1, 1] = Color.Black,
                    [0, 2] = Color.Black
                });

            sut.Rotate(-90).Apply();

            Assert.That(sut.Document.Width, Is.EqualTo(3));
            Assert.That(sut.Document.Height, Is.EqualTo(2));
            Assert.That(sut.Document[0, 1], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[1, 0], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[2, 1], Is.EqualTo(Color.Black));
        }

        [Test]
        public void Should_Invert_Colors()
        {
            var sut = new ReversiblePicture(
                new Picture(Color.White, 2, 3)
                {
                    [0, 0] = Color.Black,
                    [1, 1] = Color.Black,
                    [0, 2] = Color.Black
                });

            sut.Invert().Apply();
            
            Assert.That(sut.Document[0, 0], Is.EqualTo(Color.White));
            Assert.That(sut.Document[0, 1], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[0, 2], Is.EqualTo(Color.White));
            Assert.That(sut.Document[1, 0], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[1, 1], Is.EqualTo(Color.White));
            Assert.That(sut.Document[1, 2], Is.EqualTo(Color.Black));
        }

        [Test]
        public void Should_Fill_With_Color()
        {
            var sut = new ReversiblePicture(new Picture(Color.White, 4, 4));

            for (int i = 0; i < sut.Document.Width; ++i)
            {
                sut.Document[i, 0] = Color.Black;
            }
            for (int i = 0; i < sut.Document.Width; ++i)
            {
                sut.Document[i, sut.Document.Height - 1] = Color.Black;
            }
            for (int i = 0; i < sut.Document.Height; ++i)
            {
                sut.Document[0, i] = Color.Black;
            }
            for (int i = 0; i < sut.Document.Height; ++i)
            {
                sut.Document[sut.Document.Width - 1, i] = Color.Black;
            }

            sut.Fill(1, 2, sut.Document[1, 2], Color.Green).Apply();

            for (int i = 0; i < sut.Document.Width; ++i)
            {
                Assert.That(sut.Document[i, 0], Is.EqualTo(Color.Black));
            }
            for (int i = 0; i < sut.Document.Width; ++i)
            {
                Assert.That(sut.Document[i, sut.Document.Height - 1], Is.EqualTo(Color.Black));
            }
            for (int i = 0; i < sut.Document.Height; ++i)
            {
                Assert.That(sut.Document[0, i], Is.EqualTo(Color.Black));
            }
            for (int i = 0; i < sut.Document.Height; ++i)
            {
                Assert.That(sut.Document[sut.Document.Width - 1, i], Is.EqualTo(Color.Black));
            }

            for (int i = 1; i < sut.Document.Width - 2; ++i)
            {
                for (int j = 1; j < sut.Document.Height - 2; ++j)
                {
                    Assert.That(sut.Document[i, j], Is.EqualTo(Color.Green));
                }
            }
            
        }

        [Test]
        public void Shoud_Apply_Many_Commands()
        {
            var sut = new ReversiblePicture(
                new Picture(Color.White, 2, 3)
                {
                    [0, 0] = Color.Black,
                    [1, 1] = Color.Black,
                    [0, 2] = Color.Black
                });

            sut.Rotate(-90).Invert().Apply();

            Assert.That(sut.Document.Width, Is.EqualTo(3));
            Assert.That(sut.Document.Height, Is.EqualTo(2));
            Assert.That(sut.Document[0, 1], Is.EqualTo(Color.White));
            Assert.That(sut.Document[1, 0], Is.EqualTo(Color.White));
            Assert.That(sut.Document[2, 1], Is.EqualTo(Color.White));
        }

        [Test]
        public void Should_Undo_One_Command()
        {
            var sut = new ReversiblePicture(
                new Picture(Color.White, 2, 3)
                {
                    [0, 0] = Color.Black,
                    [1, 1] = Color.Black,
                    [0, 2] = Color.Black
                });

            sut.Rotate(-90).Invert().Apply();

            Assert.That(sut.CanUndo, Is.True);

            sut.Undo();

            Assert.That(sut.Document.Width, Is.EqualTo(3));
            Assert.That(sut.Document.Height, Is.EqualTo(2));
            Assert.That(sut.Document[0, 1], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[1, 0], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[2, 1], Is.EqualTo(Color.Black));
        }

        [Test]
        public void Should_Undo_Many_Commands()
        {
            var sut = new ReversiblePicture(
                new Picture(Color.White, 2, 3)
                {
                    [0, 0] = Color.Black,
                    [1, 1] = Color.Black,
                    [0, 2] = Color.Black
                });

            sut.Rotate(-90).Invert().Apply();

            Assert.That(sut.CanUndo, Is.True);

            sut.Undo();

            Assert.That(sut.CanUndo, Is.True);

            sut.Undo();

            Assert.That(sut.Document.Width, Is.EqualTo(2));
            Assert.That(sut.Document.Height, Is.EqualTo(3));
            Assert.That(sut.Document[0, 0], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[0, 1], Is.EqualTo(Color.White));
            Assert.That(sut.Document[0, 2], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[1, 0], Is.EqualTo(Color.White));
            Assert.That(sut.Document[1, 1], Is.EqualTo(Color.Black));
            Assert.That(sut.Document[1, 2], Is.EqualTo(Color.White));
        }

        [Test]
        public void Should_Undo_Throw_If_Stack_Empty()
        {
            var sut = new ReversiblePicture(new Picture(Color.White, 2, 3));

            Assert.That(sut.CanUndo, Is.False);

            Assert.Throws<InvalidOperationException>(() => sut.Undo());
        }

        [Test]
        public void Should_Redo()
        {
            var sut = new ReversiblePicture(new Picture(Color.White, 2, 3));

            sut.Invert().Apply();

            sut.Undo();
            sut.Redo();

            sut.Document.ForEachPixel((x, y) =>
            {
                Assert.That(sut.Document[x, y], Is.EqualTo(Color.Black));
            });
        }

        [Test]
        public void Should_Redo_History_Be_Lost_If_Execute_Different_Command()
        {
            var sut = new ReversiblePicture(new Picture(Color.White, 2, 3));

            sut.Rotate(-90).Apply();

            sut.Undo();
            sut.Invert().Apply();

            Assert.That(sut.CanRedo, Is.False);
            Assert.That(sut.Document.Width, Is.EqualTo(2));
            Assert.That(sut.Document.Height, Is.EqualTo(3));
            sut.Document.ForEachPixel((x, y) =>
            {
                Assert.That(sut.Document[x, y], Is.EqualTo(Color.Black));
            });
        }

        [Test]
        public void Should_Redo_Throw_If_Stack_Empty()
        {
            var sut = new ReversiblePicture(new Picture(Color.White, 2, 3));

            Assert.Throws<InvalidOperationException>(() => sut.Redo());
        }
    }
}