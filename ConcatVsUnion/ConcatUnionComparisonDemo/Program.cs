using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcatUnionComparisonDemo;

public record Flight(string FlightNumber, string Departure, string Arrival);

public static class Program
{
    public static void Main()
    {
        var airlineFlights = new[]
        {
            new Flight("ZB101", "YUL", "OTP"),
            new Flight("ZB205", "YUL", "CDG"),
            new Flight("ZB300", "YUL", "LHR")
        };

        var partnerFlights = new[]
        {
            new Flight("ZB101", "YUL", "OTP"),
            new Flight("ZB101", "YUL", "CDG"),
            new Flight("ZB310", "YUL", "LHR")
        };

        Console.WriteLine("==============================================");
        Console.WriteLine("       LINQ: Concat vs Union vs UnionBy");
        Console.WriteLine("==============================================");

        Display("Concat()", "Keeps every flight from both providers.", airlineFlights.Concat(partnerFlights));

        Display("Union()", "Removes exact duplicate values.", airlineFlights.Union(partnerFlights));

        Display("UnionBy(FlightNumber)", "Removes duplicates using FlightNumber as the key.", airlineFlights.UnionBy(partnerFlights, flight => flight.FlightNumber));

        Console.ReadKey();
    }

    private static void Display(string operation, string description, IEnumerable<Flight> flights)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {operation} ---");
        Console.WriteLine(description);
        Console.WriteLine();

        Console.WriteLine($"{"Flight",-12} {"Route",-15}");
        Console.WriteLine(new string('-', 30));

        foreach (var flight in flights)
        {
            Console.WriteLine($"{flight.FlightNumber,-12} {flight.Departure} -> {flight.Arrival}");
        }

        Console.WriteLine(new string('-', 30));
    }
}
