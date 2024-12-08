using SetLibrary.Collections;
using SetsLibrary.Models;
using SetsLibrary.Models.Sets;
using Xunit;

namespace SetsLibrary.Tests.Collections.Set_Collection
{
    public class SetCollectionTestsExcelSpreadSheedEmulator
    {
        private static readonly SetExtractionConfiguration<int> settings = new SetExtractionConfiguration<int>(";",",");
        private static SetCollection<int> collection;
        public SetCollectionTestsExcelSpreadSheedEmulator()
        {
            //Create an empty set
            collection = new SetCollection<int>();
            //Add 16,384 empty sets. 
            var set = new TypedSet<int>(settings);
            for (int i = 0; i < 16384; i++)
                collection.Add(set);
            string exp = "{5,6,8,5,6,5,2,{3,5}}";
            set = new TypedSet<int>(exp, settings);
            collection.Add(set);
        }

        [Fact]
        public void TestNamingUsingExcelColumns()
        {
            //Convert to list
            var lst = collection.Select(b => b.Key).ToList();

            //In excel column 16384 == XFD
            var set16384 = lst[16383];

            //In excel column 16383 == XFC
            var set16383 = lst[16382];

            //In excel column 27 == AA
            var set27 = lst[26];

            //Since excel end at 16384(XFD) we can assume that 16385 == XFE
            var set16385 = lst[16384];


            Assert.Equal("XFD", set16384);
            Assert.Equal("XFC", set16383);
            Assert.Equal("XFE", set16385);
            Assert.Equal("AA", set27);

        }//TestNamingUsingExcelColumns
        [Fact]
        public void LastSetShouldNotBeEmpty()
        {
            var lst = collection.Select(b => b.Value).ToList();
            var set = lst[lst.Count - 1];

            int count = set.Cardinality;
            bool isempty = count <= 0;
            Assert.False(isempty);
        }//LastSetShouldNotBeEmpty

        [Fact]
        public void CountShouldBe16385()
        {
            Assert.Equal(16385, collection.Count);
        }//CountShouldBe16384

    }
}
