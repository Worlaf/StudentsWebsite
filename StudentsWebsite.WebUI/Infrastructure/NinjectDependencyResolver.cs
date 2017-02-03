﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using StudentsWebsite.Domain.Abstract;
using StudentsWebsite.Domain.Concrete;
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
            kernel.Bind<IDataRepository>().To<EFDataRepository>();
            kernel.Bind<IAuthProvider>().To<DBAuthProvider>();
        }
    }
}