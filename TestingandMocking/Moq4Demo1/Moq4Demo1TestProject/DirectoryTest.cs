using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//source https://code.4noobz.net/c-moq-tutorial-for-beginners/
//source https://code.4noobz.net/moq-directory-getfiles-unit-test/

namespace Moq4Demo1TestProject
{
    public class DirectoryTest
    {

        private IDirectoryHelper _directoryHelper;
        private readonly string _file1 = "test1.txt";
        private readonly string _file2 = "test2.txt";

        [SetUp]
        public void Setup()
        {
            _directoryHelper = Mock.Of<IDirectoryHelper>();

            Mock.Get(_directoryHelper)
                .Setup(x => x.GetFiles(It.IsAny<string>()))
                .Returns((string x) =>
                    new List<string>
                    {
                        _file1,
                        _file2
                    });
        }

        [Test]
        public void TestForFiles()
        {
            ICollection<string> files = _directoryHelper.GetFiles(@"D:\");

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count == 2);
            Assert.IsTrue(files.Contains(_file1));
        }

    }

    public interface IDirectoryHelper
    {
        ICollection<string> GetFiles(string path);
    }

    public class DirectoryHelper : IDirectoryHelper
    {
        public ICollection<string> GetFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            return files;
        }
    }
}
