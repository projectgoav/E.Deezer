using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface IRadioEndpoint
    {
        Task<IEnumerable<IRadio>> GetTop5();

        Task<IEnumerable<IRadio>> GetDeezerSelection(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IRadio>> GetByGenres();
    }

    //TODO needs refactor for the Get<T> with permissions param
    internal class RadioEndpoint : IRadioEndpoint
    {
        private readonly DeezerClient iClient;

        public RadioEndpoint(DeezerClient aClient)
        {
            iClient = aClient;
        }

        public Task<IEnumerable<IRadio>> GetTop5()
        {
            ThrowIfClientUnauthenticated();
            ThrowIfNoPermission(DeezerPermissions.BasicAccess);
            
            return iClient.Get<Radio>("radio/top", RequestParameter.EmptyList)
                            .ContinueWith<IEnumerable<IRadio>>((aTask) =>
                                {
                                    return iClient.Transform<Radio, IRadio>(aTask.Result);
                                }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        public Task<IEnumerable<IRadio>> GetDeezerSelection(uint aStart = 0, uint aCount = 100)
        {
            ThrowIfClientUnauthenticated();
            ThrowIfNoPermission(DeezerPermissions.BasicAccess);

            return iClient.Get<Radio>("radio/lists", aStart, aCount).ContinueWith<IEnumerable<IRadio>>((aTask) =>
            {
                return iClient.Transform<Radio, IRadio>(aTask.Result);
            }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        public Task<IEnumerable<IRadio>> GetByGenres()
        {
            ThrowIfClientUnauthenticated();
            ThrowIfNoPermission(DeezerPermissions.BasicAccess);

            return iClient.Get<Radio>("radio/genres", RequestParameter.EmptyList).ContinueWith<IEnumerable<IRadio>>((aTask) =>
            {
                return iClient.Transform<Radio, IRadio>(aTask.Result);
            }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }        

        private void ThrowIfClientUnauthenticated()
        {
            if(!iClient.IsAuthenticated)
            {
                throw new NotLoggedInException();
            }
        }

        private void ThrowIfNoPermission(DeezerPermissions aPermission)
        {
            if (!iClient.HasPermission(aPermission))
            {
                throw new DeezerPermissionsException(aPermission);
            }
        }
    }
}
