using AutoMapper;
using Ninject;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Service;
using PolyclinicProject.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PolyclinicProject.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IPositionService>().To<PositionService>();
            _kernel.Bind<IRoleInfoService>().To<RoleInfoService>();
            _kernel.Bind<IUserInfoService>().To<UserInfoService>();
            _kernel.Bind<IPolyclinicService>().To<PolyclinicService>();
            _kernel.Bind<IPersonalService>().To<PersonalService>();
            _kernel.Bind<IScheduleService>().To<ScheduleService>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapProfile());
            });

            var mapper = mapperConfiguration.CreateMapper();

            _kernel.Bind<AutoMapper.IMapper>().ToConstant(mapper);
        }
    }
}