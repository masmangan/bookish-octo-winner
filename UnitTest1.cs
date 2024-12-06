namespace SeleniumParametrizedTest;

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

public class GoogleSearchTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly ExtentReports _extent;
    private readonly ExtentTest _test;

    public GoogleSearchTests()
    {
        //	Usar	o	WebDriverManager	para	con@igurar	o	ChromeDriver	
        new DriverManager().SetUpDriver(new ChromeConfig());
        _driver = new ChromeDriver();

        //	Con@igurar	o	ExtentReports	
        var htmlReporter = new ExtentSparkReporter("extent_report.html");
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);

        //	Criar	um	teste	no	relató rio	
        _test = _extent.CreateTest("GoogleSearchTests");
    }

    [Fact]
    public void TestGoogleSearch()
    {
        try
        {
            //	Navegar	para	a	pá gina	do	Google	
            _driver.Navigate().GoToUrl("https://www.google.com");
            _test.Log(Status.Pass, "Navegou	para	Google.com");

            //	Encontrar	a	caixa	de	pesquisa	pelo	atributo	name	
            IWebElement searchBox = _driver.FindElement(By.Name("q"));
            _test.Log(Status.Pass, "Encontrou	a	caixa	de	pesquisa");

            //	Realizar	a	pesquisa	
            var searchItem = "Selenium Webdriver";
            searchBox.SendKeys(searchItem);
            searchBox.SendKeys(Keys.Enter);
            _test.Log(Status.Pass, $"Realizou	a	pesquisa	por	{searchItem}");

            //	Esperar	um	pouco	para	ver	os	resultados	
            System.Threading.Thread.Sleep(3000);

            //	Veri@icar	se	o	tı́tulo	da	pá gina	conté m	o	termo	de	pesquisa	
            Assert.Contains(searchItem, _driver.Title, StringComparison.OrdinalIgnoreCase);
            _test.Log(Status.Pass,
            $"O título da página contém	o termo	de	pesquisa {searchItem}.");

        }
        catch (Exception e)
        {
            _test.Log(Status.Fail, e.ToString());
            throw;
        }
    }

    public void Dispose()
    {
        //	Fechar	o	navegador	
        _driver.Quit();
        _driver.Dispose();

        //	Escrever	o	relató rio	
        _extent.Flush();
    }
}