using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

var threads = new List<Thread>();
var running = true;
var meetingId = string.Empty;
var password = string.Empty;
var threadCount = 0;

Console.WriteLine("MS Teams Dummy Participant Runner - Using Chrome");
Console.WriteLine("Created by Elias Puurunen @ Tractus Events - https://www.tractusevents.com");

if (args.Length == 3)
{
    meetingId = args[0];
    password = args[1];
    threadCount = int.Parse(args[2]);
}
else
{
    Console.WriteLine("Please provide the Teams meeting ID.");

    while (string.IsNullOrEmpty(meetingId))
    {
        meetingId = Console.ReadLine();
    }


    Console.WriteLine("Please provide the Teams meeting password.");

    while (string.IsNullOrEmpty(password))
    {
        password = Console.ReadLine();
    }

    Console.WriteLine("How many test participants? (Max recommended: 5)");

    while(!int.TryParse(Console.ReadLine(), out threadCount))
    {

    }
}

for (var i = 0; i < threadCount; i++)
{
    var thread = new Thread((o) =>
    {
        var participantNumber = (int)o;
        var chromeOptions = new ChromeOptions
        {

        };

        //chromeOptions.AddArgument("--headless");
        chromeOptions.AddArgument("--window-size=1280,720");
        chromeOptions.AddArgument("--mute-audio");
        //chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument("--ignore-certificate-errors");
        chromeOptions.AddArgument("--disable-extensions");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        chromeOptions.AddArgument("--use-fake-device-for-media-stream");
        chromeOptions.AddArgument("--use-fake-ui-for-media-stream");
        chromeOptions.AddArgument("--log-level=3");
        chromeOptions.AddArgument("--disable-notifications");
        chromeOptions.AddArgument("--disable-popup-window");

        chromeOptions.AddUserProfilePreference("protocol_handler", new
        {
            excluded_schemes = new
            {
                msteams = false
            }
        });

        using var driver = new ChromeDriver(chromeOptions);

        driver.Navigate().GoToUrl($"https://teams.microsoft.com/v2/?meetingjoin=true#/meet/{meetingId.Replace(" ", "")}?launchAgent=marketing_join&laentry=hero&p={password}&anon=true&deeplinkId=251e9ce4-ef63-44dd-9115-a2d4b9c4f46d");

        while (true)
        {
            var input = driver.FindElements(By.TagName("input")).FirstOrDefault();

            if (input is null)
            {
                Thread.Sleep(250);
                continue;
            }

            input.SendKeys($"Test Participant {participantNumber}");
            break;
        }

        while (true)
        {
            var button = driver.FindElements(By.TagName("button")).FirstOrDefault(x => x.Text.Contains("join now", StringComparison.InvariantCultureIgnoreCase));

            if (button is null)
            {
                Thread.Sleep(250);
                continue;
            }

            button.Click();
            break;
        }

        while (true)
        {
            try
            {
                var button = driver.FindElements(By.TagName("button")).FirstOrDefault(x => x.GetAttribute("id").Contains("microphone-button", StringComparison.InvariantCultureIgnoreCase));

                if (button is null)
                {
                    Thread.Sleep(250);
                    continue;
                }

                button.Click();
                break;
            }
            catch
            {
                Thread.Sleep(250);
            }
        }

        while (running)
        {
            Thread.Sleep(250);
        }


        var hangup = driver.FindElements(By.TagName("button"))
            .FirstOrDefault(x => x.GetAttribute("id") == "hangup-button");

        hangup?.Click();

        Thread.Sleep(3000);
        driver.Close();

    });

    threads.Add(thread);

    thread.Start(i + 1);
}


Console.WriteLine("Launched. Type q and hit enter to exit.");
while (true)
{
    var command = Console.ReadLine();
    if (command.Contains("q", StringComparison.InvariantCultureIgnoreCase))
    {
        running = false;
        break;
    }
}

Console.WriteLine("Exiting...");
for (var i = 0; i < threads.Count; i++)
{
    threads[i].Join();
}

Console.WriteLine("All threads look finished. Exiting the app.");
