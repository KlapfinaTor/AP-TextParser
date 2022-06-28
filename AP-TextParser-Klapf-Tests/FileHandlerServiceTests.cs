using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AP_TextParser_Klapf.Services;

namespace AP_TextParser_Klapf_Tests
{
    [TestClass]
    public class FileHandlerServiceTests
    {
     
        [TestMethod]
        public void ExtractWordsFromString_InputNull_ReturnsNull() { 
            var fhs = new FileHandlerService();

            var result = fhs.ExtractWordsFromString(null);

            Assert.IsNull(result);
        
        }

        [TestMethod]
       
        public void ExtractWordsFromString_BasicStringInput_Equals()
        {
            var fhs = new FileHandlerService();
            string data = "Hallo Test\n Ja";

            var result = fhs.ExtractWordsFromString(data);
            var expected = new String[3] {"Hallo","Test","Ja"};
            CollectionAssert.AreEqual(expected, result);

        }
        [TestMethod]
        public void ExtractWordsFromString_ComplexStringInput_Equals()
        {
            var fhs = new FileHandlerService();
            string data = "1:1 Adam Seth Enos\r\n1:2\ret";

            var result = fhs.ExtractWordsFromString(data);
            var expected = new String[6] { "1:1", "Adam", "Seth", "Enos", "1:2","et" };
            CollectionAssert.AreEqual(expected, result);

        }
    }
}
