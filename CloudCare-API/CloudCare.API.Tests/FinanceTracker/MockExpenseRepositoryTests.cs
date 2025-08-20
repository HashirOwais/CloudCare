using System;

namespace CloudCare.API.Tests.FinanceTracker;

using Xunit;
using CloudCare.API.Models;


//naming convention 
//MethodUnderTest_StateUnderTest_ExpectedBehavior
public class MockExpenseRepositoryTests
{

    // //we use async Task here becuase we are testing async code. now we will return a task or we need to use await in the test (which we do)
    // [Fact]
    // public async Task GetExpensesAsync_ReturnsExpenses_ForUser()
    // {
    //     // Arrange
    //     int userId = 1;
    //     var repo = new MockExpenseRepository();
    //
    //     // Act
    //     var expenses = await repo.GetExpensesAsync(userId);
    //
    //     // Assert: Every expense must belong to the right user
    //     Assert.All(expenses, expense => Assert.Equal(userId, expense.UserId));
    //
    // }
    




}
