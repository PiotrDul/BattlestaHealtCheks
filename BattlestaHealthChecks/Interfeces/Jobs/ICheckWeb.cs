using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Interfeces
{
    public interface ICheckWeb
    {
        Task Start(IWebDriver _driver);
    }
}
