using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyMvcApp.Controllers;
using MyMvcApp.Models;

namespace MyMvcApp.Tests;

public class UserControllerTests
{
    private UserController _controller;

    public UserControllerTests()
    {
        _controller = new UserController();
        // Clear the static list before each test
        UserController.userlist.Clear();
    }

    // Index Tests
    [Fact]
    public void Index_ReturnsViewWithEmptyList_WhenNoUsers()
    {
        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
        Assert.Empty(model);
    }

    [Fact]
    public void Index_ReturnsViewWithUsers_WhenUsersExist()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com" };
        UserController.userlist.Add(user);

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
        Assert.Single(model);
        Assert.Contains(user, model);
    }

    // Details Tests
    [Fact]
    public void Details_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Act
        var result = _controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Details_ReturnsViewWithUser_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com" };
        UserController.userlist.Add(user);

        // Act
        var result = _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<User>(viewResult.Model);
        Assert.Equal(user.Id, model.Id);
        Assert.Equal(user.Name, model.Name);
        Assert.Equal(user.Email, model.Email);
    }

    // Create Tests
    [Fact]
    public void Create_Get_ReturnsView()
    {
        // Act
        var result = _controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Create_Post_RedirectsToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var user = new User { Name = "New User", Email = "new@test.com" };

        // Act
        var result = _controller.Create(user);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Single(UserController.userlist);
        Assert.Equal(1, UserController.userlist[0].Id);
        Assert.Equal(user.Name, UserController.userlist[0].Name);
        Assert.Equal(user.Email, UserController.userlist[0].Email);
    }

    [Fact]
    public void Create_Post_ReturnsView_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Name is required");
        var user = new User { Email = "new@test.com" };

        // Act
        var result = _controller.Create(user);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(user, viewResult.Model);
        Assert.Empty(UserController.userlist);
    }

    // Edit Tests
    [Fact]
    public void Edit_Get_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Act
        var result = _controller.Edit(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Get_ReturnsView_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com" };
        UserController.userlist.Add(user);

        // Act
        var result = _controller.Edit(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<User>(viewResult.Model);
        Assert.Equal(user.Id, model.Id);
    }

    [Fact]
    public void Edit_Post_ReturnsNotFound_WhenIdMismatch()
    {
        // Arrange
        var user = new User { Id = 2, Name = "Test User", Email = "test@test.com" };

        // Act
        var result = _controller.Edit(1, user);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_RedirectsToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com" };
        UserController.userlist.Add(user);
        var updatedUser = new User { Id = 1, Name = "Updated User", Email = "updated@test.com" };

        // Act
        var result = _controller.Edit(1, updatedUser);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal(updatedUser.Name, UserController.userlist[0].Name);
        Assert.Equal(updatedUser.Email, UserController.userlist[0].Email);
    }

    // Delete Tests
    [Fact]
    public void Delete_Get_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_Get_ReturnsView_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com" };
        UserController.userlist.Add(user);

        // Act
        var result = _controller.Delete(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<User>(viewResult.Model);
        Assert.Equal(user.Id, model.Id);
    }

    [Fact]
    public void Delete_Post_RedirectsToIndex_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com" };
        UserController.userlist.Add(user);
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

        // Act
        var result = _controller.Delete(1, formCollection);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Empty(UserController.userlist);
    }

    [Fact]
    public void Delete_Post_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

        // Act
        var result = _controller.Delete(1, formCollection);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
