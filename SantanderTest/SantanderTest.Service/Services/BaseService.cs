namespace SantanderTest.Service.Services
{
    /// <summary>
    /// Base Service provides common functionality for derived services
    /// </summary>
    public class BaseService
    {
        public BaseService()
        {
        }

        /// <summary>
        /// Pagination over the list of items. Input source is updated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="source"></param>
        /// <returns>paginated input collection</returns>
        protected IEnumerable<T> Paginate<T>(int? take, int? skip, ref IEnumerable<T> source)
        {
            if (skip != null)
                source = source.Skip((int)skip);

            // TODO discuss the limit of the 'take' param
            // we should talk to NewHacker team to know the API Rate limits they have
            if (take != null)
                source = source.Take((int)take);

            return source;
        }
    }
}