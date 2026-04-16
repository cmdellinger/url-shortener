using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Links;

public class ShortCodeGenerator(AppDbContext dbContext) : IShortCodeGenerator
{
    // specify length of code so easy to change later
    private const int codeLength = 7;
    // create a list of characters a-z A-Z 2-9 (excludes 0 O 1 l I)
    private const string charactersAvailable = (
        "abcdefghijk" + "mnopqrstuvwxyz" + 
        "ABCDEFGH" + "JKLMN" + "PQRSTUVWXYZ" +
        "23456789"
        );
    public async Task<string> GenerateShortCode()
    {
        while (true)
        {
            // generate new random code
            var candidate = new string(
                Enumerable.Range(0, codeLength)
                    .Select(_ => charactersAvailable[ Random.Shared.Next(charactersAvailable.Length) ])
                    .ToArray()
            );
            // test if code already exists in database
            if (!await dbContext.ShortLinks.AnyAsync(link => link.ShortCode == candidate))
            {
                return candidate;
            }
        }
    }
}
