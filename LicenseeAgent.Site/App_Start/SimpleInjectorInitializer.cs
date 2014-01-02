// --------------------------------------------------------------------------
// <copyright file="SimpleInjectorInitializer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.11.6">Created</change>
// </history>

[assembly: WebActivator.PreApplicationStartMethod(typeof(LicenseeAgent.Site.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace LicenseeAgent.Site.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerSetting.Repositories;
    using LicenseeAgent.CustomerSetting.Service;
    using LicenseeAgent.Web.Services;
    using SimpleInjector;
    using SimpleInjector.Integration.Web.Mvc;

    /// <summary>
    /// Simple Injector Initializer.
    /// </summary>
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? Go to: http://bit.ly/YE8OJj.
            var container = new Container();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcAttributeFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void InitializeContainer(Container container)
        {
            container.Register<IAuthorizeService, AuthorizeService>();
            container.Register<IUnderMaintenanceService, UnderMaintenanceService>();
            container.Register<IAccountService, AccountService>();
            container.Register<ICurrencyService, CurrencyService>();
            container.Register<ISportsbookSettingRepository, SportsbookSettingRepository>();
            container.Register<ISportsbookSettingService, SportsbookSettingService>();
            container.Register<ICustomerService, CustomerService>();
            container.Register<IIpInformationService, IpInformationService>();
            container.Register<ISearchUserService, SearchUserService>();
            container.Register<IBetListService, BetListService>();
            container.Register<ICoreService, CoreService>();
        }
    }
}