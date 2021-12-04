using Xunit;

namespace SquidGame.Core.Tests
{
  public class BingoSpotTests
  {
    [Fact]
    void BingoSpot_WhenInitialized_IsUnmarked()
    {
      var sut = new BingoSpot(42);
      Assert.False(sut.Marked);
    }

    [Fact]
    void BingoSpot_WhenInitialized_TakesValue()
    {
      var sut = new BingoSpot(123123);
      Assert.Equal(123123, sut.Value);
    }

    [Fact]
    void BingoSpot_WhenMarked_IsMarked()
    {
      var sut = new BingoSpot(23);
      sut.Mark();
      Assert.True(sut.Marked);
    }
  }
}
