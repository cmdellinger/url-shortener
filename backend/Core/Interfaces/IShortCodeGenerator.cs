using System;

namespace Core.Interfaces;

public interface IShortCodeGenerator
{
    Task<string> GenerateShortCode();
}
