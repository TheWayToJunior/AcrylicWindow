using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Entity;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Services
{
    public class ReferenceExcludableService : IReferenceExcludable
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReferenceExcludableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = Has.NotNull(unitOfWork);
        }

        /// <summary>
        /// Deletes installed links in groups
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="referencesAction"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Exclude(Action<IReferencesDeleteable> referencesAction, IGroupsReferense model)
        {
            Has.NotNull(model);

            if (model.Groups == null)
                return;

            foreach (var id in model.Groups)
            {
                var groupEntity = await _unitOfWork.Groups.GetByIdAsync(id);

                if (groupEntity == null)
                {
                    throw new InvalidOperationException($"Missing id specified: {id}");
                }

                referencesAction?.Invoke(groupEntity);
                await _unitOfWork.Groups.UpdateAsync(id, groupEntity);
            }
        }
    }
}
