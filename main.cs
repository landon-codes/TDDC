using System;


namespace TDDC;

static class Program
{
	public static int GetPagesPerDay(ArgumnentParser.Arguments arg)
	{
		var assignmentDuration = arg.DueDate - arg.StartingDate;
		float daysToRead = assignmentDuration.Days;

		return (int)MathF.Round(arg.Pages / daysToRead);
	}

	public static void Main(string[] args)
	{
		var arguments = ArgumnentParser.ParseArguments(args);

		Console.WriteLine($"Starting date: {arguments.StartingDate}");
		Console.WriteLine($"Due date: {arguments.DueDate}");
		Console.WriteLine($"Pages: {arguments.Pages}");

		Console.WriteLine();

		var pagesPerDay = GetPagesPerDay(arguments);
		Console.WriteLine($"You will need to read {pagesPerDay} per day.");
	}
}
