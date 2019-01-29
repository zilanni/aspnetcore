using System;
using System.Linq;
using System.Threading.Tasks;
using ILanni.Common;

namespace Nest
{
    public static class Extenstions
    {
        private static (T Object, bool IsValid, IGetResponse<T>) GetByIdImp<T>(this ElasticClient client, DocumentPath<T> id) where T : class
        {
            var response = client.Get<T>(id);
            if (response.IsValid)
            {
                return (response.Source, true, response);
            }
            return (null, false, response);
        }

        public static (T Object, bool IsValid, IGetResponse<T>) GetById<T>(this ElasticClient client, int id) where T : class
        {
            return GetByIdImp<T>(client, (long)id);
        }

        public static (T Object, bool IsValid, IGetResponse<T>) GetById<T>(this ElasticClient client, long id) where T : class
        {
            return GetByIdImp<T>(client, id);
        }

       

        public static (T Object, bool IsValid, IGetResponse<T>) GetById<T>(this ElasticClient client, string id) where T : class
        {
            return GetByIdImp<T>(client, id);
        }

        public static (T Object, bool IsValid, IGetResponse<T>) GetById<T>(this ElasticClient client, Guid id) where T : class
        {
            return GetByIdImp<T>(client, id);
        }


        private static async Task<(T Object, bool IsValid, IGetResponse<T>)> GetByIdAsyncImp<T>(this ElasticClient client, DocumentPath<T> id) where T : class
        {
            var response = await client.GetAsync<T>(id);
            if (response.IsValid)
            {
                return (response.Source, true, response);
            }
            return (null, false, response);
        }

        public static Task<(T Object, bool IsValid, IGetResponse<T>)> GetByIdAsync<T>(this ElasticClient client, int id) where T : class
        {
            return GetByIdAsyncImp<T>(client, (long)id);
        }

        public static Task<(T Object, bool IsValid, IGetResponse<T>)> GetByIdAsync<T>(this ElasticClient client, long id) where T : class
        {
            return GetByIdAsyncImp<T>(client, id);
        }



        public static  Task<(T Object, bool IsValid, IGetResponse<T>)> GetByIdAsync<T>(this ElasticClient client, string id) where T : class
        {
            return GetByIdAsyncImp<T>(client, id);
        }

        public static Task<(T Object, bool IsValid, IGetResponse<T>)> GetByIdAsync<T>(this ElasticClient client, Guid id) where T : class
        {
            return GetByIdAsyncImp<T>(client, id);
        }

        public static PageSupport<T> List<T>(this ElasticClient client, Func<SearchDescriptor<T>, ISearchRequest> selector = null, bool isThrowEx = false) where T : class
        {
            var response = client.Search<T>(selector);
            if (response.IsValid)
            {
                var raws = response.Hits.Select(h => h.Source);
                return new PageSupport<T>(raws, response.Total);
            }
            else
            {
                if (isThrowEx)
                {
                    throw response.OriginalException;
                }
            }
            return PageSupport<T>.Empty();
        }

        public async static Task< PageSupport<T>> ListAsnyc<T>(this ElasticClient client, Func<SearchDescriptor<T>, ISearchRequest> selector = null, bool isThrowEx = false) where T : class
        {
            var response = await client.SearchAsync<T>(selector);
            if (response.IsValid)
            {
                var raws = response.Hits.Select(h => h.Source);
                return new PageSupport<T>(raws, response.Total);
            }
            else
            {
                if (isThrowEx)
                {
                    throw response.OriginalException;
                }
            }
            return PageSupport<T>.Empty();
        }
    }
}
