using NetArchTest.Rules;
using Xunit;

namespace Raftlabs.Architecture.Tests;

public class CleanArchitectureTests
{
    private const string ApplicationNamespace = "Raftlabs.Application";
    private const string InfrastructureNamespace = "Raftlabs.Infra.Client";
    private const string PresentationNamespace = "Raftlabs.Presentation";

    [Fact]
    public void Domain_Should_Not_Have_Dependency_On_Other_Projects()
    {
        var result = Types.InAssembly(typeof(Raftlabs.Core.User).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace, PresentationNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure_Or_Web()
    {
        var result = Types.InAssembly(typeof(Raftlabs.Application.Services.Implementations.UserService).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, PresentationNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
