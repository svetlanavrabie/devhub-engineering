using AutoFixture;
using System.Linq;

namespace ConcatUnionComparisonDemo.Tests;

public class ConcatOperationsTests
{
    private readonly Fixture _fixture = new();

    private (Flight[] AirlineFlights, Flight[] PartnerFlights) SetupFlights()
    {
        var airlineFlights = new[]
        {
            CreateFlight("ZB101", "YUL", "OTP"),
            CreateFlight("ZB205", "YUL", "CDG"),
            CreateFlight("ZB300", "YUL", "LHR")
        };

        var partnerFlights = new[]
        {
            CreateFlight("ZB101", "YUL", "OTP"),
            CreateFlight("ZB101", "YUL", "CDG"),
            CreateFlight("ZB310", "YUL", "LHR")
        };

        return (airlineFlights, partnerFlights);
    }

    [Fact]
    public void Concat_ShouldKeepAllFlightsIncludingDuplicates()
    {
        var (airlineFlights, partnerFlights) = SetupFlights();
        var result = airlineFlights.Concat(partnerFlights).ToArray();

        var expectedFlights = new[]
        {
            CreateFlight("ZB101", "YUL", "OTP"),
            CreateFlight("ZB205", "YUL", "CDG"),
            CreateFlight("ZB300", "YUL", "LHR"),
            CreateFlight("ZB101", "YUL", "OTP"),
            CreateFlight("ZB101", "YUL", "CDG"),
            CreateFlight("ZB310", "YUL", "LHR")
        };

        VerifyFlights(result, expectedFlights);
    }

    [Fact]
    public void Union_ShouldRemoveOnlyExactDuplicates()
    {
        var (airlineFlights, partnerFlights) = SetupFlights();
        var result = airlineFlights.Union(partnerFlights).ToArray();

        var expectedFlights = new[]
        {
            CreateFlight("ZB101", "YUL", "OTP"),
            CreateFlight("ZB205", "YUL", "CDG"),
            CreateFlight("ZB300", "YUL", "LHR"),
            CreateFlight("ZB101", "YUL", "CDG"),
            CreateFlight("ZB310", "YUL", "LHR")
        };

        VerifyFlights(result, expectedFlights);
    }

    [Fact]
    public void UnionBy_ShouldUseFlightNumberAsDeduplicationKey()
    {
        var (airlineFlights, partnerFlights) = SetupFlights();
        var result = airlineFlights.UnionBy(partnerFlights, flight => flight.FlightNumber).ToArray();

        var expectedFlights = new[]
        {
            CreateFlight("ZB101", "YUL", "OTP"),
            CreateFlight("ZB205", "YUL", "CDG"),
            CreateFlight("ZB300", "YUL", "LHR"),
            CreateFlight("ZB310", "YUL", "LHR")
        };

        VerifyFlights(result, expectedFlights);
    }

    private Flight CreateFlight(string flightNumber, string departure, string arrival) =>
        _fixture.Build<Flight>()
            .With(flight => flight.FlightNumber, flightNumber)
            .With(flight => flight.Departure, departure)
            .With(flight => flight.Arrival, arrival)
            .Create();

    private static void VerifyFlights(Flight[] actualFlights, Flight[] expectedFlights)
    {
        Assert.Equal(expectedFlights.Length, actualFlights.Length);
        Assert.Equal(expectedFlights, actualFlights);
    }
}
