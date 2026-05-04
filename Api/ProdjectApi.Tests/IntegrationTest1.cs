using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdjectApi.Controllers;
using ProdjectApi.Data;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Requests;
using ProdjectApi.Domain.Requests.ExpenseParticipants;
using Xunit;

namespace ProdjectApi.Tests.Controllers
{
    public class ExpenseParticipantControllerTests
    {
        private static ProjectDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ProjectDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ProjectDbContext(options);
        }

        [Fact]
        public async Task Create_ExpenseNotFound_ReturnsNotFound()
        {
            using var context = CreateContext();
            var controller = new ExpenseParticipantController(context);

            context.Users.Add(new Users
            {
                Id = 1,
                Name = "User 1",
                Surname = "Testov",
                Email = "user1@test.com",
                PasswordHash = "hash"
            });
            await context.SaveChangesAsync();

            var request = new ExpenseParticipantCreateRequest
            {
                ExpensesId = 10,
                UsersId = 1,
                TotalDebt = 100
            };

            var result = await controller.Create(request);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Раcход не найден", notFound.Value);
        }

        [Fact]
        public async Task Create_UserNotFound_ReturnsNotFound()
        {
            using var context = CreateContext();
            var controller = new ExpenseParticipantController(context);

            context.Expenses.Add(new Expenses
            {
                Id = 10,
                Name = "Dinner",
                ReceiptAmount = 1000,
                PaymentDate = DateTime.UtcNow,
                PayerId = 1,
                DistributionType = "Equal",
                RoomsId = 1,
                CurrenciesId = 1
            });
            await context.SaveChangesAsync();

            var request = new ExpenseParticipantCreateRequest
            {
                ExpensesId = 10,
                UsersId = 2,
                TotalDebt = 100
            };

            var result = await controller.Create(request);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Пользователь не найден", notFound.Value);
        }

        [Fact]
        public async Task Create_ValidData_ReturnsOkAndSavesParticipant()
        {
            using var context = CreateContext();
            var controller = new ExpenseParticipantController(context);

            context.Expenses.Add(new Expenses
            {
                Id = 11,
                Name = "Dinner 2",
                ReceiptAmount = 1000,
                PaymentDate = DateTime.UtcNow,
                PayerId = 1,
                DistributionType = "Equal",
                RoomsId = 1,
                CurrenciesId = 1
            });

            context.Users.Add(new Users
            {
                Id = 2,
                Name = "User 2",
                Surname = "Testov",
                Email = "user1@test.com",
                PasswordHash = "hash"
            });

            await context.SaveChangesAsync();

            var request = new ExpenseParticipantCreateRequest
            {
                ExpensesId = 11,
                UsersId = 2,
                TotalDebt = 100
            };

            var result = await controller.Create(request);

            var ok = Assert.IsType<OkObjectResult>(result);

            var saved = await context.ExpenseParticipants.FirstOrDefaultAsync();
            Assert.NotNull(saved);
            Assert.Equal(11, saved.ExpensesId);
            Assert.Equal(2, saved.UsersId);
            Assert.Equal(100, saved.TotalDebt);
        }
    }
}
