using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    private Mock<SeatsService> _service;
    private Mock<SeatsController> _controller;

    public SeatsControllerTests()
    {
        _service = new Mock<SeatsService>();
        _controller = new Mock<SeatsController>(_service.Object) { CallBase = true };
    }

    [TestMethod]
    public void ReserveSeatValid()
    {
        string userId = "1";
        _controller.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 1;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };

        _service.Setup(s => s.ReserveSeat(userId, It.Is<int>(i => i <= 100))).Returns(seat);

        var result = _controller.Object.ReserveSeat(seatnumber);

        var ok = result.Result as OkObjectResult;

        Assert.AreEqual(seat, ok.Value);
    }

    [TestMethod]
    public void ReserveSeatAlreayTaken()
    {
        string userId = "1";
        _controller.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 1;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };

        _service.Setup(s => s.ReserveSeat(userId, seatnumber)).Throws(new SeatAlreadyTakenException());

        var result = _controller.Object.ReserveSeat(seatnumber);

        Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));

        Assert.IsNull(result.Value);
    }

    [TestMethod]
    public void ReserveSeatNotFound()
    {
        string userId = "1";
        _controller.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 101;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };

        _service.Setup(s => s.ReserveSeat(userId, It.Is<int>(i => i > 100))).Throws(new SeatOutOfBoundsException());

        var result = _controller.Object.ReserveSeat(seatnumber);

        var ok = result.Result as NotFoundObjectResult;

        Assert.AreEqual("Could not find " + seatnumber, ok.Value);
    }

    [TestMethod]
    public void ReserveSeatAlreadySeated()
    {
        string userId = "1";
        _controller.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 1;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };

        _service.Setup(s => s.ReserveSeat(userId, seatnumber)).Throws(new UserAlreadySeatedException());

        var result = _controller.Object.ReserveSeat(seatnumber);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));

        Assert.IsNull(result.Value);
    }
}
