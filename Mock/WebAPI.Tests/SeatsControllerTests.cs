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
    private Mock<SeatsService> _seatsService;
    private Mock<SeatsController> _controller; 

    public SeatsControllerTests()
    {
        _seatsService = new Mock<SeatsService>();
        _controller = new Mock<SeatsController>(_seatsService.Object) { CallBase = true };
    }


    [TestMethod]
    public void ReserveSeatValid()
    {
        string userId = "1";
        _controller.Setup(c => c.UserId).Returns(userId);
        int seatNumber = 1;

        Seat seat = new Seat()
        {
            Id = 1,
            Number = seatNumber
        };
        _seatsService.Setup(s => s.ReserveSeat(userId, It.Is<int>(i => i <= 100))).Returns(seat);

        var result =_controller.Object.ReserveSeat(seatNumber);

        var ok = result.Result as OkObjectResult;
        Assert.IsNotNull(ok.Value);
        Assert.AreEqual(seat, ok.Value);
    }

    [TestMethod]
    public void ReserveSeatAlreadyTaken()
    {
        string userId = "1";
        _controller.Setup(c => c.UserId).Returns(userId);
        int seatNumber = 1;

        Seat seat = new Seat()
        {
            Id = 1,
            Number = seatNumber
        };
        _seatsService.Setup(s => s.ReserveSeat(userId, seatNumber)).Throws(new SeatAlreadyTakenException());

        var result = _controller.Object.ReserveSeat(seatNumber);

        Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));
    }

    [TestMethod]
    public void ReserveSeatOutOfBounds()
    {
        string userId = "1";
        _controller.Setup(c => c.UserId).Returns(userId);
        int seatNumber = 101;

        Seat seat = new Seat()
        {
            Id = 1,
            Number = seatNumber
        };
        _seatsService.Setup(s => s.ReserveSeat(userId, seatNumber)).Throws(new SeatOutOfBoundsException());

        var result = _controller.Object.ReserveSeat(seatNumber);

        var bad = result.Result as NotFoundObjectResult;
        Assert.AreEqual("Could not find " + seatNumber, bad.Value);
    }
}
