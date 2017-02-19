using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StudentsWebsite.Data.Abstract;
using StudentsWebsite.Data.Concrete;
using StudentsWebsite.WebUI.Infrastructure.Abstract;
using StudentsWebsite.WebUI.Infrastructure.Concrete;


namespace StudentsWebsite.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IDataRepositoryOld>().To<EFDataRepository>();
            kernel.Bind<IAuthProvider>().To<DBAuthProvider>();
        }
    }
}