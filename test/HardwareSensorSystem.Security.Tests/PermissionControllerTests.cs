using HardwareSensorSystem.Security.Controllers;
using HardwareSensorSystem.Security.Models;
using HardwareSensorSystem.Security.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HardwareSensorSystem.Security.Tests
{
    public class PermissionControllerTests : IDisposable
    {
        private ApplicationDbContext _context;
        private IEnumerable<ApplicationPermission> _permissions;

        public PermissionControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _permissions = new List<ApplicationPermission>()
            {
                new ApplicationPermission()
                {
                    Id=1,
                    Name="Admin"
                },
                new ApplicationPermission()
                {
                    Id=2,
                    Name="User"
                },
                new ApplicationPermission()
                {
                    Id=3,
                    Name="Demo"
                }
            };

            _context.Permissions.AddRange(_permissions);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetAll_Call_ReturnCollectionOfPermissionsFromDatabase()
        {
            // Arrange
            var controller = new PermissionController(_context);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var permissions = Assert.IsAssignableFrom<IEnumerable<PermissionViewModel>>(okObjectResult.Value);
            Assert.All(_permissions, _permission =>
            {
                var permission = permissions.SingleOrDefault(p => p.Id == _permission.Id);
                Assert.NotNull(permission);
                Assert.Equal(_permission.Name, permission.Name);
            });
        }
    }
}
