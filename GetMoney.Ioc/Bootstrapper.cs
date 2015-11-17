﻿using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Application.Card;
using GetMoney.Data.Card;
using GetMoney.Dal;
using GetMoney.Application.OnlyNameTest;
using GetMoney.Data.OnlyNameTest;

namespace GetMoney.Ioc
{
    public class Bootstrapper
    {
        public void Boot()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType(typeof(DBContextOfUnitWork), new PerResolveLifetimeManager());

            container.RegisterType<ICardBll, CardBll>();
            container.RegisterType<IOnlyNameTestBll, OnlyNameTestBll>();

            container.RegisterType<ICardRepository, CardRepository>();
            container.RegisterType<IOnlyNameTestRepository, OnlyNameTestRepository>();

            

            return container;
        }
    }
}
