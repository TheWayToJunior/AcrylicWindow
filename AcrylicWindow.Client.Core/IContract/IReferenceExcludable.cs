using AcrylicWindow.Client.Entity;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IReferenceExcludable
    {
        Task Exclude(Action<IReferencesDeleteable> referencesAction, IGroupsReferense model);
    }
}
