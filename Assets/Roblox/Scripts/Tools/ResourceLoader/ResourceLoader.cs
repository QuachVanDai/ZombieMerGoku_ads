using UnityEngine;

namespace ExampleProject.Tools
{
    public class ResourceLoader<ResourceType> where ResourceType : Object
    {
        #region Fields

        private readonly string resourcePath;

        private ResourceType loadedResource;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public ResourceLoader(string _resourcePath)
        {
            this.resourcePath = _resourcePath;
        }

        public ResourceType Resource
        {
            get
            {
                ResourceType _resource;
                if ((_resource = this.loadedResource) == null)
                {
                    _resource = Resources.Load<ResourceType>(this.resourcePath);
                }
                this.loadedResource = _resource;
                return this.loadedResource;
            }
        }

        #endregion
    }
}
