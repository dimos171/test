using System;
using SantanderTest.Contract.Entities;

namespace SantanderTest.Contract.Services
{
    /// <summary>
    /// News Service provides ability to work with news sources
    /// </summary>
    public interface INewsService
    {
        /// <summary>
        /// Get News from the source
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns>List of News</returns>
        Task<IEnumerable<Story>> GetNewsAsync(int? take, int? skip);
    }
}

