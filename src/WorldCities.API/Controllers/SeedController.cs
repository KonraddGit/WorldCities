using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security;
using WorldCities.Domain.Entities;
using WorldCities.Persistence;

namespace WorldCities.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private const string WorldCitiesPath = "Source/worldcities.xlsx";

    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public SeedController(
        ApplicationDbContext context,
        IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<ActionResult> ImportAsync()
    {
        if (!_env.IsDevelopment())
            throw new SecurityException("Not allowed");

        var path = Path.Combine(
            _env.ContentRootPath,
            WorldCitiesPath);

        using var stream = System.IO.File.OpenRead(path);
        using var excelPackage = new ExcelPackage(stream);

        var worksheet = excelPackage.Workbook.Worksheets[0];

        var nEndRow = worksheet.Dimension.End.Row;

        var numberOfCountriesAdded = 0;
        var numberOfCitiesAdded = 0;

        var countriesByName = await _context.Countries
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Name, StringComparer.OrdinalIgnoreCase);

        for (int nRow = 2; nRow < nEndRow; nRow++)
        {
            var row = worksheet.Cells[
                nRow, 1, nRow, worksheet.Dimension.End.Column];

            var countryName = row[nRow, 5].GetValue<string>();
            var iso2 = row[nRow, 6].GetValue<string>();
            var iso3 = row[nRow, 7].GetValue<string>();

            if (countriesByName.ContainsKey(countryName))
                continue;

            var country = new Country
            {
                Name = countryName,
                ISO2 = iso2,
                ISO3 = iso3,
            };

            await _context.Countries.AddAsync(country);

            countriesByName.Add(countryName, country);

            numberOfCountriesAdded++;
        }

        if (numberOfCountriesAdded > 0)
            await _context.SaveChangesAsync();

        var cities = await _context.Cities
            .AsNoTracking()
            .ToDictionaryAsync(x => (
                x.Name,
                x.Lat,
                x.Lon,
                x.CountryId));

        for (int nRow = 2; nRow <= nEndRow; nRow++)
        {
            var row = worksheet.Cells[
                nRow, 1, nRow, worksheet.Dimension.End.Column];

            var name = row[nRow, 1].GetValue<string>();
            var nameAscii = row[nRow, 2].GetValue<string>();
            var lat = row[nRow, 3].GetValue<decimal>();
            var lon = row[nRow, 4].GetValue<decimal>();
            var countryName = row[nRow, 5].GetValue<string>();

            var countryId = countriesByName[countryName].Id;

            if (cities.ContainsKey((name, lat, lon, countryId)))
                continue;

            var city = new City
            {
                Name = name,
                Lat = lat,
                Lon = lon,
                CountryId = countryId,
            };

            await _context.Cities.AddAsync(city);

            numberOfCitiesAdded++;
        }

        if (numberOfCitiesAdded > 0)
            await _context.SaveChangesAsync();

        return new JsonResult(new
        {
            Cities = numberOfCitiesAdded,
            Countries = numberOfCountriesAdded
        });
    }
}