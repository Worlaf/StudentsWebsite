using StudentsWebsite.Data;
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StudentsWebsite.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(StudentsWebsite.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace StudentsWebsite.WebUI.App_Start
{
    using System;
    using System.Web;
    using Data.Repositories;
    using Data.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;


    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                kernel.Bind<IDbUserRepository>().To<DbUserRepository>();
                kernel.Bind<ILecturerRepository>().To<LecturerRepository>();
                kernel.Bind<IStudentRepository>().To<StudentRepository>();
                kernel.Bind<IRatingRepository>().To<RatingRepository>();
                kernel.Bind<IDbUserService>().To<DbUserService>();
                kernel.Bind<IStudentService>().To<StudentService>();
                kernel.Bind<ILecturerService>().To<LecturerService>();
                kernel.Bind<IRatingService>().To<RatingService>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            System.Web.Mvc.DependencyResolver.SetResolver(new
                Infrastructure.NinjectDependencyResolver(kernel));
        }        
    }
}
