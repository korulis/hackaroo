using System.Collections.Generic;
using System.Linq;
using Xunit;
using RabbitHunter.V2;


namespace RabbitHunterTests.V2
{
    public class BlobCompositionTests
    {

        [Theory]
        [MemberData(nameof(Data))]
        public void BuildAnagramTest(List<Blob> blobs, List<string> expected)
        {
            //Arrange
            var sut = new BlobComposition(blobs);

            //Act
            var actual = sut.BuildAnagrams();

            //Assert
            expected.Sort();
            actual.ToList().Sort();
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                    new List<Blob> {BlobBuilder.Build("a")},
                    new List<string> {"a"} },
                new object[] {
                    new List<Blob> {BlobBuilder.Build("b"),BlobBuilder.Build("b")},
                    new List<string> {"b b"} },
                new object[] {
                    new List<Blob> {BlobBuilder.Build("ab","ba")},
                    new List<string> {"ab","ba"} },
                new object[] {
                    new List<Blob> {BlobBuilder.Build("ab","ba"), BlobBuilder.Build("c")},
                    new List<string> {"ab c","ba c"} },
                new object[] {
                    new List<Blob> {BlobBuilder.Build("ab","ba"), BlobBuilder.Build("cd", "dc")},
                    new List<string> {"ab cd","ba cd", "ab dc", "ba dc" } },
            };

    }
}
