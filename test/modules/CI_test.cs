using DefaultNamespace;
using Xunit;

namespace test;

public class CI_test
{
    [Fact]
    public void run_test()
    {
        var ciTest = new CI();
        var result = ciTest.Run();

        Assert.Equal("CI rodando!", result);    
    }
}