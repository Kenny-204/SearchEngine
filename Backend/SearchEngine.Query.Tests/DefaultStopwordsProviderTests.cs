using System;
using System.Linq;
using Xunit;
using SearchEngine.Query.Services;

namespace SearchEngine.Query.Tests
{
    public class DefaultStopwordsProviderTests
    {
        private readonly DefaultStopwordsProvider _provider;

        public DefaultStopwordsProviderTests()
        {
            _provider = new DefaultStopwordsProvider();
        }

        [Fact]
        public void GetStopwords_Should_Return_NonEmpty_Collection()
        {
            var stopwords = _provider.GetStopwords();

            Assert.NotNull(stopwords);
            Assert.True(stopwords.Count > 0);
        }

        [Fact]
        public void IsStopword_Should_Return_True_For_Common_Stopwords()
        {
            Assert.True(_provider.IsStopword("the"));
            Assert.True(_provider.IsStopword("a"));
            Assert.True(_provider.IsStopword("an"));
            Assert.True(_provider.IsStopword("and"));
            Assert.True(_provider.IsStopword("or"));
            Assert.True(_provider.IsStopword("but"));
            Assert.True(_provider.IsStopword("in"));
            Assert.True(_provider.IsStopword("on"));
            Assert.True(_provider.IsStopword("at"));
            Assert.True(_provider.IsStopword("to"));
            Assert.True(_provider.IsStopword("for"));
            Assert.True(_provider.IsStopword("of"));
            Assert.True(_provider.IsStopword("with"));
            Assert.True(_provider.IsStopword("by"));
        }

        [Fact]
        public void IsStopword_Should_Return_False_For_NonStopwords()
        {
            Assert.False(_provider.IsStopword("computer"));
            Assert.False(_provider.IsStopword("algorithm"));
            Assert.False(_provider.IsStopword("search"));
            Assert.False(_provider.IsStopword("engine"));
            Assert.False(_provider.IsStopword("query"));
            Assert.False(_provider.IsStopword("parser"));
        }

        [Fact]
        public void IsStopword_Should_Be_Case_Insensitive()
        {
            Assert.True(_provider.IsStopword("THE"));
            Assert.True(_provider.IsStopword("The"));
            Assert.True(_provider.IsStopword("tHe"));
            Assert.True(_provider.IsStopword("the"));
        }

        [Fact]
        public void Reload_Should_Work_Without_Errors()
        {
            // This test ensures the Reload method doesn't throw exceptions
            var exception = Record.Exception(() => _provider.Reload());
            Assert.Null(exception);
        }
    }
} 