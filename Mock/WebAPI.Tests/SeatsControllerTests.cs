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
    private Mock<SeatsController> _seatsController;

    public SeatsControllerTests()
    {
        _seatsService = new Mock<SeatsService>();
        _seatsController = new Mock<SeatsController>(_seatsService.Object) { CallBase = true };

        
    }

    [TestMethod]
    public void ReserveSeatValid()
    {
        string userId = "1";
        _seatsController.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 1;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };
        _seatsService.Setup(s => s.ReserveSeat(userId, seatnumber)).Returns(seat);

        var result = _seatsController.Object.ReserveSeat(seatnumber);

        var ok = result.Result as OkObjectResult;
        Assert.AreEqual(seat, ok.Value);

    }

    [TestMethod]
    public void ReserveSeatAlreadyTaken()
    {
        string userId = "1";
        _seatsController.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 1;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };
        _seatsService.Setup(s => s.ReserveSeat(userId, seatnumber)).Throws(new SeatAlreadyTakenException());

        var result = _seatsController.Object.ReserveSeat(seatnumber);

        Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));
    }

    [TestMethod]
    public void ReserveSeatOverMax()
    {
        string userId = "1";
        _seatsController.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 101;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };
        _seatsService.Setup(s => s.ReserveSeat(userId, seatnumber)).Throws(new SeatOutOfBoundsException());

        var result = _seatsController.Object.ReserveSeat(seatnumber);

        var bad = result.Result as NotFoundObjectResult;
        Assert.AreEqual("Could not find " + seatnumber, bad.Value);
    }

    [TestMethod]
    public void ReserveSeatAlreadySeated()
    {
        string userId = "1";
        _seatsController.Setup(c => c.UserId).Returns(userId);
        int seatnumber = 1;

        Seat seat = new Seat
        {
            Id = 1,
            Number = seatnumber,
        };
        _seatsService.Setup(s => s.ReserveSeat(userId, seatnumber)).Throws(new UserAlreadySeatedException());

        var result = _seatsController.Object.ReserveSeat(seatnumber);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
    }
}
