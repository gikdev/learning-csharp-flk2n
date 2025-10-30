using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfApi1.Models.ValueConverters;

public class DateTimeToChar8Converter() : ValueConverter<DateTime, string>(
  dateTime => dateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
  str => DateTime.ParseExact(str, "yyyyMMdd", CultureInfo.InvariantCulture)
);
