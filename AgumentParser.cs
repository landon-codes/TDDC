using System.Globalization;

namespace TDDC;



public static class ArgumnentParser
{	
	public readonly struct Arguments
	{
		public readonly DateTime StartingDate;
		public readonly DateTime DueDate;
		public readonly int Pages;

		public Argument(DateTime startingDate, DateTime dueDate, int pages)
		{
			StartingDate = startingDate;
			DueDate = dueDate;
			Pages = pages;
		}
	}

	private static string CannotParseDateError = "We were unable to parse the date(s) given.\n Maybe try another format.";
	private static string CannotParsePageError = "We were unable to parse the page(s) given.\n Make sure they are only a number.";
	
	public static Arguments ParseArguments(string[] args)
	{		
		string[] formats = {
            "yyyy-MM-dd",    // ISO standard (2026-10-03)
            "MM/dd/yyyy",    // US standard (10/03/2026)
            "dd/MM/yyyy",    // UK/International standard (03/10/2026)
            "yyyy/MM/dd",    // Alternative slash standard
            "d-MMM-yyyy",    // Text month short (3-Oct-2026)
            "dd-MMM-yyyy"    // Text month short padded (03-Oct-2026)
        };	

		bool ParseDate(string date, out DateTime parsedData) 
		{
			var result = DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime parsedDate);
			parsedData = parsedDate;

			return result;
		}

		void ThrowMissingArgumentError(string argumentName)
		{			 
			Console.WriteLine($"Argument {argumentName} is missing a value.");
			Environment.Exit(1);
		}

		int index = 0;

		bool CanContinue() => index+1 < args.Length;
		string GetNextArg() => args[index++];

		DateTime startingDate = DateTime.Now;
		DateTime? endDate = null;
		int? startPage = null;
		int? endPage = null;
		int? pages = null;
		
		while (index < args.Length)
		{
			var argument = GetNextArg();

			if (argument == "-s" || argument == "--start" && CanContinue())
			{
				var argumentValue = GetNextArg();

				if (!ParseDate(argumentValue, out var parsedDate)) 
				{
					Console.WriteLine(CannotParseDateError);
					Environment.Exit(1);					
				}

				startingDate = parsedDate;
			}

			else if (argument == "-d" || argument == "--dueDate" && CanContinue())
			{
				var argumentValue = GetNextArg();

				if (!ParseDate(argumentValue, out var parsedDate))
				{
					Console.WriteLine(CannotParseDateError);
					Environment.Exit(1);
				}

				endDate = parsedDate;
			}

			else if (argument == "-p" || argument == "--startingPage" && CanContinue())
			{
				var argumentValue = GetNextArg();

				try
				{
					startPage = Convert.ToInt16(argumentValue);
				}
				catch
				{
					Console.WriteLine(CannotParsePageError);
					Environment.Exit(1);
				}
			}

			else if (argument == "-e" || argument == "--endPage" && CanContinue())
			{
				var argumentValue = GetNextArg();

				try
				{
					endPage = Convert.ToInt16(argumentValue);
				}
				catch
				{
					Console.WriteLine(CannotParsePageError);
					Environment.Exit(1);
				}
			}

			else if (argument == "-c" || argument == "--pageCount" && CanContinue())
			{
				var argumentValue = GetNextArg();

				try
				{
					pages = Convert.ToInt16(argumentValue);
				}
				catch
				{
					Console.WriteLine(CannotParsePageError);
					Environment.Exit(-1);
				}
			}
		}

		bool exit = false;
		if (endDate == null)
		{
			Console.WriteLine("An ending date must be provided.");
			exit = true;
		}
		if (startPage == null && pages == null)
		{
			Console.WriteLine("A starting page must be provided.");
			exit = true;
		}
		if (endPage == null && pages == null)
		{
			Console.WriteLine("An ending page must be provided.");
			exit = true;
		}
		if (exit) Environment.Exit(1);

		if (pages == null && startPage != null && endPage != null)
		{
			pages = endPage - startPage;
		}

		return new Arguments(startingDate, (DateTime)endDate!, (int)pages!);
	}
}
